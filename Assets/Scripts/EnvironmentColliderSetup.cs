using UnityEngine;

public class EnvironmentColliderSetup : MonoBehaviour
{
    void Start()
    {
        int added = 0;
        int existing = 0;

        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>(true);

        foreach (MeshFilter meshFilter in meshes)
        {
            if (meshFilter.sharedMesh == null)
                continue;

            GameObject obj = meshFilter.gameObject;

            // Ignore the Player if it somehow exists inside Env
            if (obj.CompareTag("Player"))
                continue;

            MeshCollider collider = obj.GetComponent<MeshCollider>();

            if (collider == null)
            {
                collider = obj.AddComponent<MeshCollider>();
                added++;
            }
            else
            {
                existing++;
            }

            collider.sharedMesh = meshFilter.sharedMesh;
            collider.convex = false;
            collider.isTrigger = false;
        }

        Debug.Log(
            "Environment Collider Setup Complete. Added: "
            + added + " | Existing: " + existing
        );
    }
}