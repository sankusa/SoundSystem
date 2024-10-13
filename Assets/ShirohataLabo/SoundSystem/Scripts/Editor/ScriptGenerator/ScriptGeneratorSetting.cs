using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(ScriptGeneratorSetting), menuName = nameof(SoundSystem) + "/Develop/" + nameof(ScriptGeneratorSetting))]
    public class ScriptGeneratorSetting : ScriptableObject {
        [SerializeField] DefaultAsset _targetFolder;
        public string TargetFolderPath => AssetDatabase.GetAssetPath(_targetFolder);

        [SerializeField] TextAsset _soundManagerHelperScriptFrame;
        public string SoundManagerHelperScriptFrame => _soundManagerHelperScriptFrame.text;

        [SerializeField] TextAsset _soundManagerHelperScriptBodyByGroup;
        public string SoundManagerHelperScriptBodyByGroup => _soundManagerHelperScriptBodyByGroup.text;

        [SerializeField] TextAsset _soundManagerHelperScriptBodyVolume;
        public string SoundManagerHelperScriptBodyByVolume => _soundManagerHelperScriptBodyVolume.text;

        [SerializeField] string _soundManagerHelperScriptName;
        public string SoundManagerHelperScriptName => _soundManagerHelperScriptName;


        [SerializeField] TextAsset _soundKeyScriptFrame;
        public string SoundKeyScriptFrame => _soundKeyScriptFrame.text;

        [SerializeField] TextAsset _soundKeyScriptBodyByKey;
        public string SoundKeyScriptBodyByKey => _soundKeyScriptBodyByKey.text;

        [SerializeField] string _soundKeyScriptName;
        public string SoundKeyScriptName => _soundKeyScriptName;


        [SerializeField] TextAsset _soundPlayerGroupKeyScriptFrame;
        public string SoundPlayerGroupKeyScriptFrame => _soundPlayerGroupKeyScriptFrame.text;

        [SerializeField] TextAsset _soundPlayerGroupKeyScriptBodyByKey;
        public string SoundPlayerGroupKeyScriptBodyByKey => _soundPlayerGroupKeyScriptBodyByKey.text;

        [SerializeField] string _soundPlayerGroupKeyScriptName;
        public string SoundPlayerGroupKeyScriptName => _soundPlayerGroupKeyScriptName;
    }
}