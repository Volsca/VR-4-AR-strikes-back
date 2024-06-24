using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandDetectionZone : MonoBehaviour
{
    #region  Attributes
    private int _ZoneNumber;
    private bool _IsActive;
    #endregion

    private void Awake()
    {
        _IsActive = false;
        ExperienceController._ActivateZone += OnActivateZone; // Add a listener to the event
    }

    

    // Listener function
    private void OnActivateZone(int zone)
    {
        if (zone == _ZoneNumber)
        {
            _IsActive = true;
        }
    }

    public void SetZoneNumber(int zone)
    {
        _ZoneNumber = zone;
        Debug.Log("Zone instanciated at number : " + _ZoneNumber);
    }

    /* Main hand detection logic
    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("Something entered zone" + other.gameObject.layer);
        if (other.CompareTag("Hands") || other.gameObject.layer == 13)
        {

             Both hands edge case
            if (_HandController.canGrab)
            {
                _HandController.canGrab2 = true;
            }
            else
            {
                _HandController.canGrab = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.LogError("Something exited zone" + other.gameObject.layer);
        if (other.CompareTag("Hands") || other.gameObject.layer == 13)
        {
             Both hands edge case
            if (_HandController.canGrab2)
            {
                _HandController.canGrab2 = false;
            }
            else
            {
                _HandController.canGrab = false;
            }
        }
    }*/

}