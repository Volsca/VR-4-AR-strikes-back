using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIFunctions : MonoBehaviour
{
    public GameObject grabbableUI;
    public GameObject rHand;
    public GameObject mainCamera;

    // Move the menu when activated
    public void GrabbableToggle()
    {
        //grabbableUI.transform.localPosition = rHand.transform.localPosition;
        //Vector3 offset = new Vector3(0, 180, 0);
        //grabbableUI.transform.LookAt(mainCamera.transform.position + offset);
        //grabbableUI.transform.Rotate(grabbableUI.transform.forward, 180.0f);
    }
}