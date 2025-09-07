using LastBreakthrought.Logic.ChargingPlace;
using LastBreakthrought.NPC.Base;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Robot.States
{
    public class RobotRechargingState : INPCState
    {
        private const string IS_Moving = "isMoving";

        private readonly RobotBase _robot;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;
        private readonly RobotBattary _robotBattary;

        private RobotChargingPlace _chargingPlace;
        private readonly float _followingSpeed;

        private bool _isRechedChargingPlace = false;

        public RobotRechargingState(RobotBase robot, NavMeshAgent agent, Animator animator,
            RobotBattary robotBattary, float followingSpeed)
        {
            _robot = robot;
            _agent = agent;
            _animator = animator;
            _robotBattary = robotBattary;
            _followingSpeed = followingSpeed;
        }

        public void Enter()
        {
            _agent.isStopped = false;
            _agent.speed = _followingSpeed;
            _agent.stoppingDistance = Constants.RECHARGING_STOP_DISTANCE;
            _animator.SetBool(IS_Moving, true);

            _chargingPlace = _robot.FindAvelableCharingPlace();
            _chargingPlace.IsOccupiad = true;
        }

        public void Exit() { }

        public void Update()
        {
            if (!_isRechedChargingPlace)
            {
                _agent.SetDestination(_chargingPlace.GetChargingPosition());
                CheckIfRobotRechedChargingPlace();
                _animator.SetBool(IS_Moving, false);
            }
            else
                PerformRecharging();
        }

        private void CheckIfRobotRechedChargingPlace()
        {
            if (_agent.remainingDistance <= Constants.RECHARGING_STOP_DISTANCE && !_agent.pathPending)
                _isRechedChargingPlace = true;
        }

        private void PerformRecharging()
        {
            _robotBattary.IncreaseCapacity();

            if (_robotBattary.IsCapacityFull())
            {
                _robotBattary.NeedToBeRecharged = false;
                _chargingPlace.IsOccupiad = false;
                _isRechedChargingPlace = false;
            }
        }
    }
}
