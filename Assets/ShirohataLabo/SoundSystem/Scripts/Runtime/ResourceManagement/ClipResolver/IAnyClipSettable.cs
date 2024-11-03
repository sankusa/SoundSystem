using UnityEngine;

namespace SoundSystem {
    public interface IAnyClipSettable {
        abstract void SetAudioClip(AudioClip audioClip);
        abstract void SetCustomClip(CustomClip customClip);
    }
}