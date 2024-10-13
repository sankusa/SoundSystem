using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public static class TreeViewItemExtension {
        public static void InsertChild(this TreeViewItem parent, int index, TreeViewItem child) {
            parent.children ??= new List<TreeViewItem>();
            parent.children.Insert(index, child);
            if (child != null) child.parent = parent;
        }

        public static void InsertRangeChild(this TreeViewItem parent, int index, IEnumerable<TreeViewItem> childs) {
            parent.children ??= new List<TreeViewItem>();
            parent.children.InsertRange(index, childs);
            foreach (TreeViewItem child in childs) {
                if (child != null) child.parent = parent;
            }
        }
    }
}