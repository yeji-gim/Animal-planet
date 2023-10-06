using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            if (SceneTransitionManager.Instance != null)
            {

                if (locationToSwitch == SceneTransitionManager.Location.shop)
                {
                    GameStateManager.Instance.ShopTime(TimeManager.Instance.GetGameTimeStamp());

                }
                
                else
                {
                    SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);

                }
                InventoryManager.Instance.RenderHand();
            }

        }

    }


}
