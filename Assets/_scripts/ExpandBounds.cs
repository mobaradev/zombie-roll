using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ExpandBounds : MonoBehaviour
{
    // Set this in the Inspector. 
    // This is the maximum distance (in world units) your shader
    // might displace a vertex.
    public float maxDisplacement = 5.0f;

    void Awake()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.mesh != null)
        {
            Mesh mesh = meshFilter.mesh;

            // Optional: Recalculate just in case they are off
            // mesh.RecalculateBounds(); 

            Bounds bounds = mesh.bounds;

            // Expand the bounds by your max displacement in all directions
            bounds.Expand(maxDisplacement * 2.0f); // Expand takes a total size, so multiply by 2

            mesh.bounds = bounds;
        }
    }
}