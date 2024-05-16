using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GlobalVariables : MonoBehaviour
{
    #region Hands
    // Hand meshes
    public GameObject l_handMeshNode;
    public GameObject r_handMeshNode;

    public GameObject _FullBodyAvatar;

    public HandsFade l_handFade;
    public HandsFade r_handFade;

    // Hand interactors
    public GameObject[] l_HandInteractors;
    public GameObject[] r_HandInteractors;

    public float alpha; // hand transparency
    public float handOutlineSize; // Pretty self explanatory

    public Material[] _HandMaterials;
    #endregion

    #region Menu
    public TMP_Dropdown _HandModelDropdown;
    public Scrollbar _HandFadeScrollbar;
    public Scrollbar _HandScaleScrollbar;
    public Scrollbar _HandOutlineScrollbar;

    public DebugWindow _DebugWindow;
    #endregion
}
