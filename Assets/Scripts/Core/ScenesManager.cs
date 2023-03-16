using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public GAME_MODE gameMode;
	public TIME_OF_DAY timeOfDay;

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
		gameMode = GAME_MODE.NONE;
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
