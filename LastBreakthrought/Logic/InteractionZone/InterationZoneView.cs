using DG.Tweening;
using UnityEngine;

namespace LastBreakthrought.Logic.InteractionZone
{
    public class InterationZoneView : MonoBehaviour
    {
        [SerializeField] private Vector3 _currentScale;
        private bool _isHidden;
        private bool _isOpened;

        private void OnValidate() => 
           _currentScale = transform.localScale;

        public void Show()
        {
            if (_isOpened) return;

            var duration = 50f;
            gameObject.SetActive(true);
            transform.DOScale(_currentScale, duration * Time.deltaTime)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => { 
                    _isOpened = true;
                    _isHidden = false;
                });
        }
        
        public void Hide()
        {
            if (_isHidden) return;

            var duration = 50f;
            transform.DOScale(0, duration * Time.deltaTime)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => { 
                    gameObject.SetActive(false);
                    _isHidden = true;
                    _isOpened = false;
                });
        }

        public void HideOnInit()
        {
            Hide();
            gameObject.SetActive(false);
        }
    }
}
