using UnityEngine.Events;

namespace SoundSystem {
    public partial class SoundManager {
        public SoundPlayerGroup BgmGroup => FindPlayerGroup("Bgm");
        public SoundPlayer GetUnusedBgmPlayer() => BgmGroup.GetUnusedPlayer();

        public SoundPlayer PlayBgm(AudioUnit audioUnit, UnityAction onComplete = null) {
            return BgmGroup.Play(audioUnit, onComplete);
        }

        public SoundPlayer PlayBgm(Sound sound, UnityAction onComplete = null) {
            return BgmGroup.Play(sound, onComplete);
        }

        public SoundPlayer PlayBgm(string soundKey, UnityAction onComplete = null) {
            return BgmGroup.Play(soundKey, onComplete);
        }

        public SoundPlayer PlayBgmWithFadeIn(AudioUnit audioUnit, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return BgmGroup.PlayWithFadeIn(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayBgmWithFadeIn(Sound sound, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return BgmGroup.PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayBgmWithFadeIn(string soundKey, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return BgmGroup.PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public void StopBgm() {
            BgmGroup.Stop();
        }

        public void StopBgmWithFadeOut(float? fadeDuration = null, UnityAction onComplete = null) {
            BgmGroup.StopWithFadeOut(fadeDuration, onComplete);
        }

        public SoundPlayer SwitchBgm(AudioUnit audioUnit, UnityAction onComplete = null) {
            return BgmGroup.Switch(audioUnit, onComplete);
        }

        public SoundPlayer SwitchBgm(Sound sound, UnityAction onComplete = null) {
            return BgmGroup.Switch(sound, onComplete);
        }

        public SoundPlayer SwitchBgm(string soundKey, UnityAction onComplete = null) {
            return BgmGroup.Switch(soundKey, onComplete);
        }

        public SoundPlayer CrossFadeBgm(AudioUnit audioUnit, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return BgmGroup.CrossFade(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeBgm(Sound sound, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return BgmGroup.CrossFade(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeBgm(string soundKey, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return BgmGroup.CrossFade(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayerGroup SeGroup => FindPlayerGroup("Se");
        public SoundPlayer GetUnusedSePlayer() => SeGroup.GetUnusedPlayer();

        public SoundPlayer PlaySe(AudioUnit audioUnit, UnityAction onComplete = null) {
            return SeGroup.Play(audioUnit, onComplete);
        }

        public SoundPlayer PlaySe(Sound sound, UnityAction onComplete = null) {
            return SeGroup.Play(sound, onComplete);
        }

        public SoundPlayer PlaySe(string soundKey, UnityAction onComplete = null) {
            return SeGroup.Play(soundKey, onComplete);
        }

        public SoundPlayer PlaySeWithFadeIn(AudioUnit audioUnit, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return SeGroup.PlayWithFadeIn(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlaySeWithFadeIn(Sound sound, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return SeGroup.PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlaySeWithFadeIn(string soundKey, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return SeGroup.PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public void StopSe() {
            SeGroup.Stop();
        }

        public void StopSeWithFadeOut(float? fadeDuration = null, UnityAction onComplete = null) {
            SeGroup.StopWithFadeOut(fadeDuration, onComplete);
        }

        public SoundPlayer SwitchSe(AudioUnit audioUnit, UnityAction onComplete = null) {
            return SeGroup.Switch(audioUnit, onComplete);
        }

        public SoundPlayer SwitchSe(Sound sound, UnityAction onComplete = null) {
            return SeGroup.Switch(sound, onComplete);
        }

        public SoundPlayer SwitchSe(string soundKey, UnityAction onComplete = null) {
            return SeGroup.Switch(soundKey, onComplete);
        }

        public SoundPlayer CrossFadeSe(AudioUnit audioUnit, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return SeGroup.CrossFade(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeSe(Sound sound, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return SeGroup.CrossFade(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeSe(string soundKey, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return SeGroup.CrossFade(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public Volume MasterVolume => FindVolume("Master");

        public Volume BgmVolume => FindVolume("Bgm");

        public Volume SeVolume => FindVolume("Se");


    }
}