using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    private Queue<IEnumerator> menuAnimationQueue = new Queue<IEnumerator>();
    private Queue<IEnumerator> inGameAnimationQueue = new Queue<IEnumerator>();

    private bool isPlayingMenuAnimation = false;
    private bool isPlayingInGameAnimation = false;

    private bool isPausedInGameAnimation = false;

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
