using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopListing : MonoBehaviour, IPointerClickHandler
{
    public Image itemThumbnail;
    public TMP_Text nameText;
    public TMP_Text costText;


    public ItemData itemData { get; private set; }
    public void Display(ItemData itemData)
    {
        this.itemData = itemData;
        itemThumbnail.sprite = itemData.thumbnail;
        nameText.text = itemData.name;
        costText.text = itemData.cost + PlayerStats.Currency;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.ShopListManager.OpenConfirmationScreen(itemData);
    }


}
