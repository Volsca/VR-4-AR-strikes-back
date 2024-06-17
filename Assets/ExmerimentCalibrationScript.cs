using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExmerimentCalibrationScript : MonoBehaviour
{
    public GameObject _ExpPlane;
    public ExperienceController _ExpController;
    public float calibMult;

    private Vector3 initPosition;

    private void Awake()
    {
        initPosition = _ExpPlane.transform.localPosition;
    }

    void changePlaneHeight(float v)
    {
        _ExpPlane.transform.localPosition = initPosition + new Vector3(0, (v - 0.7f)*calibMult, 0);
    }

    void endCalibration()
    {
        _ExpController.ExperimentSetup();
    }
}
