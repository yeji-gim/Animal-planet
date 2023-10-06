using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour, ITimeTracker
{
    [Header("Status Bar")]
    // Tool equip slot on the status bar
    public Image toolEquipSlots;
    // Tool Quantity text on the status bar
    public TMP_Text toolQuantityText;
    // Time UI
    public TMP_Text timeText;
    public TMP_Text dateText;
    [Header("Inventory Systems")]
    // The Inventory panel
    public GameObject InventoryPanel;
    // The tool equip slot UI on the Inventory panel
    public HandelInventory toolHandSlot;
    // The tool slot UI's
    public InventorySlot[] toolSlots;
    // The Item slot UI's
    // The Item equip slot UI on the Inventory panel
    public HandelInventory itemHandSlot;
    public InventorySlot[] ItemSlots;
    // Item info box
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    [Header("Map")]
    public GameObject mapPanel;


    [Header("Yes No Prompt")]
    public YesNoPrompt yesNoPrompt;
    public namingPrompt namingPrompt;
    public GameObject unlockpanel;
    public TMP_Text planetcost;
    [Header("Button Prompt")]
    public DialogePrompt DialogePrompt;

    [Header("PlayerStats")]
    public TMP_Text moneyText;
    public TMP_Text shopmoneyText;
    public GameObject selectbutton;
    public GameObject shopmoney;

     [Header("OpenNotice")]
    public GameObject openNotice;

    [Header("Shop")]
    public ShopListManager ShopListManager;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();
        RenderPlayerStats();
        TimeManager.Instance.RegisterTracker(this);

    }

    public void TriggerYesNoPrompt(string message, string head, System.Action onYesCallback)
    {
        yesNoPrompt.gameObject.SetActive(true);

        yesNoPrompt.CreatePrompt(message, head, onYesCallback);
    }
    public void TriggerNamingPrompt(string message, System.Action<string> onConfirmCallback)
    {
        if(namingPrompt.gameObject.activeSelf)
        {
            // Queue the prompt
            namingPrompt.QueuePormptAction(() => TriggerNamingPrompt(message, onConfirmCallback));
            return;
        }
        namingPrompt.gameObject.SetActive(true);
        namingPrompt.CreatePrompt(message, onConfirmCallback);
    }
    public void TriggerDialogePrompt(string name1, string name2, System.Action first, System.Action second)
    {
        DialogePrompt.gameObject.SetActive(true);

        DialogePrompt.Createbutton(name1, name2, first, second);
    }
    public void AssignSlotIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);

        }

        for (int i = 0; i < ItemSlots.Length; i++)
        {
            ItemSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        ItemSlotData[] inventoryToolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);

        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        RenderInventoryPanel(inventoryItemSlots, ItemSlots);

        toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));

        ItemData equippedTool = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        toolQuantityText.text = "";

        if (equippedTool != null)
        {
            toolEquipSlots.sprite = equippedTool.thumbnail;
            toolEquipSlots.gameObject.SetActive(true);

            int quantity = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool).quantity;

            if (quantity > 1)
            {
                toolQuantityText.text = quantity.ToString();
            }
            return;
        }

        toolEquipSlots.gameObject.SetActive(false);
    }

    void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }
   
    public void ToggleInventoryPanel()
    {

        InventoryPanel.SetActive(!InventoryPanel.activeSelf);

        RenderInventory();
    }

    public void ToggleImapPanel()
    {
        mapPanel.SetActive(!mapPanel.activeSelf);
    }

    public void DisplayItemInfo(ItemData data)
    {
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            return;
        }
        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = "AM";

        if (hours > 12)
        {
            prefix = "PM";
            hours -= 12;
        }

        timeText.text = prefix + " " + hours + " : " + minutes.ToString("00");

        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day + "(" + dayOfTheWeek + ")";
    }

    public void RenderPlayerStats()
    {
        moneyText.text = PlayerStats.Money + PlayerStats.Currency;
    }

    public void ToggleopenNoticel()
    {
        openNotice.SetActive(!openNotice.activeSelf);
    }

    public void OpenShop(List<ItemData> shopItem)
    {
        ShopListManager.gameObject.SetActive(true);
        ShopListManager.RenderShop(shopItem);
    }
    public bool IsMapPanelActive()
    {
        return mapPanel.activeSelf;
    }
    public bool IsInventoryPanelActive()
    {
        return InventoryPanel.activeSelf;
    }
}