using Assets.LastBreakthrought.UI.Inventory.ShipDetail;
using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Logic.Camera;
using LastBreakthrought.Player;
using LastBreakthrought.UI.Inventory.Mechanism;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LastBreakthrought.UI.Inventory
{
    public class InventoryMenuPanelView : BasePanelView
    {
        [Header("UI")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private RectTransform _detailContainer;
        [SerializeField] private RectTransform _mechanismContainer;
        [SerializeField] private Button _openClosedInventoryMenuButton;

        [field: SerializeField] public DetailContainerUI DetailsContainerUI { get; private set; }
        [field: SerializeField] public MechanismsContainerUI MechanismsContainer { get; private set; }

        public override void Init()
        {
            _openClosedInventoryMenuButton.onClick.AddListener(() => PerformOpenAndClose());
            EventBus.SubscribeEvent<OnRobotMenuOpenedSignal>(CheckIfNeedToBeClose);
            EventBus.SubscribeEvent<OnMapMenuOpenedSignal>(CheckIfNeedToBeClose);

            EventBus.SubscribeEvent((OnTutorialEndedSignal signal) => IsTutorialEnded = true);

            _root.localScale = Vector3.zero;
            _root.gameObject.SetActive(false);
        }

        public RectTransform GetDetailContainer() => _detailContainer;
        public RectTransform GetMechanismContainer() => _mechanismContainer;

        public void OpenInventory()
        {
            EventBus.Invoke(new OnInventoryMenuOpenedSignal());

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
            foreach (Transform item in _detailContainer)
                item.localScale = IsMenuOpen ? Vector3.one : Vector3.zero;

            foreach (Transform item in _mechanismContainer)
                item.localScale = IsMenuOpen ? Vector3.one : Vector3.zero;
        }

        public override void Open()
        {
            EventBus.Invoke(new OnInventoryMenuOpenedSignal());
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

        private void CheckIfNeedToBeClose(OnRobotMenuOpenedSignal signal)
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
