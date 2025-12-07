using System.CodeDom;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PrefabPaintWindow : EditorWindow
{
    [SerializeField] private GameObject _groupObject;

    [SerializeField] private float _brushSize = 1.0f;
    [SerializeField] private GameObject _prefabToPaint;
    [SerializeField] private int _prefabMinCount = 1;
    [SerializeField] private int _prefabMaxCount = 5;

    [SerializeField] private int _layerMask = 0;

    [SerializeField] private float _rotationVariance = 30f;
    [SerializeField] private float _scaleVariance = 0.1f;

    [SerializeField] private float _angleTreshold = 90f;

    private SerializedObject _paintWindowSO;

    private SerializedProperty _groupObjectProp;

    private SerializedProperty _brushSizeProp;
    private SerializedProperty _prefabToPaintProp;
    private SerializedProperty _prefabMinCountProp;
    private SerializedProperty _prefabMaxCountProp;
    private SerializedProperty _layerMaskProp;

    private SerializedProperty _rotationVarianceProp;
    private SerializedProperty _scaleVarianceProp;

    private SerializedProperty _angleTresholdProp;

    private const float RAYCAST_HEIGHT_OFFSET = 3f;
    private const float RAYCAST_MAX_DISTANCE = 10f;

    void OnEnable()
    {
        SceneView.duringSceneGui += this.OnSceneGUI;

        _paintWindowSO = new SerializedObject(this);

        _groupObjectProp = _paintWindowSO.FindProperty("_groupObject");

        _brushSizeProp = _paintWindowSO.FindProperty("_brushSize");
        _prefabToPaintProp = _paintWindowSO.FindProperty("_prefabToPaint");
        _prefabMinCountProp = _paintWindowSO.FindProperty("_prefabMinCount");
        _prefabMaxCountProp = _paintWindowSO.FindProperty("_prefabMaxCount");
        _layerMaskProp = _paintWindowSO.FindProperty("_layerMask");

        _rotationVarianceProp = _paintWindowSO.FindProperty("_rotationVariance");
        _scaleVarianceProp = _paintWindowSO.FindProperty("_scaleVariance");

        _angleTresholdProp = _paintWindowSO.FindProperty("_angleTreshold");
    }
    void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    [MenuItem("Tools/Lugu/Prefab Paint #&P")]
    public static void OpenWindow()
    {
        PrefabPaintWindow window = GetWindow<PrefabPaintWindow>("Prefab Paint Tool");
        window.minSize = new Vector2(450, 200);
        window.Show();
    }

    private void OnGUI()
    {
        _paintWindowSO.Update();

        _groupObjectProp.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Parent Object", _groupObject, typeof(GameObject), true);
        _prefabToPaintProp.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Prefab to Paint", _prefabToPaint, typeof(GameObject), false);

        _brushSizeProp.floatValue = EditorGUILayout.FloatField("Brush Size", _brushSize);

        Rect rect = GUILayoutUtility.GetRect(0, 20);
        //_layerMaskProp.intValue = EditorGUI.LayerField(rect, "Layer Mask", _layerMask); // Placeholder for Layer Mask field

        GUILayout.Label("Prefab Count", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        _prefabMinCountProp.intValue = EditorGUILayout.IntField("Min", _prefabMinCount);
        _prefabMaxCountProp.intValue = EditorGUILayout.IntField("Max", _prefabMaxCount);
        EditorGUILayout.EndHorizontal();

        _rotationVarianceProp.floatValue = EditorGUILayout.FloatField("Rotation Variance", _rotationVariance);
        _scaleVarianceProp.floatValue = EditorGUILayout.FloatField("Scale Variance", _scaleVariance);

        _angleTresholdProp.floatValue = EditorGUILayout.FloatField("Angle Treshold", _angleTreshold);

        _paintWindowSO.ApplyModifiedProperties();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        //Checking if Raycast hits something
        if (RaycastFromMouse(out Vector3 point, out Vector3 normal))
        {
            //Checking for paint input
            if (CheckPaintInput())
            {
                PlaceAllPrefabs(point, normal);
            }
        }

    }

    private bool RaycastFromMouse(out Vector3 point, out Vector3 normal)
    {
        point = Vector3.zero;
        normal = Vector3.zero;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            point = hit.point;
            normal = hit.normal;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.DrawWireDisc(hit.point, hit.normal, _brushSize);
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
            return true;
        }

        return false;
    }

    private bool CheckPaintInput()
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            e.Use();
            return true;
        }

        return false;
    }

    private void PlaceAllPrefabs(Vector3 point, Vector3 normal)
    {
        if (_groupObject == null)
        {
            CreateGroupObject();
        }

        int prefabCount = Random.Range(Mathf.RoundToInt(_prefabMinCount), Mathf.RoundToInt(_prefabMaxCount) + 1);
        for (int i = 0; i < prefabCount; i++)
        {
            Vector3 randompos = GetRandomPointInCircle(point, normal);
            GetFixedPosition(randompos, normal);
        }
    }

    private void PlacePrefabAtPoint(Vector3 point, Vector3 normal)
    {
        if (_prefabToPaint)
        {
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(_prefabToPaint);
            newObj.transform.position = point;
            newObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);

            float randomYRotation = Random.Range(-_rotationVariance, _rotationVariance);
            newObj.transform.Rotate(Vector3.up, randomYRotation, Space.Self);

            float randomScaleFactor = 1 + Random.Range(-_scaleVariance, _scaleVariance);
            newObj.transform.localScale = newObj.transform.localScale * randomScaleFactor;

            newObj.transform.parent = _groupObject.transform;

            Undo.RegisterCreatedObjectUndo(newObj, "Paint Prefab");
        }
    }

    private Vector3 GetRandomPointInCircle(Vector3 point, Vector3 normal)
    {
        Vector2 randomPoint = Random.insideUnitCircle * _brushSize;

        Vector3 tangent = Vector3.Cross(normal, Vector3.up).normalized;

        if (tangent == Vector3.zero)
        {
            tangent = Vector3.Cross(normal, Vector3.right).normalized;
        }

        Vector3 bitangent = Vector3.Cross(normal, tangent).normalized;

        Vector3 offset = tangent * randomPoint.x + bitangent * randomPoint.y;
        point += offset;

        return point;
    }

    private void GetFixedPosition(Vector3 point, Vector3 normal)
    {
        Ray ray = new Ray(point + normal * RAYCAST_HEIGHT_OFFSET, -normal);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYCAST_MAX_DISTANCE))
        {
            if (Vector3.Dot(Vector3.up, hit.normal) >= Mathf.Sin(_angleTreshold))
            {
                PlacePrefabAtPoint(hit.point, hit.normal);
            }
        }
    }

    private void CreateGroupObject()
    {
        _paintWindowSO.Update();
        GameObject group = new GameObject("PaintedPrefabs_Group");
        _groupObjectProp.objectReferenceValue = group;
        _paintWindowSO.ApplyModifiedProperties();
        Undo.RegisterCreatedObjectUndo(group, "Paint Group Object");
    }
}
