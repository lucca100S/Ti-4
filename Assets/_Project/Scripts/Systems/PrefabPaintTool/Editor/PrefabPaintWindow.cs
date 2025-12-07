using System.CodeDom;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PrefabPaintWindow : EditorWindow
{
    [SerializeField] private float _brushSize = 1.0f;
    [SerializeField] private GameObject _prefabToPaint;
    [SerializeField] private int _prefabMinCount = 1;
    [SerializeField] private int _prefabMaxCount = 5;
    [SerializeField] private int _layerMask = 0;

    private SerializedObject _paintWindowSO;
    private SerializedProperty _brushSizeProp;
    private SerializedProperty _prefabToPaintProp;
    private SerializedProperty _prefabMinCountProp;
    private SerializedProperty _prefabMaxCountProp;
    private SerializedProperty _layerMaskProp;

    private const float RAYCAST_HEIGHT_OFFSET = 3f;
    private const float RAYCAST_MAX_DISTANCE = 10f;

    void OnEnable() 
    { 
        SceneView.duringSceneGui += this.OnSceneGUI;

        _paintWindowSO = new SerializedObject(this);
        _brushSizeProp = _paintWindowSO.FindProperty("_brushSize");
        _prefabToPaintProp = _paintWindowSO.FindProperty("_prefabToPaint");
        _prefabMinCountProp = _paintWindowSO.FindProperty("_prefabMinCount");
        _prefabMaxCountProp = _paintWindowSO.FindProperty("_prefabMaxCount");
        _layerMaskProp = _paintWindowSO.FindProperty("_layerMask");
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

        _brushSizeProp.floatValue = EditorGUILayout.FloatField("Brush Size", _brushSize);
        _prefabToPaintProp.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Prefab to Paint", _prefabToPaint, typeof(GameObject), false);

        Rect rect = GUILayoutUtility.GetRect(0, 20);
        //_layerMaskProp.intValue = EditorGUI.LayerField(rect, "Layer Mask", _layerMask); // Placeholder for Layer Mask field

        GUILayout.Label("Prefab Count", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        _prefabMinCountProp.intValue = EditorGUILayout.IntField("Min", _prefabMinCount);
        _prefabMaxCountProp.intValue = EditorGUILayout.IntField("Max", _prefabMaxCount);
        EditorGUILayout.EndHorizontal();

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
        //Debug.Log("Raycast from: " + ray.origin);
        if (Physics.Raycast(ray, out RaycastHit hit, RAYCAST_MAX_DISTANCE))
        {
            
            PlacePrefabAtPoint(hit.point, hit.normal);
        }
    }
}
