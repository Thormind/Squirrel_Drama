using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWormsAnimation : MonoBehaviour
{
    public AudioSource wormSound;
    public Animator wormAnimator;
    private float inAnimationTime;
    private float derpAnimationTime;
    private float numberOfDerps;
    private float derpSpeed;

    private Vector3 initialPosition;

    private float outAnimationRotationOffset = 0f; // Rotation angle to rotate on the Z-axis during the out animation

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
    public void TestHandleNewWormAnimationFunction()
    {
        float randomInAnimationTime = Random.Range(1.5f - 0.5f, 1.5f + 0.5f);
        float randomNumberOfDerpsAnimation = Random.Range(2f - 1f, 2f + 1f);
        float randomAnimationSpeed = Random.Range(200f - 50f, 200f + 50f);

        wormAnimator = GetComponent<Animator>();
        inAnimationTime = 1.5f;
        numberOfDerps = 2f;
        derpSpeed = 200f;



        StartCoroutine(HandleNewWormInAnimation());
    }


    public void HandleNewWormAnimationFunction(float inTime, int derpTurns, float animationSpeed)
    {
        wormAnimator = GetComponent<Animator>();
        inAnimationTime = inTime;
        numberOfDerps = derpTurns;
        derpSpeed = animationSpeed;

        StartCoroutine(HandleNewWormInAnimation());
    }

    IEnumerator HandleNewWormInAnimation()
    {
        Debug.Log("testing animation!");
        float t = 0;

        wormAnimator.Play("Sort");

        // Both while loops are needed because the clip does not start instantly. The first loop
        // wait for the clip to start and the second one wait for the clip to finish.

        while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

        while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

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
        Debug.Log("wormOut");

        wormAnimator.Play("SeCrinque");

        // wormSound.Play();

        // Both while loops are needed because the clip does not start instantly. The first loop
        // wait for the clip to start and the second one wait for the clip to finish. 

        while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

        while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("SeCrinque termine");


        StartCoroutine(HandleNewWormDerpAnimation());
     
        yield return null;
    }

    IEnumerator HandleNewWormDerpAnimation()
    {
        float remainingTurns = numberOfDerps;

        while (remainingTurns > 0)
        {
            
            wormAnimator.Play("seTourne");

            // Both while loops are needed because the clip does not start instantly. The first loop
            // wait for the clip to start and the second one wait for the clip to finish.

            while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                yield return new WaitForFixedUpdate();
            }

            while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return new WaitForFixedUpdate();
            }

            remainingTurns--;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleNewWormBackInAnimation());

        yield return null;
    }

    IEnumerator HandleNewWormBackInAnimation()
    {
        
        wormAnimator.Play("seRevient");

        // Both while loops are needed because the clip does not start instantly. The first loop
        // wait for the clip to start and the second one wait for the clip to finish.

        while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

        while (wormAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
        
        yield return null;

    }




}
