using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.Logic.Mechanisms;
using System;
using UnityEngine;

namespace LastBreakthrought.UI.CraftingMachine.Crafts
{
    public class MechanismCraftUIFactory : BaseFactory<MechanismCraftHandler>
    {
        public MechanismCraftUIFactory(IAssetProvider assetProvider) : base(assetProvider){}

        public override MechanismCraftHandler Create(Vector3 at, Transform parent) => 
            AssetProvider.Instantiate<MechanismCraftHandler>(AssetPath.MechanismCraftPath, at, parent);

        public MechanismCraftHandler SpawnCraft(RectTransform parent, MechanismSO mechanism, Action craftAction)
        {
            var craft = Create(Vector3.zero, parent);
            craft.Init(craftAction, mechanism);
            return craft;
        }
    }
}
