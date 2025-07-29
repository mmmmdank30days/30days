using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameTimer : MonoBehaviour
{
    public float dayDuration = 600f; // 10 min per day
    public int totalDays = 3;
    public TMP_Text dayText;
    public TMP_Text timerText;

    private int currentDay = 1;
    private float timeLeft;
    private SafeZone safeZone;

    public GameObject endGameOverlay;
    public TMP_Text endGameMessage;

    private float impactCountdown = 1f;
    private bool gameEnded = false;

    private bool asteroidReleased = false;

    void Start()
    {
        timeLeft = dayDuration;
        safeZone = FindFirstObjectByType<SafeZone>();
        UpdateUI();
    }

    void Update()
    {
        if (currentDay > totalDays+impactCountdown+2)
            return;

        timeLeft -= Time.deltaTime;
       
        if (timeLeft <= 0f)
        {
            currentDay++;
            if (currentDay > totalDays && asteroidReleased && gameEnded)
            {
                EndGame();
                return;
            }
            else
            {
                if (currentDay >= totalDays && !asteroidReleased && !gameEnded)
                {
                    AsteroidFall asteroid = Object.FindFirstObjectByType<AsteroidFall>();
                    if (asteroid != null)
                    {
                        asteroid.Release();
                        asteroidReleased = true;
                    }
                    timeLeft -= Time.deltaTime;
                }
                else if (currentDay >= totalDays && asteroidReleased && !gameEnded && impactCountdown == 0f)
                {
                    gameEnded = true;
                }
                else
                {
                    timeLeft = dayDuration;
                    if (asteroidReleased) impactCountdown = impactCountdown - 1;
                }
            }
        }
        

        UpdateUI();
    }

    void UpdateUI()
    {
        if (dayText)
            dayText.text = "Day " + currentDay + " / " + totalDays;

        if (timerText)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);
            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }

    void EndGame()
    {


        if (endGameOverlay)
            endGameOverlay.SetActive(true);

        if (safeZone != null && safeZone.playerInside)
        {
            Debug.Log("You survived!");
            if (endGameMessage) endGameMessage.text = "YOU SURVIVED!";
            endGameMessage.color = Color.green;
        }
        else
        {
            Debug.Log("You did not survive.");
            if (endGameMessage) endGameMessage.text = "YOU DID NOT SURVIVE.";
            endGameMessage.color = Color.red;
        }

        // Optionally freeze time
        Time.timeScale = 0f;
    }
}
