using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public float timerDuration = 30f;
    private float currentTime;
    public float totalTimePlayed;
    private bool isGameOver = false;


    public int countdownTime;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public int coin;

    public AudioClip objectCollectClip;

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

    private void Start()
    {
        currentTime = timerDuration;
        UpdateTimerText();
        UpdateCoinText();

        totalTimePlayed = 0f;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            currentTime -= Time.deltaTime;
            totalTimePlayed += Time.deltaTime;

            if (currentTime <= 1)
            {
                currentTime = 0;
                isGameOver = true;
                Time.timeScale = 0f;

                float timePlayed = timerDuration - currentTime;

                DisplayTotalTimePlayed();
                UIManager.instance.gameOverPanel.SetActive(true);
                UIManager.instance.UpdateTotalDistance();

                //                FinishLane.fL.GameOver();
                GameDataManager.Instance.enduranceRank.time = totalTimePlayed;
                GameDataManager.Instance.enduranceRank.mile = UIManager.instance.Distance;
                GameDataManager.Instance.AddEnduranceRank();
                Debug.Log("Game Over");
            }

            UpdateTimerText();
        }
    }

    public void DisplayTotalTimePlayed()
    {
        if (UIManager.instance.timePlayedText != null)
        {
            int minutes = Mathf.FloorToInt(totalTimePlayed / 60);
            int seconds = Mathf.FloorToInt(totalTimePlayed % 60);
            GameDataManager.Instance.enduranceRank.time = totalTimePlayed;
            string minutesString = minutes.ToString("00");
            string secondsString = seconds.ToString("00");
            UIManager.instance.timePlayedText.text = minutesString + ":" + secondsString.ToString();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string minutesString = minutes.ToString("00");
        string secondsString = seconds.ToString("00");

        timerText.text = minutesString + ":" + secondsString;

        if (minutes == 0 && seconds == 0)
        {
            timerText.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Timer"))
        {
            SoundManager.instance.PlaySoundFX(objectCollectClip, 0.9f);
            currentTime += 10f;
            UpdateTimerText();
            other.transform.parent.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Coin"))
        {
            SoundManager.instance.PlaySoundFX(objectCollectClip, 0.9f);
            coin++;
            GameDataManager.Instance.playerCoins++;
            PlayerPrefs.SetInt("TotalScore", GameDataManager.Instance.playerCoins);
            UpdateCoinText();
            //int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            Debug.Log("Total Coins: " + GameDataManager.Instance.playerCoins);
            other.transform.parent.gameObject.SetActive(false);
        }
    }

    private void UpdateCoinText()
    {
        scoreText.text = coin.ToString();
    }
}