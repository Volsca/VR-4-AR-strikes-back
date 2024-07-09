using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExmerimentCalibrationScript : MonoBehaviour
{
    #region Attributes
    [SerializeField] private GameObject _ExpPlane;
    [SerializeField] private ExperienceController _ExpController;
    [SerializeField] private Slider slidySlideBoy;
    [SerializeField] private float calibMult;
    [SerializeField] private GameObject _Menu;

    private Vector3 initPosition;
    #endregion

    private void Awake()
    {
        initPosition = _ExpPlane.transform.localPosition;
    }
    
    public void changePlaneHeight()
    {
        _ExpPlane.transform.localPosition = initPosition + new Vector3(0, (slidySlideBoy.value - 0.7f) * calibMult, 0);
    }

    public void EndCalibration()
    {
        _ExpController.OnCalibrationEnd();
        _Menu.SetActive(false);
    }
    public void SetupExperiment()
    {
        _ExpController.ExperimentSetup();
    }
}
