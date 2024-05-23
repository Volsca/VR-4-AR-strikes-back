using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertFadeDir : MonoBehaviour
{
    public GlobalVariables GV;

    public void InvertFade()
    {
        GV.l_handFade.InvertFade();
        GV.r_handFade.InvertFade();
    }
}
