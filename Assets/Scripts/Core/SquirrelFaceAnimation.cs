using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelFaceAnimation : MonoBehaviour
{
    public Animator squirrelFaceAnimator;

    public IEnumerator HandleHappySquirrelAnimation()
    {
        AudioManager.instance.PlaySound(SOUND.SQUIRREL_HAPPY, 30000);
        AudioManager.instance.PlaySound(SOUND.SQUIRREL_KISS, 110000);
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
        AudioManager.instance.PlaySound(SOUND.SQUIRREL_SAD, 40000);
        squirrelFaceAnimator.Play("triste");

        yield return null;
    }

}
