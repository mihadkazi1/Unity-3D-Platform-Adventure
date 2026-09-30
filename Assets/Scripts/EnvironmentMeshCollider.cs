using UnityEngine;

public class EnvironmentMeshCollider : MonoBehaviour
{
    [ContextMenu("Setup Environment Mesh Colliders")]
    public void SetupColliders()
    {
        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>(true);

        int added = 0;
        int existing = 0;

        foreach (MeshFilter meshFilter in meshes)
        {
            if (meshFilter.sharedMesh == null)
                continue;

            GameObject obj = meshFilter.gameObject;

            // Ignore the player if it somehow exists under Env
            if (obj.CompareTag("Player"))
                continue;

            // Check for existing collider
            Collider existingCollider = obj.GetComponent<Collider>();

            if (existingCollider != null)
            {
                existing++;
                continue;
            }

            MeshCollider meshCollider =
                obj.AddComponent<MeshCollider>();

            meshCollider.sharedMesh =
                meshFilter.sharedMesh;

            // Environment is static, so non-convex is okay.
            meshCollider.convex = false;

            meshCollider.isTrigger = false;

            added++;
        }

        Debug.Log(
            "Environment Mesh Colliders Complete. " +
            "Added: " + added +
            " | Existing: " + existing
        );
    }
}