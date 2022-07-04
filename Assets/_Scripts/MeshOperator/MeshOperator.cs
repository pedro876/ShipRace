using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshOperator
{
    private static readonly int[] edgeOrder = new int[] { 0, 1, 0, 2, 1, 2 };

    public Vector3[] Vertices { get; private set; }
    public Vector3[] Normals { get; private set; }
    public Vector2[] UV { get; private set; }
    public int[] Triangles { get; private set; }

    #region Creation

    public MeshOperator(Mesh mesh, bool deepCopy = false)
    {
        if (deepCopy)
        {
            DeepCopy(mesh.vertices, mesh.normals, mesh.uv, mesh.triangles);
        }
        else
        {
            Vertices = mesh.vertices;
            Normals = mesh.normals;
            UV = mesh.uv;
            Triangles = mesh.triangles;
        }
    }

    public MeshOperator(Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, bool deepCopy = false)
    {
        if (deepCopy)
        {
            DeepCopy(vertices, normals, uv, triangles);
        }
        else
        {
            this.Vertices = vertices;
            this.Normals = normals;
            this.UV = uv;
            this.Triangles = triangles;
        }
    }

    private void DeepCopy(Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles)
    {
        this.Vertices = new Vector3[vertices.Length];
        vertices.CopyTo(this.Vertices, 0);
        this.Normals = new Vector3[normals.Length];
        normals.CopyTo(this.Normals, 0);
        this.UV = new Vector2[uv.Length];
        uv.CopyTo(this.UV, 0);
        this.Triangles = new int[triangles.Length];
        triangles.CopyTo(this.Triangles, 0);
    }

    #endregion

    #region FindVertex

    public (Vector3 v, int idx) FindClosestVertex(ref Vector3 p)
    {
        int closestVertex = 0;
        float minDistance = float.MaxValue;
        for (int i = 0; i < Vertices.Length; i++)
        {
            float dist = Vector3.Distance(p, Vertices[i]);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestVertex = i;
            }
        }
        if(closestVertex >= 0)
        {
            return (Vertices[closestVertex], closestVertex);
        }
        else
        {
            return (Vector3.zero, -1);
        }
    }

    #endregion

    #region FindEdge

    public Edge FindClosestEdge(ref Vector3 p)
    {
        /*
         * origin = inicio del segmento
         * a = origin -> p
         * b = origin -> fin del segmento
         * dot(a,b) = dist proyectada sobre b de a hacia fin de segmento
         * si dot(a,b) > |(a,b)| se sale por la derecha, si es menor que 0 por la izquierda
         * en otro caso está dentro del segmento
         */

        float minDist = float.MaxValue;
        int src = 0;
        int dst = 0;
        for(int i = 0; i < Triangles.Length; i += 3)
        {
            for(int j = 0; j < edgeOrder.Length; j += 2)
            {
                ref Vector3 srcP = ref Vertices[Triangles[i + edgeOrder[j]]];
                ref Vector3 dstP = ref Vertices[Triangles[i + edgeOrder[j+1]]];
                Vector3 proj = Geometry.ProjectOnEdge(ref p, ref srcP, ref dstP);
                float dist = (proj - p).magnitude;
                if(dist < minDist)
                {
                    minDist = dist;
                    src = Triangles[i + edgeOrder[j]];
                    dst = Triangles[i + edgeOrder[j + 1]];
                }
            }
        }

        return new Edge(ref Vertices[src], ref Vertices[dst], src, dst);
    }

    #endregion

    #region FindTriangle

    public void ForEachTriangle(Action<int, Vector3, Vector3, Vector3> action)
    {
        for (int loopI = 0; loopI < Triangles.Length; loopI += 3)
        {
            ref Vector3 a = ref Vertices[Triangles[loopI]];
            ref Vector3 b = ref Vertices[Triangles[loopI + 1]];
            ref Vector3 c = ref Vertices[Triangles[loopI + 2]];

            action(loopI, a, b, c);
        }
    }

    public Triangle FindClosestTriangle(Vector3 p)
    {
        float minDist = float.MaxValue;
        int aIdx = 0, bIdx = 0, cIdx = 0;

        ForEachTriangle((loopI, a, b, c) =>
        {
            Vector3 proj = Geometry.ProjectOnTriangle(ref p, ref a, ref b, ref c, out var wasOutside);
            float dist = (p - proj).magnitude;

            if (dist < minDist)
            {
                minDist = dist;
                aIdx = Triangles[loopI];
                bIdx = Triangles[loopI + 1];
                cIdx = Triangles[loopI + 2];
            }
        });

        return new Triangle(ref Vertices[aIdx], ref Vertices[bIdx], ref Vertices[cIdx], aIdx, bIdx, cIdx);
    }

    public Triangle FindClosestTriangleInDir(Vector3 p, Vector3 dir)
    {
        float minDist = float.MaxValue;
        int aIdx = 0, bIdx = 0, cIdx = 0;
        bool anyInside = false;

        ForEachTriangle((loopI, a, b, c) =>
        {
            Vector3 proj = Geometry.ProjectLineOnTriangle(ref p, ref dir, ref a, ref b, ref c, out var wasOutside);
            float dist = (p - proj).magnitude;

            //Se escoge como mejor triángulo aquel que esté más cerca, dando prioridad a aquellos que intersecten con la recta
            // Si no ocurre que éste no intersecte pero se haya encontrado uno que sí, se descarta
            if (!(wasOutside && anyInside)) 
            {
                //Se escoge como mejor si la distancia es menor o intersecta y aún no se había
                //encontrado ningún triángulo que intersectara con la recta
                if ((dist < minDist || (!anyInside && !wasOutside))) 
                {
                    minDist = dist;
                    aIdx = Triangles[loopI];
                    bIdx = Triangles[loopI + 1];
                    cIdx = Triangles[loopI + 2];

                    if (!wasOutside)
                    {
                        anyInside = true;
                    }
                        
                }
            }
            
        });

        return new Triangle(ref Vertices[aIdx], ref Vertices[bIdx], ref Vertices[cIdx], aIdx, bIdx, cIdx);
    }

    #endregion

    #region uv

    public List<Triangle> FindClosestTrianglesInUVs(ref Vector3 p)
    {
        float minDist = float.MaxValue;
        List<Triangle> triangles = new List<Triangle>();
        Vector3 localP = p;

        ForEachTriangle((loopI, a, b, c) =>
        {
            //Se obtienen los vértices de la malla destino
            int v0id = Triangles[loopI];
            int v1id = Triangles[loopI + 1];
            int v2id = Triangles[loopI + 2];

            //Se obtienen los uv de la malla destino transformadas al tamaño de textura
            Vector3 uv0 = new Vector3(UV[v0id].x, UV[v0id].y, 0);
            Vector3 uv1 = new Vector3(UV[v1id].x, UV[v1id].y, 0);
            Vector3 uv2 = new Vector3(UV[v2id].x, UV[v2id].y, 0);

            Vector3 proj = Geometry.ProjectOnTriangle(ref localP, ref uv0, ref uv1, ref uv2, out var wasOutside);
            float dist = Vector3.Distance(proj, localP);

            if (dist < minDist)
            {
                triangles = new List<Triangle>();
                triangles.Add(new Triangle(ref uv0, ref uv1, ref uv2, v0id, v1id, v2id));
                minDist = dist;
            }
            else if (dist.Equals(minDist))
            {
                triangles.Add(new Triangle(ref uv0, ref uv1, ref uv2, v0id, v1id, v2id));
            }
        });

        return triangles;
    }

    public List<Vector3> UVToWorldSpace(ref Vector3 p, out List<Triangle> triangles)
    {
        triangles = FindClosestTrianglesInUVs(ref p);

        List<Vector3> positions = new List<Vector3>();

        foreach(var triangle in triangles)
        {
            Vector3 proj = Geometry.ProjectOnTriangle(
            ref p, ref triangle.v0, ref triangle.v1, ref triangle.v2, out var wasOutside);
            Vector3 spacePoint = Geometry.TransformPointBetweenTriangles(
                proj, ref triangle.v0, ref triangle.v1, ref triangle.v2,
                ref Vertices[triangle.i0], ref Vertices[triangle.i1], ref Vertices[triangle.i2]);
            positions.Add(spacePoint);
        }

        return positions;
    }

    #endregion

    #region FindInMesh

    public Vector3 FindClosestPointInMesh(ref Vector3 p, out Triangle triangle)
    {
        triangle = FindClosestTriangle(p);
        return Geometry.ProjectOnTriangle(ref p, ref triangle.v0, ref triangle.v1, ref triangle.v2, out var wasOutside);
    }

    public Vector3 FindClosestNormalInMesh(ref Vector3 p/*, ref Vector3 dir*/)
    {
        /*var triangle = FindClosestTriangleInDir(p, dir);
        var pOnTriangle = GeometryOps.ProjectLineOnTriangle(ref p, ref dir,
            ref Vertices[triangle.i0],
            ref Vertices[triangle.i1],
            ref Vertices[triangle.i2], out var wasOutside);
        */
        var triangle = FindClosestTriangle(p);
        var pOnTriangle = Geometry.ProjectOnTriangle(ref p,
            ref Vertices[triangle.i0],
            ref Vertices[triangle.i1],
            ref Vertices[triangle.i2], out var wasOutside);

        (float w1, float w2, float w3) = Geometry.GetLerpWeightsForPointInTriangle(ref pOnTriangle, 
            ref Vertices[triangle.i0],
            ref Vertices[triangle.i1],
            ref Vertices[triangle.i2]);

        Vector3 normal = Normals[triangle.i0] * w1 +
            Normals[triangle.i1] * w2 +
            Normals[triangle.i2] * w3;


        return normal.normalized;
    }

    #endregion
}
