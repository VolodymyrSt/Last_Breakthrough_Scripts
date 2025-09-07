using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace LastBreakthrought.Infrustructure.Services.Massage
{
    public class MassageView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private TextMeshProUGUI _massage;

        private bool _isHidden;

        private void Start() => HideAtStart();

        public void Show(string massage)
        {
            if (!_isHidden) return;

            var duration = 0.2f;
            _massage.text = massage;
            _root.gameObject.SetActive(true);
            _root.DOScale(1f, duration)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => {
                    _isHidden = false;
                    StartCoroutine(Hide());
                });
        }

        public IEnumerator Hide()
        {
            yield return new WaitForSecondsRealtime(Constants.TIME_AFTER_HIDE_MASSAGE);

            var duration = 0.2f;
            _root.gameObject.SetActive(true);
            _root.DOScale(0f, duration)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => _isHidden = true);
        }

        private void HideAtStart()
        {
            _root.transform.localScale = Vector3.zero;
            _root.gameObject.SetActive(false);
            _isHidden = true;
        }
    }
}
