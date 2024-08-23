using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
using UnityEngine;
using TMPro;

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
    [SerializeField] private ExperimentDataLogger experimentDataLogger;
    #endregion

    #region Attributes
    private ICalibrator Calibrator;
    private List<GameObject> zoneList = new List<GameObject>();
    private List<set> setList = new List<set>();
    private NewDebugWindow newDebugWindow = NewDebugWindow.GetInstance();
    public GameObject CalibrationUI;
    public GameObject WaitUI;
    public TMP_Text TextMeshProUGUI;
    private const int MAX_ROUNDS = 2;
    //public GameObject experiment;
    private int setListIndex;
    private bool isCalibrated;
    private bool ButtonPressed;
    private bool waiting;
    private bool wantNextSet;
    private bool wantReset;
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
        wantReset = false;
        waiting = false;
        wantNextSet = false;
        ButtonPressed = false;
        isCalibrated = false;
        isCurrentlyInExperiment = false;

        setListIndex = 0;

        currentPhase = ExperiementPhase.Inactive;

        ButtonDown.AButtonDown += AButtonDown;
        ButtonDown.BButtonDown += BButtonDown;
        ButtonDown.ThumbstickDown += ThumbstickDown;

        RoundController.OnSetEnded += EndSet;
    }

    void Update()
    {
        if (!isCalibrated & ButtonPressed)
        {
            OnInitialCalibration();
            ButtonPressed = false;
        }
        if (wantNextSet)
        {
            StartSet();
            WaitUI.SetActive(false);
            wantNextSet = false;
        }
        if (wantReset)
        {
            wantReset = false;
            ResetSet();
        }
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
            roundController.StartSet(setList[setListIndex], setListIndex + 1);
        }
        else
        {
            NewDebugWindow.GetInstance().writeDebugMessage("ERROR : " + setListIndex + " does not exist in setList", 1, "");
        }
    }

    private void WaitForNextSet()
    {
        WaitUI.SetActive(true);
        waiting = true;
    }

    private void EndSet()
    {
        setListIndex++;
        Calibrator.DeleteCans();

        if (setListIndex < setList.Count)
        {
            WaitForNextSet();
            SetNextSetText();
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
            experimentDataLogger.StartRecordingData();
            isCurrentlyInExperiment = true;
            setListIndex = 0;
            StartSet();
        }
    }

    public void AButtonDown()
    {
        ButtonPressed = true;
        /*if (!isCalibrated)
        {
            OnInitialCalibration();
        }*/
    }

    public void BButtonDown()
    {
        if (waiting)
        {
            wantNextSet = true;
            waiting = false;
        }
    }

    public void ThumbstickDown()
    {
        wantReset = true;
    }


    public void SetNextSetText()
    {
        TextMeshProUGUI.text = WhatTextToSet();
    }

    // Is an absolutely discusting function, and should burn. DO NOT LOOK INSIDE
    private string WhatTextToSet()
    {
        switch (setList[setListIndex].hand)
        {
            case HandAndZoneCondition.hybrid:
                switch (setList[setListIndex].zone)
                {
                    case HandAndZoneCondition.hybrid:
                        return "H/H";

                    case HandAndZoneCondition.virt:
                        return "H/V";


                    case HandAndZoneCondition.real:
                        return "H/R";
                }
                break;

            case HandAndZoneCondition.virt:
                switch (setList[setListIndex].zone)
                {
                    case HandAndZoneCondition.hybrid:
                        return "V/H";

                    case HandAndZoneCondition.virt:
                        return "V/V";

                    case HandAndZoneCondition.real:
                        return "V/R";
                }
                break;

            case HandAndZoneCondition.real:
                switch (setList[setListIndex].zone)
                {
                    case HandAndZoneCondition.hybrid:
                        return "R/H";

                    case HandAndZoneCondition.virt:
                        return "R/V";

                    case HandAndZoneCondition.real:
                        return "R/R";
                }
                break;
        }
        return "ERROR";
    }

    // Don't need as they deactivate them selves
    /*private void DeactivateCurrentZone()
    {

    }*/

    /// <summary>
    /// Reset the current set
    /// </summary>
    private void ResetSet()
    {
        NewDebugWindow.GetInstance().writeDebugMessage("ResetSet called", 0, "");
        if (isCurrentlyInExperiment)
        {
            setListIndex--;
            roundController.ResetSet();
            EndSet();
        }
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

    #region Laziness
    public void AddHybridHybridSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.hybrid, HandAndZoneCondition.hybrid, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddHybridVirtualSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.hybrid, HandAndZoneCondition.virt, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddHybridRealSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.hybrid, HandAndZoneCondition.real, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddVirtualHybridSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.virt, HandAndZoneCondition.hybrid, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddVirtualVirtualSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.virt, HandAndZoneCondition.virt, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddVirtualRealSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.virt, HandAndZoneCondition.real, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddRealHybridSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.real, HandAndZoneCondition.hybrid, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddRealVirtualSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.real, HandAndZoneCondition.virt, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }

    public void AddRealRealSet()
    {
        ReceiveOrdersFromCommand(HandAndZoneCondition.real, HandAndZoneCondition.real, MAX_ROUNDS);
        NewDebugWindow.GetInstance().writeDebugMessage("setList.Count = " + setList.Count, 0, "");
    }
    #endregion

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
    public void DeleteCans();
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