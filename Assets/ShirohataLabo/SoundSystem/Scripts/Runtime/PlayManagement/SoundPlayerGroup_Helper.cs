using UnityEngine.Events;

namespace SoundSystem {
    public partial class SoundPlayerGroup {
        public SoundPlayer Play(AudioUnit audioUnit, UnityAction onComplete = null) {
            return GetUnusedPlayer().Play(audioUnit, onComplete);
        }

        public SoundPlayer Play(Sound sound, UnityAction onComplete = null) {
            return GetUnusedPlayer().Play(sound, onComplete);
        }

        public SoundPlayer Play(string soundKey, UnityAction onComplete = null) {
            return GetUnusedPlayer().Play(soundKey, onComplete);
        }

        public SoundPlayer PlayWithFadeIn(AudioUnit audioUnit, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return GetUnusedPlayer().PlayWithFadeIn(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayWithFadeIn(Sound sound, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return GetUnusedPlayer().PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer PlayWithFadeIn(string soundKey, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            return GetUnusedPlayer().PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }

        public void Stop() {
            foreach (SoundPlayer player in _players) {
                player.Stop();
            }
        }

        public void StopWithFadeOut(float? fadeDuration = null, UnityAction onComplete = null) {
            bool isInvoked = false;
            UnityAction action = null;
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

        public SoundPlayer Switch(AudioUnit audioUnit, UnityAction onComplete = null) {
            Stop();
            return Play(audioUnit, onComplete);
        }

        public SoundPlayer Switch(Sound sound, UnityAction onComplete = null) {
            Stop();
            return Play(sound, onComplete);
        }

        public SoundPlayer Switch(string soundKey, UnityAction onComplete = null) {
            Stop();
            return Play(soundKey, onComplete);
        }

        public SoundPlayer CrossFade(AudioUnit audioUnit, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            StopWithFadeOut(fadeDuration);
            return PlayWithFadeIn(audioUnit, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFade(Sound sound, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            StopWithFadeOut(fadeDuration);
            return PlayWithFadeIn(sound, fadeDuration, onComplete, onFadeComplete);
        }

        public SoundPlayer CrossFade(string soundKey, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            StopWithFadeOut(fadeDuration);
            return PlayWithFadeIn(soundKey, fadeDuration, onComplete, onFadeComplete);
        }
    }
}