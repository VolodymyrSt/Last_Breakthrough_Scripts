using LastBreakthrought.Logic;
using LastBreakthrought.Other;
using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Zenject;

namespace LastBreakthrought.Infrustructure.State
{
    public class GameStateMachine 
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentStates;

        public GameStateMachine(LoadingCurtain loadingCurtain, SceneLoader sceneLoader,
            Game game)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootStrapState)] = new BootStrapState(this, sceneLoader),
                [typeof(LoadMenuState)] = new LoadMenuState(this, loadingCurtain, sceneLoader),
                [typeof(LoadGameplayState)] = new LoadGameplayState(this, loadingCurtain, sceneLoader, game)
            };
        }

        public void Enter<TState>() where TState : IState 
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        private IState ChangeState<TState>()
        {
            _currentStates?.Exit();

            var state = _states[typeof(TState)];
            _currentStates = state;
            return state;
        }
    }
}
