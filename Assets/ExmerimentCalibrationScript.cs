using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExmerimentCalibrationScript : MonoBehaviour
{
    #region Attributes
    [SerializeField] private GameObject _ExpPlane;
    [SerializeField] private ExperienceController _ExpController;
    [SerializeField] private Slider slidySlideBoy;
    [SerializeField] private float calibMult;

    private Vector3 initPosition;
    #endregion

    private void Awake()
    {
        initPosition = _ExpPlane.transform.localPosition;
    }

    public void changePlaneHeight()
    {
        _ExpPlane.transform.localPosition = initPosition + new Vector3(0, (slidySlideBoy.value - 0.7f)*calibMult, 0);
    }

    public void endCalibration()
    {
        _ExpController.ExperimentSetup();
    }
}
