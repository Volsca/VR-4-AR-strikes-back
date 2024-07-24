using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Condition
{
    SetupPhase,
    InitialPhase,
    NoAvatarPhase,
    OnlyAvatarPhase,
    VaryingAvatarPhase
}

public class ConditionController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown drop;

    public void ConditionChange()
    {
        NewDebugWindow.GetInstance().writeDebugMessage("Dropdawn value : " + drop.value, 0, "");
    }
}
