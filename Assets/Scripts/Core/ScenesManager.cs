using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public enum GAME_STATE
{
	ACTIVE,
	INACTIVE,
	PAUSED,
	PRE_GAME,
	LEVEL_COMPLETED,
	GAME_COMPLETED,
	GAME_OVER,
	DIED,
	PREPARING,
	LOADING
};

public enum GAME_MODE
{
	NONE, 
	INFINITE_MODE,
	LEGACY_MODE
};

public enum TIME_OF_DAY
{
	NOON,
	NIGHT
};

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;

    public float loadProgress;
    public float unloadProgress;

	public static GAME_MODE gameMode;
	public static TIME_OF_DAY timeOfDay;

	public delegate void GameStateChangedEventHandler(GAME_STATE newGameState);
	public static event GameStateChangedEventHandler OnGameStateChanged;

	public static GAME_STATE gameState
	{
		get { return _gameState; }
		set
		{
			_gameState = value;
			OnGameStateChanged?.Invoke(_gameState);
		}
	}
	private static GAME_STATE _gameState;

	private void Awake()
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

	private void Start()
	{
		OnGameStateChanged += HandleGameStateChanged;

		gameState = GAME_STATE.INACTIVE;
		gameMode = GAME_MODE.NONE;
	}

	private void OnDestroy()
	{
		OnGameStateChanged -= HandleGameStateChanged;
	}

	private void HandleGameStateChanged(GAME_STATE newGameState)
	{
		//print($"{gameState}");
	}


	private IEnumerator LoadSceneAsyncInternal(string sceneName, IEnumerator sceneLoadedCallback)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			loadProgress = asyncLoad.progress;

			yield return null;
		}

		if (sceneLoadedCallback != null) StartCoroutine(sceneLoadedCallback);
	}

	public void LoadSceneAsync(string sceneName, IEnumerator sceneLoadedCallback = null)
	{
		StartCoroutine(LoadSceneAsyncInternal(sceneName, sceneLoadedCallback));
	}


	private IEnumerator UnloadSceneAsyncInternal(string sceneName, IEnumerator sceneUnloadedCallback)
	{
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

		// Wait until the asynchronous scene fully loads
		while (!asyncUnload.isDone)
		{
			unloadProgress = asyncUnload.progress;

			yield return null;
		}

		if (sceneUnloadedCallback != null) StartCoroutine(sceneUnloadedCallback);

	}

	public void UnloadSceneAsync(string sceneName, IEnumerator sceneUnloadedCallback = null)
	{
		StartCoroutine(UnloadSceneAsyncInternal(sceneName, sceneUnloadedCallback));
	}
}
