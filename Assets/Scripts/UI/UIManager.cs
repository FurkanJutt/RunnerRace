using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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
    }

    [SerializeField] public GameObject gameOverPanel, pauseMenuPanel;

    public TextMeshProUGUI timePlayedText;
    public TextMeshProUGUI respawnChancesText;
    public Text countdownDisplay;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI finishText;
    public TextMeshProUGUI totalDistanceText;
    public float Distance;
    public TextMeshProUGUI distanceText;

    [Header("Joysticks")]
    public GameObject leftJoystick;
    public GameObject rightJoystick;

    //public GameObject damageImage;

    public Image[] lifeHearts;

    private bool isGamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("StickToggle", 0) == 0)
        {
            leftJoystick.SetActive(true);
            rightJoystick.SetActive(false);
        }
        else
        {
            leftJoystick.SetActive(false);
            rightJoystick.SetActive(true);
        }

        ResumeGame(); 
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
        SceneManager.LoadScene("Start Scene");
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
}


