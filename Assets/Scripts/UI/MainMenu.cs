using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;

    public void NewGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.Farm,null));
        
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.Farm, LoadGame));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void LoadGame()
    {
        if(GameStateManager.Instance == null)
        {
            Debug.LogError("Cannot find Game State Manager!");
            return;
        }
        GameStateManager.Instance.LoadSave();
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFireFrameLoad)
    {
        AsyncOperation ayncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        DontDestroyOnLoad(gameObject);
        // Wait for the scene to load
        while(!ayncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading");
        }

        // Scene Loaded
        Debug.Log("Loaded!");
        yield return new WaitForEndOfFrame();
        Debug.Log("First Frame is load");
        // If there is an Action assign it
        Destroy(gameObject);
    }

    private void Start()
    {
        loadGameButton.interactable = SaveManager.HasSave();
    }


}
