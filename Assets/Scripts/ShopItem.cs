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
    public static int[] LifeCost = { 0, 300, 500, 700, 1000 };

    public GameObject BuyItem;

    public Sprite CarImage;
    public Image BuyItemCarImage;

    public Button yesButton;

    public GameObject InsufficentFunds;

    private void OnEnable()
    {
        //PlayerPrefs.SetInt("TotalScore", 60000);
        if (shopItemType == ShopItemType.Car)
        {
            cost = carCost[id];
        }
        else
        {
            cost = LifeCost[id];
        }
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
        else
        {
            if (PlayerPrefs.GetInt("Life" + id, 0) == 1)
            {
                PlayerPrefs.SetInt("SelectLife", id);
            }
            else
            {
                if (Menue.instance.playerCoins >= cost)
                {
                    //AudioManager.Instance.PlaySFX(SFXType.unlockLifes);

                    PlayerPrefs.SetInt("Life" + id, 1);
                    PlayerPrefs.SetInt("SelectLife", id);
                    Menue.instance.playerCoins -= cost;
                    Menue.instance.UpdateCoinText();
                    //PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) - cost);
                }
            }
        }
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
        else
        {
            if (PlayerPrefs.GetInt("Life" + id, 0) == 1)
            {
                costObject.gameObject.SetActive(false);
            }
            else
            {
                costObject.gameObject.SetActive(true);
            }
            selectedImage.SetActive(PlayerPrefs.GetInt("SelectLife", 0) == id);
            if (selectedImage.activeSelf)
            {
                GetComponent<Image>().sprite = SlectImage;
            }
            else
            {
                GetComponent<Image>().sprite = DefaultImage; ;

            }
            //BGForLife.SetActive(false);
            //BGForLife.SetActive(true);
        }
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
    Life
}
