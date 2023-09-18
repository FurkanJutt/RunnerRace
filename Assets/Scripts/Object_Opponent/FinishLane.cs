using UnityEngine;
using UnityEngine.UI;

public class FinishLane : MonoBehaviour
{
    public static FinishLane fL;

    public string playerTag = "Player";
    public string opponentTag = "OtherCar";

    private int playerRounds = 0;
    private int playerFinishCount = 0; // Count of times the player finishes first
    private int playerRoundFinishCount = 0; // Count of rounds completed

    private bool raceFinished = false;
    private bool playerFinishedRound = false;

    private void Awake()
    {
        if (fL == null)
        {
            fL = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!raceFinished)
        {
            if (other.CompareTag(playerTag))
            {
                playerRoundFinishCount++; // Increment round completion count

                if (!playerFinishedRound)
                {
                    playerFinishCount++; // Increment finishes count if player finished first
                    playerFinishedRound = true; // Player finished this round
                }
                DisplayRaceResult(playerRoundFinishCount, playerFinishCount); // Update UI with the round and finish counts
            }
            else if (other.CompareTag(opponentTag))
            {
                playerFinishedRound = false; // Opponent touched the finish line before the player
            }
        }
    }

    private void DisplayRaceResult(int playerRoundFinishCount, int playerFinishCount)
    {
        UIManager.instance.finishText.text = "Positions " + playerFinishCount.ToString();
        UIManager.instance.roundText.text = "Rounds " + playerRoundFinishCount.ToString();
    }

    public void GameOver()
    {
        // Call this function when the game is over (e.g., when you want to end the race)
        GameDataManager.Instance.sprintRank.round = playerRoundFinishCount;
        GameDataManager.Instance.sprintRank.crash = Player_Movement.instance.crash;
        GameDataManager.Instance.sprintRank.finish = playerFinishCount;
        GameDataManager.Instance.AddSprintRank();
        raceFinished = true;
        Time.timeScale = 0f;
    }
}
