using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.Camera;
using LastBreakthrought.UI.SlotItem;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace LastBreakthrought.UI.Slot
{
    public class SlotView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Item _item;
        [SerializeField] private RectTransform _selectedFrame;

        private IAudioService _audioService;
        private FollowCamera _followCamera;

        private bool _isSelected = false;
        private bool _isSoundRestored = true;

        [Inject]
        private void Construct(IAudioService audioService, FollowCamera followCamera)
        {
            _audioService = audioService;
            _followCamera = followCamera;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                _isSelected = true;
                _selectedFrame.gameObject.SetActive(true);
                transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                _item.Select();
            }
            else
            {
                _isSelected = false;
                _selectedFrame.gameObject.SetActive(false);
                _item.UnSelect();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            PlaySelectedSound();

            var scaledSize = 1.1f;
            transform.DOScale(scaledSize, Constants.ANIMATION_DURATION)
            .SetEase(Ease.InOutCubic)
            .Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;

            var normalSize = 1f;

            transform.DOScale(normalSize, Constants.ANIMATION_DURATION)
            .SetEase(Ease.InOutCubic)
            .Play();
        }

        private void PlaySelectedSound()
        {
            if (_isSoundRestored)
            {
                _isSoundRestored = false;
                _audioService.PlayOnObject(Configs.Sound.SoundType.Selected, _followCamera, false, 0.2f);
                StartCoroutine(RestorSound());
            }
        }

        private IEnumerator RestorSound()
        {
            yield return new WaitForSeconds(0.5f);
            _isSoundRestored = true;
        }
    }
}
