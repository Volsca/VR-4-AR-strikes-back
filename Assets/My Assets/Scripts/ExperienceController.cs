using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    #region Attributes
    public static ExperienceController _ExperienceController { get; private set; }

    [SerializeField] private GameObject _ZonePrefab;
    [SerializeField] private List<int> _SideLength; // Will use only index 0 so far
    [SerializeField] private List<int> _ZoneOrder10; // And by extension only _ZoneOrder10
    [SerializeField] private List<int> _ZoneOrder12;
    [SerializeField] private float _Rayon;
    [SerializeField] private lockY _LockY;
    [SerializeField] private GameObject _CalibrationMenu;
    //[SerializeField] private DebugWindow _Debugwindow;

    private int currentZone; // Saved index of which zone is active
    public static event Action<int> _ActivateZone; // The event all the zones will be listening to
    #endregion

    private void Awake()
    {
        currentZone = 0;
        // Singleton
        if (_ExperienceController == null)
        {
            _ExperienceController = this;
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("Button pressed");
            _LockY.setOnTable = true;
            _CalibrationMenu.SetActive(true);
        }
    }


    // Initial setup of the experiment
    public void ExperimentSetup()
    {
        // Spawn in two lists the detection zones, for n as one side (2n = number of zones)
        float angle = 2 * Mathf.PI / (2 * _SideLength[0]);
        Vector3 newPointCoords = new Vector3(_Rayon, 0, 0);// + this.transform.position;
        NewDebugWindow.GetInstance().writeDebugMessage("Initial position at : " + newPointCoords, 0, "");
        Quaternion rot = Quaternion.identity;
        //rot.SetEulerRotation(-Mathf.PI / 2, 0, 0); // Rotating the cans to be upright

        // all the objects
        for (int i = 0; i < _SideLength[0] * 2; i++)
        {
            GameObject tmp = Instantiate(_ZonePrefab, newPointCoords + this.transform.position, Quaternion.identity);
            tmp.SetActive(true);
            tmp.GetComponent<HandDetectionZone>().SetZoneNumber(i);

            // Execute rotation by angle around the center of the cube
            float tmpX = newPointCoords.x * Mathf.Cos(angle) - newPointCoords.z * Mathf.Sin(angle);
            float tmpZ = newPointCoords.x * Mathf.Sin(angle) + newPointCoords.z * Mathf.Cos(angle);
            newPointCoords.x = tmpX;
            newPointCoords.z = tmpZ;
            NewDebugWindow.GetInstance().writeDebugMessage(i + " initialized at : " + newPointCoords, 0, "");
        }

        ActivateZone();
    }

    public void ZoneComplete(int z)
    {
        
        if (z == _ZoneOrder10[currentZone])
        {
            currentZone++;
            ActivateZone();
        }
    }
    private void ActivateZone()
    {
        if (currentZone >= _ZoneOrder10.Count)
        {
            currentZone = 0;
        }
        
        _ActivateZone?.Invoke(_ZoneOrder10[currentZone]);
    }
}
