using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.CrashedShip 
{
    public class CrashedShipsContainer
    {
        public List<ICrashedShip> CrashedShips {  get; set; }

        public CrashedShipsContainer() => 
            CrashedShips = new List<ICrashedShip>();

        public Vector3 GetClosestCrashedShipPosition(Vector3 target)
        {
            var closestShip = CrashedShips[0];
            var closestDistance = Vector3.Distance(target, closestShip.GetPosition());

            foreach (var currentShip in CrashedShips)
            {
                var distance = Vector3.Distance(target, currentShip.GetPosition());

                if (distance < closestDistance)
                {
                    closestShip = currentShip;
                    closestDistance = distance;
                }
            }

            return closestShip.GetPosition();
        }

        public bool IsEmpty() => CrashedShips.Count <= 0;
    }
}
