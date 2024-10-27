using System;
using UnityEngine;

namespace SoundSystem {
    public partial class SoundManager {
        public SoundPlayerGroup BgmGroup => FindPlayerGroup("Bgm");
        public SoundPlayer GetUnusedBgmPlayer() => BgmGroup.GetUnusedPlayer();

        public SoundPlayer PlayBgm(AudioClip audioClip, Action onComplete = null) {
            return BgmGroup.Play(audioClip, onComplete);
        }

        public SoundPlayer PlayBgm(CustomClip customClip, Action onComplete = null) {
            return BgmGroup.Play(customClip, onComplete);
        }

        public SoundPlayer PlayBgm(Sound sound, Action onComplete = null) {
            return BgmGroup.Play(sound, onComplete);
        }

        public SoundPlayer PlayBgm(string soundKey, Action onComplete = null) {
            return BgmGroup.Play(soundKey, onComplete);
        }

        public SoundPlayer PlayBgmIfNotPlaying(AudioClip audioClip, Action onComplete = null) {
            return BgmGroup.PlayIfNotPlaying(audioClip, onComplete);
        }

        public SoundPlayer PlayBgmIfNotPlaying(CustomClip customClip, Action onComplete = null) {
            return BgmGroup.PlayIfNotPlaying(customClip, onComplete);
        }

        public SoundPlayer PlayBgmIfNotPlaying(Sound sound, Action onComplete = null) {
            return BgmGroup.PlayIfNotPlaying(sound, onComplete);
        }

        public SoundPlayer PlayBgmIfNotPlaying(string soundKey, Action onComplete = null) {
            return BgmGroup.PlayIfNotPlaying(soundKey, onComplete);
        }

        public SoundPlayer PlayBgmAsRestart(AudioClip audioClip, Action onComplete = null) {
            return BgmGroup.PlayAsRestart(audioClip, onComplete);
        }

        public SoundPlayer PlayBgmAsRestart(CustomClip customClip, Action onComplete = null) {
            return BgmGroup.PlayAsRestart(customClip, onComplete);
        }

        public SoundPlayer PlayBgmAsRestart(Sound sound, Action onComplete = null) {
            return BgmGroup.PlayAsRestart(sound, onComplete);
        }

        public SoundPlayer PlayBgmAsRestart(string soundKey, Action onComplete = null) {
            return BgmGroup.PlayAsRestart(soundKey, onComplete);
        }

