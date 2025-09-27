using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;
using ProjectMud.Utils;
using static UnityEngine.Mesh;
using System;

namespace ProjectMud.Systems.MeshGenerator
{
    [RequireComponent(typeof(SplineContainer)), ExecuteInEditMode()]
    public class MeshGenerator : MonoBehaviour
    {
        GameObject _childMesh;
        private SplineContainer _splineContainer;

        private List<Vector3> _points = new List<Vector3>();
        private List<Vector3> _tans = new List<Vector3>();
        [SerializeField, Min(1)]private int _height = 1;
        [SerializeField] private float _topHeight = 0;
        [SerializeField, Range(0f,1f)] private float _topEdgePoint;

        [SerializeField] private List<Material> _materials = new List<Material>();
        [SerializeField] private List<MeshData> _wallMeshs;
        [Tooltip("Layer Assinged to the generated Mesh")]
        private int _layer = 6;
        [SerializeField] private bool _isConvex = true;

        private float _distanceMesh;

        private void Awake()
        {
            _splineContainer = GetComponent<SplineContainer>();
        }

        private void OnEnable()
        {
            if(_childMesh == null && transform.childCount > 0)
            {
                _childMesh = transform.GetChild(0)?.gameObject;
            }
            Spline.Changed += OnSplineChanged;
        }

        private void OnDisable()
        {
            Spline.Changed -= OnSplineChanged;
        }

        private void OnSplineChanged(Spline spline, int value, SplineModification modification)
        {
            if (spline == _splineContainer.Spline && modification == SplineModification.KnotModified)
            {
                CalculatePoints();
                GenerateMesh();
            }
        }

        private void OnValidate()
        {
            if (_wallMeshs == null) return;
            if (_wallMeshs.Count == 0) return;
            if (_wallMeshs[0].Mesh == null) return;

            _distanceMesh = _wallMeshs[0].Mesh.bounds.size.x;

        }

        private void CalculatePoints()
        {
            _points = new List<Vector3>();
            _tans = new List<Vector3>();

            Spline spline = _splineContainer.Spline;

            _points.Add(spline.EvaluatePosition(0));
            _tans.Add(spline.EvaluateTangent(0));

            if (_distanceMesh <= 0) return;

            spline.GetPointAtLinearDistance(0, _distanceMesh, out float dist);

            while (dist < 1f)
            {
                _points.Add(spline.EvaluatePosition(dist));
                _tans.Add(spline.EvaluateTangent(dist));

                spline.GetPointAtLinearDistance(dist, _distanceMesh, out dist);
            }

        }

        private MeshData GetRandomMeshData()
        {
            float[] weights = new float[_wallMeshs.Count];
            for (int i = 0; i < _wallMeshs.Count; i++)
            {
                weights[i] = _wallMeshs[i].Weight;
            }
            int index = Chance.WeightedChance(weights);
            return _wallMeshs[index];
        }

        [ContextMenu("Update")]
        private void GenerateMesh()
        {
            CreateGameObject();


            List<CombineInstance> instances = new();

            Vector3 position;
            Matrix4x4 offsetMatrix;
            CombineInstance combineInstance;

            Dictionary<MeshData, Mesh> meshVariants = new Dictionary<MeshData, Mesh>();

            for (int i = 0; i < _wallMeshs.Count; i++)
            {
                Mesh m = Instantiate(_wallMeshs[i].Mesh);
                meshVariants.Add(_wallMeshs[i], m);
            }


            Mesh meshInstance = meshVariants[_wallMeshs[0]];

            for (int h = 0; h < _height; h++)
            {
                for (int i = 0; i < _points.Count; i++)
                {
                    MeshData meshData = GetRandomMeshData();
                    meshInstance = meshVariants[meshData];
                    Vector3 dir;
                    Vector3 scale = Vector3.one;
                    float dist;

                    if (i == _points.Count - 1)
                    {
                        dist = Vector3.Distance(_points[0], _points[_points.Count - 1]);

                        if (dist < 0.01f) { return; }

                        dir = _points[i] - _points[0];
                        scale.x = dist / meshInstance.bounds.size.x;
                    }
                    else
                    {
                        dist = Vector3.Distance(_points[i], _points[i + 1]);
                        scale.x = dist / meshInstance.bounds.size.x;

                        dir = _points[i] - _points[i + 1];
                    }

                    position = _points[i] + new Vector3(0, h * meshInstance.bounds.size.y);

                    Vector3 lookDir = new Vector3(-dir.z, dir.y, dir.x);
                    offsetMatrix = Matrix4x4.TRS(position, Quaternion.LookRotation(lookDir), scale);

                    for (int j = 0; j < meshInstance.subMeshCount; j++)
                    {
                        combineInstance = new CombineInstance();

                        combineInstance.mesh = meshInstance;
                        combineInstance.transform = offsetMatrix;
                        combineInstance.subMeshIndex = j;
                        instances.Add(combineInstance);
                    }
                }
            }

            BuildTopMeshData(instances);

            instances = CombineBySubmeshIndex(instances);
            List<SubMeshDescriptor> subMeshes = InstancesToSubMeshData(instances);

            #region FinalMesh

            Mesh finalMesh = new Mesh();
            finalMesh.name = "WallMesh";
            finalMesh.indexFormat = IndexFormat.UInt32;

            finalMesh.CombineMeshes(instances.ToArray(), false);
            finalMesh.SetSubMeshes(subMeshes.ToArray());

            MeshFilter filter = _childMesh.GetComponent<MeshFilter>();
            filter.sharedMesh = finalMesh;

            MeshRenderer renderer = _childMesh.GetComponent<MeshRenderer>();
            renderer.materials = _materials.ToArray();

            MeshCollider meshCollider = _childMesh.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = finalMesh;
            meshCollider.convex = _isConvex;
            #endregion
        }

