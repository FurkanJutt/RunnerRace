using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] ShopItemType shopItemType;
    [SerializeField] int id;
    [SerializeField] int cost;
    [SerializeField] GameObject costObject;
    [SerializeField] GameObject selectedImage;
    //[SerializeField] GameObject BGForLife;
    [SerializeField] TMP_Text costText;
    public GameObject Panel;

    public Sprite DefaultImage, SlectImage;
    //  private bool isPanelActive=true;


    public static int[] carCost = { 200, 200, 200, 200, 200, 200, 200, 200, 200, 200,
    200,200,200,200,200,200,200,200,200,200,200,200,200,200,200};
    //public static int[] LifeCost = { 0, 300, 500, 700, 1000 };

    public GameObject BuyItem;

    public Sprite CarImage;
    public Image BuyItemCarImage;

    public Button yesButton;

    public GameObject InsufficentFunds;

    private void OnEnable()
    {
        PlayerPrefs.SetInt("Car" + 0, 1);
        //PlayerPrefs.SetInt("TotalScore", 60000);
        if (shopItemType == ShopItemType.Car)
        {
            cost = carCost[id];
        }
        //else
        //{
        //    cost = LifeCost[id];
        //}
        costText.text = cost.ToString() + " coins";

        UpdateState();
    }


    public void BuyConfirm()
    {
        // AudioManager.Instance.PlaySFX(SFXType.unlockCar);
        PlayerPrefs.SetInt("Car" + id, 1);
        PlayerPrefs.SetInt("SelectCar", id);
        //PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) - cost);
        Menue.instance.playerCoins -= cost;
        Menue.instance.UpdateCoinText();
        BuyItem.SetActive(false);
        UpdateAllItem();
    }


    public void NotConfirm()
    {
        BuyItem.SetActive(false);
    }
    public void ItemClick()
    {
        //AudioManager.Instance.PlaySFX(SFXType.button);

        if (shopItemType == ShopItemType.Car)
        {
            //   Panel.SetActive(true);
            if (PlayerPrefs.GetInt("Car" + id, 0) == 1)
            {
                PlayerPrefs.SetInt("SelectCar", id);
            }
            else
            {
                if (Menue.instance.playerCoins >= cost)
                {
                    BuyItem.SetActive(true);
                    if (yesButton != null)
                    {
                        BuyItemCarImage.sprite = CarImage;
                        // Clear the current listeners
                        yesButton.onClick.RemoveAllListeners();

                        // Add the new function as a listener
                        yesButton.onClick.AddListener(BuyConfirm);
                    }
                    // AudioManager.Instance.PlaySFX(SFXType.unlockCar);
                    //PlayerPrefs.SetInt("Car" + id, 1);
                    //PlayerPrefs.SetInt("SelectCar", id);
                    //PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) - cost);
                    //Menue.instance.playerCoins-=cost;
                    Menue.instance.UpdateCoinText();
                }
                else
                {
                    InsufficentFunds.SetActive(true);
                }
            }

        }
        //else if (shopItemType == ShopItemType.Life)
        //{
        //    if (Menue.instance.playerCoins >= cost)
        //    {
        //        //AudioManager.Instance.PlaySFX(SFXType.unlockLifes);

        //        PlayerPrefs.SetInt("Life" + id, 1);
        //        //PlayerPrefs.SetInt("SelectLife", id);
        //        Menue.instance.playerCoins -= cost;
        //        Menue.instance.UpdateCoinText();
        //        //PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) - cost);
        //    }
        //    else
        //    {
        //        InsufficentFunds.SetActive(true);
        //    }
        //}
        //else if (shopItemType == ShopItemType.Time)
        //{
        //    if (Menue.instance.playerCoins >= cost)
        //    {
        //        PlayerPrefs.SetInt("Time" + id, 1);
        //        //PlayerPrefs.SetInt("SelectTime", id);
        //        Menue.instance.playerCoins -= cost;
        //        Menue.instance.UpdateCoinText();
        //    }
        //    else
        //    {
        //        InsufficentFunds.SetActive(true);
        //    }
        //}
        //else if (shopItemType == ShopItemType.Respawn)
        //{
        //    if (Menue.instance.playerCoins >= cost)
        //    {
        //        PlayerPrefs.SetInt("Respawn" + id, 1);
        //        //PlayerPrefs.SetInt("SelectRespawn", id);
        //        Menue.instance.playerCoins -= cost;
        //        Menue.instance.UpdateCoinText();
        //    }
        //    else
        //    {
        //        InsufficentFunds.SetActive(true);
        //    }
        //}
        // MainMenuController.Instance.UpdateTextScore();
        //UpdateState();
        UpdateAllItem();
    }
    public void UpdateState()
    {
        if (shopItemType == ShopItemType.Car)
        {
            if (PlayerPrefs.GetInt("Car" + id, 0) == 1)
            {
                costObject.gameObject.SetActive(false);
            }
            else
            {
                costObject.gameObject.SetActive(true);
            }
            selectedImage.SetActive(PlayerPrefs.GetInt("SelectCar", 0) == id);
            if (selectedImage.activeSelf)
            {
                GetComponent<Image>().sprite = SlectImage;
            }
            else
            {
                GetComponent<Image>().sprite = DefaultImage;
            }
        }
        //else if(shopItemType == ShopItemType.Life)
        //{
        //    //if (PlayerPrefs.GetInt("Life" + id, 0) == 1)
        //    //{
        //    //    costObject.gameObject.SetActive(false);
        //    //}
        //    //else
        //    //{
        //    //    costObject.gameObject.SetActive(true);
        //    //}
        //    selectedImage.SetActive(PlayerPrefs.GetInt("Life"+id, 0) == 1);
        //    if (PlayerPrefs.GetInt("Life" + id, 0) == 1)
        //    {
        //        //GetComponent<Image>().sprite = SlectImage;
        //        GetComponent<Button>().interactable = false;
        //    }
        //    else
        //    {
        //        //GetComponent<Image>().sprite = DefaultImage;
        //        GetComponent<Button>().interactable = true;

        //    }
        //    //BGForLife.SetActive(false);
        //    //BGForLife.SetActive(true);
        //}
        //else if (shopItemType == ShopItemType.Time)
        //{
        //    if (PlayerPrefs.GetInt("Time" + id, 0) == 1)
        //    {
        //        GetComponent<Button>().interactable = false;
        //    }
        //    else
        //    {
        //        GetComponent<Button>().interactable = true;
        //    }
        //}
        //else if (shopItemType == ShopItemType.Respawn)
        //{
            
        //    if (PlayerPrefs.GetInt("Respawn" + id, 0) == 1)
        //    {
        //        GetComponent<Button>().interactable = false;
        //    }
        //    else
        //    {
        //        GetComponent<Button>().interactable = true;
        //    }
        //}
    }
    void UpdateAllItem()
    {
        ShopItem[] shopItems = FindObjectsOfType<ShopItem>();
        foreach (ShopItem shopItem in shopItems)
            shopItem.UpdateState();
    }
}
enum ShopItemType
{
    Car,
    Life,
    Time,
    Respawn
}
