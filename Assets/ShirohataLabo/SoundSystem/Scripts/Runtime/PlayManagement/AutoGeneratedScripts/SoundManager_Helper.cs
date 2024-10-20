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

        public SoundPlayer PlayBgmIfNotPlaying(AudioUnit audioUnit, UnityAction onComplete = null) {
            return BgmGroup.PlayIfNotPlaying(audioUnit, onComplete);
        }

        public SoundPlayer PlayBgmIfNotPlaying(Sound sound, UnityAction onComplete = null) {
            return BgmGroup.PlayIfNotPlaying(sound, onComplete);
        }

        public SoundPlayer PlayBgmIfNotPlaying(string soundKey, UnityAction onComplete = null) {
            return BgmGroup.PlayIfNotPlaying(soundKey, onComplete);
        }

        public SoundPlayer PlayBgmAsRestart(AudioUnit audioUnit, UnityAction onComplete = null) {
            return BgmGroup.PlayAsRestart(audioUnit, onComplete);
        }

        public SoundPlayer PlayBgmAsRestart(Sound sound, UnityAction onComplete = null) {
            return BgmGroup.PlayAsRestart(sound, onComplete);
        }

        public SoundPlayer PlayBgmAsRestart(string soundKey, UnityAction onComplete = null) {
            return BgmGroup.PlayAsRestart(soundKey, onComplete);
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

        public void StopBgm(AudioUnit audioUnit) {
            BgmGroup.Stop(audioUnit);
        }

        public void StopBgm(Sound sound) {
            BgmGroup.Stop(sound);
        }

        public void StopBgm(string soundKey) {
            BgmGroup.Stop(soundKey);
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

        public SoundPlayer FindPlayingBgmPlayer(AudioUnit audioUnit) {
            return BgmGroup.FindPlayingPlayer(audioUnit);
        }

        public SoundPlayer FindPlayingBgmPlayer(Sound sound) {
            return BgmGroup.FindPlayingPlayer(sound);
        }

        public SoundPlayer FindPlayingBgmPlayer(string soundKey) {
            return BgmGroup.FindPlayingPlayer(soundKey);
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

        public SoundPlayer PlaySeIfNotPlaying(AudioUnit audioUnit, UnityAction onComplete = null) {
            return SeGroup.PlayIfNotPlaying(audioUnit, onComplete);
        }

        public SoundPlayer PlaySeIfNotPlaying(Sound sound, UnityAction onComplete = null) {
            return SeGroup.PlayIfNotPlaying(sound, onComplete);
        }

        public SoundPlayer PlaySeIfNotPlaying(string soundKey, UnityAction onComplete = null) {
            return SeGroup.PlayIfNotPlaying(soundKey, onComplete);
        }

        public SoundPlayer PlaySeAsRestart(AudioUnit audioUnit, UnityAction onComplete = null) {
            return SeGroup.PlayAsRestart(audioUnit, onComplete);
        }

        public SoundPlayer PlaySeAsRestart(Sound sound, UnityAction onComplete = null) {
            return SeGroup.PlayAsRestart(sound, onComplete);
        }

        public SoundPlayer PlaySeAsRestart(string soundKey, UnityAction onComplete = null) {
            return SeGroup.PlayAsRestart(soundKey, onComplete);
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

        public void StopSe(AudioUnit audioUnit) {
            SeGroup.Stop(audioUnit);
        }

        public void StopSe(Sound sound) {
            SeGroup.Stop(sound);
        }

        public void StopSe(string soundKey) {
            SeGroup.Stop(soundKey);
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

        public SoundPlayer FindPlayingSePlayer(AudioUnit audioUnit) {
            return SeGroup.FindPlayingPlayer(audioUnit);
        }

        public SoundPlayer FindPlayingSePlayer(Sound sound) {
            return SeGroup.FindPlayingPlayer(sound);
        }

        public SoundPlayer FindPlayingSePlayer(string soundKey) {
            return SeGroup.FindPlayingPlayer(soundKey);
        }

        public Volume MasterVolume => FindVolume("Master");

        public Volume BgmVolume => FindVolume("Bgm");

        public Volume SeVolume => FindVolume("Se");


    }
}