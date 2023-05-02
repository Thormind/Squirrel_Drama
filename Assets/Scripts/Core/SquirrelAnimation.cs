using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelAnimation : MonoBehaviour
{
    public Animator squirrelAnimator;

    public IEnumerator HandleSquirrelTakeFruitAnimation()
    {
        squirrelAnimator.Play("prendPomme");

        while (squirrelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

        while (squirrelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

    }

    public IEnumerator HandleSquirrelLoveFruitAnimation()
    {
        squirrelAnimator.Play("aimePomme");

        while (squirrelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

        while (squirrelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }

    }

}
