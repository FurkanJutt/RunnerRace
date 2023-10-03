using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menue : MonoBehaviour
{
    public static Menue instance;

    [Header("Race Panel")]
    [SerializeField] private GameObject MainMenu, SettingMenu, Tunning, HallOfFame, RaceMenu;

    [Space]
    public GameObject BackButton;

    [Space]
    public Button sprintModeButton;
    public Button enduranceModeButton;

    [Space]
    public Sprite sprintOriginalImage;
    public Sprite sprintMaskImage;
    public Sprite enduranceOriginalImage;
    public Sprite enduranceMaskImage;

    [Header("Coins")]
    public TextMeshProUGUI coinTextMenu;
    public TextMeshProUGUI coinTextShop;
    public TextMeshProUGUI coinTextConfirmBuy;

    [Header("Hall OF Fame UI")]
    public Transform EnduranceBoardParent;
    public Transform SprintboardParent;

    [Header("Sound UI")]
    public Image soundOn;
    public TextMeshProUGUI soundOnText;
    public Image soundOff;
    public TextMeshProUGUI soundOffText;
    public Color soundButtonColor;

    [Header("Stick UI")]
    public Image stickRight;
    public TextMeshProUGUI stickRightText;
    public Image stickLeft;
    public TextMeshProUGUI stickLeftText;

    public void ToggleSound(bool toggle)
    {
        if (toggle) // sound on
        {
            soundOn.color = soundButtonColor;
            soundOnText.color = Color.white;
            soundOff.color = Color.white;
            soundOffText.color = soundButtonColor;
            PlayerPrefs.SetInt("SoundToggle", 1);
        }
        else // sound off
        {
            soundOff.color = soundButtonColor;
            soundOffText.color = Color.white;
            soundOn.color = Color.white;
            soundOnText.color = soundButtonColor;
            PlayerPrefs.SetInt("SoundToggle", 0);
        }

    }

    public void ToggleStick(bool toggle)
    {
        if (toggle) // Stick right
        {
            stickRight.color = soundButtonColor;
            stickRightText.color = Color.white;
            stickLeft.color = Color.white;
            stickLeftText.color = soundButtonColor;
            PlayerPrefs.SetInt("StickToggle", 1);
        }
        else // Stick Left
        {
            stickLeft.color = soundButtonColor;
            stickLeftText.color = Color.white;
            stickRight.color = Color.white;
            stickRightText.color = soundButtonColor;
            PlayerPrefs.SetInt("StickToggle", 0);
        }

    }

    public int playerCoins
    {
        get
        {
            return PlayerPrefs.GetInt("TotalScore", 0);
        }
        set
        {
            PlayerPrefs.SetInt("TotalScore", value);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    private void Start()
    {
        // sound check
        if (PlayerPrefs.GetInt("SoundToggle", 1) == 1)
            ToggleSound(true);
        else 
            ToggleSound(false);

        // stick check
        if (PlayerPrefs.GetInt("StickToggle", 0) == 1)
            ToggleStick(true);
        else
            ToggleStick(false);

        UpdateCoinText();
        ActivatePannel(MainMenu.name);
        sprintModeButton.image.sprite = sprintOriginalImage;
        enduranceModeButton.image.sprite = enduranceMaskImage;
    }

    public void ShowHallOfFameOfEnduranceMode()
    {
        /*foreach (Transform item in boardParent)
        {
            item.gameObject.SetActive(false);
        }*/
        for (int i = 0; i < GameDataManager.Instance.enduranceRanks.Count; i++)
        {
            EnduranceBoardParent.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            float totalTimePlayed = GameDataManager.Instance.enduranceRanks[i].time;
            int minutes = Mathf.FloorToInt(totalTimePlayed / 60);
            int seconds = Mathf.FloorToInt(totalTimePlayed % 60);
            GameDataManager.Instance.enduranceRank.time = totalTimePlayed;
            string minutesString = minutes.ToString("00");
            string secondsString = seconds.ToString("00");
            EnduranceBoardParent.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = minutesString + ":" + secondsString.ToString();

            EnduranceBoardParent.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = (GameDataManager.Instance.enduranceRanks[i].mile).ToString();
        }
    }

    public void ShowHallOfFameOfSprintMode()
    {
        /*foreach (Transform item in boardParent)
        {
            item.gameObject.SetActive(false);
        }*/
        for (int i = 0; i < GameDataManager.Instance.sprintRanks.Count; i++)
        {
            SprintboardParent.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            SprintboardParent.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameDataManager.Instance.sprintRanks[i].round.ToString();
            SprintboardParent.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameDataManager.Instance.sprintRanks[i].crash.ToString();
            SprintboardParent.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = (GameDataManager.Instance.sprintRanks[i].finish).ToString();
        }
    }
    public void UpdateCoinText()
    {
        //int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        GameDataManager.Instance.playerCoins = playerCoins;
        coinTextMenu.text = playerCoins.ToString();
        coinTextShop.text = playerCoins.ToString();
        coinTextConfirmBuy.text = playerCoins.ToString();
    }

    public void ActivatePannel(string pannelWillBeActivated)
    {
        MainMenu.SetActive(MainMenu.name.Equals(pannelWillBeActivated));
        SettingMenu.SetActive(SettingMenu.name.Equals(pannelWillBeActivated));
        Tunning.SetActive(Tunning.name.Equals(pannelWillBeActivated));
        HallOfFame.SetActive(HallOfFame.name.Equals(pannelWillBeActivated));
        RaceMenu.SetActive(RaceMenu.name.Equals(pannelWillBeActivated));
    }

    public void BackButtonFun()
    {
        ActivatePannel(MainMenu.name);
    }

    public void EnduranceMode()
    {
        SceneManager.LoadScene("Endurance mode");
    }

    public void SprintMode()
    {
        SceneManager.LoadScene("Sprint mode");
    }

    public void HallOfFameMenue()
    {
        ActivatePannel(HallOfFame.name);
    }

    public void FameOfEnduranceMode()
    {
        ShowHallOfFameOfEnduranceMode();
        sprintModeButton.image.sprite = sprintOriginalImage;
        enduranceModeButton.image.sprite = enduranceMaskImage;
    }

    public void FameOfSprintMode()
    {
        ShowHallOfFameOfSprintMode();
        sprintModeButton.image.sprite = sprintMaskImage;
        enduranceModeButton.image.sprite = enduranceOriginalImage;
    }

    public void UpdateCoins(int coins)
    {
        coinTextMenu.text = coins.ToString();
    }
}
