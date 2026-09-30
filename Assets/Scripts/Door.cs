using UnityEngine;

public class Door : MonoBehaviour
{
    private bool doorTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (doorTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        Debug.Log("🚪 PLAYER REACHED DOOR!");

        // Check if KeyManager exists
        if (KeyManager.Instance == null)
        {
            Debug.LogError("❌ KeyManager Instance is NULL!");
            return;
        }

        // Check key
        if (KeyManager.Instance.HasKey)
        {
            Debug.Log("✅ KEY FOUND! OPENING DOOR!");

            doorTriggered = true;

            if (LevelCompleteManager.Instance != null)
            {
                LevelCompleteManager.Instance.CompleteLevel();
            }
            else
            {
                Debug.LogError(
                    "❌ LevelCompleteManager Instance is NULL!"
                );
            }
        }
        else
        {
            Debug.Log(
                "❌ PLAYER DOES NOT HAVE THE KEY!"
            );

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowNeedKey();
            }
        }
    }
}