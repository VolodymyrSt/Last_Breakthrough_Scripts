using LastBreakthrought.CrashedShip;
using LastBreakthrought.Player;
using Zenject;
using UnityEngine;
using LastBreakthrought.Infrustructure.Services.Massage;

namespace LastBreakthrought.UI.SlotItem.WreckageDetector
{
    public class WreckageDetectorItem : Item
    {
        [SerializeField] private WreckageDetectorItemView _wreckageDetectorItemView;

        private PlayerHandler _player;
        private CrashedShipsContainer _shipsContainer;

        [Inject]
        private void Construct(PlayerHandler player, CrashedShipsContainer shipsContainer)
        {
            _player = player;
            _shipsContainer = shipsContainer;
        }

        private void OnEnable() => UnSelect();

        private void Update()
        {
            if (IsItemSelected)
                UpdateDistance();
        }

        private void UpdateDistance()
        {
            var closestCrashedShipPosition = _shipsContainer.GetClosestCrashedShipPosition(_player.transform.position);
            var distance = (int)(_player.transform.position - closestCrashedShipPosition).magnitude;

            _wreckageDetectorItemView.SetDistanceUI(distance);
        }

        public override void Select()
        {
            _player.ShowDetectorItem();
            _player.SetMovingAnimation(true);

            IsItemSelected = true;
            _wreckageDetectorItemView.Show();
        }

        public override void UnSelect()
        {
            _player.HideDetectorItem();
            _player.SetMovingAnimation(false);

            IsItemSelected = false;
            _wreckageDetectorItemView.Hide();
        }
    }
}