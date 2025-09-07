using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.FSX;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.NPC.Base;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Robot.States
{
    public class RobotDestroyedState : INPCState
    {
        private const string IS_DESTROYED = "isDestroyed";

        private readonly RobotBase _robot;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;
        private readonly BoxCollider _boxCollider;
        private readonly InteractionZoneHandler _zoneHandler;
        private readonly EffectCreator _effectCreator;
        private readonly IAudioService _audioService;

        public RobotDestroyedState(RobotBase robotBase, NavMeshAgent agent, Animator animator, BoxCollider boxCollider
            , InteractionZoneHandler zoneHandler, EffectCreator effectCreator, IAudioService audioService)
        {
            _robot = robotBase;
            _agent = agent;
            _animator = animator;
            _boxCollider = boxCollider;
            _zoneHandler = zoneHandler;
            _effectCreator = effectCreator;
            _audioService = audioService;
        }

        public void Enter()
        {
            _agent.enabled = false;
            _boxCollider.enabled = false;
            _robot.enabled = false;
            _animator.SetBool(IS_DESTROYED, true);
            _zoneHandler.Activate();

            _audioService.PlayOnObject(Configs.Sound.SoundType.RobotDestroyed, _robot);
            _effectCreator.CreateFireEffect(_robot.transform);
        }

        public void Exit() 
        {
            _agent.enabled = true;
            _boxCollider.enabled = true;
            _robot.enabled = true;
            _animator.SetBool(IS_DESTROYED, false);
            _zoneHandler.Disactivate();
        }

        public void Update(){}
    }
}
