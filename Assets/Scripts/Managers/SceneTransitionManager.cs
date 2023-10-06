using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    public static SceneTransitionManager Instance { get; private set; }

    Transform playerPoint;

    public enum SceneState
    {
        Farm, Home, Universe,shop,blue
    }
    public SceneState sceneState;

    public enum Location { Farm, Universe, Home, Alien, shop,blue}
    public Location currentLocation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnLocationLoad;
        playerPoint = FindObjectOfType<PlayerController>().transform;
    }

    public void SwitchLocation(Location locationToSwitch)
    {
        if (currentLocation == locationToSwitch)
        {
            Debug.LogWarning("Already in the target location.");
            return;
        }

        string sceneName = locationToSwitch.ToString();
        StartCoroutine(LoadSceneAsync(locationToSwitch));
    }
    private IEnumerator LoadSceneAsync(Location newLocation)
    {
        string sceneName = newLocation.ToString();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }


        if (UIManager.Instance != null)
        {
            UIManager.Instance.RenderInventory();
        }
    }
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        Location oldLocation = currentLocation;

        Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);

        if (currentLocation == newLocation) return;
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);
        playerPoint.position = startPoint.position;
        currentLocation = newLocation;
    }

}