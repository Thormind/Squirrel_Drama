using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && gameIsActive)
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
            //menu.transform.localPosition = new Vector2(Screen.width, Screen.height);
        }

        foreach (var menu in runtimeMenuRefs)
        {
            if (menu.Key == desiredMenu)
            {
                menu.Value.SetActive(true);
            }
            else
            {
                menu.Value.SetActive(false);
            }
        }

        print($"Current Menu : {currentMenu}");
        print($"Last Menu : {lastMenu}");
    }

    public void SetMenu(MENU desiredMenu)
    {

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

    public void ResumeGame()
    {
        SetLastMenu();
        gameIsPaused = false;
    }

    public void PauseGame()
    {
        SetMenu(MENU.MENU_PAUSE);
        gameIsPaused = true;
    }

    public void StartGame(int gameMode)
    {
        SetMenu(MENU.MENU_HUD);
        SetMenu(MENU.MENU_LOADING);

        ScenesManager.instance.gameMode = gameMode;

        switch (gameMode)
        {
            case 0:
                ScenesManager.instance.LoadSceneAsync("story_game_scene", LoadGameCompletedCallback());
                break;
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
        gameIsPaused = false;
        gameIsActive = true;
    }

    public void ReturnToMainMenu()
    {
        SetMenu(MENU.MENU_LOADING);

        ScenesManager.instance.LoadSceneAsync("ui_world_scene", UnloadGameCompletedCallback());

        switch (ScenesManager.instance.gameMode)
        {
            case 0:
                ScenesManager.instance.UnloadSceneAsync("story_game_scene");
                break;
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

        gameIsPaused = false;
        gameIsActive = false;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    IEnumerator LoadGameCompletedCallback()
    {
        yield return null;
        SetMenu(MENU.MENU_PAUSE);
        SetMenu(MENU.MENU_HUD);
    }

    IEnumerator UnloadGameCompletedCallback()
    {
        yield return null;
        SetMenu(MENU.MENU_MAIN);
    }
}
