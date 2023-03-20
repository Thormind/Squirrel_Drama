using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ANIMATION_TYPE
{
    // INACTIVE
    MENU,
    // ACTIVE / INGAME
    HUD,
    OBSTACLES,
    ELEVATOR,
    LEVEL_COMPLETED,
    SQUIRREL
};

[System.Serializable]
public struct AnimationQueue
{
    public Queue<IEnumerator> animationQueue;
    public bool isPlayingAnimation;
    public bool isPausedAnimation;
}

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    private Queue<IEnumerator> menuAnimationQueue = new Queue<IEnumerator>();
    private Queue<IEnumerator> inGameAnimationQueue = new Queue<IEnumerator>();
    private Queue<IEnumerator> obstaclesAnimationQueue = new Queue<IEnumerator>();

    private bool isPlayingMenuAnimation = false;
    private bool isPlayingInGameAnimation = false;
    private bool isPlayingObstaclesAnimation = false;

    private bool isPausedInGameAnimation = false;
    private bool isPausedObstaclesAnimation = false;


    //[SerializeField] private Dictionary<ANIMATION_TYPE, AnimationQueue> runtimeAnimationRefs = new Dictionary<ANIMATION_TYPE, AnimationQueue>();

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

    // =========== MENU ANIMATION =========== //

    public void PlayMenuAnimation(IEnumerator animation, System.Action callback = null)
    {
        // Add the animation coroutine to the queue
        menuAnimationQueue.Enqueue(Animate(animation, callback));

        // Start playing the queue if it's not already playing
        if (!isPlayingMenuAnimation)
        {
            StartCoroutine(PlayMenuQueue());
        }
    }

    private IEnumerator PlayMenuQueue()
    {
        // Set the playing flag to true
        isPlayingMenuAnimation = true;

        while (menuAnimationQueue.Count > 0)
        {
            // Dequeue the next animation coroutine and start it
            yield return StartCoroutine(menuAnimationQueue.Dequeue());
        }

        // Set the playing flag to false
        isPlayingMenuAnimation = false;

        GlobalUIManager.instance.EnableInputs(true);
    }

    public void ClearMenuQueue()
    {
        menuAnimationQueue.Clear();

        // Set the playing flag to false
        isPlayingMenuAnimation = false;

    }

    // =========== IN GAME ANIMATION =========== //

    public void PlayInGameAnimation(IEnumerator animation, System.Action callback = null)
    {
        // Add the animation coroutine to the queue
        inGameAnimationQueue.Enqueue(Animate(animation, callback));

        // Start playing the queue if it's not already playing
        if (!isPlayingInGameAnimation)
        {
            StartCoroutine(PlayInGameQueue());
        }
    }

    public void PauseInGameAnimations()
    {
        // Set the paused flag to true
        isPausedInGameAnimation = true;
    }

    public void ResumeInGameAnimations()
    {
        // Set the paused flag to false
        isPausedInGameAnimation = false;
    }

    private IEnumerator PlayInGameQueue()
    {
        // Set the playing flag to true
        isPlayingInGameAnimation = true;

        while (inGameAnimationQueue.Count > 0)
        {
            if (!isPausedInGameAnimation)
            {
                // Dequeue the next animation coroutine and start it
                yield return StartCoroutine(inGameAnimationQueue.Dequeue());
            }
            else
            {
                // Pause the coroutine and wait until we're unpaused
                yield return null;
            }
        }

        // Set the playing flag to false
        isPlayingInGameAnimation = false;
    }

    public void ClearInGameQueue()
    {
        inGameAnimationQueue.Clear();

        // Set the playing flag to false
        isPlayingMenuAnimation = false;

        isPausedInGameAnimation = false;
    }

    // =========== OBSTACLES ANIMATION =========== //

    public void PlayObstaclesAnimation(IEnumerator animation, System.Action callback = null)
    {
        // Add the animation coroutine to the queue
        obstaclesAnimationQueue.Enqueue(Animate(animation, callback));

        // Start playing the queue if it's not already playing
        if (!isPlayingObstaclesAnimation)
        {
            StartCoroutine(PlayObstaclesQueue());
        }
    }

    private IEnumerator PlayObstaclesQueue()
    {
        // Set the playing flag to true
        isPlayingObstaclesAnimation = true;

        while (obstaclesAnimationQueue.Count > 0)
        {
            if (!isPausedObstaclesAnimation)
            {
                // Dequeue the next animation coroutine and start it
                yield return StartCoroutine(obstaclesAnimationQueue.Dequeue());
            }
            else
            {
                // Pause the coroutine and wait until we're unpaused
                yield return null;
            }

        }

        // Set the playing flag to false
        isPlayingObstaclesAnimation = false;
    }

    public void PauseObstaclesAnimations()
    {
        // Set the paused flag to true
        isPausedObstaclesAnimation = true;
    }

    public void ResumeObstaclesAnimations()
    {
        // Set the paused flag to false
        isPausedObstaclesAnimation = false;
    }

    public void ClearObstaclesQueue()
    {
        obstaclesAnimationQueue.Clear();

        // Set the playing flag to false
        isPlayingObstaclesAnimation = false;

        isPausedObstaclesAnimation = false;
    }

    // ============================ //

    private IEnumerator Animate(IEnumerator animation, System.Action callback)
    {
        // Start the animation
        yield return StartCoroutine(animation);

        // Animation finished, invoke the callback if provided
        if (callback != null)
        {
            callback();
        }
    }
}
