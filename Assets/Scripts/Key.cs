using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioClip keyCollectSound;

    [Range(0f, 1f)]
    public float volume = 1f;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        collected = true;

        Debug.Log("🔑 PLAYER TOUCHED KEY!");

        // Play key sound
        if (keyCollectSound != null)
        {
            AudioSource.PlayClipAtPoint(
                keyCollectSound,
                transform.position,
                volume
            );
        }

        // Tell KeyManager
        if (KeyManager.Instance != null)
        {
            KeyManager.Instance.CollectKey();

            Debug.Log(
                "🔑 KEY STATE: " +
                KeyManager.Instance.HasKey
            );
        }
        else
        {
            Debug.LogError("❌ KeyManager Instance is NULL!");
        }
    }
}