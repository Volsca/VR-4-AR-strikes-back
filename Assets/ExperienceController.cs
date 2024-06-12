using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public GlobalVariables GV;

    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
        {
            GV._DebugWindow.writeDebugMessage("Button pressed", 0, "");
        }
    }

    void onButtonPress()
    {
        
    }
}
