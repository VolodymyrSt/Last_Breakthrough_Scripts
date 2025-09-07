using LastBreakthrought.Infrustructure.State;
using LastBreakthrought.Logic;
using LastBreakthrought.Logic.RobotFactory;
using LastBreakthrought.Other;
using Unity.AI.Navigation;

namespace LastBreakthrought.Infrustructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; private set; }
        public SpawnersContainer SpawnersContainer { get; set; }
        public RobotFactoryMachine RobotFactoryMachine { get; set; }
        public NavMeshSurface NavMeshSurface { get; set; }

        public Game(LoadingCurtain loadingCurtain, SceneLoader sceneLoader) => 
            StateMachine = new GameStateMachine(loadingCurtain, sceneLoader, this);
    }
}
