using LastBreakthrought.CrashedShip;
using LastBreakthrought.NPC.Enemy;
using System.Collections.Generic;

namespace LastBreakthrought.Other
{
    public class SpawnersContainer
    {
        private readonly List<CrashedShipSpawner> _crashedShipSpawners = new();
        private readonly List<EnemySpawner> _enemySpawners = new();

        public void AddEnemySpawner(EnemySpawner enemySpawner) => _enemySpawners.Add(enemySpawner);
        public void AddCrashedShipSpawner(CrashedShipSpawner crashedShipSpawner) => _crashedShipSpawners.Add(crashedShipSpawner);

        public void SpawnAllCrashedShips()
        {
            foreach (var spawner in _crashedShipSpawners)
                spawner.SpawnCrashedShip();
        }
        
        public void SpawnAllEnemies()
        {
            foreach (var spawner in _enemySpawners)
                spawner.SpawnEnemy();
        }
    }
}
