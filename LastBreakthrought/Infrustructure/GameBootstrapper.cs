using LastBreakthrought.Infrustructure.State;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Infrustructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        [Inject]
        private void Construct(Game game) =>
            _game = game;

        private void Awake()
        {
            _game.StateMachine.Enter<BootStrapState>();

            DontDestroyOnLoad(gameObject);
        }
    }
}
