using Assets.LastBreakthrought.UI.Inventory.ShipDetail;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Logic.ShipDetail
{
    public class ShipDetailsGeneratorUI
    {
        private ShipDetailUIFactory _shipDetailUIFactory;

        public ShipDetailsGeneratorUI(ShipDetailUIFactory shipDetailUIFactory) => 
            _shipDetailUIFactory = shipDetailUIFactory;

        public void GenerateRequireDetails(List<ShipDetailEntity> requiredDetails, RectTransform parent)
        {
            foreach (var requiredDetail in requiredDetails)
            {
                var neededDitalView = _shipDetailUIFactory.SpawnAt(parent);
                neededDitalView.Init(requiredDetail);
            }
        }
    }
}
