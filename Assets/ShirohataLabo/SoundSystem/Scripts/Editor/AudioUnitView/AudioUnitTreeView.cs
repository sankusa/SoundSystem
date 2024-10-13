using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class AudioUnitTreeView : TreeView {
        const string DragDataKey = nameof(SoundSystem) + "_" + nameof(AudioUnitTreeView) + "_DragData";
        List<AudioUnit> _audioUnits;
        List<AudioUnitCategory> _categories;

        public AudioUnitTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            // rowHeight = 20;
            // showAlternatingRowBackgrounds = true;
            // showBorder = true;
            // enableItemHovering = true;
            depthIndentWidth = 18;
        }

        public void Reload(List<AudioUnit> audioUnits, List<AudioUnitCategory> categories) {
            _audioUnits = audioUnits;
            _categories = categories;
            Reload();
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {depth = -1};

            int id = 0;

            List<AudioUnitTreeViewItem_AudioUnitCategory> categoryItems = new();
            foreach (AudioUnitCategory category in _categories) {
                categoryItems.Add(new AudioUnitTreeViewItem_AudioUnitCategory(++id, category));
            }
            foreach (AudioUnitTreeViewItem_AudioUnitCategory categoryItem in categoryItems) {
                if (categoryItem.Category.Parent == null) {
                    root.AddChild(categoryItem);
                }
                else {
                    categoryItems
                        .Find(x => x.Category == categoryItem.Category.Parent)
                        .AddChild(categoryItem);
                }
            }

            foreach(var group in _audioUnits.GroupBy(x => x.Category)) {
                if (group.Key == null) {
                    root.InsertRangeChild(0, group.Select(x => new AudioUnitTreeViewItem_AudioUnit(++id, x)));
                }
                else {
                    AudioUnitTreeViewItem_AudioUnitCategory targetCategoryItem = categoryItems.Find(x => x.Category == group.Key);
                    targetCategoryItem.InsertRangeChild(0, group.Select(x => new AudioUnitTreeViewItem_AudioUnit(++id, x)));
                }
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args) {
            Rect rect = new(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item)};
            if (args.item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                Rect backRect = new Rect(rect) {xMin = rect.xMin - 16};
                GUI.Box(backRect, "", GUIStyles.AudioUnitRowBackground);

                string assetPath = AssetDatabase.GetAssetPath(unitItem.AudioUnit);
                Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                EditorGUI.LabelField(rect, new GUIContent(unitItem.Label, icon));
            }
            else if(args.item is AudioUnitTreeViewItem_AudioUnitCategory categoryDataItem) {
                Rect backRect = new Rect(rect) {xMin = rect.xMin - 16};
                GUI.Box(backRect, "", GUIStyles.CategoryRowBackground);

                string assetPath = AssetDatabase.GetAssetPath(categoryDataItem.Category);
                Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                EditorGUI.LabelField(rect, new GUIContent(categoryDataItem.Label, icon));
            }
            else {
                base.RowGUI(args);
            }
        }

        protected override void SingleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                unitItem.OnSingleClick();
            }
            else if (item is AudioUnitTreeViewItem_AudioUnitCategory categoryDataItem) {
                categoryDataItem.OnSingleClick();
            }
        }

        protected override void DoubleClickedItem(int id) {
            TreeViewItem item = FindItem(id, rootItem);
            if (item is AudioUnitTreeViewItem_AudioUnit unitItem) {
                unitItem.OnDoubleClick();
            }
            else if (item is AudioUnitTreeViewItem_AudioUnitCategory categoryDataItem) {
                categoryDataItem.OnDoubleClick();
            }
        }

        protected override bool CanStartDrag(CanStartDragArgs args) => true;

        protected override bool CanMultiSelect(TreeViewItem item) => false;

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args) {
            Object[] dragObjects = GetRows()
                .Where(item => args.draggedItemIDs.Contains(item.id))
                .Select(x => {
                    if (x is AudioUnitTreeViewItem_AudioUnitCategory categoryItem) {
                        return categoryItem.Category as Object;
                    }
                    if (x is AudioUnitTreeViewItem_AudioUnit unitItem) {
                        return unitItem.AudioUnit as Object;
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
                        // Categoryにドロップ
                        if (args.parentItem is AudioUnitTreeViewItem_AudioUnitCategory parentCategoryItem) {
                            foreach (Object obj in droppedObjects) {
                                if (obj is AudioUnitCategory category) {
                                    category.SetParentWithUndo(parentCategoryItem.Category);
                                }
                                else if (obj is AudioUnit audioUnit) {
                                    audioUnit.SetCategoryWithUndo(parentCategoryItem.Category);
                                }
                            }
                        }
                        // Categoryにドロップ
                        else if (args.parentItem is AudioUnitTreeViewItem_AudioUnit parentUnitItem) {
                            foreach (Object obj in droppedObjects) {
                                if (obj is AudioUnitCategory category) {
                                    category.SetParentWithUndo(parentUnitItem.AudioUnit.Category);
                                }
                                else if (obj is AudioUnit audioUnit) {
                                    audioUnit.SetCategoryWithUndo(parentUnitItem.AudioUnit.Category);
                                }
                            }
                        }
                        Reload();
                        break;
                    case DragAndDropPosition.OutsideItems:
                        foreach (Object obj in droppedObjects) {
                            if (obj is AudioUnitCategory category) {
                                category.SetParentWithUndo(null);
                            }
                            else if (obj is AudioUnit audioUnit) {
                                audioUnit.SetCategoryWithUndo(null);
                            }
                        }
                        Reload();
                        break;
                    default:
                        return DragAndDropVisualMode.None;
                }
            }
            else if (isDragging) {
                if (args.dragAndDropPosition == DragAndDropPosition.UponItem) {
                    return DragAndDropVisualMode.Move;
                }
                else if (args.dragAndDropPosition == DragAndDropPosition.OutsideItems) {
                    return DragAndDropVisualMode.Move;
                }
                else {
                    return DragAndDropVisualMode.None;
                }
            }
            return DragAndDropVisualMode.Move;
        }

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }
    }
}