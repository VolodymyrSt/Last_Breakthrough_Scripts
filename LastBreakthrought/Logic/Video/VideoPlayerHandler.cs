using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Logic.Camera;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Zenject;

namespace LastBreakthrought.Logic.Video
{
    public class VideoPlayerHandler : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _beginningVideo;
        [SerializeField] private VideoPlayer _startExplodingVideo;
        [SerializeField] private VideoPlayer _victoryVideo;

        private UnityEngine.Camera _camera;
        private IEventBus _eventBus;

        [Inject]
        private void Construct(UnityEngine.Camera camera, IEventBus eventBus)
        {
            _camera = camera;
            _eventBus = eventBus;

            InitVideos();
        }

        private void Awake() =>
            PlayBeginningVideo(() => _eventBus.Invoke(new OnBeginningVideoEndedSignal()));

        private void Start()
        {
            _eventBus.SubscribeEvent((OnGameEndedSignal signal) =>
                PlayStarExplodingVideo(() => _eventBus.Invoke(new OnExploededStarVideoEndedSignal())));

            _eventBus.SubscribeEvent((OnGameWonSignal signal) =>
                PlayVictoryVideo(() => _eventBus.Invoke(new OnVictoryVideoEndedSignal())));
        }

        public void PlayBeginningVideo(Action onEnded) => StartCoroutine(Play(_beginningVideo, onEnded, 9f));

        private void PlayStarExplodingVideo(Action onEnded) => StartCoroutine(Play(_startExplodingVideo, onEnded, 24f, false));

        private void PlayVictoryVideo(Action onEnded) => StartCoroutine(Play(_victoryVideo, onEnded, 6f, false));

        private IEnumerator Play(VideoPlayer video, Action onEnded, float videoLenght, bool needToHideAtTheEnd = true)
        {
            _eventBus.Invoke(new OnVideoPlayedSignal());
            video.gameObject.SetActive(true);
            video.targetCamera = _camera;
            video.Play();
            yield return new WaitForSeconds(videoLenght);
            video.gameObject.SetActive(!needToHideAtTheEnd);
            onEnded?.Invoke();
        }

        private void InitVideos()
        {
            _beginningVideo.source = VideoSource.Url;
            _startExplodingVideo.source = VideoSource.Url;
            _victoryVideo.source = VideoSource.Url;

            _beginningVideo.url = System.IO.Path.Combine(Application.streamingAssetsPath, AssetPath.BEGINNING_VIDEO_PATH);
            _startExplodingVideo.url = System.IO.Path.Combine(Application.streamingAssetsPath, AssetPath.STAR_EXPLOTION_VIDEO_PATH);
            _victoryVideo.url = System.IO.Path.Combine(Application.streamingAssetsPath, AssetPath.VICTORY_VIDEO_PATH);
        }

        private void OnDestroy()
        {
            _eventBus?.UnSubscribeEvent((OnGameEndedSignal signal) =>
                PlayStarExplodingVideo(() => _eventBus.Invoke(new OnExploededStarVideoEndedSignal())));

            _eventBus?.UnSubscribeEvent((OnGameWonSignal signal) =>
                PlayVictoryVideo(() => _eventBus.Invoke(new OnVictoryVideoEndedSignal())));
        }
    }
}
