using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace SoundSystem {
    public static class ScriptGenerator {
        public static void GenerateSoundManagerHelperScript(SoundManagerConfig soundManagerConfig) {
            ScriptGeneratorSetting setting = EditorUtil.LoadAllAsset<ScriptGeneratorSetting>().First();
            string targetFolder = setting.TargetFolderPath;
            string script = setting.SoundManagerHelperScriptFrame;
            string scriptBody = string.Empty;
            foreach (SoundPlayerGroupSetting groupSetting in soundManagerConfig.PlayerGroupSettings) {
                scriptBody += setting.SoundManagerHelperScriptBodyByGroup.Replace("#GROUPKEY#", groupSetting.Key);
            }
            foreach (VolumeSetting volumeSetting in soundManagerConfig.VolumeSettings) {
                scriptBody += setting.SoundManagerHelperScriptBodyByVolume.Replace("#VOLUMEKEY#", volumeSetting.Key);
            }
            script = script.Replace("#BODY#", scriptBody);

            string filePath = targetFolder + "/" + setting.SoundManagerHelperScriptName + ".cs";
            SaveScriptFile(filePath, script);
        }

        public static void GenerateSoundKeyScript() {
            ScriptGeneratorSetting setting = EditorUtil.LoadAllAsset<ScriptGeneratorSetting>().First();
            IEnumerable<SoundContainer> soundContainers = EditorUtil.LoadAllAsset<SoundContainer>();
            string targetFolder = setting.TargetFolderPath;
            string script = setting.SoundKeyScriptFrame;
            string scriptBody = string.Empty;
            foreach (string soundKey in soundContainers.SelectMany(x => x.SoundDic.Keys).Distinct().OrderBy(x => x)) {
                scriptBody += setting.SoundKeyScriptBodyByKey.Replace("#SOUNDKEY#", soundKey);
            }
            script = script.Replace("#BODY#", scriptBody);

            string filePath = targetFolder + "/" + setting.SoundKeyScriptName + ".cs";
            SaveScriptFile(filePath, script);
        }

        static void SaveScriptFile(string filePath, string script) {
            File.WriteAllText(filePath, script);
            AssetDatabase.Refresh();
        }
    }
}