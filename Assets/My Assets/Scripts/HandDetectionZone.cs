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

    public static event Action<int, GameObject> ZoneComplete;

    #endregion

    #region Awake/Start/Update Callbacks
    private void Awake()
    {
        _IsActive = false;

        // Add listeners to the events // TODO Change events
        RoundController.ActivateZone += OnActivateZone;
        ZoneCalibration.SpawnCansEvent += OnCalibrationEnd;
        //ExperienceController._ExperienceController._ResetExperiment += ResetExperiment;
        ZoneCalibration.DeleteCansEvent += DeleteCan;
        RoundController.OnResetSet += DeactivateZone;
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

    public void DeleteCan()
    {
        if (_Can != null)
        {
            Destroy(_Can);
            _Can = null;
        }
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
    }

    // Reset positions for testing
    private void ResetExperiment()
    {
        DeactivateZone();
        DeleteCan();
    }

    private void ActivateZone()
    {
        _IsActive = true;
        ChangeState();
    }

    public void DeactivateZone()
    {
        _IsActive = false;
        ChangeState();
    }
    #endregion


    #region Main Line Functions
    // Listener function
    private void OnActivateZone(int zone)
    {
        if (zone == _ZoneNumber)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("Zone " + _ZoneNumber + " activated", 0, "");
            ActivateZone();
        }

    }

    // Pretty self explanatory
    private void OnCalibrationEnd(List<int> z)
    {
        if (z != null)
        {
            if (HasToSpawnCan(z))
            {
                NewDebugWindow.GetInstance().writeDebugMessage("Zone " + _ZoneNumber + " hastospawncan", 0, "");
                SpawnCan();
            }
        }
    }
    private bool HasToSpawnCan(List<int> z)
    {
        foreach (int i in z)
        {
            if (i == _ZoneNumber)
            {
                return true;
            }
        }

        return false;
    }

    // Detecting hands entering to enable the zone visuals switching
    private void OnTriggerEnter(Collider other)
    {
        if (/*other.CompareTag("Hands") ||*/ other.gameObject.layer == 13)
        {
            //NewDebugWindow.GetInstance().writeDebugMessage("Something entered zone : " + _ZoneNumber, 1, "");
            DeactivateZone();
            //ExperienceController._ExperienceController.ZoneComplete(_ZoneNumber);
            ZoneComplete?.Invoke(_ZoneNumber, other.gameObject);
        }
    }

    #endregion
}