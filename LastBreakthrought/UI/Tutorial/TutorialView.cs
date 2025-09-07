using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Logic.Camera;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace LastBreakthrought.UI.Tutorial
{
    public class TutorialView : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnDialogueEnded;

        [Header("UI")]
        [SerializeField] private RectTransform _robotSpeaker;
        [SerializeField] private TextMeshProUGUI _dialogueContent;

        private IEventBus _eventBus;
        private IAudioService _audioService;
        private FollowCamera _followCamera;

        private Coroutine _currentDialogueCoroutine;
        private List<string> _dialogueLines;
        private float _waitTime;
        private int _currentLine;

        [Inject]
        private void Construct(FollowCamera followCamera) =>
            _followCamera = followCamera;

        public void Init(List<string> dialogueLines, float waitTime, IEventBus eventBus, IAudioService audioService)
        {
            _currentLine = 0;
            _dialogueContent.text = string.Empty;
            _dialogueLines = new List<string>(dialogueLines);
            _waitTime = waitTime;
            _eventBus = eventBus;
            _audioService = audioService;

            _eventBus.SubscribeEvent<OnGamePausedSignal>(StopDialogue);
            _eventBus.SubscribeEvent<OnGameResumedSignal>(ContinueDialogue);
        }

        public void OnPointerClick(PointerEventData eventData) =>
            HandleDialogueClick();

        private void HandleDialogueClick()
        {
            if (_dialogueLines[_currentLine] == _dialogueContent.text)
                GoToNextLine();
            else
                CompleteCurrentLine();
        }

        public void RunCurrentDialogueLine()
        {
            _currentDialogueCoroutine = StartCoroutine(RunDialogueLine());
            PlayAnimation();
        }

        private IEnumerator RunDialogueLine()
        {
            foreach (var character in _dialogueLines[_currentLine].ToCharArray())
            {
                _dialogueContent.text += character;
                yield return new WaitForSecondsRealtime(_waitTime);
            }
            ResetAnimation();
        }


        private void CompleteCurrentLine()
        {
            ResetAnimation();
            if (_currentDialogueCoroutine != null)
            {
                StopCoroutine(_currentDialogueCoroutine);
                _dialogueContent.text = _dialogueLines[_currentLine];
            }
        }

        private void GoToNextLine()
        {
            _currentLine++;

            if (_currentLine < _dialogueLines.Count)
            {
                _dialogueContent.text = string.Empty;
                RunCurrentDialogueLine();
            }
            else
            {
                OnDialogueEnded.Invoke();
                Destroy(gameObject);
            }
        }

        private void StopDialogue(OnGamePausedSignal signal) =>
            StopCoroutine(_currentDialogueCoroutine);

        private void ContinueDialogue(OnGameResumedSignal signal)
        {
            _dialogueContent.text = string.Empty;
            RunCurrentDialogueLine();
        }

        private void ResetAnimation()
        {
            _audioService.StopOnObject(_followCamera, Configs.Sound.SoundType.RobotTalking);
            _robotSpeaker.DOKill();
            _robotSpeaker.DOScale(1f, Constants.ANIMATION_DURATION).SetEase(Ease.Linear).Play();
        }

        private void PlayAnimation()
        {
            _audioService.PlayOnObject(Configs.Sound.SoundType.RobotTalking, _followCamera, false, 1);
            _robotSpeaker.DOScale(1.05f, Constants.ANIMATION_DURATION)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
        }

        private void OnDestroy()
        {
            if (_currentDialogueCoroutine != null)
                StopCoroutine(_currentDialogueCoroutine);

            _eventBus?.UnSubscribeEvent<OnGamePausedSignal>(StopDialogue);
            _eventBus?.UnSubscribeEvent<OnGameResumedSignal>(ContinueDialogue);
        }
    }
}
