using LastBreakthrought.Logic;

namespace LastBreakthrought.Infrustructure.State
{
    public class LoadGameplayState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly SceneLoader _sceneLoader;
        private readonly Game _game;

        public LoadGameplayState(GameStateMachine gameStateMachine, LoadingCurtain loadingCurtain
            , SceneLoader sceneLoader, Game game)
        {
            _gameStateMachine = gameStateMachine;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _game = game;
        }

        public void Enter()
        {
            _loadingCurtain.Procced();
            _sceneLoader.Load(SceneName.Gameplay, InitWorld);
        }

        private void InitWorld()
        {
            var spawnersContainer = _game.SpawnersContainer;
            UnityEngine.Debug.Log("vssv");

            spawnersContainer.SpawnAllCrashedShips();

            //_game.NavMeshSurface.BuildNavMesh();

            _game.RobotFactoryMachine.CreateStartedRobotsAtTheBeginning();

            spawnersContainer.SpawnAllEnemies();
        }

        public void Exit(){}
    }
}
