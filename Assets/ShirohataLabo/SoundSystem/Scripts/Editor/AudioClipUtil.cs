using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public static class AudioClipUtil {
        public static void ImportDraggingExternalSoundFiles(string folderPath) {
            DragAndDropUtil.CopyDraggingExternalFiles(folderPath, new string[] {".wav", ".mp3", ".ogg"});
        }
    }
}