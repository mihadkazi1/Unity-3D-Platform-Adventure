using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("Coins")]
    public int totalCoins = 16;
    public int collectedCoins = 0;

    [Header("UI")]
    public TMP_Text coinText;

    private bool finished = false;

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
        collectedCoins = 0;
        finished = false;

        UpdateCoinText();

        Debug.Log("🪙 CoinManager Started. Total Coins = " + totalCoins);
    }

    public void CollectCoin()
    {
        if (finished)
            return;

        if (collectedCoins >= totalCoins)
            return;

        collectedCoins++;

        UpdateCoinText();

        Debug.Log(
            "🪙 COIN COLLECTED: " +
            collectedCoins + " / " + totalCoins
        );

        // ALL COINS
        if (collectedCoins == totalCoins)
        {
            finished = true;

            Debug.Log("🎉🎉 ALL COINS COLLECTED! 🎉🎉");

            // Show message
            if (GameUIManager.Instance != null)
            {
                Debug.Log("✅ Calling ShowAllCoinsCollected()");
                GameUIManager.Instance.ShowAllCoinsCollected();
            }
            else
            {
                Debug.LogError("❌ GameUIManager.Instance is NULL!");
            }

            // Show key
            if (KeyManager.Instance != null)
            {
                Debug.Log("🔑 Calling KeyManager.ShowKey()");
                KeyManager.Instance.ShowKey();
            }
            else
            {
                Debug.LogError("❌ KeyManager.Instance is NULL!");
            }
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = collectedCoins + " / " + totalCoins;
        }
        else
        {
            Debug.LogError("❌ Coin Text is not assigned!");
        }
    }
}