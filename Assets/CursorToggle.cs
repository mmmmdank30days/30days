using UnityEngine;
using UnityEngine.UI;

public class CursorToggle : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject pauseMenuUI; // Assign this in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }
}
