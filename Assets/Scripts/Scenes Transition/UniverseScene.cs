using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UniverseScene : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
        UIManager.Instance.ToggleImapPanel();
    }
}
