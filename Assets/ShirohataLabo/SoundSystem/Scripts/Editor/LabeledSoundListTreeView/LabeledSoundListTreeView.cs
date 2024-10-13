using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class LabeledSoundListTreeView : TreeView {
        SerializedProperty _labeledSoundListProp;

        public LabeledSoundListTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            showBorder = true;
            rowHeight = 20;
            // showAlternatingRowBackgrounds = true;
            useScrollView = false;
            // enableItemHovering = true;
        }

        public void Reload(SerializedProperty labeledSoundListProp) {
            _labeledSoundListProp = labeledSoundListProp;
            Reload();
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {
                id = -1,
                depth = -1,
                children = new List<TreeViewItem>()
            };


            int id = 0;

            if (_labeledSoundListProp != null) {
                for (int i = 0; i < _labeledSoundListProp.arraySize; i++) {
                    root.AddChild(
                        new LabeledSoundListTreeViewItem(
                            ++id,
                            _labeledSoundListProp.GetArrayElementAtIndex(i)
                        )
                    );
                }
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args) {
            // GUI.Box(args.rowRect, "", GUIStyles.SoundRowBackground);

            int row = FindRowOfItem(args.item);

            SerializedProperty elementProp = _labeledSoundListProp.GetArrayElementAtIndex(row);
            for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                Rect cellRect = args.GetCellRect(i);
                int columnIndex = args.GetColumn(i);
                switch (columnIndex) {
                    case 0:
                        cellRect.xMin += 2;
                        EditorGUI.LabelField(cellRect, elementProp.FindPropertyRelative("_label").stringValue);
                        // EditorGUI.PropertyField(cellRect, elementProp.FindPropertyRelative("_label"), GUIContent.none);
                        break;
                    case 1:
                        cellRect.yMin += 1;
                        EditorGUI.PropertyField(cellRect, elementProp.FindPropertyRelative("_sound"), GUIContent.none);
                        break;
                }
            }
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item) {
            LabeledSoundListTreeViewItem targetItem = item as LabeledSoundListTreeViewItem;
            return EditorGUI.GetPropertyHeight(targetItem.LabeledSoundProp, GUIContent.none, true);
        }

        protected override bool CanBeParent(TreeViewItem item) => false;

        protected override bool CanStartDrag(CanStartDragArgs args)  => true;

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args) {
            DragAndDropUtil.SendGenericObject(
                nameof(LabeledSoundListTreeViewItem),
                GetRows().Where(item => args.draggedItemIDs.Contains(item.id)).ToList()
            );
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args) {
            if (args.performDrop) {
                List<TreeViewItem> items = DragAndDrop.GetGenericData(nameof(LabeledSoundListTreeViewItem)) as List<TreeViewItem>;
                if (items == null || items.Count == 0) {
                    return DragAndDropVisualMode.None;
                }
                if (args.dragAndDropPosition == DragAndDropPosition.BetweenItems) {
                    foreach (TreeViewItem item in items) {
                        int srcIndex = FindRowOfItem(item);
                        int dstIndex = args.insertAtIndex > srcIndex ? args.insertAtIndex - 1 : args.insertAtIndex;
                        _labeledSoundListProp.MoveArrayElement(srcIndex, dstIndex);
                        Reload();
                        this.SetSelection(new int[] {dstIndex + 1});
                    }
                }
                return DragAndDropVisualMode.Move;
            }
            else if (isDragging) {
                return DragAndDropVisualMode.Move;
            }
            return DragAndDropVisualMode.None;
        }

        protected override bool CanRename(TreeViewItem item) => true;
        protected override Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item) {
            return GetCellRectForTreeFoldouts(rowRect);
        }
        protected override void RenameEnded(RenameEndedArgs args) {
            if (args.acceptedRename) {
                _labeledSoundListProp.GetArrayElementAtIndex(FindRowOfItem(FindItem(args.itemID, rootItem))).FindPropertyRelative("_label").stringValue = args.newName;
                Reload();
            }
        }
    }
}