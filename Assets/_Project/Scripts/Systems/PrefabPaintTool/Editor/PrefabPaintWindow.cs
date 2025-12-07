using System.CodeDom;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PrefabPaintWindow : EditorWindow
{
    [SerializeField] private float _brushSize = 1.0f;
    [SerializeField] private GameObject _prefabToPaint;
    [SerializeField] private float _prefabMinCount = 1;
    [SerializeField] private float _prefabMaxCount = 5;

    private SerializedObject _paintWindowSO;
    private SerializedProperty _brushSizeProp;
    private SerializedProperty _prefabToPaintProp;
    private SerializedProperty _prefabMinCountProp;
    private SerializedProperty _prefabMaxCountProp;

    private const float RAYCAST_HEIGHT_OFFSET = 3f;

    void OnEnable() 
    { 
        SceneView.duringSceneGui += this.OnSceneGUI;

        _paintWindowSO = new SerializedObject(this);
        _brushSizeProp = _paintWindowSO.FindProperty("_brushSize");
        _prefabToPaintProp = _paintWindowSO.FindProperty("_prefabToPaint");
        _prefabMinCountProp = _paintWindowSO.FindProperty("_prefabMinCount");
        _prefabMaxCountProp = _paintWindowSO.FindProperty("_prefabMaxCount");
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

        _brushSize = EditorGUILayout.FloatField("Brush Size", _brushSize);
        _prefabToPaint = (GameObject)EditorGUILayout.ObjectField("Prefab to Paint", _prefabToPaint, typeof(GameObject), false);


        GUILayout.Label("Prefab Count", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        _prefabMinCount = EditorGUILayout.FloatField("Min", _prefabMinCount);
        _prefabMaxCount = EditorGUILayout.FloatField("Max", _prefabMaxCount);
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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            point = hit.point;
            normal = hit.normal;
            Handles.DrawWireDisc(hit.point, hit.normal, _brushSize);
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
            GetRandomPointInCircle(point, normal);
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

    private void GetRandomPointInCircle(Vector3 point, Vector3 normal)
    {
        Vector2 randomPoint = Random.insideUnitCircle * _brushSize;
        Vector3 tangent = Vector3.Cross(normal, Vector3.up).normalized;
        Vector3 bitangent = Vector3.Cross(normal, tangent).normalized;
        Vector3 offset = tangent * randomPoint.x + bitangent * randomPoint.y;
        point += offset;

        Ray ray = new Ray(point + normal * RAYCAST_HEIGHT_OFFSET, -normal);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            PlacePrefabAtPoint(hit.point, hit.normal);
        }
    }
}
