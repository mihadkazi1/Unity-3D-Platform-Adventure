using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Key")]
    public GameObject key;

    private bool allCoinsCollected = false;
    private bool hasKey = false;

    void Awake()
    {
        Instance = this;

        if (key == null)
        {
            Debug.LogError("LEVEL MANAGER: Key is NOT assigned!");
        }
        else
        {
            // Hide key when game starts
            key.SetActive(false);
            Debug.Log("LEVEL MANAGER: Key hidden at start.");
        }
    }

    public void AllCoinsCollected()
    {
        if (allCoinsCollected)
            return;

        allCoinsCollected = true;

        Debug.Log("================================");
        Debug.Log("ALL COINS COLLECTED!");
        Debug.Log("NOW SHOWING KEY!");
        Debug.Log("================================");

        if (key != null)
        {
            key.SetActive(true);

            Debug.Log(
                "KEY ACTIVE = " + key.activeSelf
            );

            Debug.Log(
                "KEY POSITION = " + key.transform.position
            );
        }
        else
        {
            Debug.LogError(
                "KEY IS NULL! Drag HJD_FP_key00 into LevelManager > Key."
            );
        }
    }

    public void CollectKey()
    {
        hasKey = true;

        Debug.Log("======================");
        Debug.Log("KEY COLLECTED!");
        Debug.Log("======================");

        if (key != null)
        {
            key.SetActive(false);
        }
    }

    public bool HasKey()
    {
        return hasKey;
    }

    public void GoToNextLevel()
    {
        int currentScene =
            SceneManager.GetActiveScene().buildIndex;

        int nextScene = currentScene + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogError("No next level found!");
        }
    }
}