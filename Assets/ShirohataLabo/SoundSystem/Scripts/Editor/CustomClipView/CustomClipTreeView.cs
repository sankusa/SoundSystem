using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    [Serializable]
    public class CustomClipTreeView : TreeView {
        public Action<IEnumerable<CustomClip>> _onSelectedCustomClipChanged;

        public CustomClipTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            rowHeight = 16;
            // showAlternatingRowBackgrounds = true;
            // showBorder = true;
            // enableItemHovering = true;
            // depthIndentWidth = 18;
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {depth = -1};

            int id = 0;

            string folderPath = SoundSystemSetting.Instance.CustomClipFolderRoot;
            CustomClipTreeViewItem_Folder folderItem = new CustomClipTreeViewItem_Folder(folderPath);
            root.AddChild(folderItem);

            BuildItemRecursive(folderItem, ref id);

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        void BuildItemRecursive(CustomClipTreeViewItem_Folder parentFolderItem, ref int id) {
            string folderPath = parentFolderItem.FolderPath;
            int nextClipItemInsertIndex = 0;
            foreach (string guid in AssetDatabase.FindAssets("", new string[] {folderPath})) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // フォルダパス以降に'/'が入っていたら直下ではない
                if (assetPath.Substring(folderPath.Length + 1).IndexOf('/') != -1) continue;

                if (AssetDatabase.IsValidFolder(assetPath)) {
                    CustomClipTreeViewItem_Folder folderItem = new CustomClipTreeViewItem_Folder(assetPath);
                    parentFolderItem.AddChild(folderItem);
                    BuildItemRecursive(folderItem, ref id);
                    continue;
                }
                
                CustomClip customClip = AssetDatabase.LoadAssetAtPath<CustomClip>(assetPath);
                if (customClip != null) {
                    parentFolderItem.InsertChild(nextClipItemInsertIndex++, new CustomClipTreeViewItem_CustomClip(customClip));
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args) {
            Rect backgroundRect = new(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item) - 16};

            if (args.item is CustomClipTreeViewItem_Folder folderItem) {
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
            else if (args.item is CustomClipTreeViewItem_CustomClip clipItem) {
                GUI.Box(backgroundRect, "", GUIStyles.BasicRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(clipItem.CustomClip);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(clipItem.CustomClip.name, icon));
                            break;
                    }
                }
            }
        }

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }

        protected override void SingleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is CustomClipTreeViewItem_Folder folderItem) {
                folderItem.OnSingleClick();
            }
            else if (item is CustomClipTreeViewItem_CustomClip clipItem) {
                clipItem.OnSingleClick();
            } 
        }

        protected override void DoubleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is CustomClipTreeViewItem_Folder folderItem) {
                folderItem.OnDoubleClick();
            }
            else if (item is CustomClipTreeViewItem_CustomClip clipItem) {
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
                    if (x is CustomClipTreeViewItem_Folder folderItem) {
                        return folderItem.FolderAsset as Object;
                    }
                    if (x is CustomClipTreeViewItem_CustomClip clipItem) {
                        return clipItem.CustomClip as Object;
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
                        if (args.parentItem is CustomClipTreeViewItem_Folder parentFolderItem) {
                            string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                            HandleDroppedObjects(droppedObjects, folderPath);
                        }
                        if (args.parentItem is CustomClipTreeViewItem_CustomClip parentClipItem) {
                            string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentClipItem.CustomClip));
                            HandleDroppedObjects(droppedObjects, folderPath);
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

        static void HandleDroppedObjects(Object[] droppedObjects, string folderPath) {
            IEnumerable<CustomClip> customClips = droppedObjects.OfType<CustomClip>();
            EditorUtil.MoveAssetsWithUndo(customClips, folderPath);

            IEnumerable<DefaultAsset> folders = droppedObjects.OfType<DefaultAsset>().Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
            EditorUtil.MoveAssetsWithUndo(folders, folderPath);

            IEnumerable<AudioClip> clips = droppedObjects.OfType<AudioClip>();
            CustomClipUtil.CreateCustomClips(clips, folderPath);
        }

        protected override void SelectionChanged(IList<int> selectedIds) {
            base.SelectionChanged(selectedIds);
            _onSelectedCustomClipChanged?.Invoke(
                selectedIds
                    .Select(id => FindItem(id, rootItem))
                    .OfType<CustomClipTreeViewItem_CustomClip>()
                    .Select(x => x.CustomClip)
            );
        }

        protected override bool CanRename(TreeViewItem item) => true;

        protected override void RenameEnded(RenameEndedArgs args) {
            if (args.acceptedRename == false) return;
            TreeViewItem item = FindItem(args.itemID, rootItem);
            if (item is CustomClipTreeViewItem_CustomClip clipItem) {
                string assetPath = AssetDatabase.GetAssetPath(clipItem.CustomClip);
                AssetDatabase.RenameAsset(assetPath, args.newName);
            }
            else if (item is CustomClipTreeViewItem_Folder folderItem) {
                string assetPath = AssetDatabase.GetAssetPath(folderItem.FolderAsset);
                AssetDatabase.RenameAsset(assetPath, args.newName);
            }
        }
    }
}