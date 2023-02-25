using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public enum MENU
{
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

        foreach (MenuPrefab prefab in prefabsRef)
        {
            prefabsDictionary.Add(prefab.menuType, prefab);
        }

        SetMenu(MENU.MENU_MAIN);
    }

    public void OnPauseResume()
    {
        //(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && 
        if (gameIsActive && es.enabled)
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

    private void SetMenuInternal(MENU desiredMenu, bool addToStack = true)
    {

        if (!runtimeMenuRefs.ContainsKey(desiredMenu))
        {
            GameObject menu = Instantiate(prefabsDictionary[desiredMenu].prefabRef);
            runtimeMenuRefs.Add(desiredMenu, menu);
            menu.transform.SetParent(gameObject.transform);
            menu.transform.localPosition = new Vector2(Screen.width, Screen.height);
        }

        foreach (var menu in runtimeMenuRefs)
        {
            if (menu.Key == desiredMenu)
            {
                StartCoroutine(SetAnimation(menu.Key, menu.Value, 1));
            }
            if (menu.Key == lastMenu)
            {
                StartCoroutine(SetAnimation(menu.Key, menu.Value, 0));
            }
        }

        print($"Current Menu : {currentMenu}");
        print($"Last Menu : {lastMenu}");
    }

    public void SetMenu(MENU desiredMenu)
    {
        es.enabled = false;

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
        es.enabled = false;

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

    public IEnumerator SetAnimation(MENU menuType, GameObject menuObject, int animationInOut)
    {

        if (menuType == MENU.MENU_CREDITS)
        {
            if (animationInOut == 1)
            {
                menuObject.SetActive(true);
            }
            else
            {
                menuObject.SetActive(false);
            }
        }

        UIAnimation animation = menuObject.GetComponent<UIAnimation>();
        animation.animationInOut = animationInOut;
        animation.startPosition = new Vector2(0, Screen.height);  // Set the starting position for the animation
        animation.endPosition = Vector2.zero;  // Set the ending position for the animation
        animation.play();

        //yield return new WaitForSecondsRealtime(0.01f);

        yield return new WaitForSecondsRealtime(animation.animationDuration + 0.2f);

        ClearMenusException();
        es.enabled = true;
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
        SetMenu(MENU.MENU_HUD);
        Time.timeScale = 1f;
        gameIsPaused = false;
        StartCoroutine(PauseResumeCallback());
    }

    public void PauseGame()
    {
        SetMenu(MENU.MENU_PAUSE);
        //Time.timeScale = 0f;
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

    public void LoadGame(int gameMode)
    {
        SetMenu(MENU.MENU_LOADING);

        ScenesManager.instance.gameMode = gameMode;

        switch (gameMode)
        {
            case 1:
                ScenesManager.instance.LoadSceneAsync("infinite_game_scene", LoadGameCompletedCallback());
                break;
            case 2:
                ScenesManager.instance.LoadSceneAsync("legacy_game_scene", LoadGameCompletedCallback());
                break;
            default:
                Debug.LogAssertion("Unknown Game Type!");
                break;
        }

        ScenesManager.instance.UnloadSceneAsync("ui_world_scene");
        Time.timeScale = 1f;
        gameIsPaused = false;
        gameIsActive = true;
    }

    public void ReturnToMainMenu()
    {
        SetMenu(MENU.MENU_LOADING);

        ScenesManager.instance.LoadSceneAsync("ui_world_scene", UnloadGameCompletedCallback());

        switch (ScenesManager.instance.gameMode)
        {
            case 1:
                ScenesManager.instance.UnloadSceneAsync("infinite_game_scene");
                break;
            case 2:
                ScenesManager.instance.UnloadSceneAsync("legacy_game_scene");
                break;
            default:
                Debug.LogAssertion("Unknown Game Type!");
                break;
        }

        Time.timeScale = 1f;
        gameIsPaused = false;
        gameIsActive = false;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void UpdateHUD()
    {
        GameObject HUDmenu = runtimeMenuRefs[MENU.MENU_HUD];
        //HUDmenu.GetComponent<HUDMenuManager>().UpdateHUD();
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
        ClearMenus();
        SetMenu(MENU.MENU_PREGAME);
    }

    IEnumerator UnloadGameCompletedCallback()
    {
        yield return null;
        ClearMenus();
        SetMenu(MENU.MENU_MAIN);
    }
}
