using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class AudioClipTreeView : TreeView {
        public AudioClipTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            rowHeight = 16;
            // showAlternatingRowBackgrounds = true;
            // showBorder = true;
            // enableItemHovering = true;
            // depthIndentWidth = 18;
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {depth = -1};

            int id = 0;

            string folderPath = SoundSystemSetting.Instance.AudioClipFolderRoot;
            AudioClipTreeViewItem_Folder folderItem = new AudioClipTreeViewItem_Folder(++id, folderPath);
            root.AddChild(folderItem);

            BuildItemRecursive(folderItem, ref id);

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        void BuildItemRecursive(AudioClipTreeViewItem_Folder parentFolderItem, ref int id) {
            string folderPath = parentFolderItem.FolderPath;
            foreach (string guid in AssetDatabase.FindAssets("", new string[] {folderPath})) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // フォルダパス以降に'/'が入っていたら直下ではない
                if (assetPath.Substring(folderPath.Length + 1).IndexOf('/') != -1) continue;

                if (AssetDatabase.IsValidFolder(assetPath)) {
                    AudioClipTreeViewItem_Folder folderItem = new AudioClipTreeViewItem_Folder(++id, assetPath);
                    parentFolderItem.AddChild(folderItem);
                    BuildItemRecursive(folderItem, ref id);
                    continue;
                }
                
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
                if (clip != null) {
                    parentFolderItem.AddChild(new AudioClipTreeViewItem_AudioClip(++id, clip));
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args) {
            Rect backgroundRect = new(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item) - 16};

            if (args.item is AudioClipTreeViewItem_Folder folderItem) {
                GUI.Box(backgroundRect, "", GUIStyles.FolderRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(folderItem.FolderAsset);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(folderItem.FolderAsset.name, icon));
                            break;
                    }
                }
            }
            else if (args.item is AudioClipTreeViewItem_AudioClip clipItem) {
                GUI.Box(backgroundRect, "", GUIStyles.SoundRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(clipItem.Clip);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(clipItem.Clip.name, icon));
                            break;
                    }
                }
            }
        }

        protected override void SingleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioClipTreeViewItem_Folder folderItem) {
                folderItem.OnSingleClick();
            }
            else if (item is AudioClipTreeViewItem_AudioClip clipItem) {
                clipItem.OnSingleClick();
            } 
        }

        protected override void DoubleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioClipTreeViewItem_Folder folderItem) {
                folderItem.OnDoubleClick();
            }
            else if (item is AudioClipTreeViewItem_AudioClip clipItem) {
                clipItem.OnDoubleClick();
            }
        }

        protected override bool CanStartDrag(CanStartDragArgs args) {
            // TreeViewを1ウィンドウに複数描画＋ドラッグ有効だと一部のSingleClickedItemが呼ばれない事象への対処
            SingleClickedItem(args.draggedItem.id);
            return true;
        }

        protected override bool CanMultiSelect(TreeViewItem item) => false;

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args) {
            Object[] dragObjects = GetRows()
                .Where(item => args.draggedItemIDs.Contains(item.id))
                .Select(x => {
                    if (x is AudioClipTreeViewItem_Folder folderItem) {
                        return folderItem.FolderAsset as Object;
                    }
                    if (x is AudioClipTreeViewItem_AudioClip clipItem) {
                        return clipItem.Clip as Object;
                    }
                    return null;
                })
                .Where(x => x != null)
                .ToArray();
            DragAndDropUtil.SendObjects(dragObjects);
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args) {
            if (args.performDrop) {
                Object[] droppedObjects = DragAndDrop.objectReferences;
                if (droppedObjects == null || droppedObjects.Length == 0) {
                    return DragAndDropVisualMode.None;
                }

                switch (args.dragAndDropPosition) {
                    case DragAndDropPosition.UponItem:
                        if (args.parentItem is AudioClipTreeViewItem_Folder parentFolderItem) {
                            string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);

                            IEnumerable<DefaultAsset> folders = droppedObjects.OfType<DefaultAsset>().Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
                            EditorUtil.MoveAssetsWithUndo(folders, folderPath);

                            IEnumerable<AudioClip> clips = droppedObjects.OfType<AudioClip>();
                            EditorUtil.MoveAssetsWithUndo(clips, folderPath);
                        }
                        if (args.parentItem is AudioUnitTreeViewItem_AudioUnit parentUnitItem) {
                            string audioUnitFolder = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentUnitItem.AudioUnit));

                            IEnumerable<DefaultAsset> folders = droppedObjects.OfType<DefaultAsset>().Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
                            EditorUtil.MoveAssetsWithUndo(folders, audioUnitFolder);

                            IEnumerable<AudioClip> clips = droppedObjects.OfType<AudioClip>();
                            EditorUtil.MoveAssetsWithUndo(clips, audioUnitFolder);
                        }
                        Reload();
                        break;
                    default:
                        return DragAndDropVisualMode.None;
                }
            }

            if (args.dragAndDropPosition == DragAndDropPosition.UponItem) {
                return DragAndDropVisualMode.Move;
            }
            else {
                return DragAndDropVisualMode.None;
            }
        }

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }
    }
}