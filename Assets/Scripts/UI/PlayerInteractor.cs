using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    PlayerController playerController;
    Land selectedLand = null;
    private Camera _Camera;
    private InteractableObject selectedInteractable = null;
    Transform playerTransform;

    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        _Camera = FindObjectOfType<Camera>();
        FindGround();
    }

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            Interactable(hit);
            
        }
    }

    void FindGround()
    {
        GameObject player = GameObject.Find("Player");
        playerTransform = player.transform;
        RaycastHit hit;
        float raycastDistance = 100f;  
        if (Physics.Raycast(playerTransform.position, Vector3.down, out hit, raycastDistance, LayerMask.GetMask("ground")))
        {
            playerTransform.position = new Vector3(playerTransform.position.x, hit.point.y, playerTransform.position.z);
        }
    }

    void Update()
    {
        CheckGround();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                OnInteractableHititem(hit);
            }
        }
    }

    void OnInteractableHititem(RaycastHit hit)
    {
     
        Collider other = hit.collider;
        if (hit.collider.CompareTag("item"))
        {
            Debug.Log("item");
            selectedInteractable = hit.collider.GetComponent<InteractableObject>();

            return;
        }
        if(selectedInteractable != null)
        {
            selectedInteractable = null;
        }

    }
    void Interactable(RaycastHit hit)
    {
        Collider other = hit.collider;
        if (other.tag == "Land")
        {
            Land land = hit.collider.GetComponent<Land>();
            SelectLand(land);
            return;
        }
        if (selectedLand)
        {
            selectedLand.Select(false);
        }
    }
    void OnInteractableHitLand(RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Land")
            {
                Land land = hit.collider.GetComponent<Land>();
                SelectLand(land);
                return;
            }
        }
        if (selectedLand)
        {
            selectedLand.Select(false);
        }

    }

    
    void SelectLand(Land land)
    {
        if(selectedLand)
        {
            selectedLand.Select(false);
        }
        selectedLand = land;
        land.Select(true);
    }

    public void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
            {
                return;
            }
        }
        if(selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }
    }

    public void ItemInteract()
    {
        if(selectedInteractable != null)
        {
            selectedInteractable.Pickup();
        }
    }

    public void ItemKeep()
    {
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }
    }


}
