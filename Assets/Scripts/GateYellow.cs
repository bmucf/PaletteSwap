using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateYellow : MonoBehaviour
{
    public int requiredCollectables = 3;
    private bool gateOpened = false;

    public YellowGem gem;

    private void Update()
    {
        if (!gateOpened && BucketYellow.collectedCount >= requiredCollectables)
        {
            gateOpened = true;
            OpenGate();
        }
    }

    private void OpenGate()
    {
        Debug.Log("Gate opened!");
        Destroy(gameObject);

        // Activate the gem so it can now be collected
        if (gem != null)
            gem.Activate();
    }
}
