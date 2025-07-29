using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause if paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}