using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panels_Holder : MonoBehaviour
{
    [Header("Menue Panels")]
    [SerializeField] public GameObject StartPanel, RacePanel;//, SettingPanel, TuningPanel, HalloffamePanel;

    private void Start()
   {
        ActivatePannel(StartPanel.name);
   }

    public void ActivatePannel(string pannelWillBeActivated)
    {
        RacePanel.SetActive(RacePanel.name.Equals(pannelWillBeActivated));
        StartPanel.SetActive(StartPanel.name.Equals(pannelWillBeActivated));
        //SettingPanel.SetActive(SettingPanel.name.Equals(pannelWillBeActivated));
        //TuningPanel.SetActive(TuningPanel.name.Equals(pannelWillBeActivated));
       // HalloffamePanel.SetActive(HalloffamePanel.name.Equals(pannelWillBeActivated));
    }

    public void EnduranceMode()
    {
        SceneManager.LoadScene("Endurance mode");
    }

    public void SprintMode()
    {
        SceneManager.LoadScene("Sprint mode");
    }
}
