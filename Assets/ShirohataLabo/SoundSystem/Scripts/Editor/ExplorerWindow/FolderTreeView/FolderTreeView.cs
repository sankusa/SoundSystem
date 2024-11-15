using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    public class FolderTreeView : TreeView {
        public event Action<IEnumerable<DefaultAsset>> OnSelectionChanged;

        TargetFolders _targetFolders;

        public FolderTreeView(TreeViewState state, TargetFolders targetFolders) : base(state) {
            rowHeight = 16;
            // showAlternatingRowBackgrounds = true;
            // showBorder = true;
            // enableItemHovering = true;
            // depthIndentWidth = 18;
            _targetFolders = targetFolders;
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {
                depth = -1,
                children = new List<TreeViewItem>(),
            };

            foreach (string folderPath in _targetFolders.SafeGetFolderPaths()) {
                FolderTreeViewItem folderItem = new FolderTreeViewItem(folderPath);
                folderItem.OnReleaseFolder += () => Reload();
                root.AddChild(folderItem);
                BuildItemRecursive(folderItem);
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        void BuildItemRecursive(FolderTreeViewItem parentFolderItem) {
            string folderPath = parentFolderItem.FolderPath;
            foreach (string guid in AssetDatabase.FindAssets("", new string[] {folderPath})) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // フォルダパス以降に'/'が入っていたら直下ではない
                if (assetPath.Substring(folderPath.Length + 1).IndexOf('/') != -1) continue;

                if (AssetDatabase.IsValidFolder(assetPath)) {
                    FolderTreeViewItem folderItem = new FolderTreeViewItem(assetPath);
                    parentFolderItem.AddChild(folderItem);
                    BuildItemRecursive(folderItem);
                    continue;
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args) {
            if (args.item is FolderTreeViewItem folderItem) {
                if (args.item.depth == 0) {
                    GUI.Box(args.rowRect, "", GUIStyles.FolderRowBackground);
                }

                Rect rect = args.rowRect;
                rect.xMin += GetContentIndent(args.item);
                string assetPath = AssetDatabase.GetAssetPath(folderItem.FolderAsset);
                Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                EditorGUI.LabelField(rect, new GUIContent($"{folderItem.FolderAsset.name} ({folderItem.PlayableObjectCount})", icon));

                if (folderItem.StandardImportSettings != null) {
                    Rect importSettingRect = new Rect(rect) {xMin = rect.xMax - 28};
                    string importSettingPath = AssetDatabase.GetAssetPath(folderItem.StandardImportSettings);
                    Texture importSettingIcon = AssetDatabase.GetCachedIcon(importSettingPath);
                    if (GUI.Button(importSettingRect, new GUIContent(importSettingIcon))) {
                        Selection.activeObject = folderItem.StandardImportSettings;
                    }
                }

                Rect importSettingCheckResultRect = new Rect(rect) {width = 20};
                if (folderItem.ImportSettingCheckResult.HasValue == false) {
                    // EditorGUI.LabelField(importSettingCheckResultRect, "-");
                }
                else if (folderItem.ImportSettingCheckResult.Value) {
                    EditorGUI.LabelField(importSettingCheckResultRect, new GUIContent(Icons.OkIcon));
                }
                else {
                    EditorGUI.LabelField(importSettingCheckResultRect, new GUIContent(Icons.NgIcon));
                }
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds) {
            base.SelectionChanged(selectedIds);
            OnSelectionChanged?.Invoke(
                selectedIds
                    .Select(id => FindItem(id, rootItem))
                    .Select(item => {
                        FolderTreeViewItem folderItem = item as FolderTreeViewItem;
                        return folderItem.FolderAsset;
                    })
            );
        }

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }

        protected override void DoubleClickedItem(int id) {
            FolderTreeViewItem folderItem = FindItem(id, rootItem) as FolderTreeViewItem;
            folderItem.OnDoubleClick();
        }

        protected override void ContextClickedItem(int id) {
            Event.current.Use();

            FolderTreeViewItem folderItem = FindItem(id, rootItem) as FolderTreeViewItem;
            folderItem.OnContextClick();
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
                    FolderTreeViewItem folderItem = x as FolderTreeViewItem;
                    return folderItem.FolderAsset;
                })
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
                        case DragAndDropPosition.BetweenItems:
                            FolderTreeViewItem parentFolderItem = args.parentItem as FolderTreeViewItem;
                            string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                            MoveTargetAssetsWithUndo(droppedObjects, folderPath);
                            Reload();
                            break;
                        default:
                            return DragAndDropVisualMode.None;
                    }
                }
                else if (droppedPaths.Length > 0) {
                    FolderTreeViewItem parentFolderItem = args.parentItem as FolderTreeViewItem;
                    string folderPath = AssetDatabase.GetAssetPath(parentFolderItem.FolderAsset);
                    AudioClipUtil.ImportDraggingExternalSoundFiles(folderPath);
                }
                else {
                    return DragAndDropVisualMode.None;
                }
            }

            if (args.dragAndDropPosition == DragAndDropPosition.UponItem || args.dragAndDropPosition == DragAndDropPosition.BetweenItems) {
                return DragAndDropVisualMode.Generic;
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
                if (item is FolderTreeViewItem folderItem) {
                    folderItem.CheckImportSettings(null);
                }
            }
        }

        public void ApplyImportSetting() {
            if (rootItem.children == null) return;

            foreach (TreeViewItem item in rootItem.children) {
                if (item is FolderTreeViewItem folderItem) {
                    folderItem.ApplyImportSettings(null);
                }
            }
        }

        protected override bool CanRename(TreeViewItem item) => true;

        protected override void RenameEnded(RenameEndedArgs args) {
            if (args.acceptedRename == false) return;
            FolderTreeViewItem folderItem = FindItem(args.itemID, rootItem) as FolderTreeViewItem;
            string assetPath = AssetDatabase.GetAssetPath(folderItem.FolderAsset);
            AssetDatabase.RenameAsset(assetPath, args.newName);
        }
    }
}