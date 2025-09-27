using System;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectMud.Systems.MeshGenerator
{
    [System.Serializable]
    public class MeshData
    {
        [SerializeField] private Mesh _mesh;
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

        #endregion
    }
}