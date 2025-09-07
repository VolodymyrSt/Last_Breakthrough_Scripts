using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.Camera;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace LastBreakthrought.Other
{
    public class MenuButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _selectedArrowRoot;
        public Ease Ease;

        private IAudioService _audioService;
        private SoundHolder _soundHolder;

        private bool _isSoundRestored = true;

        [Inject]
        private void Construct(IAudioService audioService, SoundHolder soundHolder)
        {
            _audioService = audioService;
            _soundHolder = soundHolder;
        }

        private void OnEnable() => 
            _selectedArrowRoot.gameObject.SetActive(false);

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySelectedSound();
            _selectedArrowRoot.gameObject.SetActive(true);

            transform.DOScale(Constants.SCALED_VALUE, Constants.ANIMATION_DURATION)
                .SetEase(Ease)
                .Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _selectedArrowRoot.gameObject.SetActive(false);

            transform.DOScale(Constants.UNSCALED_VALUE, Constants.ANIMATION_DURATION)
                .SetEase(Ease)
                .Play();
        }

        private void PlaySelectedSound()
        {
            if (_isSoundRestored)
            {
                _isSoundRestored = false;
                _audioService.PlayOnObject(Configs.Sound.SoundType.Selected, _soundHolder, false, 0.2f);
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
