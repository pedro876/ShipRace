using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDebug : MonoBehaviour
{
    private List<Mesh> meshes;

    [Header("DEBUG")]
    [SerializeField] bool DEBUG = true;
    [SerializeField] bool drawMesh = true;
    [SerializeField] Color meshColor = Color.red;

   #region UNITY_CALLBACKS

    void Awake()
    {
        meshes = new List<Mesh>();

        foreach(var meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            meshes.Add(meshRenderer.sharedMesh);
        }

        foreach(var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            meshes.Add(meshFilter.sharedMesh);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (!DEBUG) return;

        if (drawMesh)
        {
            Gizmos.color = meshColor;
            foreach(var mesh in meshes)
            {
                var triangles = mesh.triangles;
                var vertices = mesh.vertices;
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    Gizmos.DrawLine(vertices[triangles[i]]+transform.position, vertices[triangles[i + 1]] + transform.position);
                    Gizmos.DrawLine(vertices[triangles[i]] + transform.position, vertices[triangles[i + 2]] + transform.position);
                    Gizmos.DrawLine(vertices[triangles[i + 1]] + transform.position, vertices[triangles[i + 2]] + transform.position);
                }
            }
        }
    }

    #endregion

    
}
