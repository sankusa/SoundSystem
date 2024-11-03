namespace SoundSystem {
    public static class AudioClipUtil {
        public static void ImportDraggingExternalSoundFiles(string folderPath) {
            DragAndDropUtil.CopyDraggingExternalFiles(folderPath, new string[] {".mp3", ".ogg", ".wav", ".aiff", ".aif", ".mod", ".it", ".s3m", ".xm"});
        }
    }
}