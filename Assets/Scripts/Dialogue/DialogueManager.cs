using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue Components")]
    public GameObject dialougePanel;
    public TMP_Text speakerText;
    public TMP_Text dialougeText;
    [Header("button panel")]
    public GameObject buttonPanel;
    public TMP_Text button1_name;
    public TMP_Text button2_name;
    Queue<DialogueLine> dialogueQueue;
    [Header("ForShop")]
    public List<ItemData> shopItem;
    Action onDialogueEnd = null;
    bool istyping = false;

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


    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;

        if (PlayerStats.Money >= totalCost)
        {
            PlayerStats.Spend(totalCost);
            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);

            InventoryManager.Instance.ShopToInventory(purchasedItem);
        }
    }

    public static void Purchase(int cost)
    {
        if (PlayerStats.Money >= cost)
        {
            PlayerStats.Spend(cost);
        }
    }
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue)
    {
        
        dialogueQueue = new Queue<DialogueLine>(dialogueLinesToQueue);
        Debug.Log($"dialogueQueue.Count {dialogueQueue.Count}");
        UpdateDialogue();
    }

    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue, Action onDialogueEnd)
    {
        StartDialogue(dialogueLinesToQueue);
        this.onDialogueEnd = onDialogueEnd;
    }

    public void UpdateDialogue()
    {
        if(istyping)
        {
            istyping = false;
            return;
        }
        dialougeText.text = string.Empty;
        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLine line = dialogueQueue.Dequeue();


        if (!string.IsNullOrEmpty(line.first_button) && !string.IsNullOrEmpty(line.second_button))
        {
            Talk(line.speaker, line.message, line.first_button, line.second_button);
            UIManager.Instance.TriggerDialogePrompt(line.first_button, line.second_button, OnFirstButtonClicked, OnSecondButtonClicked);
        }
        else
        {
            buttonPanel.SetActive(false);
            Talk(line.speaker, line.message);
        }
    }

    public void EndDialogue()
    {
        dialougePanel.SetActive(false);

        Debug.Log("EndDialogue");
        onDialogueEnd = null;
    }

    public void Talk(string speaker, string message)
    {
        dialougePanel.SetActive(true);

        speakerText.text = speaker;

        speakerText.transform.parent.gameObject.SetActive(speaker != "");

        StartCoroutine(TypeText(message));
    }

    public void Talk(string speaker, string message, string f_button, string s_button)
    {
 
        dialougePanel.SetActive(true);

        speakerText.text = speaker;

        speakerText.transform.parent.gameObject.SetActive(speaker != "");

        StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string textToType)
    {
        istyping = true;
        char[] charsToType = textToType.ToCharArray();
        for(int i=0; i<charsToType.Length;i++)
        {
            dialougeText.text += charsToType[i];
            yield return new WaitForSeconds(0.01f);
            if(!istyping)
            {
                dialougeText.text = textToType;
                break;
            }

        }
        istyping = false;
    }

    public void OnFirstButtonClicked()
    {
        if (button1_name.text.Contains("sell"))
        {
            SellItem();
            
        }
        else if (button1_name.text.Contains("buy"))
        {
            BuyItem();
        }

    }

    public void OnSecondButtonClicked()
    {
        if (button2_name.text.Contains("sell"))
        {
            SellItem();

        }
        else if (button2_name.text.Contains("buy"))
        {
            BuyItem();
        }

    }
    private void SellItem()
    {
        UIManager.Instance.InventoryPanel.SetActive(true);
    }

    private void BuyItem()
    {
        Debug.Log("BuyItem");
        UIManager.Instance.OpenShop(shopItem);
    }

    
    public bool IsDialoguePanelActive()
    {
        return dialougePanel.activeSelf;
    }

    public static List<DialogueLine> CreateSimpleMessage(string message)
    {
        DialogueLine messageDialogeLine = new DialogueLine(" ", message);
        List<DialogueLine> listToReturn = new List<DialogueLine>();
        listToReturn.Add(messageDialogeLine);
        return listToReturn;
    }
}
