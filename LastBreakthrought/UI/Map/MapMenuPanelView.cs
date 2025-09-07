using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.Map
{
    public class MapMenuPanelView : BasePanelView
    {
        [Header("UI")]
        [SerializeField] private RectTransform _markersContent;
        [SerializeField] private RectTransform _root;
        [SerializeField] private Button _openClosedMapMenuButton;

        public override void Init()
        {
            _openClosedMapMenuButton.onClick.AddListener(() => PerformOpenAndClose());
            EventBus.SubscribeEvent<OnInventoryMenuOpenedSignal>(CheckIfNeedToBeClose);
            EventBus.SubscribeEvent<OnRobotMenuOpenedSignal>(CheckIfNeedToBeClose);

            EventBus.SubscribeEvent((OnTutorialEndedSignal signal) => IsTutorialEnded = true);

            _root.localScale = Vector3.zero;
            _root.gameObject.SetActive(false);
        }

        public RectTransform GetMarkerContainer() => _markersContent;


        public void UpdateChildrenScale()
        {
            foreach (Transform item in _markersContent)
                item.localScale = IsMenuOpen ? Vector3.one : Vector3.zero;
        }

        public override void Open()
        {
            EventBus.Invoke(new OnMapMenuOpenedSignal());
            AudioService.PlayOnObject(Configs.Sound.SoundType.PanelOpen, FollowCamera);

            _root.gameObject.SetActive(true);
            _root.DOScale(1f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play().OnComplete(() =>
                {
                    IsMenuOpen = true;
                    UpdateChildrenScale();
                });
        }

        public override void Close()
        {
            _root.DOScale(0f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() =>
                {
                    _root.gameObject.SetActive(false);
                    IsMenuOpen = false;
                });
        }

        private void CheckIfNeedToBeClose(OnInventoryMenuOpenedSignal signal)
        {
            if (IsMenuOpen)
                Close();
        }

        private void CheckIfNeedToBeClose(OnRobotMenuOpenedSignal signal)
        {
            if (IsMenuOpen)
                Close();
        }
    }
}
