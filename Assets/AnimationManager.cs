using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    private Queue<IEnumerator> animationQueue = new Queue<IEnumerator>();

    private bool isPlaying = false;
    private bool isPaused = false;

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

    public void PlayAnimation(IEnumerator animation, System.Action callback = null)
    {
        // Add the animation coroutine to the queue
        animationQueue.Enqueue(Animate(animation, callback));

        // Start playing the queue if it's not already playing
        if (!isPlaying)
        {
            StartCoroutine(PlayQueue());
        }
    }

    public void PauseAnimations()
    {
        // Set the paused flag to true
        isPaused = true;
    }

    public void ResumeAnimations()
    {
        // Set the paused flag to false
        isPaused = false;
    }

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

    private IEnumerator PlayQueue()
    {
        // Set the playing flag to true
        isPlaying = true;

        while (animationQueue.Count > 0)
        {
            if (!isPaused)
            {
                // Dequeue the next animation coroutine and start it
                yield return StartCoroutine(animationQueue.Dequeue());
            }
            else
            {
                // Pause the coroutine and wait until we're unpaused
                yield return null;
            }
        }

        // Set the playing flag to false
        isPlaying = false;
    }
}
