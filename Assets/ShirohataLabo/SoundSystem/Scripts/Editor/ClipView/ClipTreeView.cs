using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    public class ClipTreeView : TreeView {
        bool _showAudioClip;
        bool _showCustomClip;

        public Action<IEnumerable<Object>> OnSelectionChanged { get; set; }

        TargetFolders _targetFolders;

        public ClipTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, TargetFolders targetFolders) : base(state, multiColumnHeader) {
            rowHeight = 16;
            // showAlternatingRowBackgrounds = true;
            // showBorder = true;
            // enableItemHovering = true;
            // depthIndentWidth = 18;
            _targetFolders = targetFolders;
        }

        public void Reload(bool showAudioClip, bool showCustomClip) {
            _showAudioClip = showAudioClip;
            _showCustomClip = showCustomClip;
            Reload();
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {
                depth = -1,
                children = new List<TreeViewItem>()
            };

            foreach (string folderPath in _targetFolders.SafeGetFolderPaths()) {
                ClipTreeViewItem_Folder folderItem = new ClipTreeViewItem_Folder(folderPath);
                root.AddChild(folderItem);
                BuildItemRecursive(folderItem);
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        void BuildItemRecursive(ClipTreeViewItem_Folder parentFolderItem) {
            string folderPath = parentFolderItem.FolderPath;
            int nextClipItemInsertIndex = 0;
            foreach (string guid in AssetDatabase.FindAssets("", new string[] {folderPath})) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // フォルダパス以降に'/'が入っていたら直下ではない
                if (assetPath.Substring(folderPath.Length + 1).IndexOf('/') != -1) continue;

                if (AssetDatabase.IsValidFolder(assetPath)) {
                    ClipTreeViewItem_Folder folderItem = new ClipTreeViewItem_Folder(assetPath);
                    parentFolderItem.AddChild(folderItem);
                    BuildItemRecursive(folderItem);
                    continue;
                }
                
                Type assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (_showAudioClip && assetType == typeof(AudioClip)) {
                    AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
                    parentFolderItem.InsertChild(nextClipItemInsertIndex++, new ClipTreeViewItem_AudioClip(clip));
                }
                else if (_showCustomClip && assetType == typeof(CustomClip)) {
                    CustomClip clip = AssetDatabase.LoadAssetAtPath<CustomClip>(assetPath);
                    parentFolderItem.InsertChild(nextClipItemInsertIndex++, new ClipTreeViewItem_CustomClip(clip));
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args) {
            // Rect backgroundRect = new(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item) - 16};

            if (args.item is ClipTreeViewItem_Folder folderItem) {
                GUI.Box(args.rowRect, "", GUIStyles.FolderRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(folderItem.FolderAsset);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(folderItem.FolderAsset.name, icon));

                            if (folderItem.StandardImportSetting == null) break;
                            Rect importSettingRect = new Rect(cellRect) {xMin = cellRect.xMax - 28};
                            string importSettingPath = AssetDatabase.GetAssetPath(folderItem.StandardImportSetting);
                            Texture importSettingIcon = AssetDatabase.GetCachedIcon(importSettingPath);
                            if (GUI.Button(importSettingRect, new GUIContent(importSettingIcon))) {
                                Selection.activeObject = folderItem.StandardImportSetting;
                            }
                            break;
                        case 1:
                            if (folderItem.ImportSettingCheckResult.HasValue == false) {
                                EditorGUI.LabelField(cellRect, "-");
                            }
                            else if (folderItem.ImportSettingCheckResult.Value) {
                                EditorGUI.LabelField(cellRect, new GUIContent(Icons.OkIcon));
                            }
                            else {
                                EditorGUI.LabelField(cellRect, new GUIContent(Icons.NgIcon));
                            }
                            break;
                    }
                }
            }
            else if (args.item is ClipTreeViewItem_AudioClip clipItem) {
                GUI.Box(args.rowRect, "", GUIStyles.BasicRowBackground);
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
            else if (args.item is ClipTreeViewItem_CustomClip customClipItem) {
                GUI.Box(args.rowRect, "", GUIStyles.BasicRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(customClipItem.CustomClip);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(customClipItem.CustomClip.name, icon));
                            break;
                        case 1:
                            EditorGUI.LabelField(cellRect, "");
                            break;
                    }
                }
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds) {
            base.SelectionChanged(selectedIds);
            OnSelectionChanged?.Invoke(
                selectedIds
                    .Select(id => FindItem(id, rootItem))
                    .Select(item => {
                        if (item is ClipTreeViewItem_AudioClip clipItem) {
                            return clipItem.Clip as Object;
                        }
                        if (item is ClipTreeViewItem_CustomClip customClipItem) {
                            return customClipItem.CustomClip as Object;
                        }
                        return null;
                    })
                    .Where(x => x != null)
            );
        }

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }

        protected override void SingleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is ClipTreeViewItem_Folder folderItem) {
                folderItem.OnSingleClick();
            }
            else if (item is ClipTreeViewItem_AudioClip clipItem) {
                clipItem.OnSingleClick();
            }
            else if (item is ClipTreeViewItem_CustomClip customClipItem) {
                customClipItem.OnSingleClick();
            }
        }

        protected override void DoubleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is ClipTreeViewItem_Folder folderItem) {
                folderItem.OnDoubleClick();
            }
            else if (item is ClipTreeViewItem_AudioClip clipItem) {
                clipItem.OnDoubleClick();
            }
            else if (item is ClipTreeViewItem_CustomClip customClipItem) {
                customClipItem.OnDoubleClick();
            }
        }

        protected override void ContextClickedItem(int id) {
            Event.current.Use();

            TreeViewItem item = FindItem(id, rootItem);
            if (item is ClipTreeViewItem_Folder folderItem) {
                folderItem.OnContextClick();
            }
            else if (item is ClipTreeViewItem_AudioClip clipItem) {
                clipItem.OnContextClick();
            }
            else if (item is ClipTreeViewItem_CustomClip customClipItem) {
                customClipItem.OnContextClick();
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
                    if (x is ClipTreeViewItem_Folder folderItem) {
                        return folderItem.FolderAsset as Object;
                    }
                    if (x is ClipTreeViewItem_AudioClip clipItem) {
                        return clipItem.Clip as Object;
                    }
                    if (x is ClipTreeViewItem_CustomClip customClipItem) {
                        return customClipItem.CustomClip as Object;
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
                            if (args.parentItem is ClipTreeViewItem_Folder parentFolderItem) {
                                string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                                MoveTargetAssetsWithUndo(droppedObjects, folderPath);
                            }
                            if (args.parentItem is ClipTreeViewItem_AudioClip parentClipItem) {
                                string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentClipItem.Clip));
                                MoveTargetAssetsWithUndo(droppedObjects, folderPath);
                            }
                            if (args.parentItem is ClipTreeViewItem_CustomClip parentCustomClipItem) {
                                string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentCustomClipItem.CustomClip));
                                MoveTargetAssetsWithUndo(droppedObjects, folderPath);
                            }
                            Reload();
                            break;
                        default:
                            return DragAndDropVisualMode.None;
                    }
                }
                else if (droppedPaths.Length > 0) {
                    if (args.parentItem is ClipTreeViewItem_Folder parentFolderItem) {
                        string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                        AudioClipUtil.ImportDraggingExternalSoundFiles(folderPath);
                    }
                    if (args.parentItem is ClipTreeViewItem_AudioClip parentClipItem) {
                        string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentClipItem.Clip));
                        AudioClipUtil.ImportDraggingExternalSoundFiles(folderPath);
                    }
                    if (args.parentItem is ClipTreeViewItem_CustomClip parentCustomClipItem) {
                        string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(parentCustomClipItem.CustomClip));
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

            IEnumerable<Object> playableObjects = droppedObjects.Where(x => PlayableObjectTypes.Types.Contains(x.GetType()));
            EditorUtil.MoveAssetsWithUndo(playableObjects, folderPath);

            IEnumerable<StandardAudioClipImportSettings> importSettings = droppedObjects.OfType<StandardAudioClipImportSettings>();
            EditorUtil.MoveAssetsWithUndo(importSettings, folderPath);
        }

        public void CheckImportSetting() {
            if (rootItem.children == null) return;

            foreach (TreeViewItem item in rootItem.children) {
                if (item is ClipTreeViewItem_Folder folderItem) {
                    folderItem.CheckImportSettings(null);
                }
            }
        }

        public void ApplyImportSetting() {
            if (rootItem.children == null) return;

            foreach (TreeViewItem item in rootItem.children) {
                if (item is ClipTreeViewItem_Folder folderItem) {
                    folderItem.ApplyImportSettings(null);
                }
            }
        }

        protected override bool CanRename(TreeViewItem item) => true;

        protected override void RenameEnded(RenameEndedArgs args) {
            if (args.acceptedRename == false) return;
            TreeViewItem item = FindItem(args.itemID, rootItem);
            if (item is ClipTreeViewItem_Folder folderItem) {
                string assetPath = AssetDatabase.GetAssetPath(folderItem.FolderAsset);
                AssetDatabase.RenameAsset(assetPath, args.newName);
            }
            else if (item is ClipTreeViewItem_AudioClip clipItem) {
                string assetPath = AssetDatabase.GetAssetPath(clipItem.Clip);
                AssetDatabase.RenameAsset(assetPath, args.newName);
            }
            else if (item is ClipTreeViewItem_CustomClip customClipItem) {
                string assetPath = AssetDatabase.GetAssetPath(customClipItem.CustomClip);
                AssetDatabase.RenameAsset(assetPath, args.newName);
            }
        }
    }
}