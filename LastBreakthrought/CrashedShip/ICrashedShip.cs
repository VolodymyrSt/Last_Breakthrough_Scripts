using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.CrashedShip
{
    public interface ICrashedShip
    {
        event Action OnDestroyed;

        List<ShipMaterialEntity> Materials { get; }
        List<ShipMaterialEntity> MinedMaterials { get; }

        Vector3 GetPosition();
        void OnInitialized();
        ShipMaterialEntity MineEntireMaterial();
        IEnumerator DestroySelf();
        void RemoveMinedMaterialView();
        List<ShipMaterialEntity> GetMaterialsForMarker();
    }
}
