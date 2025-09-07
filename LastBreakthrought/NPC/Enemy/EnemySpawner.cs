using LastBreakthrought.Infrustructure;
using LastBreakthrought.NPC.Enemy.Factory;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.NPC.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private BoxCollider _wanderingZone;

        private EnemyFactory _enemyFactory;

        [Inject]
        private void Construct(EnemyFactory enemyFactory, Game game)
        {
            _enemyFactory = enemyFactory;
            game.SpawnersContainer.AddEnemySpawner(this);
        }

        public void SpawnEnemy()
        {
            if (ShouldSpawnEnemy())
            {
                var enemy = _enemyFactory.SpawnAt(transform.position, transform);
                enemy.OnSpawned(_wanderingZone, _enemyFactory.EnemyID);
            }
        }

        private bool ShouldSpawnEnemy()
        {
            int randomNumber = Random.Range(1, 101);
            return randomNumber % 2 == 0;
        }
    }
}
