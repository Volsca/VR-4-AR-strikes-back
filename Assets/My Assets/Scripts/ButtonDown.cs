using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDown : MonoBehaviour
{
    public static event Action AButtonDown;
    public static event Action BButtonDown;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            AButtonDown?.Invoke();
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            BButtonDown?.Invoke();
        }
    }
}
