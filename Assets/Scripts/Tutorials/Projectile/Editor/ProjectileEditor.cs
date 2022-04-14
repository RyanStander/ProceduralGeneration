using System;
using UnityEditor;
using UnityEngine;

namespace Projectile
{
    [CustomEditor(typeof(Projectile))]
    public class ProjectileEditor : UnityEditor.Editor
    {

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawGizmosSelected(Component projectile, GizmoType gizmoType)
        {
            Gizmos.DrawSphere(projectile.transform.position,0.125F);
        }

        private void OnSceneGUI()
        {
            var projectile = target as Projectile;
            if (projectile == null) return;
            var transform = projectile.transform;
            projectile.damageRadius =
                Handles.RadiusHandle(transform.rotation, transform.position, projectile.damageRadius);
        }
    }
}