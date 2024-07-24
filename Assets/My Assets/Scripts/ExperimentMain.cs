using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private ICalibrator Calibrator;
    #endregion

    #region Attributes
    private List<GameObject> zoneList = new List<GameObject>();
    private List<set> setList = new List<set>();
    private NewDebugWindow newDebugWindow = NewDebugWindow.GetInstance();
    private bool isCalibrated;
    private bool ButtonPressed;
    private bool thisJustIn;
    private bool roundEnded;
    #endregion

    /// <summary>
    /// Its most important attribute, the current state of the experiment
    /// </summary>
    public ExperiementPhase currentPhase { get; private set; }

    void Awake()
    {
        roundEnded = false;
        thisJustIn = false;
        ButtonPressed = false;
        isCalibrated = false;
        currentPhase = ExperiementPhase.Inactive;

        ButtonDown.AButtonDown += AButtonDown;
        ButtonDown.BButtonDown += BButtonDown;
    }

    void Update()
    {
        if(!isCalibrated & ButtonPressed)
        {
            OnInitialCalibration();
            ButtonPressed = true;
        }
        else if (thisJustIn)
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
        }
    }

    private void StartRound()
    {

    }

    public void AButtonDown()
    {
        ButtonPressed = true;
    }

    public void BButtonDown()
    {

    }

    /// <summary>
    /// Advances the index by one, with all the necessary checks
    /// </summary>
    private void AdvanceInRound()
    {

    }

    private void AdvanceInSet()
    {

    }

    private void ActivateCurrentZone()
    {

    }

    // Don't need as they deactivate them selves
    /*private void DeactivateCurrentZone()
    {

    }*/

    private void ResetExperiment()
    {

    }

    private void OnExperimentStart()
    {

    }

    private void OnInitialCalibration()
    {
        if (!isCalibrated)
        {
            // Calibration code
            Calibrator.Calibrate();
            zoneList = Calibrator.GetHandDetectionZones();
            isCalibrated = true;
        }
        else
        {
            newDebugWindow.writeDebugMessage("Already Calibrated", 0, "");
        }
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