        public SoundPlayer PlayBgmWithFadeIn(AudioClip audioClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.PlayWithFadeIn(audioClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayBgmWithFadeIn(CustomClip customClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.PlayWithFadeIn(customClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayBgmWithFadeIn(Sound sound, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayBgmWithFadeIn(string soundKey, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public void StopBgm() {
            BgmGroup.Stop();
        }

        public void StopBgm(AudioClip audioClip) {
            BgmGroup.Stop(audioClip);
        }

        public void StopBgm(CustomClip customClip) {
            BgmGroup.Stop(customClip);
        }

        public void StopBgm(Sound sound) {
            BgmGroup.Stop(sound);
        }

        public void StopBgm(string soundKey) {
            BgmGroup.Stop(soundKey);
        }

        public void StopBgmWithFadeOut(float? fadeDuration = null, Action onComplete = null) {
            BgmGroup.StopWithFadeOut(fadeDuration, onComplete);
        }

        public SoundPlayer SwitchBgm(AudioClip audioClip, Action onComplete = null) {
            return BgmGroup.Switch(audioClip, onComplete);
        }

        public SoundPlayer SwitchBgm(CustomClip customClip, Action onComplete = null) {
            return BgmGroup.Switch(customClip, onComplete);
        }

        public SoundPlayer SwitchBgm(Sound sound, Action onComplete = null) {
            return BgmGroup.Switch(sound, onComplete);
        }

        public SoundPlayer SwitchBgm(string soundKey, Action onComplete = null) {
            return BgmGroup.Switch(soundKey, onComplete);
        }

        public SoundPlayer CrossFadeBgm(AudioClip audioClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.CrossFade(audioClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeBgm(CustomClip customClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.CrossFade(customClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeBgm(Sound sound, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.CrossFade(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeBgm(string soundKey, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return BgmGroup.CrossFade(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer FindPlayingBgmPlayer(AudioClip audioClip) {
            return BgmGroup.FindPlayingPlayer(audioClip);
        }

        public SoundPlayer FindPlayingBgmPlayer(CustomClip customClip) {
            return BgmGroup.FindPlayingPlayer(customClip);
        }

        public SoundPlayer FindPlayingBgmPlayer(Sound sound) {
            return BgmGroup.FindPlayingPlayer(sound);
        }

        public SoundPlayer FindPlayingBgmPlayer(string soundKey) {
            return BgmGroup.FindPlayingPlayer(soundKey);
        }

        public SoundPlayerGroup SeGroup => FindPlayerGroup("Se");
        public SoundPlayer GetUnusedSePlayer() => SeGroup.GetUnusedPlayer();

        public SoundPlayer PlaySe(AudioClip audioClip, Action onComplete = null) {
            return SeGroup.Play(audioClip, onComplete);
        }

        public SoundPlayer PlaySe(CustomClip customClip, Action onComplete = null) {
            return SeGroup.Play(customClip, onComplete);
        }

        public SoundPlayer PlaySe(Sound sound, Action onComplete = null) {
            return SeGroup.Play(sound, onComplete);
        }

        public SoundPlayer PlaySe(string soundKey, Action onComplete = null) {
            return SeGroup.Play(soundKey, onComplete);
        }

        public SoundPlayer PlaySeIfNotPlaying(AudioClip audioClip, Action onComplete = null) {
            return SeGroup.PlayIfNotPlaying(audioClip, onComplete);
        }

        public SoundPlayer PlaySeIfNotPlaying(CustomClip customClip, Action onComplete = null) {
            return SeGroup.PlayIfNotPlaying(customClip, onComplete);
        }

        public SoundPlayer PlaySeIfNotPlaying(Sound sound, Action onComplete = null) {
            return SeGroup.PlayIfNotPlaying(sound, onComplete);
        }

        public SoundPlayer PlaySeIfNotPlaying(string soundKey, Action onComplete = null) {
            return SeGroup.PlayIfNotPlaying(soundKey, onComplete);
        }

        public SoundPlayer PlaySeAsRestart(AudioClip audioClip, Action onComplete = null) {
            return SeGroup.PlayAsRestart(audioClip, onComplete);
        }

        public SoundPlayer PlaySeAsRestart(CustomClip customClip, Action onComplete = null) {
            return SeGroup.PlayAsRestart(customClip, onComplete);
        }

        public SoundPlayer PlaySeAsRestart(Sound sound, Action onComplete = null) {
            return SeGroup.PlayAsRestart(sound, onComplete);
        }

        public SoundPlayer PlaySeAsRestart(string soundKey, Action onComplete = null) {
            return SeGroup.PlayAsRestart(soundKey, onComplete);
        }

        public SoundPlayer PlaySeWithFadeIn(AudioClip audioClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.PlayWithFadeIn(audioClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlaySeWithFadeIn(CustomClip customClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.PlayWithFadeIn(customClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlaySeWithFadeIn(Sound sound, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlaySeWithFadeIn(string soundKey, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public void StopSe() {
            SeGroup.Stop();
        }

        public void StopSe(AudioClip audioClip) {
            SeGroup.Stop(audioClip);
        }

        public void StopSe(CustomClip customClip) {
            SeGroup.Stop(customClip);
        }

        public void StopSe(Sound sound) {
            SeGroup.Stop(sound);
        }

        public void StopSe(string soundKey) {
            SeGroup.Stop(soundKey);
        }

        public void StopSeWithFadeOut(float? fadeDuration = null, Action onComplete = null) {
            SeGroup.StopWithFadeOut(fadeDuration, onComplete);
        }

        public SoundPlayer SwitchSe(AudioClip audioClip, Action onComplete = null) {
            return SeGroup.Switch(audioClip, onComplete);
        }

        public SoundPlayer SwitchSe(CustomClip customClip, Action onComplete = null) {
            return SeGroup.Switch(customClip, onComplete);
        }

        public SoundPlayer SwitchSe(Sound sound, Action onComplete = null) {
            return SeGroup.Switch(sound, onComplete);
        }

        public SoundPlayer SwitchSe(string soundKey, Action onComplete = null) {
            return SeGroup.Switch(soundKey, onComplete);
        }

        public SoundPlayer CrossFadeSe(AudioClip audioClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.CrossFade(audioClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeSe(CustomClip customClip, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.CrossFade(customClip, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeSe(Sound sound, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.CrossFade(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFadeSe(string soundKey, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return SeGroup.CrossFade(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer FindPlayingSePlayer(AudioClip audioClip) {
            return SeGroup.FindPlayingPlayer(audioClip);
        }

        public SoundPlayer FindPlayingSePlayer(CustomClip customClip) {
            return SeGroup.FindPlayingPlayer(customClip);
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