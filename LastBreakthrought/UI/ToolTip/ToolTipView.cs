using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.ToolTip
{
    public class ToolTipView : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private LayoutElement _layoutElement;

        public bool IsVisible => _root.gameObject.activeSelf;

        public void Init() => HideAtStart();

        private void Update()
        {
            var headerTextLength = _headerText.text.Length;
            var contentTextLength = _contentText.text.Length;
            _layoutElement.enabled = headerTextLength > Constants.CHARACTER_WRAP_LIMIT || 
                contentTextLength  > Constants.CHARACTER_WRAP_LIMIT ? true : false;
        }

        public void Show(string headerText, string contentText)
        {
            _headerText.text = headerText;
            _contentText.text = contentText;

            _root.gameObject.SetActive(true);

            _root.DOKill();
            _root.DOScale(1f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.OutQuad)
                .Play();
        }

        public void Hide()
        {
            _root.DOKill();
            _root.DOScale(0f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.InQuad)
                .Play()
                .OnComplete(() => _root.gameObject.SetActive(false));
        }

        public void SetLocalPosition(Vector3 localPosition) => 
            _root.localPosition = localPosition;

        private void HideAtStart()
        {
            _root.localScale = Vector3.zero;
            _root.gameObject.SetActive(false);
        }
    }
}
