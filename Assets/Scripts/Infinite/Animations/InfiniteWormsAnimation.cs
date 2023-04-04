using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWormsAnimation : MonoBehaviour
{
    public AudioSource wormSound;
    private float inAnimationTime;
    private float derpAnimationTime;
    private float numberOfDerps;
    private float derpSpeed;

    private Vector3 initialPosition;

    private float outAnimationRotationOffset = 180f; // Rotation angle to rotate on the Z-axis during the out animation

    public void HandleWormAnimationFunction(float inTime, float derpTime, float animationSpeed, Vector3 initialPos)
    {
        inAnimationTime = inTime;
        derpAnimationTime = derpTime;
        derpSpeed = animationSpeed;
        initialPosition = initialPos;

        StartCoroutine(HandleWormInAnimation());

    }

    IEnumerator HandleWormInAnimation()
    {
        float t = 0;

        while (t <= 1)
        {
            t += Time.fixedDeltaTime / inAnimationTime;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleWormOutAnimation());

        yield return null;
    }

    IEnumerator HandleWormOutAnimation()
    {
        Vector3 _initialPosition = transform.localPosition;
        Quaternion _initialRotation = transform.localRotation;

        Vector3 outStartPosition = new Vector3(_initialPosition.x, _initialPosition.y, 0);
        Quaternion outStartRotation = Quaternion.Euler(0f, outAnimationRotationOffset, Random.Range(0f, outAnimationRotationOffset));

        float t = 0;

        wormSound.Play();

        while (t <= 1)
        {
            t += Time.fixedDeltaTime / 0.5f;

            transform.localPosition = Vector3.Lerp(_initialPosition, outStartPosition, t);
            transform.localRotation = Quaternion.Lerp(_initialRotation, outStartRotation, t);

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleWormDerpAnimation());

        yield return null;
    }

    IEnumerator HandleWormDerpAnimation()
    {
        float t = 0;

        while (t <= 1)
        {
            transform.Rotate(Vector3.forward * Time.fixedDeltaTime * derpSpeed, Space.World);

            t += Time.fixedDeltaTime / derpAnimationTime;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleWormBackInAnimation());

        yield return null;
    }

    IEnumerator HandleWormBackInAnimation()
    {
        Vector3 _initialPosition = transform.localPosition;
        Quaternion _initialRotation = transform.localRotation;

        Vector3 inStartPosition = new Vector3(_initialPosition.x, _initialPosition.y, 1f);
        Quaternion inStartRotation = Quaternion.Euler(-90f, 0f, 0f);

        float t = 0;

        while (t <= 1)
        {
            t += Time.fixedDeltaTime / 0.5f;

            transform.localPosition = Vector3.Lerp(_initialPosition, inStartPosition, t);
            transform.localRotation = Quaternion.Lerp(_initialRotation, inStartRotation, t);

            yield return new WaitForFixedUpdate();
        }

        InfiniteWormsController.instance.RemoveSpawnedPosition(initialPosition);
        Destroy(gameObject);

        yield return null;
    }


    // ===== NEW ANIMATION ===== //

    [ContextMenu("Test Animation")]
    public void HandleNewWormAnimationFunction(float inTime, int derpTurns, float animationSpeed)
    {
        inAnimationTime = inTime;
        numberOfDerps = derpTurns;
        derpSpeed = animationSpeed;

        StartCoroutine(HandleNewWormInAnimation());
    }

    IEnumerator HandleNewWormInAnimation()
    {
        float t = 0;

        while (t <= 1)
        {
            t += Time.fixedDeltaTime / inAnimationTime;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleNewWormOutAnimation());

        yield return null;
    }

    IEnumerator HandleNewWormOutAnimation()
    {
        /*
        wormAnimator.Play("seCrinque");

        wormSound.Play();

        while (seCrinque.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleNewWormDerpAnimation());
        */
        yield return null;
    }

    IEnumerator HandleNewWormDerpAnimation()
    {
        float remainingTurns = numberOfDerps;

        while (remainingTurns > 0)
        {
            /*
            wormAnimator.Play("seTourne");

            while (seTourne.isPlaying)
            {
                yield return new WaitForFixedUpdate();
            }
            */
            remainingTurns--;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleNewWormBackInAnimation());

        yield return null;
    }

    IEnumerator HandleNewWormBackInAnimation()
    {
        /*
        wormAnimator.Play("seRevient"); 

        while (seRevient.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
        */
        yield return null;

    }
}
