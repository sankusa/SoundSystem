using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SoundSystem {
    [AddComponentMenu(nameof(SoundSystem) + "/" + nameof(VolumeSlider))]
    public class VolumeSlider : MonoBehaviour {
        [SerializeField] Slider _slider;
        [SerializeField] TMP_Text _text;
        [SerializeField, VolumeKey] string _volumeKey;
        [SerializeField] UnityEvent<float> _onSliderValueChangedWithoutOnAwake;

        void Awake() {
            // Slider.onValueChangedが発火してしまうのでイベント登録前にスライダーの値を更新しておく
            UpdateUI();

            _slider.onValueChanged.AddListener(_onSliderValueChangedWithoutOnAwake.Invoke);
            _onSliderValueChangedWithoutOnAwake.AddListener(OnSliderValueChanged);
        }

        void Update() {
            UpdateUI();
        }

        void UpdateUI() {
            _slider.value = SoundManager.Instance.FindVolume(_volumeKey).Value * _slider.maxValue;
            _text?.SetText(_slider.wholeNumbers ? _slider.value.ToString() : _slider.value.ToString("0.0"));
        }

        void OnSliderValueChanged(float value) {
            SoundManager.Instance.FindVolume(_volumeKey).Value = value / _slider.maxValue;
        }
    }
}