using UnityEngine.Events;

namespace SoundSystem {
    public partial class SoundPlayer {
        public SoundPlayer Play(AudioUnit audioUnit, UnityAction onComplete = null) {
            SetAudioUnit(audioUnit);
            AddOnComplete(onComplete);
            return Play();
        }

        public SoundPlayer Play(Sound sound, UnityAction onComplete = null) {
            SetSound(sound);
            AddOnComplete(onComplete);
            return Play();
        }

        public SoundPlayer Play(string soundKey, UnityAction onComplete = null) {
            SetSoundByKey(soundKey);
            AddOnComplete(onComplete);
            return Play();
        }

        public SoundPlayer PlayWithFadeIn(AudioUnit audioUnit, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            SetAudioUnit(audioUnit);
            SetFadeIn(fadeDuration, onFadeComplete);
            AddOnComplete(onComplete);
            return Play();
        }

        public SoundPlayer PlayWithFadeIn(Sound sound, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            SetSound(sound);
            SetFadeIn(fadeDuration, onFadeComplete);
            AddOnComplete(onComplete);
            return Play();
        }

        public SoundPlayer PlayWithFadeIn(string soundKey, float? fadeDuration = null, UnityAction onComplete = null, UnityAction onFadeComplete = null) {
            SetSoundByKey(soundKey);
            SetFadeIn(fadeDuration, onFadeComplete);
            AddOnComplete(onComplete);
            return Play();
        }

        public void StopWithFadeOut(float? fadeDuration = null, UnityAction onComplete = null) {
            if (onComplete == null) {
                SetFadeOut(fadeDuration, Stop);
            }
            else {
                SetFadeOut(fadeDuration, () => {
                    Stop();
                    onComplete.Invoke();
                });
            }
        }
    }
}