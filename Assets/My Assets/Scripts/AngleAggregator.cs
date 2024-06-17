using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleAggregator : MonoBehaviour
{
    public GameObject _IndexTip;
    public GameObject _IndexFMiddle;
    public GameObject _IndexMiddle;
    public GameObject _IndexKnuckle;
    public GameObject _IndexPalm;
    public GameObject _MiddleTip;
    public GameObject _MiddleFMiddle;
    public GameObject _MiddleMiddle;
    public GameObject _MiddleKnuckle;
    public GameObject _MiddlePalm;
    public GameObject _AnnularTip;
    public GameObject _AnnularFMidlle;
    public GameObject _AnnularMiddle;
    public GameObject _AnnularKnuckle;
    public GameObject _AnnularPalm;
    public GameObject _PinkyTip;
    public GameObject _PinkyFMiddle;
    public GameObject _PinkyMiddle;
    public GameObject _PinkyKnuckle;
    public GameObject _PinkyPalm;

    public GlobalVariables GV;
    
    public float AverageAngleCalculation()
    {
        // Index
        // Length Calculations 
        float AdjTip1 = (_IndexTip.transform.position - _IndexFMiddle.transform.position).magnitude;
        float AdjTip2 = (_IndexFMiddle.transform.position - _IndexMiddle.transform.position).magnitude;
        float OpTip1 = (_IndexTip.transform.position - _IndexMiddle.transform.position).magnitude;
        float AdjMid1 = AdjTip2;
        float AdjMid2 = (_IndexMiddle.transform.position - _IndexKnuckle.transform.position).magnitude;
        float OpMid1 = (_IndexKnuckle.transform.position - _IndexFMiddle.transform.position).magnitude;
        float AdjKnuck1 = AdjMid2;
        float AdjKnuck2 = (_IndexPalm.transform.position - _IndexKnuckle.transform.position).magnitude;
        float OpKnuck1 = (_IndexPalm.transform.position - _IndexMiddle.transform.position).magnitude;

        // Angle Calculations taking only positive angles
        float tipAngle = Mathf.Acos((AdjTip1 * AdjTip1 + AdjTip2 * AdjTip2 - OpTip1 * OpTip1) / (2 * AdjTip1 * AdjTip2));

        // Debug
        //GV._DebugWindow.writeDebugMessage("Opposite side values : " + OpTip1 + " ; " + OpMid1 + " ; " + OpKnuck1, 0, "AngleAggregator.cs");

        // Middle
        // Ring
        // Pinky

        return tipAngle; // Am now going to test every seperate angle to optain ranges and verify it's working
        //return 0.0f;
    }
}
