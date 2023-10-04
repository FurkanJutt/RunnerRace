using UnityEngine;
using UnityEngine.UI;

public class FinishLane : MonoBehaviour
{
    public static FinishLane instance;

    public string playerTag = "Player";
    public string opponentTag = "OtherCar";

    private int playerRounds = 0;
    private int playerFinishCount = 0; // Count of times the player finishes first
    private int playerRoundFinishCount = 0; // Count of rounds completed

    private bool raceFinished = false;
    private bool playerFinishedRound = false;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            //playerRoundFinishCount++; // Increment round completion count

            //if (!playerFinishedRound)
            //{
            //    playerFinishCount++; // Increment finishes count if player finished first
            //    playerFinishedRound = true; // Player finished this round
            //}
            Time.timeScale = 0f;
            UIManager.instance.gameWinPanel.SetActive(true);
            UIManager.instance.DisplaySprintRaceResult();
            //DisplayRaceResult(playerRoundFinishCount, playerFinishCount); // Update UI with the round and finish counts
        }
        else if (other.CompareTag(opponentTag))
        {
            //playerFinishedRound = false; // Opponent touched the finish line before the player
            Time.timeScale = 0f;
            GameOver();
            UIManager.instance.gameOverPanel.SetActive(true);
            UIManager.instance.DisplaySprintRaceResult();
            //DisplayRaceResult(playerRoundFinishCount, playerFinishCount); // Update UI with the round and finish counts
        }
    }

    //private void DisplayRaceResult(int playerRoundFinishCount, int playerFinishCount)
    //{
    //    UIManager.instance.gameOverPositionText.text = "Position " + playerFinishCount.ToString();
    //    UIManager.instance.gameOverRoundText.text = "Rounds " + playerRoundFinishCount.ToString();
    //    UIManager.instance.gameWinPositionText.text = "Position " + playerFinishCount.ToString();
    //    UIManager.instance.gameWinRoundText.text = "Rounds " + playerRoundFinishCount.ToString();
    //}

    public void GameOver()
    {
        if (GameDataManager.Instance.sprintRoundCount >= 1)
        {
            // Call this function when the game is over (e.g., when you want to end the race)
            GameDataManager.Instance.sprintRank.round = GameDataManager.Instance.sprintRoundCount;
            GameDataManager.Instance.sprintRank.crash = Player_Movement.instance.crash;
            GameDataManager.Instance.sprintRank.finish = int.Parse(UIManager.instance.positionText.text);
            GameDataManager.Instance.AddSprintRank();
        }
    }

    private void OnDestroy()
    {
        GameOver();
    }
}
