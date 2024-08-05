using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class RoundController : MonoBehaviour
{
    [SerializeField]
    private List<ZoneOrder> zoneOrdersIfVirtual = new List<ZoneOrder>();
    [SerializeField]
    private List<ZoneOrder> zoneOrdersIfReal = new List<ZoneOrder>();
    [SerializeField]
    private List<ZoneOrder> zoneOrdersIfHybrid = new List<ZoneOrder>();

    private List<GameObject> zoneList = new List<GameObject>();
    private List<ZoneOrder> zoneOrders = new List<ZoneOrder>();
    private set currentSet;
    private bool isCurrentlyInSet;
    private bool roundJustEnded;
    private int currentRound;
    private int currentZone;

    public static event Action<GameObject, bool> FadeHands;
    public static event Action<int> ActivateZone;
    public static event Action OnSetEnded;
    public static event Action<set> SetFadeCondition;





    private void Awake()
    {
        currentSet = new set();
        isCurrentlyInSet = false;
        currentZone = 0;
        currentRound = 0;

        HandDetectionZone.ZoneComplete += ZoneActivated;
    }

    public void Init(List<GameObject> zL)
    {
        zoneList = zL;
    }

    public void StartSet(set set)
    {
        if (set.rounds > 0 && isCurrentlyInSet == false)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("StartSet()", 0, "");
            currentSet = set;
            SetZoneOrders(currentSet);
            SetFadeCondition?.Invoke(currentSet);
            StartRound();
        }
        else if (isCurrentlyInSet)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("ERROR set StartSet() called while in set", 1, "");
        }
        else
        {
            NewDebugWindow.GetInstance().writeDebugMessage("ERROR set StartSet() called with invalid set", 1, "");
        }
    }

    /// <summary>
    /// Starts a round of the current set.
    /// Called if EndRound() and set unfinished, or if StartSet() is called
    /// </summary>
    private void StartRound()
    {
        if (isCurrentlyInSet)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("StartRound() in set", 0, "");
            currentRound++;
        }
        else
        {
            NewDebugWindow.GetInstance().writeDebugMessage("StartRound() out of set", 0, "");
            isCurrentlyInSet = true;
            currentRound = 1;
        }

        currentZone = -1;
        AdvanceInRound();
    }

    private void EndRound()
    {
        if (currentRound < currentSet.rounds)
        {
            StartRound();
        }
        else
        {
            isCurrentlyInSet = false;
            OnSetEnded?.Invoke();
        }
    }

    /// <summary>
    /// Advances the index by one, with all the necessary checks
    /// </summary>
    private void AdvanceInRound()
    {
        NewDebugWindow.GetInstance().writeDebugMessage("AdvanceInRound w currentZone : " + currentZone, 0, "");
        currentZone++;

        if (currentZone < zoneOrders.Count)
        {
            ActivateCurrentZone();
        }
        else // End round 
        {
            EndRound();
        }
    }

    /// <summary>
    /// ACtivate the zone at zoneList[currentZone]
    /// </summary>
    private void ActivateCurrentZone()
    {
        // Direct hand fade orders
        InvokeFade();
        // Zone activation
        InvokeActivateZone();
    }

    /// <summary>
    /// Callback for when a zone gets activated, enabling the round to continue
    /// </summary>
    private void ZoneActivated(int z)
    {
        if (z == zoneOrders[currentZone]._Zone)
        {
            AdvanceInRound();
        }
    }

    private void InvokeFade()
    {
        if (zoneOrders[currentZone]._IsCan)
        {
            FadeHands?.Invoke(zoneList[zoneOrders[currentZone]._Zone], false);
        }
        else
        {
            FadeHands?.Invoke(zoneList[zoneOrders[currentZone]._Zone], true);
        }
    }

    private void InvokeActivateZone()
    {
        NewDebugWindow.GetInstance().writeDebugMessage("IvokeActivateZone()", 0, "");
        ActivateZone?.Invoke(zoneOrders[currentZone]._Zone);
    }

    private void SetZoneOrders(set s)
    {
        switch (s.zone)
        {
            case HandAndZoneCondition.real:
                zoneOrders = zoneOrdersIfReal;
                break;

            case HandAndZoneCondition.virt:
                zoneOrders = zoneOrdersIfVirtual;
                break;

            case HandAndZoneCondition.hybrid:
                zoneOrders = zoneOrdersIfHybrid;
                break;
        }
    }
}