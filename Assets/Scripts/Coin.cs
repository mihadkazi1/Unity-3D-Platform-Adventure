using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip collectSound;

    [Range(0f, 1f)]
    public float volume = 1f;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        // Already collected
        if (collected)
            return;

        // Only Player can collect
        if (!other.CompareTag("Player"))
            return;

        // Lock immediately
        collected = true;

        Debug.Log("🪙 COLLECTING: " + gameObject.name);

        // Tell CoinManager
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.CollectCoin();
        }
        else
        {
            Debug.LogError("❌ CoinManager Instance is NULL!");
        }

        // Play pickup sound as 2D sound
        PlayPickupSound();

        // Disable coin
        gameObject.SetActive(false);
    }

    private void PlayPickupSound()
    {
        if (collectSound == null)
        {
            Debug.LogWarning("⚠️ Coin collect sound is not assigned!");
            return;
        }

        // Create temporary audio object
        GameObject soundObject = new GameObject("CoinCollectSound");

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        // Make it 2D
        audioSource.spatialBlend = 0f;

        // Full volume
        audioSource.volume = volume;

        // Don't play automatically
        audioSource.playOnAwake = false;

        // Play
        audioSource.PlayOneShot(collectSound);

        // Destroy after sound finishes
        Destroy(soundObject, collectSound.length + 0.1f);
    }
}