using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCmanager : MonoBehaviour
{
    public Transform player;
    private Camera mainCamera;
    private Transform targetNPC;
    private Quaternion originalRotation;
    private float originalFOV;
    npccontroller npcController;

    public static NPCmanager Instance { get; private set; }
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
    void Start()
    {
        mainCamera = Camera.main;
        originalRotation = transform.rotation;
        originalFOV = mainCamera.fieldOfView;
        

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                OnInteractablenpccontroller(hit);
            }
        }
    }

    void OnInteractablenpccontroller(RaycastHit hit)
    {

        Collider other = hit.collider;
        if (hit.collider.CompareTag("NPC"))
        {
            npcController = hit.collider.GetComponent<npccontroller>();


            if (npcController != null)
            {
                if (DialogueManager.Instance != null)
                {
                    npcController.transform.LookAt(player.position);
                    mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 22, Time.deltaTime * 4f);
                    
 
                    List<DialogueLine> dialogueToHave = npcController.charcterData.defaultDialoge;                  
                    DialogueManager.Instance.StartDialogue(dialogueToHave);

                }
                
            }
            else
            {
                Debug.LogWarning("npccontroller 컴포넌트가 없는 NPC입니다.");
            }
        }
    }
}

