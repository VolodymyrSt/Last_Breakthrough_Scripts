using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel
{
    public class RobotMenuPanelView : BasePanelView
    {
        [Header("UI")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Button _openClosedRobotsMenuButton;

        public override void Init()
        {
            _openClosedRobotsMenuButton.onClick.AddListener(() => PerformOpenAndClose());
            EventBus.SubscribeEvent<OnInventoryMenuOpenedSignal>(CheckIfNeedToBeClose);
            EventBus.SubscribeEvent<OnMapMenuOpenedSignal>(CheckIfNeedToBeClose);

            EventBus.SubscribeEvent((OnTutorialEndedSignal signal) => IsTutorialEnded = true);

            _root.localScale = Vector3.zero;
            _root.gameObject.SetActive(false);
        }

        public RectTransform GetContainer() => _content;

        public void OpenPanel()
        {
            EventBus.Invoke(new OnRobotMenuOpenedSignal());

            _root.gameObject.SetActive(true);
            _root.DOScale(1f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play().OnComplete(() =>
                {
                    IsMenuOpen = true;
                    UpdateChildrenScale();
                });
        }

        public void UpdateChildrenScale()
        {
            foreach (Transform item in _content)
                item.localScale = IsMenuOpen ? Vector3.one : Vector3.zero;
        }

        public override void Open()
        {
            EventBus.Invoke(new OnRobotMenuOpenedSignal());
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

        private void CheckIfNeedToBeClose(OnMapMenuOpenedSignal signal)
        {
            if (IsMenuOpen)
                Close();
        }
    }
}
