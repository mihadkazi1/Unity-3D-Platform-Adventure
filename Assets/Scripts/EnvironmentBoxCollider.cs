using UnityEngine;

public class EnvironmentBoxCollider : MonoBehaviour
{
    [ContextMenu("Add Box Colliders To Environment")]
    public void AddColliders()
    {
        MeshRenderer[] renderers =
            GetComponentsInChildren<MeshRenderer>();

        int added = 0;

        foreach (MeshRenderer renderer in renderers)
        {
            GameObject obj = renderer.gameObject;

            // Don't add another collider if one already exists
            if (obj.GetComponent<Collider>() != null)
                continue;

            BoxCollider box = obj.AddComponent<BoxCollider>();

            box.isTrigger = false;

            added++;
        }

        Debug.Log(
            "Environment Box Colliders Added: " + added
        );
    }
}