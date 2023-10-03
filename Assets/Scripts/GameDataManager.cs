using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [HideInInspector] public int playerCoins;
    [HideInInspector] public int sprintRoundCount;
    [HideInInspector] public EnduranceRank enduranceRank = new EnduranceRank();
    [HideInInspector] public SprintRank sprintRank = new SprintRank();

    [HideInInspector] public List<EnduranceRank> enduranceRanks = new List<EnduranceRank>();
    [HideInInspector] public List<SprintRank> sprintRanks = new List<SprintRank>();

    public string enduranceRanksJson
    {
        get
        {
            return PlayerPrefs.GetString("enduranceRanksJson", "");
        }
        set
        {
            PlayerPrefs.SetString("enduranceRanksJson", value);
        }
    }

    public string sprintRanksJson
    {
        get
        {
            return PlayerPrefs.GetString("sprintRanksJson", "");
        }
        set
        {
            PlayerPrefs.SetString("sprintRanksJson", value);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sprintRoundCount = 0;
        if (string.IsNullOrEmpty(sprintRanksJson))
        {
            SaveSpringRank();
        }
        else
        {
            LoadSprintRanks();
        }
        if (string.IsNullOrEmpty(enduranceRanksJson))
        {
            SaveEnduranceRank();
        }
        else
        {
            LoadEnduranceRank();
        }
    }

    public void AddEnduranceRank()
    {
        Debug.Log("AddEnduranceRank");
        enduranceRanks.Add(enduranceRank);
        enduranceRanks = enduranceRanks.OrderByDescending(o => o.time).ToList();
        if (enduranceRanks.Count>10)
        {
            enduranceRanks.RemoveAt((enduranceRanks.Count - 1));
        }
        SaveEnduranceRank();
        enduranceRank = new EnduranceRank();
    }

    public void AddSprintRank()
    {
        Debug.Log("AddSprintRank");
        sprintRanks.Add(sprintRank);
        sprintRanks = sprintRanks.OrderByDescending(o => o.crash).ToList();
        if (sprintRanks.Count>10)
        {
            sprintRanks.RemoveAt(sprintRanks.Count-1);
        }
        SaveSpringRank();
        sprintRank = new SprintRank();
    }

    public void LoadSprintRanks()
    {
        sprintRanks = JsonConvert.DeserializeObject<List<SprintRank>>(sprintRanksJson);
    }

    public void LoadEnduranceRank()
    {
        enduranceRanks=JsonConvert.DeserializeObject<List<EnduranceRank>>(enduranceRanksJson);
    }

    public void SaveEnduranceRank()
    {
        enduranceRanksJson=JsonConvert.SerializeObject(enduranceRanks);
    }

    public void SaveSpringRank()
    {
        sprintRanksJson = JsonConvert.SerializeObject(sprintRanks);
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        if (!UIManager.instance.sprintMode)
        {
            enduranceRank.time = TimerController.instance.totalTimePlayed;
            enduranceRank.mile = UIManager.instance.Distance;
            AddEnduranceRank();
            TimerController.instance.DisplayTotalTimePlayed();
            UIManager.instance.UpdateTotalDistance();
        }
        else
            FinishLane.instance.GameOver();
    }
}

[System.Serializable]
public class EnduranceRank
{
    public float time;
    public float mile;
}

[System.Serializable]
public class SprintRank
{
    public int round;
    public int crash;
    public int finish;
}