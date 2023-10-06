using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopListManager : MonoBehaviour
{
    public GameObject shopListing;
    public Transform listingGrid;

    ItemData itemToBuy;
    int quantity;

    [Header("Confirmation Screen")]
    public GameObject confirmationScreen;
    public Image icon;
    public TMP_Text confirmationPrompt;
    public TMP_Text quantity_Text;
    public TMP_Text totalcost;
    public Button purchaseButton;
    
    public void RenderShop(List<ItemData> shopItems)
    {
        if(listingGrid.childCount > 0)
        {
            foreach(Transform child in listingGrid)
            {
                Destroy(child.gameObject);
            }
        }

        foreach(ItemData shopItem in shopItems)
        {
            GameObject listingGameObject = Instantiate(shopListing, listingGrid);
            listingGameObject.GetComponent<ShopListing>().Display(shopItem);
        }
    }

    public void OpenConfirmationScreen(ItemData item)
    {
        itemToBuy = item;
        icon.sprite = item.thumbnail;
        quantity = 1;
        RenderConfirmationScreen();
    }

    public void RenderConfirmationScreen()
    {
        if (itemToBuy == null)
        {
            Debug.LogError("itemToBuy is null!");
            return;
        }
        confirmationScreen.SetActive(true);
        confirmationPrompt.text = $"Buy {itemToBuy.name}";
        quantity_Text.text = "x" + quantity;
        int cost = itemToBuy.cost * quantity;
        int playerMoneyLeft = PlayerStats.Money - cost;
        if(playerMoneyLeft<0)
        {
            totalcost.text = "You don't have money";
            purchaseButton.interactable = false;
            return;
        }
        purchaseButton.interactable = true;
        totalcost.text = $"{PlayerStats.Money} > {playerMoneyLeft}";
    }
    public void AddQuantity()
    {
        quantity++;
        RenderConfirmationScreen();
    }

    public void SubtractQuantity()
    {
        if(quantity>1)
        {
            quantity--;
        }
        RenderConfirmationScreen();
    }

    public void ConfirmPurchase()
    {
        DialogueManager.Instance.UpdateDialogue();
        DialogueManager.Purchase(itemToBuy, quantity);
        confirmationScreen.SetActive(false);
    }

    public void CancelPurchase()
    {
        confirmationScreen.SetActive(false);
    }

    
}
