using UnityEngine;

namespace LastBreakthrought.Logic.ChargingPlace
{
    public class RobotChargingPlace : MonoBehaviour
    {
        [SerializeField] private Transform _chargingPoint;

        public bool IsOccupiad {  get; set; } = false;

        public Vector3 GetChargingPosition() => _chargingPoint.position;
    }
}
