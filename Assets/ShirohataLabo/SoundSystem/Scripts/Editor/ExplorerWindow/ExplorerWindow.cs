using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class ExplorerWindow : EditorWindow {
        [MenuItem("ShirohataLabo/" + nameof(SoundSystem) + "/" + nameof(ExplorerWindow))]
        static void Open() {
            GetWindow<ExplorerWindow>();
        }

        [SerializeField] FolderView _folderView = new();
        [SerializeField] FolderContentView _folderContentView = new();

        SearchField _searchField;

        LayoutSplitter _splitter;

        PlayableObjectDatabase _objectDatabase;
        TargetFolders _targetFolders;

        void OnEnable() {
            _objectDatabase = PlayableObjectDatabase.Instance;
            _targetFolders = TargetFolders.Instance;

            titleContent = new GUIContent("Explorer", Skin.Instance.MainIcon);

            _searchField = new();

            _folderView.OnEnable(_targetFolders);
            _folderContentView.OnEnable(_objectDatabase, _targetFolders);
            _folderView.OnSelectionChanged += selections => {
                _folderContentView.Reload(selections.FirstOrDefault());
            };

            _splitter = new(true, sessionStateKey: nameof(ExplorerWindow) + "_splitter") {
                ResizeHandleMouseAcceptRectOffset = new RectOffset(2, 2, 0, 0),
                HandleColor = new Color(0.1f, 0.1f, 0.1f),
                HandleWidth = 1,
            };

            OnProjectChanged();

            EditorApplication.projectChanged += OnProjectChanged;
        }

        void OnDisable() {
            _folderContentView.OnDisable();
            
            EditorApplication.projectChanged -= OnProjectChanged;
        }

        void OnGUI() {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar)) {
                EditorGUI.BeginChangeCheck();
                string searchString = _searchField.OnToolbarGUI(_folderContentView.SearchString);
                if (EditorGUI.EndChangeCheck()) {
                    _folderContentView.SearchString = searchString;
                }

                if (GUILayout.Button(Icons.RefleshIcon, EditorStyles.toolbarButton, GUILayout.Width(26))) {
                    _folderView.Reload();
                    _folderContentView.Reload();
                }
                if (GUILayout.Button(Icons.CommandIcon, EditorStyles.toolbarButton, GUILayout.Width(26))) {
                    GenericMenu menu = new();
                    menu.AddItem(
                        new GUIContent("Check Import Setting"),
                        false,
                        () => {
                            _folderView.CheckImportSetting();
                            _folderContentView.CheckImportSetting();
                        }
                    );
                    menu.ShowAsContext();
                }
            }
            using (new EditorGUILayout.HorizontalScope()) {
                _splitter.Begin();
                _folderView.OnGUI();
                _splitter.Split();
                _folderContentView.OnGUI();
                _splitter.End();
            }
        }

        void OnProjectChanged() {
            _objectDatabase.UpdateDatabase(_targetFolders.SafeGetFolderPaths().ToArray());
            _folderView.Reload();
            _folderContentView.Reload();
        }
    }
}