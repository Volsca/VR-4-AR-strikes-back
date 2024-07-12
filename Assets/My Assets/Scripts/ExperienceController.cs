using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    [Serializable]
    public struct ZoneOrder
    {
        [SerializeField] public int _Zone;
        [SerializeField] public bool _IsCan;
    }

    #region Attributes ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static ExperienceController _ExperienceController { get; private set; }
    public GameObject _CanPrefab; // { get; private set; }


    [SerializeField] private GameObject _ZonePrefab;
    [SerializeField] private List<int> _SideLength; // Will use only index 0 so far
    [SerializeField] private List<ZoneOrder> _ZoneOrder12;


    [SerializeField] private lockY _LockY;
    [SerializeField] private GameObject _CalibrationMenu;
    //[SerializeField] private DebugWindow _Debugwindow;
    private float angle;
    Vector3 newPointCoords;

    // Data to be set by the ExperimentManager later (BMLTux ?)
    [SerializeField][Range(0, 0.4f)] private float _Rayon;
    [SerializeField][Range(0, 0.5f)] private float _ZoneRayon; // Not yet used

    private int currentZone; // Saved index of which zone is active
    private List<GameObject> Zones = new List<GameObject>(); // Saved table of all zones, for the fade script.
    public static event Action<int> _ActivateZone; // The event all the zones will be listening to
    public static event Action<List<int>> _CalibrationEnd;
    public static event Action _StepEnd;
    public event Action _ResetExperiment;
    public static event Action<int> _ResetLastZone;
    public static event Action<GameObject, bool> _FadeHands;
    // public static event Action<GameObject, bool> _DeFadeHands;
    #endregion

    #region Initial Setup ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        currentZone = 0;
        // Singleton
        if (_ExperienceController == null)
        {
            _ExperienceController = this;
        }

        angle = -2 * Mathf.PI / (2 * _SideLength[0]);
        newPointCoords = new Vector3(_Rayon, 0, 0) + new Vector3(0, -0.04f, 0);
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
    #endregion

    #region Utility Methods /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Activates current zone
    private void ActivateZone()
    {
        //NewDebugWindow.GetInstance().writeDebugMessage("Current value : " + currentZone, 0, "");
        if (currentZone > _ZoneOrder12.Count - 1)
        {
            currentZone = 0;
            _StepEnd?.Invoke();
        }

        if (_ZoneOrder12[currentZone]._IsCan)
        {
            _FadeHands?.Invoke(Zones[_ZoneOrder12[currentZone]._Zone], false);
        }
        else
        {
            _FadeHands?.Invoke(Zones[_ZoneOrder12[currentZone]._Zone], true);
        }

        //NewDebugWindow.GetInstance().writeDebugMessage("Zone activated : " + _ZoneOrder12[currentZone]._Zone, 0, "");
        _ActivateZone?.Invoke(_ZoneOrder12[currentZone]._Zone);
    }

    // Resets for testing
    public void ResetExperiment()
    {
        _ResetExperiment?.Invoke();
        currentZone = 0;
        ActivateZone();
    }

    private void CreateZoneInstance(int i)
    {
        GameObject tmp = Instantiate(_ZonePrefab, newPointCoords + this.transform.position, Quaternion.identity);
        tmp.SetActive(true);
        tmp.GetComponent<HandDetectionZone>().SetZoneNumber(i);
        //NewDebugWindow.GetInstance().writeDebugMessage("Created Zone N°" + i, 0, "");

        Zones.Add(tmp);
        if (!(Zones[i] == tmp))
        {
            //NewDebugWindow.GetInstance().writeDebugMessage("List instertion error at" + i, 1, "");
        }
        tmp.transform.parent = this.transform; // In case calibration is to be done after spawning the objects
    }

    // Deportation of basic ExpSetup Logic
    private void IncrementRotationAngle() // Execute rotation by angle around the center of the cube
    {
        float tmpX = newPointCoords.x * Mathf.Cos(angle) - newPointCoords.z * Mathf.Sin(angle);
        float tmpZ = newPointCoords.x * Mathf.Sin(angle) + newPointCoords.z * Mathf.Cos(angle);
        newPointCoords.x = tmpX;
        newPointCoords.z = tmpZ;
    }

    // Deciding what zones spawn cans
    private List<int> WhatZonesSpawnCans()
    {
        List<int> c = new List<int>(new int[5]);
        for (int i = 0; i < 3; i++)
        {
            c[i] = i * 2; // Returns (0, 2, 4)
        }

        return c;
    }

    // Invoking the Event to spawn cans
    private void SpawnCans(List<int> canZones)
    {
        _CalibrationEnd?.Invoke(canZones);
    }
    #endregion

    #region Main Line Methods /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Initial setup of the experiment
    public void ExperimentSetup()
    {
        //NewDebugWindow.GetInstance().writeDebugMessage("Initial position at : " + newPointCoords, 0, "");

        // all the objects
        for (int i = 0; i < _SideLength[0] * 2; i++)
        {
            CreateZoneInstance(i);
            IncrementRotationAngle();
            //NewDebugWindow.GetInstance().writeDebugMessage(i + " initialized at : " + newPointCoords, 0, "");
        }

        if (Zones[12] == null)
        {
            //NewDebugWindow.GetInstance().writeDebugMessage("big erreur ça marche pas", 1, "");
        }
    }

    public void OnCalibrationEnd()
    {
        // Temporary "spawn cans here" protocol
        SpawnCans(WhatZonesSpawnCans());
    }

    public void OnExperimentStart()
    {
        ActivateZone();
    }

    // Callback for when a zone gets completed
    public void ZoneComplete(int z)
    {
        if (z < _ZoneOrder12[currentZone]._Zone + 1 && z > _ZoneOrder12[currentZone]._Zone - 1)
        {
            //NewDebugWindow.GetInstance().writeDebugMessage("Zone has been completed : " + z, 0, "");
            //NewDebugWindow.GetInstance().writeDebugMessage("Next zone should be : " + _ZoneOrder12[(currentZone + 1) % _ZoneOrder12.Count]._Zone, 0, "");
            currentZone++;
            ActivateZone();
        }
    }
    #endregion


}