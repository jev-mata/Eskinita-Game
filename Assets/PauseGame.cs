using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class PauseGame : MonoBehaviour
{
    public GameObject pauseIcon;  // Icon to show when paused
    public GameObject pauseOverlay;  // Optional overlay (if used)

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if (pauseIcon != null)
            pauseIcon.SetActive(true);

        if (pauseOverlay != null)
            pauseOverlay.SetActive(true);
    }

    void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseIcon != null)
            pauseIcon.SetActive(false);

        if (pauseOverlay != null)
            pauseOverlay.SetActive(false);
    }
}

