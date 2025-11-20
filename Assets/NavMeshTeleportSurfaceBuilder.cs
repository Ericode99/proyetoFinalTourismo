using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(TeleportationArea))]
public class NavMeshTeleportSurfaceBuilder : MonoBehaviour
{
    [Tooltip("Rebuild from baked NavMesh on play. Turn off after you save the mesh as an asset.")]
    public bool rebuildOnAwake = true;

    void Awake()
    {
        if (rebuildOnAwake) RebuildFromNavMesh();
    }

    [ContextMenu("Rebuild From NavMesh (Editor)")]
    public void RebuildFromNavMesh()
    {
        var tri = NavMesh.CalculateTriangulation();
        if (tri.vertices == null || tri.vertices.Length == 0)
        {
            Debug.LogWarning("No NavMesh triangulation found. Make sure a NavMeshSurface is baked.");
            return;
        }

        var mesh = new Mesh { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        mesh.name = "Teleport_NavMeshProxy";
        mesh.vertices = tri.vertices;
        mesh.triangles = tri.indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var mf = GetComponent<MeshFilter>();
        var mc = GetComponent<MeshCollider>();

        mf.sharedMesh = mesh;
        mc.sharedMesh = mesh; // collider uses same mesh
    }
}
