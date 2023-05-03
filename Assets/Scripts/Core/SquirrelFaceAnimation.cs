using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelFaceAnimation : MonoBehaviour
{
    public Animator squirrelFaceAnimator;

    public IEnumerator HandleHappySquirrelAnimation()
    {
        squirrelFaceAnimator.Play("content");

        yield return null;
    }

    public IEnumerator HandleAngrySquirrelAnimation()
    {
        squirrelFaceAnimator.Play("fache");

        yield return null;
    }

    public IEnumerator HandleSadSquirrelAnimation()
    {
        squirrelFaceAnimator.Play("triste");

        yield return null;
    }

}
