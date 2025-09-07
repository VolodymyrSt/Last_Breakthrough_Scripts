namespace LastBreakthrought.Infrustructure.State
{
    public class BootStrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootStrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => 
            _sceneLoader.Load(SceneName.BootStrapper, EnterMenuState);

        private void EnterMenuState() => 
            _gameStateMachine.Enter<LoadMenuState>();

        public void Exit(){}
    }
}
