using LastBreakthrought.Other;
using LastBreakthrought.Player;
using Zenject;

namespace LastBreakthrought.UI.Home
{
    public class HomeDistanceCounter : ITickable
    {
        private readonly HomePoint _homePoint;
        private readonly PlayerHandler _player;
        private readonly HomeDistanceView _homeDistanceView;

        public HomeDistanceCounter(HomePoint homePoint, PlayerHandler player, HomeDistanceView homeDistanceView)
        {
            _homePoint = homePoint;
            _player = player;
            _homeDistanceView = homeDistanceView;
        }

        public void Tick()
        {
            var distance = (int)(_homePoint.GetPosition() - _player.transform.position).magnitude;
            _homeDistanceView.SetHomeDistance(distance);
        }
    }
}
