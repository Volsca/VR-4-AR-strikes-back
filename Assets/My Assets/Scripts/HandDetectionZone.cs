using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;

public class HandDetectionZone : MonoBehaviour
{
    #region  Attributes
    public List<Material> _MaterialList;

    private GameObject _Can;
    private int _ZoneNumber;
    private bool _IsActive;
    private bool _WasActive;

    public static event Action FadeChanger;

    #endregion

    #region Awake/Start/Update Callbacks
    private void Awake()
    {
        _IsActive = false;
        _WasActive = false;

        // Add listeners to the events
        ExperienceController._ActivateZone += OnActivateZone;
        ExperienceController._CalibrationEnd += OnCalibrationEnd;
        ExperienceController._StepEnd += ResetCan;
        ExperienceController._ExperienceController._ResetExperiment += ResetExperiment;
    }
    #endregion


    #region Utilitarian/Setup Methods
    // Spawn can at what I believe to be an alright position
    public void SpawnCan()
    {
        _Can = Instantiate(ExperienceController._ExperienceController._CanPrefab, new Vector3(0, 0.2f, 0), Quaternion.identity);
        _Can.transform.Rotate(new Vector3(-90f, 0.0f, 0.0f));
        _Can.transform.position += this.transform.position;
    }

    // Set the corresponding material and alert the Experience controller
    private void ChangeState()
    {
        if (_IsActive)
        {
            this.GetComponent<Renderer>().material = _MaterialList[1];
        }
        else
        {
            this.GetComponent<Renderer>().material = _MaterialList[0];
        }
    }

    // Listener for setup
    public void SetZoneNumber(int zone)
    {
        _ZoneNumber = zone;
    }

    // Each end of round
    private void ResetCan()
    {
        _Can.transform.position = new Vector3(0, 0.2f, 0) + this.transform.position;
    }

    // OnDestroy not really important yet.
    void OnDestroy()
    {
        ExperienceController._ActivateZone -= OnActivateZone;
        ExperienceController._CalibrationEnd -= OnCalibrationEnd;
        ExperienceController._StepEnd -= ResetCan;
    }

    // Reset positions for testing
    void ResetExperiment()
    {
        if (_ZoneNumber == 0)
        {
            _IsActive = true;
        }
        else
        {
            _IsActive = false;
        }

        if (_Can != null) { ResetCan(); }
    }
    #endregion


    #region Main Line Functions
    // Listener function
    private void OnActivateZone(int zone)
    {
        if (zone == _ZoneNumber)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("Zone " + _ZoneNumber + " activated", 0, "");
            _IsActive = true;
        }
        ChangeState();
    }

    // Pretty self explanatory
    private void OnCalibrationEnd(List<int> z)
    {
        bool hastospawncanlikerightnow = false;
        //NewDebugWindow.GetInstance().writeDebugMessage("Zone " + _ZoneNumber + " Detected calib end ", 0, "HandDetectionZone");   
        foreach (int i in z)
        {
            if (i == _ZoneNumber)
            {
                hastospawncanlikerightnow = true;
            }
        }

        if (hastospawncanlikerightnow)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("Zone " + _ZoneNumber + " hastospawncan", 0, "");
            SpawnCan();
        }

        // Calibration only ends once.
        ExperienceController._CalibrationEnd -= OnCalibrationEnd;
    }

    // Detecting hands entering to enable the zone visuals switching
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hands") || other.gameObject.layer == 13)
        {
            //NewDebugWindow.GetInstance().writeDebugMessage("Something entered zone : " + _ZoneNumber, 1, "");
            if (_IsActive)
            {
                _WasActive = true;
            }
            _IsActive = false;

            ChangeState();
            ExperienceController._ExperienceController.ZoneComplete(_ZoneNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Hands") || other.gameObject.layer == 13) && _WasActive)
        {
            _WasActive = false;
            FadeChanger?.Invoke();
        }
    }
    #endregion
}