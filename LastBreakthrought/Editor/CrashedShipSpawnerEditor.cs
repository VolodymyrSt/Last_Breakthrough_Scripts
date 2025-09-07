using LastBreakthrought.CrashedShip;
using UnityEditor;
using UnityEngine;

namespace LastBreakthrought.Editor
{
    [CustomEditor(typeof(CrashedShipSpawner))]
    public class CrashedShipSpawnerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.Selected)]
        public static void RenderCustomGizmo(CrashedShipSpawner spawner, GizmoType type)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 1f);
        }
    }
}
