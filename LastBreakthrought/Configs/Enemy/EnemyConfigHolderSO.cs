using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Configs.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfigHolder", menuName = "Configs/EnemyHolder")]
    public class EnemyConfigHolderSO : ScriptableObject
    {
        public List<EnemyType> Enemies = new();

        public EnemyConfigSO GetEnemyDataById(string enemyId)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy.Id == enemyId)
                    return enemy.EnemyData;
            }

            throw new Exception($"The enemyConfig with id: {enemyId} was not found");
        }
    }

    [Serializable]
    public struct EnemyType
    {
        public string Id;
        public EnemyConfigSO EnemyData;
    }
}

