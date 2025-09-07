using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using UnityEngine;

namespace LastBreakthrought.NPC.Enemy.Factory
{
    public class EnemyFactory : BaseFactory<IEnemy>
    {
        public string EnemyID { get; private set; }

        public EnemyFactory(IAssetProvider assetProvider) : 
            base(assetProvider){}

        //<summery>
        //this method create random enemy base on EnemyPath
        //</summery>
        public override IEnemy Create(Vector3 at, Transform parent)
        {
            EnemyID = AssetPath.GetRandomEnemyPath();
            return AssetProvider.Instantiate<IEnemy>(EnemyID, at, parent);
        }
    }
}