        private void BuildTopMeshData(List<CombineInstance> instances)
        {
            Vector3 center = new Vector3();
            foreach (Vector3 point in _points)
            {
                center += point;
            }
            center = center / _points.Count;
            center.y += _topHeight;

            float tileHeight = _wallMeshs[0].Mesh.bounds.size.y;

            Vector3 height = new Vector3(0, tileHeight * _height, 0);

            List<Quad> faces = new List<Quad>();
            for (int i = 1; i <= _points.Count; i++)
            {
                Vector3 p1 = _points[i - 1];
                Vector3 p2 = (i == _points.Count) ? _points[0] : _points[i];
                Vector3 p3 = Vector3.Lerp(p1, center, _topEdgePoint);
                Vector3 p4 = Vector3.Lerp(p2, center, _topEdgePoint);

                p1 += height;
                p2 += height;
                p3 += height;
                p4 += height;

                Quad q = new Quad((i - 1) * 4, new Vector3[] { p1, p3, p4, p2 });
                faces.Add(q);
            }

            Mesh mesh = GetMeshFromQuads(faces);

            CombineInstance topMeshData = new CombineInstance();

            topMeshData.transform = Matrix4x4.identity;
            topMeshData.mesh = mesh;
            topMeshData.subMeshIndex = 0;

            instances.Add(topMeshData);
        }

        private Mesh GetMeshFromQuads(List<Quad> faces)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            float distance = 0f;
            int count = 0;

            foreach (Quad quad in faces)
            {
                tris.AddRange(quad.Triangles);
                verts.AddRange(quad.GetVertices());

                if(count > 0)
                {
                    distance += Vector3.Distance(faces[count - 1].Vertices[0].Position, quad.Vertices[0].Position);
                }

                count++;
                uvs.AddRange(quad.GetUVs());
            }

            Mesh mesh = new Mesh();
            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();

            return mesh;
        }

        private List<SubMeshDescriptor> InstancesToSubMeshData(List<CombineInstance> instances)
        {
            List<SubMeshDescriptor> descriptors = new List<SubMeshDescriptor>();

            int triangleOffset = 0;

            foreach (CombineInstance c1 in instances)
            {
                Mesh mesh = c1.mesh;
                Matrix4x4 transform = c1.transform;
                int[] meshTris = mesh.GetTriangles(0);
                descriptors.Add(new SubMeshDescriptor(triangleOffset, meshTris.Length));
                triangleOffset += meshTris.Length;
            }

            return descriptors;
        }

        private List<CombineInstance> CombineBySubmeshIndex(List<CombineInstance> meshes)
        {
            Dictionary<int, List<CombineInstance>> instanceDictionary = new Dictionary<int, List<CombineInstance>>();
            foreach (CombineInstance item in meshes)
            {
                if (instanceDictionary.TryGetValue(item.subMeshIndex, out List<CombineInstance> values))
                {
                    values.Add(item);
                }
                else
                {
                    instanceDictionary.Add(item.subMeshIndex, new List<CombineInstance> { item });
                }
            }

            List<CombineInstance> inst = new List<CombineInstance>();

            foreach (var valueSet in instanceDictionary.Values)
            {
                List<CombineInstance> combinedInstances = valueSet;
                Mesh m = new Mesh();
                m.indexFormat = IndexFormat.UInt32;
                m.CombineMeshes(combinedInstances.ToArray(), true);

                CombineInstance c = new CombineInstance();
                c.transform = Matrix4x4.identity;
                c.mesh = m;
                c.subMeshIndex = combinedInstances[0].subMeshIndex;
                inst.Add(c);
            }

            return inst;
        }

        private void CreateGameObject()
        {
            if (_childMesh != null)
            {
                DestroyImmediate(_childMesh);
            }

            _childMesh = new GameObject("Wall", typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider));
            _childMesh.transform.SetParent(transform, false);
            _childMesh.layer = _layer;
        }
    }
}