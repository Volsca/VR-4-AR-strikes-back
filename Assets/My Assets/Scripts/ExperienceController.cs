using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

[Serializable]
public struct Set
{
    public char _Hand;

    public char _Condition;

    public int _Rounds;
}

[Serializable]
public struct ZoneOrder
{
    [SerializeField]
    public int _Zone;

    [SerializeField]
    public bool _IsCan;

    //[SerializeField] 
    //public FadeState _Fade;
}

public class ExperienceController : MonoBehaviour
{
    #region Attributes ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static ExperienceController _ExperienceController
    { get; private set; }

    public GameObject _CanPrefab; // { get; private set; }

    [SerializeField]
    private List<Set> _SetList;

    private bool test;

    private Set _TestSet;

    private int currentSetIndex;

    private int currentRound;

    [SerializeField]
    private GameObject _ZonePrefab;

    [SerializeField]
    private List<int> _SideLength; // Will use only index 0 so far

    [SerializeField]
    private List<ZoneOrder> _ZoneOrder12;

    [SerializeField]
    private List<ZoneOrder> _ZoneOrderReal;

    [SerializeField]
    private List<ZoneOrder> _ZoneOrderVirtual;

    [SerializeField]
    private lockY _LockY;

    [SerializeField]
    private GameObject _CalibrationMenu;

    //[SerializeField] 
    //private DebugWindow _Debugwindow;

    private float angle;

    Vector3 newPointCoords;

    // Data to be set by the ExperimentManager later (BMLTux ?)
    [SerializeField]
    [Range(0, 0.4f)]
    private float _Rayon;

    [SerializeField]
    [Range(0, 0.5f)]
    private float _ZoneRayon; // Not yet used

    private int currentZone; // Saved index of which zone is active

    private List<GameObject> Zones = new List<GameObject>(); // Saved table of all zones, for the fade script.

    // Rewrite as an enum // TODO
    //[SerializeField]
    //private Condition CurrentCondition;
    // These are the conditions (Defined in ICondition) : 

    // Events to control hand fade and zones
    public static event Action _ExperimentStart;

    // The event all the zones will be listening to
    public static event Action<int> _ActivateZone;

    public static event Action<List<int>> _CalibrationEnd;

    // Not done // TODO
    public event Action _ResetExperiment;

    public static event Action<GameObject, bool> _FadeHands;

    public static event Action _EndRound;

    public static event Action<char> _FadeConditionChange;

    #endregion

    #region Initial Setup ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        currentRound = -1;
        test = false;
        currentSetIndex = -1;
        currentZone = -1;
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

        if (currentZone > _ZoneOrder12.Count - 1)
        {
            _EndRound?.Invoke();
            currentRound++;
            StartRound();
        }
        else
        {
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
    }

    private char GetHandCondition() => _SetList[currentSetIndex]._Hand;

    private char GetZoneCondition() => _SetList[currentSetIndex]._Condition;

    private int GetCurrentMaxRounds() => _SetList[currentSetIndex]._Rounds;

    public void StartRound()
    {
        if (test)
        {
            
        }
        else
        {

        }

        currentZone = 0;
        ActivateZone();
    }

    /// <summary>
    /// Made to be run from a custom interface passing the correct parameters
    /// </summary>
    /// <param name="h">Hand contition, either h, r or v</param>
    /// <param name="c">Zone contition, either h, r or v</param>
    /// <param name="r">Number of rounds to play</param>
    public void StartTestRounds(char h, char c, int r)
    {
        test = true;

        _TestSet._Hand = h;
        _TestSet._Condition = c;
        _TestSet._Rounds = r;

        currentRound = 0;
        currentZone = 0;
        ActivateZone();
    }

    // Resets for testing // TODO
    public void ResetExperiment()
    {
        _ResetExperiment?.Invoke();
        currentZone = 0;
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
        char a = GetZoneCondition();
        List<int> c = new List<int>(new int[6]);

        switch (a)
        {
            case 'r':
            case 'R':
                for (int i = 0; i < c.Count; i++)
                {
                    c[i] = 0;
                }
                break;

            case 'h':
            case 'H':
                for (int i = 0; i < 3; i++)
                {
                    c[i] = i * 2; // Returns (0, 2, 4)
                }
                break;

            case 'v':
            case 'V':
                for (int i = 0; i < 3; i++)
                {
                    c[i] = i; // Returns (0, 1, 2, 3, 4, 5)
                }
                break;

            default:
                for (int i = 0; i < c.Count; i++)
                {
                    c[i] = 0;
                }
                NewDebugWindow.GetInstance().writeDebugMessage("Error in WhatZonesSpawnCans()", 1, "");
                break;
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
        // all the objects
        for (int i = 0; i < _SideLength[0] * 2; i++)
        {
            CreateZoneInstance(i);
            IncrementRotationAngle();
        }
    }

    public void OnCalibrationEnd()
    {
        // "spawn cans here" protocol
        SpawnCans(WhatZonesSpawnCans());
    }

    public void OnExperimentStart()
    {
        _ExperimentStart?.Invoke();
        //currentZone = 0;
        //ActivateZone();
    }

    // Callback for when a zone gets completed
    public void ZoneComplete(int z)
    {
        if (z < _ZoneOrder12[currentZone]._Zone + 1 && z > _ZoneOrder12[currentZone]._Zone - 1)
        {
            currentZone++;
            ActivateZone();
        }
    }
    #endregion
}

public class SetController : MonoBehaviour
{

}