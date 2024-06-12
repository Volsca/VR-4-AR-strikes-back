using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public GlobalVariables GV;
    public GameObject can;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //Debug.Log("Button pressed");
            GV._DebugWindow.writeDebugMessage("Button pressed", 0, "");
            onButtonPress();
        }
    }

    void onButtonPress()
    {
        can.transform.localPosition = GV.r_Controller.transform.localPosition;
    }
}
