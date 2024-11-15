using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    public class FolderContentTreeView : TreeView {
        const string folderAssetGuidSessionStateKey = nameof(FolderContentTreeView) + "_Folder_Guid";
        [SerializeField] DefaultAsset _currentFolder;
        DefaultAsset CurrentFolder {
            set {
                if (value == null) {
                    SessionState.EraseString(folderAssetGuidSessionStateKey);
                    _currentFolder = null;
                }

                string assetPath = AssetDatabase.GetAssetPath(_currentFolder);
                if (AssetDatabase.IsValidFolder(assetPath) == false) {
                    SessionState.EraseString(folderAssetGuidSessionStateKey);
                    _currentFolder = null;
                }

                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                SessionState.SetString(folderAssetGuidSessionStateKey, guid);
                _currentFolder = value;
            }
        }
        string CurrentFolderPath => AssetDatabase.GetAssetPath(_currentFolder);

        int _renameTargetColumnIndex;
        Rect _renameRect;

        public event Action<IEnumerable<Object>> OnSelectionChanged;

        PlayableObjectDatabase _objectDatabase;
        TargetFolders _targetFolders;

        public FolderContentTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, PlayableObjectDatabase objectDatabase, TargetFolders targetFolders) : base(state, multiColumnHeader) {
            _objectDatabase = objectDatabase;
            _targetFolders = targetFolders;
            rowHeight = 16;
            // showAlternatingRowBackgrounds = true;

            string folderGuid = SessionState.GetString(folderAssetGuidSessionStateKey, "");
            if (string.IsNullOrEmpty(folderGuid) == false) {
                _currentFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(AssetDatabase.GUIDToAssetPath(folderGuid));
            }
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {
                depth = -1,
                children = new List<TreeViewItem>(),
            };
            return root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root) {
            var rows = GetRows() ?? new List<TreeViewItem>();
            rows.Clear();

            if (string.IsNullOrEmpty(searchString)) {
                if (_currentFolder == null) return rows;

                IEnumerable<Object> assets = EditorUtil.LoadAllAssetFromTargetFolder<Object>(CurrentFolderPath);

                foreach (Object asset in assets) {
                    ObjectDatabaseRecord record = _objectDatabase.Records.FirstOrDefault(x => x.Asset == asset);
                    if (record == null) continue;
                    if (asset is AudioClip) {
                        var item = new FolderContentTreeViewItem_AudioClip(record);
                        root.AddChild(item);
                        rows.Add(item);
                    }
                    else if (asset is CustomClip) {
                        var item = new FolderContentTreeViewItem_CustomClip(record);
                        root.AddChild(item);
                        rows.Add(item);
                    }
                }
            }
            else {
                IEnumerable<Object> assets = PlayableObjectUtil.LoadAllPlayableObjects(
                    _targetFolders.SafeGetFolderPaths().ToArray()
                );
                IEnumerable<ObjectDatabaseRecord> records = assets
                    .Select(asset => _objectDatabase.Records.FirstOrDefault(x => x.Asset == asset))
                    .Where(record => record != null);
                foreach (ObjectDatabaseRecord record in records) {
                    // 検索対象かチェック
                    bool isTarget = false;
                    if (record.Asset.name.Contains(searchString)) {
                        isTarget = true;
                    }
                    else if (_objectDatabase.ColumnDefinition != null) {
                        for (int i = 0; i < _objectDatabase.ColumnDefinition.Columns.Count; i++) {
                            if (record.Columns[i].Value.Contains(searchString)) {
                                isTarget = true;
                            }
                        }
                    }
                    if (isTarget == false) continue;

                    if (record.Asset is AudioClip) {
                        var item = new FolderContentTreeViewItem_AudioClip(record);
                        root.AddChild(item);
                        rows.Add(item);
                    }
                    else if (record.Asset is CustomClip) {
                        var item = new FolderContentTreeViewItem_CustomClip(record);
                        root.AddChild(item);
                        rows.Add(item);
                    }
                }
            }

            SetupDepthsFromParentsAndChildren(root);
            return rows;
        }

        public void Reload(DefaultAsset folder) {
            CurrentFolder = folder;
            Reload();
        }

        public override void OnGUI(Rect rect) {
            AcceptDrag(rect);
            base.OnGUI(rect);
        }

        void AcceptDrag(Rect rect) {
            if (_currentFolder == null) return;

            // カーソルが範囲外ならスルー
            if (rect.Contains(Event.current.mousePosition) == false) return;

            // カーソルの見た目をドラッグ用に変更
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            
            // ドロップでなければ終了
            if (Event.current.type != EventType.DragPerform) return;

            // ドロップを受け入れる
            DragAndDrop.AcceptDrag();

            // イベントを使用済みに
            Event.current.Use();

            Object[] droppedObjects = DragAndDrop.objectReferences;
            string[] droppedPaths = DragAndDrop.paths;

            if (droppedObjects != null && droppedObjects.Length > 0) {
                MoveTargetAssetsWithUndo(droppedObjects, CurrentFolderPath);
                Reload();
            }
            else if (droppedPaths.Length > 0) {
                AudioClipUtil.ImportDraggingExternalSoundFiles(CurrentFolderPath);
            }
        }

        protected override void RowGUI(RowGUIArgs args) {
            Rect rowRect = new Rect(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item)};
            Rect backgroundRect = new(rowRect) {xMin = rowRect.xMin - 16};

            ObjectDatabaseRecord record = (args.item as FolderContentTreeViewItemBase).Record;

            GUI.Box(backgroundRect, "", GUIStyles.BasicRowBackground);

            for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                Rect cellRect = args.GetCellRect(i);
                int columnIndex = args.GetColumn(i);
                switch (columnIndex) {
                    case 0:
                        cellRect.xMin += GetContentIndent(args.item);
                        
                        string assetPath = AssetDatabase.GetAssetPath(record.Asset);
                        Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                        if (args.item is FolderContentTreeViewItem_AudioClip audioClipItem) {
                            Rect labelRect = new Rect(cellRect) {xMax = cellRect.xMax};
                            EditorGUI.LabelField(labelRect, new GUIContent(record.Asset.name, icon));

                            Rect importSettingCheckResultRect = new Rect(cellRect) {xMin = cellRect.xMin - foldoutWidth - 4, width = foldoutWidth + 4};
                            if (audioClipItem.ImportSettingCheckResult.HasValue == false) {
                                // EditorGUI.LabelField(importSettingCheckResultRect, "-");
                            }
                            else if (audioClipItem.ImportSettingCheckResult.Value) {
                                EditorGUI.LabelField(importSettingCheckResultRect, new GUIContent(Icons.OkIcon));
                            }
                            else {
                                EditorGUI.LabelField(importSettingCheckResultRect, new GUIContent(Icons.NgIcon));
                            }
                        }
                        else {
                            EditorGUI.LabelField(cellRect, new GUIContent(record.Asset.name, icon));
                        }

                        if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition)) {
                            args.item.displayName = record.Asset.name;
                            _renameTargetColumnIndex = columnIndex;
                            _renameRect = cellRect;
                        }
                        break;
                    default:
                        EditorGUI.LabelField(cellRect, record.Columns[columnIndex - 1].Value);
                        if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition)) {
                            args.item.displayName = record.Columns[columnIndex - 1].Value;
                            _renameTargetColumnIndex = columnIndex;
                            _renameRect = cellRect;
                        }
                        break;
                }
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds) {
            base.SelectionChanged(selectedIds);
            OnSelectionChanged?.Invoke(
                selectedIds
                    .Select(id => FindItem(id, rootItem))
                    .Select(item => {
                        if (item is FolderContentTreeViewItem_AudioClip clipItem) {
                            return clipItem.AudioClip as Object;
                        }
                        if (item is FolderContentTreeViewItem_CustomClip customClipItem) {
                            return customClipItem.CustomClip as Object;
                        }
                        return null;
                    })
                    .Where(x => x != null)
            );
        }

        protected override void SingleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            (item as FolderContentTreeViewItemBase).OnSingleClick();
        }

        protected override void DoubleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            (item as FolderContentTreeViewItemBase).OnDoubleClick();
        }

        protected override void ContextClickedItem(int id) {
            Event.current.Use();

            TreeViewItem item = FindItem(id, rootItem);
            (item as FolderContentTreeViewItemBase).OnContextClick();
        }

        protected override bool CanStartDrag(CanStartDragArgs args) {
            // TreeViewを1ウィンドウに複数描画＋ドラッグ有効だと一部のSingleClickedItemが呼ばれない事象への対処
            SingleClickedItem(args.draggedItem.id);
            return true;
        }

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args) {
            Object[] dragObjects = GetRows()
                .Where(item => args.draggedItemIDs.Contains(item.id))
                .Select(x => {
                    return (x as FolderContentTreeViewItemBase).Record.Asset;
                })
                .Where(x => x != null)
                .ToArray();
            DragAndDropUtil.SendObjects(dragObjects);
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
                if (item is FolderContentTreeViewItem_AudioClip audioClipItem) {
                    audioClipItem.CheckImportSettings();
                }
            }
        }

        public void ApplyImportSetting() {
            if (rootItem.children == null) return;

            foreach (TreeViewItem item in rootItem.children) {
                if (item is FolderContentTreeViewItem_AudioClip audioClipItem) {
                    audioClipItem.ApplyImportSettings();
                }
            }
        }

        protected override bool CanRename(TreeViewItem item) => true;

        protected override Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item) {
            return _renameRect;
        }

        protected override void RenameEnded(RenameEndedArgs args) {
            if (args.acceptedRename == false) return;
            TreeViewItem item = FindItem(args.itemID, rootItem);
            ObjectDatabaseRecord record = (item as FolderContentTreeViewItemBase).Record;

            if (_renameTargetColumnIndex == 0) {
                string assetPath = AssetDatabase.GetAssetPath(record.Asset);
                AssetDatabase.RenameAsset(assetPath, args.newName);
            }
            else {
                Undo.RecordObject(_objectDatabase, $"Change {nameof(PlayableObjectDatabase)}");
                record.Columns[_renameTargetColumnIndex - 1].Value = args.newName;
            }
        }
    }
}