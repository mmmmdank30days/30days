using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        // If running in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a built application
        Application.Quit();
#endif
    }
}
