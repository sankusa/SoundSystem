using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem {
    public class PopupFromScriptableObject<T> where T : ScriptableObject {
        Func<IEnumerable<T>, IEnumerable<string>> _optionCreateFunc;

        public PopupFromScriptableObject(Func<IEnumerable<T>, IEnumerable<string>> optionCreateFunc) {
            _optionCreateFunc = optionCreateFunc;

            ScriptableObjectProvider.StartCache<T>();
        }

        ~PopupFromScriptableObject() {
            ScriptableObjectProvider.EndCache<T>();
        }

        public string Draw(Rect rect, string text) {
            return Draw(rect, null, text);
        }
        public string Draw(Rect rect, string label, string text) {
            return GUIUtil.TextPopup(rect, label, text, _optionCreateFunc(ScriptableObjectProvider.GetCache<T>()).ToArray());
        }

        public string DrawLayout(string text) {
            return DrawLayout(null, text);
        }
        public string DrawLayout(string label, string text) {
            return GUIUtil.LayoutTextPopup(label, text, _optionCreateFunc(ScriptableObjectProvider.GetCache<T>()).ToArray());
        }
    }
}