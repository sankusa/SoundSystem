using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

namespace SoundSystem {
    public class DragAndDropUtil {
        /// <summary> ProjectView等のUnityのwindowと受け渡し可能 </summary>
        public static void SendObjects(Object[] objects) {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = objects;
            DragAndDrop.paths = objects.Select(x => AssetDatabase.GetAssetPath(x)).ToArray();
            DragAndDrop.StartDrag(string.Empty);
        }
        /// <summary> ProjectView等のUnityのwindowと受け渡し可能 </summary>
        public static T AcceptObject<T>(Rect dropRect) where T : Object {
            return AcceptObjectsInternal(dropRect)?.OfType<T>().FirstOrDefault();
        }
        /// <summary> ProjectView等のUnityのwindowと受け渡し可能 </summary>
        public static List<T> AcceptObjects<T>(Rect dropRect) where T : Object {
            return AcceptObjectsInternal(dropRect)?.OfType<T>().ToList();
        }
        static Object[] AcceptObjectsInternal(Rect dropRect) {
            // カーソルが範囲外ならスルー
            if (dropRect.Contains(Event.current.mousePosition) == false) return null;

            // カーソルの見た目をドラッグ用に変更
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            
            // ドロップでなければ終了
            if (Event.current.type != EventType.DragPerform) return null;

            // ドロップを受け入れる
            DragAndDrop.AcceptDrag();

            // イベントを使用済みに
            Event.current.Use();

            return DragAndDrop.objectReferences;
        }

        /// <summary> ProjectView等のUnityのwindowとは受け渡し不可 </summary>
        public static void SendGenericObject(string key, object obj) {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData(key, obj);
            DragAndDrop.StartDrag(string.Empty);
        }
        /// <summary> ProjectView等のUnityのwindowとは受け渡し不可 </summary>
        public static object AcceptGenericObject(string key, Rect dropRect) {
            // カーソルが範囲外ならスルー
            if (dropRect.Contains(Event.current.mousePosition) == false) return null;

            // カーソルの見た目をドラッグ用に変更
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

            // ドロップでなければ終了
            if (Event.current.type != EventType.DragPerform) return null;

            // ドロップを受け入れる
            DragAndDrop.AcceptDrag();

            // イベントを使用済みに
            Event.current.Use();

            return DragAndDrop.GetGenericData(key);
        }

        public static void CopyDraggingExternalFiles(string destinationFolderPath, string[] acceptExtensions) {
            string projectPath = Path.GetFullPath(Application.dataPath + "/..");

            int copyCount = 0;
            foreach (string path in DragAndDrop.paths) {
                bool isExternalFile = Path.GetFullPath(path).StartsWith(projectPath) == false;
                if (isExternalFile == false) continue;

                bool validExtension = false;
                foreach (string extenson in acceptExtensions) {
                    if (Path.GetExtension(path).ToLower() == extenson) {
                        validExtension = true;
                        break;
                    }
                }
                if (validExtension) {;
                    string fileName = Path.GetFileName(path);
                    string destinationPath = AssetDatabase.GenerateUniqueAssetPath(destinationFolderPath + "/" + fileName);
                    File.Copy(path, destinationPath);
                    copyCount++;
                }
            }
            if (copyCount > 0) AssetDatabase.Refresh();
        }
    }
}