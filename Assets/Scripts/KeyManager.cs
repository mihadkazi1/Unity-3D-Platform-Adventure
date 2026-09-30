using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    [Header("Key")]
    public GameObject keyObject;

    [Header("Sound")]
    public AudioClip keyCollectSound;

    [Range(0f, 1f)]
    public float soundVolume = 1f;

    private bool keyAvailable = false;
    private bool keyCollected = false;

    public bool HasKey
    {
        get { return keyCollected; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("🔑 KeyManager READY");
    }

    private void Start()
    {
        keyAvailable = false;
        keyCollected = false;

        if (keyObject == null)
        {
            Debug.LogError("❌ KEY OBJECT IS NOT ASSIGNED!");
            return;
        }

        // Hide key at beginning
        keyObject.SetActive(false);

        Debug.Log("🔑 Key hidden at start.");
    }

    public void ShowKey()
    {
        if (keyAvailable)
            return;

        if (keyObject == null)
        {
            Debug.LogError("❌ Cannot show key! Key Object is NULL!");
            return;
        }

        keyAvailable = true;

        keyObject.SetActive(true);

        Debug.Log("🔑🔑🔑 KEY APPEARED! 🔑🔑🔑");
    }

    public void CollectKey()
    {
        if (!keyAvailable)
        {
            Debug.LogWarning("⚠️ Key cannot be collected yet.");
            return;
        }

        if (keyCollected)
            return;

        keyCollected = true;

        // Hide key
        if (keyObject != null)
        {
            keyObject.SetActive(false);
        }

        Debug.Log("🔑 KEY COLLECTED!");

        // Play key sound
        PlayKeySound();

        // Show UI message
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowKeyCollected();
        }
        else
        {
            Debug.LogError("❌ GameUIManager.Instance is NULL!");
        }
    }

    private void PlayKeySound()
    {
        if (keyCollectSound == null)
        {
            Debug.LogWarning("⚠️ Key collect sound is not assigned!");
            return;
        }

        // Create temporary audio object
        GameObject soundObject = new GameObject("KeyCollectSound");

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        // Make it 2D
        audioSource.spatialBlend = 0f;

        // Full volume
        audioSource.volume = soundVolume;

        // Don't play automatically
        audioSource.playOnAwake = false;

        // Play
        audioSource.PlayOneShot(keyCollectSound);

        // Destroy after sound finishes
        Destroy(soundObject, keyCollectSound.length + 0.1f);
    }
}