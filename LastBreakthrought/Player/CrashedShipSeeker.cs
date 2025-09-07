using LastBreakthrought.CrashedShip;
using UnityEngine;

namespace LastBreakthrought.Player
{
    public class CrashedShipSeeker : MonoBehaviour
    {
        public ICrashedShip FoundCrashedShip {  get; private set; }

        [SerializeField] private LayerMask _crashedShipLayerMask;
        [SerializeField] private float _seekRange;
        private readonly Collider[] _crashedShipCollider = new Collider[1];

        private void Update()
        {
            if (Physics.OverlapSphereNonAlloc(transform.position, _seekRange, _crashedShipCollider, _crashedShipLayerMask) > 0)
            {
                if (_crashedShipCollider[0].TryGetComponent(out ICrashedShip crashedShip))
                    FoundCrashedShip = crashedShip;
                else
                    ClearCrashedShip();
            }
            else
                ClearCrashedShip();
        }

        private void ClearCrashedShip()
        {
            if (FoundCrashedShip != null)
            {
                _crashedShipCollider[0] = null;
                FoundCrashedShip = null;
            }
        }
    }
}

