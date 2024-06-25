using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandDetectionZone : MonoBehaviour
{
    #region  Attributes
    public List<Material> _MaterialList;

    private int _ZoneNumber;
    private bool _IsActive;
    #endregion

    private void Awake()
    {
        _IsActive = false;
        ExperienceController._ActivateZone += OnActivateZone; // Add a listener to the event
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

    // Listener function
    private void OnActivateZone(int zone)
    {
        if (zone == _ZoneNumber)
        {
            _IsActive = true;
        }
        ChangeState();
    }

    public void SetZoneNumber(int zone)
    {
        _ZoneNumber = zone;
        Debug.Log("Zone instanciated at number : " + _ZoneNumber);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("Something entered zone" + other.gameObject.layer);
        if (other.CompareTag("Hands") || other.gameObject.layer == 13)
        {
            _IsActive = false;
            ChangeState();
        }
    }

    void OnDestroy()
    {
        ExperienceController._ActivateZone -= OnActivateZone;
    }

}