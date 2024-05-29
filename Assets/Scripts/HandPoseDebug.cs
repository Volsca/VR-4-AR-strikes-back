using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseDebug : MonoBehaviour
{
    public GlobalVariables GV;

    public void ThumbsUp()
    {
        GV._DebugWindow.writeDebugMessage("Recognized!!!", 0, "HandPoseDebug");
        GV.l_handFade.BunnyFadeChange();
        GV.r_handFade.BunnyFadeChange();
    }
}
