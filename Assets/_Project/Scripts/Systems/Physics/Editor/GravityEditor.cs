using UnityEngine;
using UnityEditor;

namespace Physics
{
    [CustomEditor(typeof(Gravity))]
    public class GravityEditor : Editor
    {
        private Gravity gravity;
        private SerializedObject gravityObj;
        private SerializedProperty gravityFactorProp;

        private void OnEnable()
        {
            gravity = target as Gravity;
            gravityObj = new SerializedObject(gravity);
            gravityFactorProp = gravityObj.FindProperty("_gravityFactor");
        }

        public override void OnInspectorGUI()
        {
            gravityObj.Update();
            gravityFactorProp.floatValue = EditorGUILayout.FloatField("Gravity Factor", gravityFactorProp.floatValue);
            if(gravityObj.ApplyModifiedProperties())
            {
                gravity.ChangeGravityFactor();
            }
        }
    }
}
