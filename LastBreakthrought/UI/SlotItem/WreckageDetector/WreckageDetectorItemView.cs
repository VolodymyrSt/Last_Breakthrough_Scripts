using DG.Tweening;
using TMPro;
using UnityEngine;

namespace LastBreakthrought.UI.SlotItem.WreckageDetector
{
    public class WreckageDetectorItemView : MonoBehaviour 
    {
        private const float DURATION = 0.2f;
        private const float SCALED_VALUE = 1f;
        private const float UNSCALED_VALUE = 0f;

        [SerializeField] private RectTransform _viewRoot;
        [SerializeField] private TextMeshProUGUI _distanceText;

        public void SetDistanceUI(int distance) => 
            _distanceText.text = distance.ToString() + "m";

        public void Show()
        {
            _viewRoot.gameObject.SetActive(true);

            _viewRoot.DOScale(SCALED_VALUE, DURATION)
                .SetEase(Ease.InOutCubic)
                .Play();
        }

        public void Hide()
        {
            _viewRoot.DOScale(UNSCALED_VALUE, 0.2f)
                .SetEase(Ease.InOutCubic)
                .Play()
                .OnComplete(() => _viewRoot.gameObject.SetActive(false));
        }
    }
}