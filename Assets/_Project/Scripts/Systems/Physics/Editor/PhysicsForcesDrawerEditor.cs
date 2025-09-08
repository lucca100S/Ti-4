using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace Physics
{
    [ExecuteAlways]
    [CustomEditor(typeof(PhysicsForcesDrawer))]
    public class PhysicsForcesDrawerEditor : Editor
    {
        
        PhysicsForcesDrawer _drawer;

        private void Awake()
        {
            _drawer = (PhysicsForcesDrawer)target;
        }

        private void OnSceneGUI()
        {
            if (!Application.isPlaying) return;

            foreach (PhysicsForce force in _drawer.PhysicsController.Forces.Values)
            {
                Vector3 position = _drawer.transform.position;
                if(force.TargetPoint != null) position = force.TargetPoint.localPosition;

                float size = _drawer.Size * (force.ForceCurrent.magnitude / force.ForceMax);

                Handles.color = force.Color;
                Handles.zTest = CompareFunction.Always;
                Handles.ArrowHandleCap(0, position, Quaternion.FromToRotation(Vector3.forward, force.Direction), size, EventType.Repaint);
                Handles.zTest = CompareFunction.GreaterEqual;
                Handles.color = Color.white;
            }
        }



    }
}
