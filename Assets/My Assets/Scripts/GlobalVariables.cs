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
    public GameObject l_Controller;
    public GameObject r_Controller;

    public GameObject _HMD;

    public GameObject l_Wrist;
    public GameObject r_Wrist;

    public GameObject _FullBodyAvatar;

    public HandsFade l_handFade;
    public HandsFade r_handFade;

    public Color _HandColor; //= new Color(0.1f, 0.1f, 0.1f, 0);
    public Color[] _ListOfHandColors;

    // Hand interactors
    public GameObject[] l_HandInteractors;
    public GameObject[] r_HandInteractors;

    public GameObject l_HandAngleAggregator;
    public GameObject r_HandAngleAggregator;

    public float alpha; // hand transparency
    public float handOutlineSize; // Pretty self explanatory

    public Material[] _HandMaterials;
    #endregion

    #region Menu
    public TMP_Dropdown _HandModelDropdown;
    public Scrollbar _HandFadeScrollbar;
    public Scrollbar _HandScaleScrollbar;
    public Scrollbar _HandOutlineScrollbar;
    public Slider _HandColorScrollbar;

    public DebugWindow _DebugWindow;
    #endregion
    /*
    private void Start()
    {
        _ListOfHandColors[0] = new Color(0.19f, 0.19f, 0.19f, 0);
        _ListOfHandColors[1] = new Color(0.19f, 0.19f, 0.19f, 0);
        _ListOfHandColors[2] = new Color(0.85f, 0.65f, 0.34f, 0);
        _ListOfHandColors[3] = new Color(0.85f, 0.65f, 0.34f, 0);
        _ListOfHandColors[4] = new Color(0.42f, 0.28f, 0.16f, 0);
        _ListOfHandColors[5] = new Color(0.42f, 0.28f, 0.16f, 0);
        _ListOfHandColors[6] = new Color(0.21f, 0.94f, 0.12f, 0);
        _ListOfHandColors[7] = new Color(0.21f, 0.94f, 0.12f, 0);
        _ListOfHandColors[8] = new Color(0.44f, 0.06f, 0.98f, 0);
        _ListOfHandColors[9] = new Color(0.44f, 0.06f, 0.98f, 0);
    }
    */
}
