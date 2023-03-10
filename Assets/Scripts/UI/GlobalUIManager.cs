using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
public enum MENU
{
    MENU_TITLE_SCREEN,
    MENU_MAIN,
    MENU_SETTINGS,
    MENU_PAUSE,
    MENU_HUD,
    MENU_LOADING,
    MENU_SCOREBOARD,
    MENU_CREDITS,
    MENU_PREGAME,
    NONE
};

[System.Serializable]
public struct MenuRefToEnumPair
{
    public MENU menuType;
    public GameObject menuRef;
}

[System.Serializable]
public struct MenuPrefab
{
    public MENU menuType;
    public GameObject prefabRef;
}

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager instance;

    [SerializeField] public EventSystem es;

    [SerializeField] private MenuPrefab[] prefabsRef;
    [SerializeField] private Dictionary<MENU, MenuPrefab> prefabsDictionary = new Dictionary<MENU, MenuPrefab>();
    [SerializeField] private Dictionary<MENU, GameObject> runtimeMenuRefs = new Dictionary<MENU, GameObject>();

    private MENU currentMenu;
    private MENU lastMenu;
    private Stack<MENU> menuStack = new Stack<MENU>();
    private ushort menuStackSize = 0;

    public static bool gameIsActive = false;
    public static bool gameIsPaused = false;
    public static bool isPreGame = false;

    [SerializeField] private GameObject controllerIcon;
    public static bool isControllerConnected = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the onDeviceChange event
        InputSystem.onDeviceChange += OnDeviceChange;

        foreach (MenuPrefab prefab in prefabsRef)
        {
            prefabsDictionary.Add(prefab.menuType, prefab);
        }

        SetMenu(MENU.MENU_TITLE_SCREEN);

        DetectController();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsActive && es.enabled)
            {
                if(isPreGame)
                {
                    ReturnToMainMenu();
                }
                else
                {
                    if (gameIsPaused)
                    {
                        ResumeGame();
                    }
                    else
                    {
                        PauseGame();
                    }
                }
            }
        }
    }

    private void SetMenuInternal(MENU desiredMenu, bool addToStack = true)
    {
        StopAllCoroutines();

        if (lastMenu != MENU.NONE)
        {
            if (!runtimeMenuRefs.ContainsKey(lastMenu))
            {
                GameObject menu = Instantiate(prefabsDictionary[lastMenu].prefabRef);
                runtimeMenuRefs.Add(lastMenu, menu);
                menu.transform.SetParent(gameObject.transform);
                menu.transform.localPosition = new Vector2(Screen.width, Screen.height);
            }
        }

        if (!runtimeMenuRefs.ContainsKey(desiredMenu))
        {
            GameObject menu = Instantiate(prefabsDictionary[desiredMenu].prefabRef);
            runtimeMenuRefs.Add(desiredMenu, menu);
            menu.transform.SetParent(gameObject.transform);
            menu.transform.localPosition = new Vector2(Screen.width, Screen.height);
        }


        if (lastMenu != MENU.NONE)
        {
            AnimationManager.instance.PlayMenuAnimation(SetMenuAnimation(runtimeMenuRefs[lastMenu], false));
            AnimationManager.instance.PlayMenuAnimation(SetMenuAnimation(runtimeMenuRefs[desiredMenu], true), () => EnableInputs(true));
        }
        else
        {
            AnimationManager.instance.PlayMenuAnimation(SetMenuAnimation(runtimeMenuRefs[desiredMenu], true), () => EnableInputs(true));
        }

        //print($"Current Menu : {desiredMenu}");
        //print($"Current Menu : {currentMenu}");
        //print($"Last Menu : {lastMenu}");
    }

    public void SetMenu(MENU desiredMenu)
    {
        EnableInputs(false);

        if (menuStack.Count > 0)
        {
            lastMenu = menuStack.Peek();
        }
        else
        {
            lastMenu = MENU.NONE;
        }


        menuStack.Push(desiredMenu);
        menuStackSize++;

        currentMenu = desiredMenu;
        SetMenuInternal(desiredMenu);
    }


    public void SetLastMenu()
    {
        EnableInputs(false);

        if (menuStackSize <= 1)
        {
            Debug.LogAssertion("Can't go back, no menu left.");
        }

        if (menuStack.Count > 0)
        {
            lastMenu = menuStack.Peek();
        }
        else
        {
            lastMenu = MENU.NONE;
        }

        menuStack.Pop();
        menuStackSize--;

        currentMenu = menuStack.Peek();
        SetMenuInternal(menuStack.Peek());
    }

    public IEnumerator SetMenuAnimation(GameObject currentMenuObject, bool isGoingIn)
    {
        UIAnimation animation = currentMenuObject.GetComponent<UIAnimation>();
        animation.isGoingIn = isGoingIn;
        animation.play();

        if (isGoingIn)
        {
            yield return new WaitForSecondsRealtime(animation.animationDuration);
        }
        else
        {
            yield return new WaitForSecondsRealtime(animation.animationDuration + 0.2f);
        }
    }

    public void SetTitleScreenMenu()
    {
        CameraManager.instance.Transition(false);
        SetMenu(MENU.MENU_TITLE_SCREEN);
    }


    public void SetMainMenu()
    {
        CameraManager.instance.Transition(true);
        SetMenu(MENU.MENU_MAIN);
    }

    public void SetSettingsMenu()
    {
        SetMenu(MENU.MENU_SETTINGS);
    }

    public void SetPauseMenu()
    {
        SetMenu(MENU.MENU_PAUSE);
    }

    public void SetHUDMenu()
    {
        SetMenu(MENU.MENU_HUD);
    }

    public void SetLoadingMenu()
    {
        SetMenu(MENU.MENU_LOADING);
    }

    public void SetScoreBoardMenu()
    {
        SetMenu(MENU.MENU_SCOREBOARD);
    }

    public void SetCreditsMenu()
    {
        CameraManager.instance.TransitionToCredits();
        SetMenu(MENU.MENU_CREDITS);
    }

    public void SetPreGameMenu()
    {
        SetMenu(MENU.MENU_PREGAME);
    }

    public void ResumeGame()
    {
        AnimationManager.instance.ResumeInGameAnimations();
        SetMenu(MENU.MENU_HUD);

        Time.timeScale = 1f;

        gameIsPaused = false;
        StartCoroutine(PauseResumeCallback());
    }

    public void PauseGame()
    {
        AnimationManager.instance.PauseInGameAnimations();
        SetMenu(MENU.MENU_PAUSE);

        Time.timeScale = 0f;

        gameIsPaused = true;
        StartCoroutine(PauseResumeCallback());
    }

    public void ReplayGame()
    {
        SetMenu(MENU.MENU_PREGAME);
        Time.timeScale = 1f;
        gameIsPaused = false;
        StartCoroutine(PauseResumeCallback());
    }

    public IEnumerator PauseResumeCallback()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameIsActive = true;
    }

    public void LoadGame(GAME_MODE gameMode)
    {
        AudioManager.instance.PlaySound(SOUND.SWEEP);
        
        SetMenu(MENU.MENU_LOADING);
        
        ScenesManager.instance.gameMode = gameMode;

        switch (gameMode)
        {
            case GAME_MODE.INFINITE_MODE:
                ScenesManager.instance.LoadSceneAsync("infinite_game_scene", LoadGameCompletedCallback());
                break;
            case GAME_MODE.LEGACY_MODE:
                ScenesManager.instance.LoadSceneAsync("legacy_game_scene", LoadGameCompletedCallback());
                break;
            default:
                Debug.LogAssertion("Unknown Game Type!");
                break;
        }

        Time.timeScale = 1f;
        gameIsPaused = false;
        gameIsActive = true;
        isPreGame = true;
    }

    public void ReturnToMainMenu()
    {
        AudioManager.instance.PlayUiMusic();

        SetMenu(MENU.MENU_LOADING);

        switch (ScenesManager.instance.gameMode)
        {
            case GAME_MODE.INFINITE_MODE:
                ScenesManager.instance.UnloadSceneAsync("infinite_game_scene", UnloadGameCompletedCallback());
                break;
            case GAME_MODE.LEGACY_MODE:
                ScenesManager.instance.UnloadSceneAsync("legacy_game_scene", UnloadGameCompletedCallback());
                break;
            default:
                Debug.LogAssertion("Unknown Game Type!");
                break;
        }

        ScenesManager.instance.gameMode = GAME_MODE.NONE;

        Time.timeScale = 1f;
        gameIsPaused = false;
        gameIsActive = false;
        isPreGame = false;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ClearMenus()
    {
        foreach (var menu in runtimeMenuRefs)
        {
            Destroy(menu.Value);
        }
        runtimeMenuRefs.Clear();
    }

    public void ClearMenusException()
    {
        var keysToRemove = new List<MENU>();

        foreach (var menu in runtimeMenuRefs)
        {
            //  && lastMenu != menu.Key
            if (currentMenu != menu.Key)
            {
                Destroy(menu.Value);
                keysToRemove.Add(menu.Key);
            }
        }
        foreach (var key in keysToRemove)
        {
            runtimeMenuRefs.Remove(key);
        }
    }

    IEnumerator LoadGameCompletedCallback()
    {
        yield return null;
        //ClearMenus();
        CameraManager.instance.Transition(false);
        SetMenu(MENU.MENU_PREGAME);
    }

    IEnumerator UnloadGameCompletedCallback()
    {
        yield return null;
        //ClearMenus();
        CameraManager.instance.Transition(true);
        SetMenu(MENU.MENU_MAIN);
    }


    void DetectController()
    {
        //Get Joystick Names
        string[] temp = Input.GetJoystickNames();

        isControllerConnected = false;

        //Check whether array contains any
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    isControllerConnected = true;
                }
            }
        }

        controllerIcon.SetActive(isControllerConnected);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Check if the device type is a joystick or gamepad
        if (device is Gamepad || device is Joystick)
        {
            // Check if the device has been connected or disconnected
            if (change == InputDeviceChange.Added)
            {
                // A joystick or gamepad has been connected
                isControllerConnected = true;
            }
            else if (change == InputDeviceChange.Removed)
            {
                // A joystick or gamepad has been disconnected

                isControllerConnected = false;
            }
        }

        controllerIcon.SetActive(isControllerConnected);
    }

    public void EnableInputs(bool isEnabled)
    {
        es.enabled = isEnabled;
    }

    public void SetControllerFirstSelected (GameObject firstSelected)
    {
        if (isControllerConnected)
        {
            es.SetSelectedGameObject(firstSelected);
        }
    }
}
