using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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
    public MENU specificMenu;
    private Stack<MENU> menuStack = new Stack<MENU>();
    private ushort menuStackSize = 0;

    public DefaultInputActions UIControls;
    private InputAction back;
    private InputAction pause;
    private InputAction confirm;

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

        UIControls = new DefaultInputActions();
    }

    private void OnEnable()
    {
        pause = UIControls.UI.Pause;
        confirm = UIControls.UI.Confirm;
        back = UIControls.UI.Back;

        pause.Enable();
        confirm.Enable();
        back.Enable();

        pause.performed += OnPause;
        back.performed += OnBack;
    }

    private void OnDisable()
    {
        pause.Disable();
        confirm.Disable();
        back.Disable();

        pause.performed -= OnPause;
        back.performed -= OnBack;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (es.enabled)
        {
            AudioManager.instance.Pause();
            switch (ScenesManager.gameState)
            {
                case GAME_STATE.PAUSED: // PAUSED_STATE
                    ResumeGame();
                    break;
                case GAME_STATE.ACTIVE: // ACTIVE_STATE
                    PauseGame();
                    break;
            }
        }
    }

    private void OnBack(InputAction.CallbackContext context)
    {
        if (es.enabled)
        {
            switch (ScenesManager.gameState)
            {
                case GAME_STATE.GAME_OVER: // PAUSED_STATE
                    ReturnToMainMenu();
                    break;
                case GAME_STATE.PRE_GAME: // PRE_GAME_STATE
                    ReturnToMainMenu();
                    break;
                case GAME_STATE.PAUSED: // PAUSED_STATE
                    ResumeGame();
                    break;
                case GAME_STATE.ACTIVE: // ACTIVE_STATE
                    PauseGame();
                    break;
                case GAME_STATE.INACTIVE: // INACTIVE_STATE
                    SetSpecificMenu();
                    break;
            }
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

    private void SetMenuInternal(MENU desiredMenu)
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

        SetCameraAnimation(desiredMenu);

        if (lastMenu != MENU.NONE)
        {
            AnimationManager.instance.PlayMenuAnimation(SetMenuAnimation(lastMenu, false));
            AnimationManager.instance.PlayMenuAnimation(SetMenuAnimation(desiredMenu, true));
        }
        else
        {
            AnimationManager.instance.PlayMenuAnimation(SetMenuAnimation(desiredMenu, true));
        }

        //print($"LAST MENU: {lastMenu}");
        //print($"CURRENT MENU: {desiredMenu}");

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


        if (menuStack.Count > 1)
        {
            menuStack.Pop();
            menuStackSize--;

            currentMenu = menuStack.Peek();
            SetMenuInternal(menuStack.Peek());
        }
        else
        {
            EnableInputs(true);
        }
    }

    public void SetSpecificMenu()
    {
        if (specificMenu != MENU.NONE)
        {
            SetMenu(specificMenu);
        }
    }

    public IEnumerator SetMenuAnimation(MENU desiredMenu, bool isGoingIn)
    {
        UIAnimation animation = runtimeMenuRefs[desiredMenu].GetComponent<UIAnimation>();
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

    public void SetCameraAnimation(MENU desiredMenu)
    {
        switch (desiredMenu)
        {
            case MENU.MENU_TITLE_SCREEN:
                CameraManager.instance.Transition(false);
                break;
            case MENU.MENU_MAIN:
                CameraManager.instance.Transition(true);
                break;
            case MENU.MENU_CREDITS:
                CameraManager.instance.TransitionToCredits();
                break;
            case MENU.MENU_PREGAME:
                CameraManager.instance.Transition(false);
                break;
        }
    }

    public void SetTitleScreenMenu()
    {
        SetMenu(MENU.MENU_TITLE_SCREEN);
    }


    public void SetMainMenu()
    {
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
        SetMenu(MENU.MENU_CREDITS);
    }

    public void SetPreGameMenu()
    {
        SetMenu(MENU.MENU_PREGAME);
    }

    public void ResumeGame()
    {
        AudioManager.instance.UnPause();
        AnimationManager.instance.ResumeInGameAnimations();
        AnimationManager.instance.ResumeObstaclesAnimations();
        SetMenu(MENU.MENU_HUD); 

        Time.timeScale = 1f;

        ScenesManager.gameState = GAME_STATE.ACTIVE;
        StartCoroutine(PauseResumeCallback());
    }

    public void PauseGame()
    {
        ScenesManager.gameState = GAME_STATE.PAUSED;

        Time.timeScale = 0f;
        AnimationManager.instance.PauseInGameAnimations();
        AnimationManager.instance.PauseObstaclesAnimations();

        SetMenu(MENU.MENU_PAUSE);

        StartCoroutine(PauseResumeCallback());
    }

    public void ReplayGame()
    {
        ScenesManager.gameState = GAME_STATE.PRE_GAME;

        Time.timeScale = 1f;
        AnimationManager.instance.ClearInGameQueue();

        SetMenu(MENU.MENU_PREGAME);

        StartCoroutine(PauseResumeCallback());
    }

    public IEnumerator PauseResumeCallback()
    {
        yield return new WaitForSecondsRealtime(1f);
    }

    public void LoadGame(GAME_MODE gameMode)
    {
        AudioManager.instance.PlaySound(SOUND.SWEEP);
        
        SetMenu(MENU.MENU_LOADING);
        ScenesManager.gameState = GAME_STATE.LOADING;

        ScenesManager.gameMode = gameMode;

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
    }

    public void ReturnToMainMenu()
    {
        AnimationManager.instance.ClearInGameQueue();
        AnimationManager.instance.ClearObstaclesQueue();

        SetMenu(MENU.MENU_LOADING);
        ScenesManager.gameState = GAME_STATE.LOADING;

        switch (ScenesManager.gameMode)
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

        ScenesManager.gameMode = GAME_MODE.NONE;

        Time.timeScale = 1f;
    }

    IEnumerator LoadGameCompletedCallback()
    {
        yield return null;

        SetMenu(MENU.MENU_PREGAME);
        ScenesManager.gameState = GAME_STATE.PRE_GAME;

        AudioManager.instance.SwitchAudioListener(ScenesManager.gameMode);

        ClearMenusException();
    }

    IEnumerator UnloadGameCompletedCallback()
    {
        yield return null;

        AudioManager.instance.StopWind();
        AudioManager.instance.PlayUiMusic();

        SetMenu(MENU.MENU_MAIN);
        ScenesManager.gameState = GAME_STATE.INACTIVE;

        AudioManager.instance.SwitchAudioListener(ScenesManager.gameMode);

        ClearMenusException();
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
        menuStack.Clear();
        menuStack.Push(lastMenu);
        menuStack.Push(currentMenu);
        menuStackSize = 2;

        var keysToRemove = new List<MENU>();

        foreach (var menu in runtimeMenuRefs)
        {
            // && lastMenu != menu.Key
            if (currentMenu != menu.Key && lastMenu != menu.Key)
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

    public void SetFirstSelected (GameObject firstSelected, bool alt = false)
    {
        es.SetSelectedGameObject(null); //Resetting the currently selected GO
        es.firstSelectedGameObject = firstSelected;

        if (alt && es.currentSelectedGameObject == null)
        {
            es.SetSelectedGameObject(firstSelected, new BaseEventData(es));
        }
    }



}
