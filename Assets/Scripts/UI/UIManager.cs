using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Sprint Mode ?")]
    public bool sprintMode = false;

    [Space]
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI respawnChancesText;

    public Text countdownDisplay;

    [Header("Sprint Mode UI")]
    public GameObject gameWinPanel;
    public TextMeshProUGUI positionText;
    //public TextMeshProUGUI totalPositionText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI gameOverRoundText;
    public TextMeshProUGUI gameOverPositionText;
    public TextMeshProUGUI gameWinRoundText;
    public TextMeshProUGUI gameWinPositionText;
    [HideInInspector] public int position = 1;
    [HideInInspector] public int totalPositions = 1;

    [Header("Endurance Mode UI")]
    public TextMeshProUGUI totalDistanceText;
    public float Distance;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI timePlayedText;
    public Image[] lifeHearts;
    private int lifeCount;

    [Header("Barriers")]
    public Transform bottomBarrier;
    public Transform topBarrier;

    [Header("References")]
    public Player_Movement player;
    public Opponent_Movement opponent;

    [Header("Joysticks")]
    public GameObject leftJoystick;
    public GameObject rightJoystick;

    private bool isGamePaused = false;

    //public GameObject damageImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("StickToggle", 0) == 0)
        {
            leftJoystick.SetActive(true);
            rightJoystick.SetActive(false);
        }
        else
        {
            leftJoystick.SetActive(false);
            rightJoystick.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResumeGame();
        if(!sprintMode) 
        {
            
            lifeCount = GetMaxLife();
            UpdateLives(lifeCount);
        }
        else
        {
            player = FindObjectOfType<Player_Movement>();
            opponent = FindObjectOfType<Opponent_Movement>();
            for (int i = 0; i < lifeHearts.Length; i++)
            {
                lifeHearts[i].gameObject.SetActive(false);
            }
            UpdateSprintRoundText();
        }
        //respawnChancesText.text = GetMaxRespawn().ToString();
        //position = totalPositions;
        //UpdateSprintPostion();
    }

    public int GetMaxLife()
    {
        if (PlayerPrefs.GetInt("Life3", 0) == 1)
            return 3;
        else if (PlayerPrefs.GetInt("Life2", 0) == 1)
            return 2;
        else if (PlayerPrefs.GetInt("Life1", 0) == 1)
            return 1;
        else
            return 0;
    }

    public int GetMaxRespawn()
    {
        if (PlayerPrefs.GetInt("Respawn3", 0) == 1)
            return 3;
        else if (PlayerPrefs.GetInt("Respawn2", 0) == 1)
            return 2;
        else if (PlayerPrefs.GetInt("Respawn1", 0) == 1)
            return 1;
        else
            return 0;
    }

    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if (lives > i)
            {
                lifeHearts[i].color = Color.white;
            }
            else
            {
                lifeHearts[i].color = Color.black;
            }
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; 
        isGamePaused = true;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        pauseMenuPanel.SetActive(false);
    }

    public void Home()
    {
        GameDataManager.Instance.EndGame();
        SceneManager.LoadScene("Start Scene");
        Menue.instance.UpdateCoinText();
    }

    public void Restart()
    {
        GameDataManager.Instance.sprintRoundCount++;
        SceneManager.LoadScene("Sprint mode");
        Menue.instance.UpdateCoinText();
    }

    public void UpdateScore(int distance)
    {
        distanceText.text = distance + "m";
        Distance=distance;
    }

    public void UpdateTotalDistance()
    {
        totalDistanceText.text = distanceText.text.ToString();
        GameDataManager.Instance.enduranceRank.mile = Distance;
    }

    public void UpdateCoinText(string coins)
    {
        coinText.text = coins;
    }

    public void UpdateSprintRoundText()
    {
        roundText.text = GameDataManager.Instance.sprintRoundCount.ToString();
    }

    //public void SetTotalPositions(int total)
    //{
    //    totalPositions = total;
    //    totalPositionText.text = total.ToString();
    //}

    public void UpdateSprintPostion()
    {
        if (player.transform.position.y > opponent.transform.position.y)
            position = 1;
        else
            position = 2;

        positionText.text = position.ToString();
    }

    public void DisplaySprintRaceResult()
    {
        gameOverPositionText.text = "Position: " + positionText.text;
        gameOverRoundText.text = "Rounds: " + GameDataManager.Instance.sprintRoundCount.ToString();
        gameWinPositionText.text = "Position: " + positionText.text;
        gameWinRoundText.text = "Rounds: " + GameDataManager.Instance.sprintRoundCount.ToString();
    }

    private void Update()
    {
        if(sprintMode)
            UpdateSprintPostion();
    }
}


