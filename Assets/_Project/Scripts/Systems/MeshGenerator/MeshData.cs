using System;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectMud.Systems.MeshGenerator
{
    [System.Serializable]
    public class MeshData
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material[] _materials = new Material[] {};
        [SerializeField] private float _weight;

        #region Properties

        public Mesh Mesh
        {
            get => _mesh;
            set => _mesh = value;
        }
        public float Weight
        {
            get => _weight;
            set => _weight = value;
        }
        public Material[] Materials
        {
            get => _materials;
            set => _materials = value;
        }
        public Material Material
        {
            get => _materials[0];
            set
            {
                if (_materials.Length == 0)
                {
                    Array.Resize(ref _materials, 1);
                }

                _materials[0] = value;
                
            }
        }

        #endregion
    }
}