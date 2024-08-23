using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Diagnostics;
using System;
using Inria.HandInteractionSystem;

public class ExperimentDataLogger : MonoBehaviour
{
    public GlobalVariables GV;
    //private int roundNumber;
    private string _DATA_PATH;
    private string _DATA_PATH_CONTINUOUS;
    private StreamWriter _FILE;
    private StreamWriter _FILE_CONTINUOUS;
    private bool pickUp;
    private DataLogStorage data;
    private VelocityEstimator lHandVelocityEstimator;
    private VelocityEstimator rHandVelocityEstimator;
    private bool startRecordingData;

    void Awake()
    {
        startRecordingData = false;
        data = new DataLogStorage();
        //roundNumber = 0;
        pickUp = true;
        InitialiseStreamWriter();

        lHandVelocityEstimator = GV.l_handMeshNode.GetComponent<VelocityEstimator>();
        rHandVelocityEstimator = GV.r_handMeshNode.GetComponent<VelocityEstimator>();

        RoundController.WriteCurrentStep += WriteCurrentStep;
    }

    void FixedUpdate()
    {
        if (_FILE_CONTINUOUS != null && startRecordingData)
        {
            string logEntry = $"{Time.time}; "
                        + CurrentSet(data.currentSet) + "; "
                        + data.currentSetNum + "; "
                        + data.currentZone + "; "
                        + data.lastHandThatInteracted + "; "
                        + data.lastAction + "; "//+ CurrentAction() + "; "
                        + data.previousZonePosition + "; "
                        + GV.l_handMeshNode.transform.position + "; "
                        + lHandVelocityEstimator.GetVelocityEstimate() + "; "
                        + lHandVelocityEstimator.GetAngularVelocityEstimate() + "; "
                        + GV.r_handMeshNode.transform.position + "; "
                        + rHandVelocityEstimator.GetVelocityEstimate() + "; "
                        + rHandVelocityEstimator.GetAngularVelocityEstimate() + "; ";

            _FILE_CONTINUOUS.WriteLine(logEntry);
            _FILE_CONTINUOUS.Flush();
        }
    }

    public void StartRecordingData()
    {
        startRecordingData = true;
    }

    private void InitialiseStreamWriter()
    {
        _DATA_PATH = Path.Combine(Application.persistentDataPath, "DataLog discrete " + DateTime.Now.ToString("dd-MM-yyyy H-mm") + ".csv"); // Room for identification of test user; TODO time.date
        _DATA_PATH_CONTINUOUS = Path.Combine(Application.persistentDataPath, "DataLog continuous " + DateTime.Now.ToString("dd-MM-yyyy H-mm") + ".csv"); // Room for identification of test user; TODO time.date

        _FILE = new StreamWriter(new FileStream(_DATA_PATH, FileMode.Create), Encoding.UTF8);
        _FILE.WriteLine("TimeStamp; current_Set; current_Round; current_Zone; interactor; current_Action; zone-(x,y,z); " +
                        "lHand-(x,y,z); l_Estimated_Velocity; l_Estimated_Angular_Velocity; rHand-(x,y,z); r_Estimated_Velocity; r_Estimated_Angular_Velocity; ");

        _FILE_CONTINUOUS = new StreamWriter(new FileStream(_DATA_PATH_CONTINUOUS, FileMode.Create), Encoding.UTF8);
        _FILE_CONTINUOUS.WriteLine("TimeStamp; current_Set; current_Round; current_Zone; interactor; current_Action; zone-(x,y,z); " +
                        "lHand-(x,y,z); l_Estimated_Velocity; l_Estimated_Angular_Velocity; rHand-(x,y,z); r_Estimated_Velocity; r_Estimated_Angular_Velocity; ");
    }

    private void WriteCurrentStep(int currentZone, set currentSet, Vector3 currentZonePosition, int currentSetNum, int currentRound, string interactor) //, int currentSetnum, int currentRound)
    {
        string temp = CurrentAction();
        // All the necessary info
        string logEntry = $"{Time.time}; "
                        + CurrentSet(currentSet) + "; "
                        + currentSetNum + "; "
                        + currentZone + "; "
                        + interactor + "; "
                        + temp + "; "
                        + currentZonePosition + "; "
                        + GV.l_handMeshNode.transform.position + "; "
                        + lHandVelocityEstimator.GetVelocityEstimate() + "; "
                        + lHandVelocityEstimator.GetAngularVelocityEstimate() + "; "
                        + GV.r_handMeshNode.transform.position + "; "
                        + rHandVelocityEstimator.GetVelocityEstimate() + "; "
                        + rHandVelocityEstimator.GetAngularVelocityEstimate() + "; ";

        _FILE.WriteLine(logEntry);
        _FILE.Flush(); // Ensure each log entry is written immediately

        UpdateStoredData(currentZone, currentSet, currentZonePosition, currentSetNum, currentRound, interactor, temp);
    }

    private void UpdateStoredData(int cZ, set cS, Vector3 cZP, int cSN, int cR, string i, string lA)
    {
        data.currentZone = cZ;
        data.currentSet = cS;
        data.previousZonePosition = cZP;
        data.currentSetNum = cSN;
        data.currentRound = cR;
        data.lastHandThatInteracted = i;
        data.lastAction = lA;
    }

    private string CurrentAction()
    {
        string pickup = "Pick_up";
        string putdown = "Put_down";

        if (pickUp)
        {
            pickUp = !pickUp;
            return pickup;
        }
        else
        {
            pickUp = !pickUp;
            return putdown;
        }
    }

    private string CurrentSet(set currentSet)
    {
        char hand = 'N';
        char zone = 'N';

        switch (currentSet.hand)
        {
            case HandAndZoneCondition.hybrid:
                hand = 'H';
                break;

            case HandAndZoneCondition.virt:
                hand = 'V';
                break;

            case HandAndZoneCondition.real:
                hand = 'R';
                break;
        }

        switch (currentSet.zone)
        {
            case HandAndZoneCondition.hybrid:
                zone = 'H';
                break;

            case HandAndZoneCondition.virt:
                zone = 'V';
                break;

            case HandAndZoneCondition.real:
                zone = 'R';
                break;
        }

        return hand + "/" + zone;
    }


}

public struct DataLogStorage
{
    public int currentZone;
    public set currentSet;
    public Vector3 previousZonePosition;
    public int currentSetNum;
    public int currentRound;
    public string lastHandThatInteracted;
    public string lastAction;
}