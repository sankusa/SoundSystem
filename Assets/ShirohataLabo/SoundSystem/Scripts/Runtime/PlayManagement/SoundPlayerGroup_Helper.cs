using System;
using UnityEngine.Events;

namespace SoundSystem {
    public partial class SoundPlayerGroup {
        public SoundPlayer Play(AudioUnit audioUnit, Action onComplete = null) {
            return GetUnusedPlayer().Play(audioUnit, onComplete);
        }

        public SoundPlayer Play(Sound sound, Action onComplete = null) {
            return GetUnusedPlayer().Play(sound, onComplete);
        }

        public SoundPlayer Play(string soundKey, Action onComplete = null) {
            return GetUnusedPlayer().Play(soundKey, onComplete);
        }

        public SoundPlayer PlayIfNotPlaying(AudioUnit audioUnit, Action onComplete = null) {
            if (FindPlayingPlayer(audioUnit) != null) return null;
            return Play(audioUnit, onComplete);
        }

        public SoundPlayer PlayIfNotPlaying(Sound sound, Action onComplete = null) {
            if (FindPlayingPlayer(sound) != null) return null;
            return Play(sound, onComplete);
        }

        public SoundPlayer PlayIfNotPlaying(string soundKey, Action onComplete = null) {
            if (FindPlayingPlayer(soundKey) != null) return null;
            return Play(soundKey, onComplete);
        }

        public SoundPlayer PlayAsRestart(AudioUnit audioUnit, Action onComplete = null) {
            Stop(audioUnit);
            return Play(audioUnit);
        }

        public SoundPlayer PlayAsRestart(Sound sound, Action onComplete = null) {
            Stop(sound);
            return Play(sound);
        }

        public SoundPlayer PlayAsRestart(string soundKey, Action onComplete = null) {
            Stop(soundKey);
            return Play(soundKey);
        }

        public SoundPlayer PlayWithFadeIn(AudioUnit audioUnit, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return GetUnusedPlayer().PlayWithFadeIn(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayWithFadeIn(Sound sound, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return GetUnusedPlayer().PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayWithFadeIn(string soundKey, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            return GetUnusedPlayer().PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public void Stop() {
            foreach (SoundPlayer player in _players) {
                player.Stop();
            }
        }

        public void Stop(AudioUnit audioUnit) {
            if (audioUnit == null) return;
            foreach (SoundPlayer player in _players) {
                if (player.AudioUnit == audioUnit) player.Stop();
            }
        }

        public void Stop(Sound sound) {
            if (sound == null) return;
            foreach (SoundPlayer player in _players) {
                if (player.Sound == sound) player.Stop();
            }
        }

        public void Stop(string soundKey) {
            Sound sound = SoundCache.Instance.FindSound(soundKey);
            Stop(sound);
        }

        public void StopWithFadeOut(float? fadeDuration = null, Action onComplete = null) {
            bool isInvoked = false;
            Action action = null;
            if (onComplete != null) {
                action = () => {
                    if (isInvoked) return;
                    onComplete.Invoke();
                    isInvoked = true;
                };
            }

            int targetCount = 0;

            foreach (SoundPlayer player in _players) {
                if (player.IsUsing) {
                    player.StopWithFadeOut(fadeDuration, action);
                    targetCount++;
                }
            }

            if (targetCount == 0) {
                action?.Invoke();
            }
        }

        public SoundPlayer Switch(AudioUnit audioUnit, Action onComplete = null) {
            Stop();
            return Play(audioUnit, onComplete);
        }

        public SoundPlayer Switch(Sound sound, Action onComplete = null) {
            Stop();
            return Play(sound, onComplete);
        }

        public SoundPlayer Switch(string soundKey, Action onComplete = null) {
            Stop();
            return Play(soundKey, onComplete);
        }

        public SoundPlayer CrossFade(AudioUnit audioUnit, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            StopWithFadeOut(fadeDuration);
            return PlayWithFadeIn(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFade(Sound sound, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            StopWithFadeOut(fadeDuration);
            return PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFade(string soundKey, float? fadeDuration = null, Action onComplete = null, Action onFadeComplete = null) {
            StopWithFadeOut(fadeDuration);
            return PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer FindPlayingPlayer(AudioUnit audioUnit) {
            if (audioUnit == null) return null;
            foreach (SoundPlayer player in _players) {
                if (player.AudioUnit == audioUnit) return player;
            }
            return null;
        }

        public SoundPlayer FindPlayingPlayer(Sound sound) {
            if (sound == null) return null;
            foreach (SoundPlayer player in _players) {
                if (player.Sound == sound) return player;
            }
            return null;
        }

        public SoundPlayer FindPlayingPlayer(string soundKey) {
            Sound sound = SoundCache.Instance.FindSound(soundKey);
            return FindPlayingPlayer(sound);
        }
    }
}