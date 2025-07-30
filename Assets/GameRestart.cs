using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("Restart method!");
        Time.timeScale = 1f; // Unpause if paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}