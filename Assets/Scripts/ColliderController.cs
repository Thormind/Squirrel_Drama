using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            // Perform specific action
            Debug.Log("Collision with object with tag 'Obstacles' detected!");
            ElevatorController.instance.ResetElevator();
        }
    }
}
