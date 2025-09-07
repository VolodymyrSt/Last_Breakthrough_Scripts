using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.PlayerStats
{
    public class PlayerStatsView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _oxygenSlider;
        [SerializeField] private RectTransform _warningSign;

        private bool _isWarningSighHidden;

        private void OnEnable() => HideWarningSigh();

        public void SetHealthSliderValue(float value) => _healthSlider.value = value;
        public void SetOxygeSliderValue(float value) => _oxygenSlider.value = value;

        public void SetHealthSliderMaxValueAndStartedValue(float maxValue, float startedValue)
        {
            _healthSlider.maxValue = maxValue;
            _healthSlider.value = startedValue;
        }

        public void SetOxygeSliderMaxValueAndStartedValue(float maxValue, float startedValue)
        {
            _oxygenSlider.maxValue = maxValue;
            _oxygenSlider.value = startedValue;
        }

        public void ShowWarningSigh()
        {
            if (!_isWarningSighHidden) return;

            _warningSign.gameObject.SetActive(true);
            _isWarningSighHidden = false;

            _warningSign.DOScale(1.3f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .Play();
        }

        public void HideWarningSigh()
        {
            if (_isWarningSighHidden) return;

            _warningSign.DOKill();
            _warningSign.gameObject.SetActive(false);
            _isWarningSighHidden = true;
        }
    }
}

