using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyHolesTest : MonoBehaviour
{

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R) && LegacyHoleController.instance != null)
        {
            LegacyHoleController.instance.SpawnHoles();
        }

        if (Input.GetKeyDown(KeyCode.Space) && LegacyGameController.instance != null)
        {
            LegacyGameController.instance.StartGame();
        }

    }
}
