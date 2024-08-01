using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// This class is the main body of the experiment, conaining what's 
/// necessary to making it able to delegate all important tasks and
/// manage the global experience. 
/// It is a state machine with four phases : 
///     - Inactive, which is basically its precalibration state 
///     - Test, to try a VV condition at will
///     - Current, where the current condition is played out
///     - Wait, before first current and inbetween the other 
///     currents
/// 
/// This is version 0.1 of ExperiementMain
/// </summary>
public class ExperimentMain : MonoBehaviour
{
    #region Serialized Attributes 
    #endregion

    #region Attributes
    private ICalibrator Calibrator;
    private List<GameObject> zoneList = new List<GameObject>();
    private List<set> setList = new List<set>();
    private NewDebugWindow newDebugWindow = NewDebugWindow.GetInstance();
    public GameObject CalibrationUI;
    //public GameObject experiment;
    private int setListIndex;
    private bool isCalibrated;
    //private bool ButtonPressed;
    //private bool thisJustIn;
    //private bool roundEnded;
    private bool isCurrentlyInExperiment;

    public RoundController roundController;
    #endregion

    /// <summary>
    /// Its most important attribute, the current state of the experiment
    /// </summary>
    public ExperiementPhase currentPhase { get; private set; }

    void Awake()
    {
        Calibrator = GetComponent<ICalibrator>();
        //NewDebugWindow.GetInstance().writeDebugMessage("Calibrator : " + ((MonoBehaviour)Calibrator).name, 0, "");
        //roundEnded = false;
        //thisJustIn = false;
        //ButtonPressed = false;
        isCalibrated = false;
        isCurrentlyInExperiment = false;

        setListIndex = 0;

        currentPhase = ExperiementPhase.Inactive;

        ButtonDown.AButtonDown += AButtonDown;
        ButtonDown.BButtonDown += BButtonDown;

        RoundController.OnSetEnded += EndSet;
    }

    void Update()
    {
        /*if (!isCalibrated & ButtonPressed)
        {
            OnInitialCalibration();
            ButtonPressed = false;
        }*/
        /*else if (thisJustIn)
        {
            switch (currentPhase)
            {
                case ExperiementPhase.Inactive:
                    break;

                case ExperiementPhase.Test:
                    break;

                case ExperiementPhase.Wait:

                    break;

                case ExperiementPhase.Current:
                    break;
            }
        }*/
    }

    private void StartSet()
    {
        if (setListIndex >= 0 && setListIndex < setList.Count)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("Set Started at " + setListIndex, 0, "");
            Calibrator.SpawnCans(setList[setListIndex]);
            roundController.StartSet(setList[setListIndex]);
        }
        else
        {
            NewDebugWindow.GetInstance().writeDebugMessage("ERROR : " + setListIndex + " does not exist in setList", 1, "");
        }
    }

    private void EndSet()
    {
        setListIndex++;

        if (setListIndex < setList.Count)
        {
            StartSet();
        }
        else
        {
            isCurrentlyInExperiment = false;
            CalibrationUI.SetActive(true);

            setList = new List<set>();
        }
    }

    public void OnExperimentStart()
    {
        if (setList.Count > 0 && isCurrentlyInExperiment == false)
        {
            setListIndex = 0;
            StartSet();
        }
    }

    public void AButtonDown()
    {
        if (!isCalibrated)
        {
            OnInitialCalibration();
        }
    }

    public void BButtonDown()
    {

    }

    // Don't need as they deactivate them selves
    /*private void DeactivateCurrentZone()
    {

    }*/

    private void ResetExperiment()
    {

    }

    private void OnInitialCalibration()
    {
        if (!isCalibrated)
        {
            // Calibration code
            Calibrator.Calibrate();
            isCalibrated = true;
        }
        else
        {
            newDebugWindow.writeDebugMessage("Already Calibrated", 0, "");
        }
    }
    public void SecondCalibration()
    {
        Calibrator.SpawnZones();
        zoneList = Calibrator.GetHandDetectionZones();
        roundController.Init(zoneList);
    }

    public void AddHybridSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.hybrid, HandAndZoneCondition.hybrid, 3);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddVirtualSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.virt, HandAndZoneCondition.virt, 3);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddRealSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.real, HandAndZoneCondition.real, 3);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void ReceiveOrdersFromCommand(HandAndZoneCondition h, HandAndZoneCondition z, int r)
    {
        set s = new set();
        s.hand = h;
        s.zone = z;
        s.rounds = r;

        setList.Add(s);
    }
}


#region Extra stuff
public interface ICalibrator
{
    public void Calibrate();
    public List<GameObject> GetHandDetectionZones();
    public void SpawnCans(set s);
    public void SpawnZones();
}

public enum ExperiementPhase
{
    Inactive,
    Test,
    Wait,
    Current
}

[Serializable]
public enum HandAndZoneCondition
{
    real,
    virt, // Written virt to avoid keyword virtual
    hybrid,
}

[Serializable]
public struct set
{
    public HandAndZoneCondition hand;
    public HandAndZoneCondition zone;
    public int rounds;
}
#endregion