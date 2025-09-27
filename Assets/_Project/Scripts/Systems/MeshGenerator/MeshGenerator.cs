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

        [SerializeField] private List<Vector3> _points = new List<Vector3>();
        private List<Vector3> _tans = new List<Vector3>();

        [SerializeField] private List<MeshData> _wallMeshs;
        [Tooltip("Layer Assinged to the generated Mesh")]
        [SerializeField] private int _layer = 7;
        [SerializeField] private bool _isConvex = true;

        private float _distanceMesh;

        private void Awake()
        {
            _splineContainer = GetComponent<SplineContainer>();
        }

        private void OnEnable()
        {
            Spline.Changed += OnSplineChanged;
        }

        private void OnDisable()
        {
            Spline.Changed -= OnSplineChanged;
        }

        private void OnSplineChanged(Spline spline, int value, SplineModification modification)
        {
            if(spline == _splineContainer.Spline && modification == SplineModification.KnotModified)
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

        private void GenerateMesh()
        {
            CreateGameObject();


            List<CombineInstance> instances = new();

            Vector3 position;
            Matrix4x4 offsetMatrix;
            CombineInstance combineInstance;

            for (int i = 0; i < _points.Count; i++)
            {
                MeshData meshData = GetRandomMeshData();
                Mesh meshInstance = Instantiate(meshData.Mesh);
                


                Vector3 dir;
                Vector3 scale = Vector3.one + Vector3.up * 5;
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

                position = _points[i];

                Vector3 lookDir = new Vector3(-dir.z, dir.y, dir.x);
                offsetMatrix = Matrix4x4.TRS(position, Quaternion.LookRotation(lookDir), scale);

                for (int j = 0; j < meshInstance.subMeshCount; j++)
                {
                    combineInstance = new CombineInstance
                    {
                        mesh = meshInstance,
                        transform = offsetMatrix,
                        subMeshIndex = j,
                    };
                    instances.Add(combineInstance);
                }
            }

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
            //CHANGE LATER
            renderer.materials = _wallMeshs[0].Materials;

            MeshCollider meshCollider = _childMesh.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = finalMesh;
            meshCollider.convex = _isConvex;
            #endregion
        }

        private List<SubMeshDescriptor> InstancesToSubMeshData(List<CombineInstance> instances)
        {
            List<SubMeshDescriptor> descriptors = new List<SubMeshDescriptor>();

            int triangleOffset = 0;

            foreach (var c1 in instances)
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
                m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
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