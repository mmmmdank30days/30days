using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public Button playAgainButton; // ← NEW

    private float impactCountdown = 1f;
    private bool gameEnded = false;
    private bool asteroidReleased = false;
    public Terrain terrain;

    public TerrainGenerator terrainGenerator;

    void Start()
    {

        if (terrainGenerator != null)
            terrainGenerator.GenerateTerrain(); // ⛰️ Regenerate terrain at game start

        RaiseTerrainBaseline(10f);
        timeLeft = dayDuration;
        safeZone = FindFirstObjectByType<SafeZone>();
        UpdateUI();

        GameState.ControlsEnabled = true;

        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(RestartGame);
        }
    }

    void Update()
    {
        if (currentDay > totalDays + impactCountdown + 2)
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

        GameState.ControlsEnabled = false;
        DisablePlayerControls();

        // ✅ Unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

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

        Time.timeScale = 0f;
        playAgainButton.interactable = true;
    }

    void RestartGame() // ← NEW
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DisablePlayerControls()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Disable PlayerInput
            var input = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
            if (input != null)
                input.enabled = false;

            // Disable StarterAssetsInputs
           // var starterInput = player.GetComponent<CharacterController.StarterAssetsInputs>();
         //   if (starterInput != null)
           //     starterInput.enabled = false;

            // Disable ThirdPersonController
           // var controller = player.GetComponent<CharacterController.ThirdPersonController>();
          //  if (controller != null)
            //    controller.enabled = false;
        }

        // Disable camera follow (optional)
      //  var cinemachine = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
       // if (cinemachine != null)
         //   cinemachine.enabled = false;
    }

    void RaiseTerrainBaseline(float targetHeight = 0.3f)
    {
        TerrainData tData = terrain.terrainData;
        int res = tData.heightmapResolution;
        float[,] heights = new float[res, res];

        for (int x = 0; x < res; x++)
        {
            for (int z = 0; z < res; z++)
            {
                heights[x, z] = targetHeight;
            }
        }

        tData.SetHeights(0, 0, heights);
    }


}
