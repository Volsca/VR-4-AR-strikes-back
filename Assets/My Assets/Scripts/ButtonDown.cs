using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDown : MonoBehaviour
{
    public static event Action AButtonDown;
    public static event Action BButtonDown;
    public static event Action ThumbstickDown;

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
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            ThumbstickDown?.Invoke();
        }
    }
}
