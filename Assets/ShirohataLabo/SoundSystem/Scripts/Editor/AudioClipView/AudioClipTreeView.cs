using System.Collections.Generic;
using System.IO;
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
            useScrollView = false;
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {depth = -1};

            string folderPath = SoundSystemSetting.Instance.AudioClipFolderRoot;
            AudioClipTreeViewItem_Folder folderItem = new AudioClipTreeViewItem_Folder(folderPath);
            root.AddChild(folderItem);

            BuildItemRecursive(folderItem);

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        void BuildItemRecursive(AudioClipTreeViewItem_Folder parentFolderItem) {
            string folderPath = parentFolderItem.FolderPath;
            int nextClipItemInsertIndex = 0;
            foreach (string guid in AssetDatabase.FindAssets("", new string[] {folderPath})) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // フォルダパス以降に'/'が入っていたら直下ではない
                if (assetPath.Substring(folderPath.Length + 1).IndexOf('/') != -1) continue;

                if (AssetDatabase.IsValidFolder(assetPath)) {
                    AudioClipTreeViewItem_Folder folderItem = new AudioClipTreeViewItem_Folder(assetPath);
                    parentFolderItem.AddChild(folderItem);
                    BuildItemRecursive(folderItem);
                    continue;
                }
                
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
                if (clip != null) {
                    parentFolderItem.InsertChild(nextClipItemInsertIndex++, new AudioClipTreeViewItem_AudioClip(clip));
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
                        case 1:
                            if (folderItem.StandardImportSetting == null) break;
                            string importSettingPath = AssetDatabase.GetAssetPath(folderItem.StandardImportSetting);
                            Texture importSettingIcon = AssetDatabase.GetCachedIcon(importSettingPath);
                            if (GUI.Button(cellRect, new GUIContent(importSettingIcon))) {
                                Selection.activeObject = folderItem.StandardImportSetting;
                            }
                            break;
                    }
                }
            }
            else if (args.item is AudioClipTreeViewItem_AudioClip clipItem) {
                GUI.Box(backgroundRect, "", GUIStyles.BasicRowBackground);
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
                        case 1:
                            if (clipItem.ImportSettingCheckResult.HasValue == false) {
                                EditorGUI.LabelField(cellRect, "-");
                            }
                            else if (clipItem.ImportSettingCheckResult.Value) {
                                EditorGUI.LabelField(cellRect, new GUIContent(Icons.OkIcon));
                            }
                            else {
                                EditorGUI.LabelField(cellRect, new GUIContent(Icons.NgIcon));
                            }
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

        protected override void ContextClickedItem(int id) {
            Event.current.Use();

            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioClipTreeViewItem_Folder folderItem) {
                folderItem.OnContextClick();
            }
            else if (item is AudioClipTreeViewItem_AudioClip clipItem) {
                clipItem.OnContextClick();
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
                string[] droppedPaths = DragAndDrop.paths;
                if (droppedObjects != null && droppedObjects.Length > 0) {
                    switch (args.dragAndDropPosition) {
                        case DragAndDropPosition.UponItem:
                            if (args.parentItem is AudioClipTreeViewItem_Folder parentFolderItem) {
                                string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                                MoveTargetAssetsWithUndo(droppedObjects, folderPath);
                            }
                            if (args.parentItem is AudioClipTreeViewItem_AudioClip parentClipItem) {
                                string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentClipItem.Clip));
                                MoveTargetAssetsWithUndo(droppedObjects, folderPath);
                            }
                            Reload();
                            break;
                        default:
                            return DragAndDropVisualMode.None;
                    }
                }
                else if (droppedPaths.Length > 0) {
                    if (args.parentItem is AudioClipTreeViewItem_Folder parentFolderItem) {
                        string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                        AudioClipUtil.ImportDraggingExternalSoundFiles(folderPath);
                    }
                    if (args.parentItem is AudioClipTreeViewItem_AudioClip parentClipItem) {
                        string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentClipItem.Clip));
                        AudioClipUtil.ImportDraggingExternalSoundFiles(folderPath);
                    }
                }
                else {
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

        static void MoveTargetAssetsWithUndo(Object[] droppedObjects, string folderPath) {
            IEnumerable<DefaultAsset> folders = droppedObjects.OfType<DefaultAsset>().Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
            EditorUtil.MoveAssetsWithUndo(folders, folderPath);

            IEnumerable<AudioClip> clips = droppedObjects.OfType<AudioClip>();
            EditorUtil.MoveAssetsWithUndo(clips, folderPath);

            IEnumerable<StandardAudioClipImportSettings> importSettings = droppedObjects.OfType<StandardAudioClipImportSettings>();
            EditorUtil.MoveAssetsWithUndo(importSettings, folderPath);
        }

        public void CheckImportSetting() {
            if (rootItem.children == null) return;

            foreach (TreeViewItem item in rootItem.children) {
                if (item is AudioClipTreeViewItem_Folder folderItem) {
                    folderItem.CheckImportSettings(null);
                }
            }
        }

        public void ApplyImportSetting() {
            if (rootItem.children == null) return;

            foreach (TreeViewItem item in rootItem.children) {
                if (item is AudioClipTreeViewItem_Folder folderItem) {
                    folderItem.ApplyImportSettings(null);
                }
            }
        }
    }
}