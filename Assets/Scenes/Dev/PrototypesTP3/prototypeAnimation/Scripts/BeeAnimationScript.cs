using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeAnimationScript : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleBothWings(bool newState)
    {
        animator.SetBool("isBothWings", newState);
    }

    public void ToggleLeftWing(bool newState)
    {
        animator.SetBool("isLeftWing", newState);
    }


    public void ToggleRightWing(bool newState)
    {
        animator.SetBool("isRightWing", newState);
    }

}
