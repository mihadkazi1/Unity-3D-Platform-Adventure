using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerDeathManager : MonoBehaviour
{
    public static PlayerDeathManager Instance;

    [Header("Lives")]
    public int maxLives = 3;

    [Header("Death Settings")]
    public float fallY = -3f;
    public float restartDelay = 0.4f;

    [Header("UI")]
    public TMP_Text livesText;
    public GameObject gameOverPanel;

    [Header("Death Music")]
    public AudioClip deathSound;

    [Range(0f, 1f)]
    public float deathVolume = 1f;

    [Header("Game Over Music")]
    public AudioClip gameOverSound;

    [Range(0f, 1f)]
    public float gameOverVolume = 1f;

    private static int deaths = 0;
    private static bool livesInitialized = false;

    private bool isDead = false;

    private void Awake()
    {
        Instance = this;

        // Initialize lives only when the game starts for the first time.
        if (!livesInitialized)
        {
            deaths = 0;
            livesInitialized = true;
        }
    }

    private void Start()
    {
        isDead = false;

        Time.timeScale = 1f;

        UpdateLivesText();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        // Player falls from the level
        if (!isDead && transform.position.y <= fallY)
        {
            Die();
        }
    }

    // Spike / hazard trigger
    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        if (other.CompareTag("Hazard"))
        {
            Die();
        }
    }

    // Spike / hazard collision
    private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }

    // ==========================================
    // PLAYER DEATH
    // ==========================================

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        deaths++;

        int livesLeft = maxLives - deaths;

        Debug.Log("💀 PLAYER DIED!");
        Debug.Log("❤️ Lives Left: " + livesLeft);

        // ==========================================
        // PLAY DEATH SOUND
        // ==========================================

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(
                deathSound,
                transform.position,
                deathVolume
            );
        }

        // ==========================================
        // GAME OVER
        // ==========================================

        if (deaths >= maxLives)
        {
            GameOver();
            return;
        }

        // ==========================================
        // STILL HAS LIVES
        // ==========================================

        UpdateLivesText();

        StartCoroutine(RestartLevel());
    }

    // ==========================================
    // RESTART AFTER DEATH
    // ==========================================

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // ==========================================
    // UPDATE LIVES TEXT
    // ==========================================

    private void UpdateLivesText()
    {
        if (livesText == null)
            return;

        int livesLeft = maxLives - deaths;

        if (livesLeft == 3)
        {
            livesText.text = "3 TIMES LEFT";
        }
        else if (livesLeft == 2)
        {
            livesText.text = "2 TIMES LEFT";
        }
        else if (livesLeft == 1)
        {
            livesText.text = "1 TIME LEFT";
        }
        else
        {
            livesText.text = "GAME OVER";
        }
    }

    // ==========================================
    // GAME OVER
    // ==========================================

    private void GameOver()
    {
        Debug.Log("☠️ GAME OVER!");

        UpdateLivesText();

        // Stop the game
        Time.timeScale = 0f;

        // Show Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Play Game Over music
        if (gameOverSound != null)
        {
            AudioSource.PlayClipAtPoint(
                gameOverSound,
                Camera.main.transform.position,
                gameOverVolume
            );
        }
    }

    // ==========================================
    // RESTART BUTTON
    // ==========================================

    public void RestartGame()
    {
        Debug.Log("🔄 RESTARTING GAME...");

        // Reset lives
        deaths = 0;
        livesInitialized = true;

        // Make sure game runs normally
        Time.timeScale = 1f;

        // Reload current level
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // ==========================================
    // RESET LIVES
    // ==========================================

    public static void ResetLives()
    {
        deaths = 0;
        livesInitialized = true;
    }
}