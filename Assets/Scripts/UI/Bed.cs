using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        System.Console.WriteLine("trigger¹ß»ý");
        if (collision.collider.CompareTag("Player"))
        {

            UIManager.Instance.TriggerYesNoPrompt("Do you want to sleep?", "",GameStateManager.Instance.sleep);
        }
    }
    
}
