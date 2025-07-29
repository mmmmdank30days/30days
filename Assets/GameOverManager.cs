using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject endGameOverlay;
    public TMP_Text endGameMessage;

    public void TriggerLoss(string reason)
    {
        Debug.Log("💥 Game Over: " + reason);
        if (endGameOverlay) endGameOverlay.SetActive(true);
        if (endGameMessage)
        {
            endGameMessage.text = reason;
            endGameMessage.color = Color.red;
        }

        Time.timeScale = 0f; // Freeze game
    }
}
