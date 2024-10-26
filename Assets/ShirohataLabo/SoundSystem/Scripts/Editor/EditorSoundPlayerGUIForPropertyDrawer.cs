using System;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayerGUIForPropertyDrawer {
        EditorSoundPlayerGUI _playerGUI;
        
        bool _enabled;
        double _lastOnGUICalledTime;

        bool _disableReserved;
        double _onDisableReserveTime;

        public EditorSoundPlayerGUIForPropertyDrawer() {
            _playerGUI = new();
            _playerGUI.OnEnable();
            // コンパイル時にEditorApplication.updateに登録されたメソッドが実行されないため
            AssemblyReloadEvents.beforeAssemblyReload += () => {
                _playerGUI.OnDisable();
            };
        }

        public void OnEnableIfNeeded() {
            if (_enabled) return;
            _playerGUI.Resume();
            _lastOnGUICalledTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += Update;
            _enabled = true;
        }

        void Update() {
            if (EditorApplication.timeSinceStartup - _lastOnGUICalledTime > 0.05f) {
                ReserveOnDisable();
            }
        }

        // PropertyDrawerの破棄を検知するためにDrawGUIが一定時間呼ばれなかったかどうかを判定基準にしているが
        // GenericMenu使用時にも一定時間DrawGUIが呼ばれなくなる。
        // そのケースを除外するため、OnDisableを呼ぶまで猶予期間を設けてそれまでにDrawGUIが呼ばれたらOnDisableの呼び出しを取り消す
        void ReserveOnDisable() {
            EditorApplication.update -= Update;
            _onDisableReserveTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += UpdateOnReservedOnDisable;
            _disableReserved = true;
        }

        void CancelReserveOnDisableIfNeeded() {
            if (_disableReserved == false) return;
            EditorApplication.update -= UpdateOnReservedOnDisable;
            _disableReserved = false;
            // Updateに差し戻し
            _lastOnGUICalledTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += Update;
        }

        void UpdateOnReservedOnDisable() {
            if (EditorApplication.timeSinceStartup - _onDisableReserveTime > 0.05f) {
                OnDisable();
                EditorApplication.update -= UpdateOnReservedOnDisable;
            }
        }

        void OnDisable() {
            _playerGUI.Pause();
            _enabled = false;
        }

        public void DrawGUI(Rect rect) {
            CancelReserveOnDisableIfNeeded();
            OnEnableIfNeeded();
            _playerGUI.DrawGUI(rect);
            _lastOnGUICalledTime = EditorApplication.timeSinceStartup;
        }

        public void Bind(Sound sound) {
            _playerGUI.Bind(sound);
        }

        public void ReapplyParameters() {
            _playerGUI.ReapplyParameters();
        }
    }
}