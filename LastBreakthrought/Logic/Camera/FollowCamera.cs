using LastBreakthrought.Player;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Logic.Camera
{
    public class FollowCamera : MonoBehaviour
    {
        [Header("Setting:")]
        [SerializeField] private float _rotationAngleX;
        [SerializeField] private float _distance;
        [SerializeField] private float _offSetY;

        private Transform _followTarget;
        private PlayerHandler _player;

        [Inject]
        private void Construct(PlayerHandler player) => 
            _player = player;

        private void Awake() => 
            _followTarget = _player.transform;

        private void LateUpdate() => PerformFollow();

        private void PerformFollow()
        {
            var newRotation = Quaternion.Euler(_rotationAngleX, 0, 0);
            var newPosition = newRotation * new Vector3(0, 0, _distance * (-1)) + GetFollowingPointPosition();

            transform.position = newPosition;
            transform.rotation = newRotation;
        }

        private Vector3 GetFollowingPointPosition()
        {
            Vector3 followingPosition = _followTarget.position;
            followingPosition.y += _offSetY;
            return followingPosition;
        }
    }
}
