using System;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectMud.Systems.MeshGenerator
{
    public struct Quad
    {
        public List<VertexInfo> Vertices;
        public List<int> Triangles;

        public Quad(int offset, params Vector3[] vertexPositions)
        {
            Vertices = new List<VertexInfo>();
            Triangles = new List<int>();

            for (int i = 0; i < vertexPositions.Length; i++)
            {
                Vertices.Add(new VertexInfo(offset + i, vertexPositions[i]));
            }

            CalculateUVs();
            SetTriangles();

        }

        private void SetTriangles()
        {
            int a = Vertices[0].Index;
            int b = Vertices[1].Index;
            int c = Vertices[2].Index;
            int d = Vertices[3].Index;

            Triangles.Add(a);
            Triangles.Add(b);
            Triangles.Add(c);
            //
            Triangles.Add(c);
            Triangles.Add(d);
            Triangles.Add(a);
        }

        private void CalculateUVs()
        {
            VertexInfo a = Vertices[0];
            VertexInfo b = Vertices[1];
            VertexInfo c = Vertices[2];
            VertexInfo d = Vertices[3];

            float bottomDistanceX = Vector3.Distance(a.Position, d.Position);
            float topDistanceX = Vector3.Distance(b.Position, c.Position);
            float distanceXDifference = topDistanceX - bottomDistanceX;
            float distanceYLeft = Vector3.Distance(a.Position, b.Position);
            float distanceYRight = Vector3.Distance(d.Position, c.Position);

            Vector2 v0 = new Vector2(0, 0);
            Vector2 v1 = new Vector2(0, distanceYLeft);
            Vector2 v2 = new Vector2(topDistanceX, distanceYRight);
            Vector2 v3 = new Vector2(bottomDistanceX, 0);

            a.uv = v0;
            b.uv = v1;
            c.uv = v2;
            d.uv = v3;

            Vertices[0] = a;
            Vertices[1] = b;
            Vertices[2] = c;
            Vertices[3] = d;
        }

        public List<Vector3> GetVertices()
        {
            return new List<Vector3>
            {
                Vertices[0].Position,
                Vertices[1].Position,
                Vertices[2].Position,
                Vertices[3].Position
            };
        }
        public List<Vector2> GetUVs()
        {
            return new List<Vector2>
            {
                Vertices[0].uv,
                Vertices[1].uv,
                Vertices[2].uv,
                Vertices[3].uv
            };
        }
        public List<int> GetTriangles()
        {
            return Triangles;
        }   
    }
}

    
