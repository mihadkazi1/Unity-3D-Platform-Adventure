using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [Header("START MENU")]
    public GameObject startMenuCanvas;

    [Header("GAME UI")]
    public GameObject gameUICanvas;

    [Header("PLAYER")]
    public GameObject player;

    [Header("MUSIC")]
    public AudioSource menuMusic;
    public AudioSource playMusic;
    public AudioSource gameMusic;

    // Remembers that PLAY GAME was already clicked.
    // This survives a scene restart.
    private static bool gameHasStarted = false;

    private bool playButtonPressed = false;

    private void Start()
    {
        // ==========================================
        // FIRST TIME OPENING THE GAME
        // ==========================================

        if (!gameHasStarted)
        {
            if (startMenuCanvas != null)
                startMenuCanvas.SetActive(true);

            if (gameUICanvas != null)
                gameUICanvas.SetActive(false);

            Time.timeScale = 0f;

            if (menuMusic != null)
            {
                menuMusic.loop = true;
                menuMusic.Play();
            }

            if (playMusic != null)
                playMusic.Stop();

            if (gameMusic != null)
                gameMusic.Stop();

            return;
        }

        // ==========================================
        // SCENE RELOADED AFTER PLAYER DIED
        // ==========================================

        // DO NOT show Start Menu again.
        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(false);

        // Show normal game UI.
        if (gameUICanvas != null)
            gameUICanvas.SetActive(true);

        Time.timeScale = 1f;

        if (menuMusic != null)
            menuMusic.Stop();

        if (playMusic != null)
            playMusic.Stop();

        // Restart gameplay background music.
        if (gameMusic != null)
        {
            gameMusic.loop = true;
            gameMusic.Play();
        }

        Debug.Log("GAME RESTARTED AFTER DEATH");
    }

    public void PlayGame()
    {
        if (playButtonPressed)
            return;

        playButtonPressed = true;
        gameHasStarted = true;

        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(false);

        if (gameUICanvas != null)
            gameUICanvas.SetActive(true);

        if (menuMusic != null)
            menuMusic.Stop();

        // Start gameplay immediately.
        Time.timeScale = 1f;

        // Play the Play button/start sound first.
        if (playMusic != null &&
            playMusic.clip != null)
        {
            playMusic.loop = false;
            playMusic.Play();

            Invoke(
                nameof(StartGameMusic),
                playMusic.clip.length
            );
        }
        else
        {
            StartGameMusic();
        }

        Debug.Log("GAME STARTED!");
    }

    private void StartGameMusic()
    {
        if (gameMusic == null)
            return;

        gameMusic.loop = true;
        gameMusic.Play();
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("QUIT GAME");
    }

    // Use this later for a MAIN MENU / NEW GAME button.
    public static void ResetGameStartState()
    {
        gameHasStarted = false;
    }
}