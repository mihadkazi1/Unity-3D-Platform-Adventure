using UnityEngine;
using TMPro;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("Message Panel")]
    public GameObject messagePanel;

    [Header("Message Text")]
    public TMP_Text messageText;

    [Header("Settings")]
    public float messageDuration = 4f;

    private Coroutine messageCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        HideMessage();
    }

    // ==========================================
    // ALL COINS COLLECTED
    // ==========================================

    public void ShowAllCoinsCollected()
    {
        ShowMessage(
            "KEY APPEARED!\nGO COLLECT THE KEY!"
        );
    }

    // ==========================================
    // KEY COLLECTED
    // ==========================================

    public void ShowKeyCollected()
    {
        ShowMessage(
            "KEY COLLECTED!\nGO TO THE DOOR!"
        );
    }

    // ==========================================
    // NEED KEY
    // ==========================================

    public void ShowNeedKey()
    {
        ShowMessage(
            "YOU NEED THE\n KEY FIRST!"
        );
    }

    // ==========================================
    // LEVEL COMPLETE
    // ==========================================

    public void ShowLevelComplete()
    {
        ShowMessage(
            "CONGRATULATIONS!\nLEVEL COMPLETE!"
        );
    }

    // ==========================================
    // SHOW MESSAGE
    // ==========================================

    public void ShowMessage(string message)
    {
        if (messageText == null)
        {
            Debug.LogError(
                "❌ GameUIManager: Message Text is NOT assigned!"
            );

            return;
        }

        if (messagePanel == null)
        {
            Debug.LogError(
                "❌ GameUIManager: Message Panel is NOT assigned!"
            );

            return;
        }

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(
            DisplayMessage(message)
        );
    }

    // ==========================================
    // DISPLAY
    // ==========================================

    private IEnumerator DisplayMessage(string message)
    {
        // Turn panel ON
        messagePanel.SetActive(true);

        // Turn text ON
        messageText.gameObject.SetActive(true);

        // Set message
        messageText.text = message;

        Debug.Log("📢 UI MESSAGE: " + message);

        yield return new WaitForSeconds(messageDuration);

        HideMessage();

        messageCoroutine = null;
    }

    // ==========================================
    // HIDE
    // ==========================================

    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
            messageText.gameObject.SetActive(false);
        }

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
}