using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PrototypeSceneManager : MonoBehaviour
{

    public TMP_Dropdown prototypeDropdown;
    public GameObject loadingPanel;
    public Button exitButton;

    public Scene currentScene;

    public int previousIndex;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(() => Application.Quit());
        prototypeDropdown.onValueChanged.AddListener(delegate { HandlePrototypeInputData(prototypeDropdown.value); });

        prototypeDropdown.value = 0;
        prototypeDropdown.RefreshShownValue();
        previousIndex = prototypeDropdown.value;
        LoadPrototype(prototypeDropdown.value);
    }

    public void HandlePrototypeInputData(int index)
    {
        print($"Current Prototype : {index}");
        LoadPrototype(index);
        UnloadPrototype();
        previousIndex = index;
    }

    public void LoadPrototype(int index)
    {
        print($"Current Prototype : {index}");

        loadingPanel.SetActive(true);

        switch (index)
        {
            case 0:
                ScenesManager.instance.LoadSceneAsync("animation_prototype", LoadPrototypeCompletedCallback());
                break;
            case 1:
                ScenesManager.instance.LoadSceneAsync("3D_model_prototype", LoadPrototypeCompletedCallback());
                break;
            case 2:
                ScenesManager.instance.LoadSceneAsync("audio_prototype", LoadPrototypeCompletedCallback());
                break;
            case 3:
                ScenesManager.instance.LoadSceneAsync("elevator_prototype", LoadPrototypeCompletedCallback());
                break;
            case 4:
                ScenesManager.instance.LoadSceneAsync("generation_prototype", LoadPrototypeCompletedCallback());
                break;
            default:
                Debug.LogAssertion("Unknown Prototype Type!");
                break;
        }
    }

    public void UnloadPrototype()
    {
        switch (previousIndex)
        {
            case 0:
                ScenesManager.instance.UnloadSceneAsync("animation_prototype");
                break;
            case 1:
                ScenesManager.instance.UnloadSceneAsync("3D_model_prototype");
                break;
            case 2:
                ScenesManager.instance.UnloadSceneAsync("audio_prototype");
                break;
            case 3:
                ScenesManager.instance.UnloadSceneAsync("elevator_prototype");
                break;
            case 4:
                ScenesManager.instance.UnloadSceneAsync("generation_prototype");
                break;
            default:
                Debug.LogAssertion("Unknown Prototype Type!");
                break;
        }
    }

    IEnumerator LoadPrototypeCompletedCallback()
    {
        yield return null;
        loadingPanel.SetActive(false);
    }

}
