using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    public static LevelCompleteManager Instance;

    [Header("Victory UI")]
    public GameObject victoryPanel;

    [Header("Victory Sound")]
    public AudioClip victorySound;

    [Header("Next Level")]
    public string nextLevelName;

    private bool levelCompleted = false;

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
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        Debug.Log("🏆 LEVEL COMPLETE!");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (victorySound != null)
        {
            AudioSource.PlayClipAtPoint(
                victorySound,
                Camera.main.transform.position
            );
        }

        Time.timeScale = 0f;
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(nextLevelName))
        {
            Debug.LogError("❌ Next Level Name is empty!");
            return;
        }

        Debug.Log("Loading next level: " + nextLevelName);

        SceneManager.LoadScene(nextLevelName);
    }
}