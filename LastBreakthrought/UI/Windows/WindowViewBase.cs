using DG.Tweening;
using UnityEngine;

namespace LastBreakthrought.UI.Windows
{
    public abstract class WindowViewBase : MonoBehaviour
    {
        private bool _isHidden;

        private void Start()
        {
            Initialize();
            Hide();
        }

        private void OnDestroy() => Dispose();

        public abstract void Initialize();
        public abstract void Dispose();

        public virtual void ShowView()
        {
            if (!_isHidden) return;

            gameObject.SetActive(true);
            transform.DOScale(1f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => _isHidden = false);
        }

        public virtual void HideView()
        {
            if (_isHidden) return;

            gameObject.SetActive(true);
            transform.DOScale(0f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => {
                    gameObject.SetActive(false);
                    _isHidden = true;
                });
        }

        private void Hide()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
            _isHidden = true;
        }
    }
}
