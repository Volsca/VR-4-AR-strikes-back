using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class HandsController : MonoBehaviour
{
    /*
    public GameObject UpperBodyAvatar;
    public GameObject lHandVisual;
    public GameObject rHandVisual;
    public Material HandsDefault;
    public Material HandsBeige;
    public HandsFade lHandFade;
    public HandsFade rHandFade;
    */
    public GlobalVariables GV;

    private Renderer _rRenderer;
    private Renderer _lRenderer;
    private bool activeHandSphere;

    //public DebugWindow debug;

    //public TMP_Dropdown dropdown;
    //public Scrollbar scrollbar;

    void Start()
    {
        activeHandSphere = true;
        SetActiveHandsDefault();
        
        // Add a listener to the dropdown's onValueChanged event
        GV._HandModelDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(GV._HandModelDropdown);
        });
    }

    // Function to handle dropdown value changes
    void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        int selectedIndex = dropdown.value;

        switch (selectedIndex)
        {
            case 0:
                SetActiveHandsDefault();
                break;
            case 1:
                SetActiveHandsBeige();
                break;
            case 2:
                SetActiveUpperBodyAvatar();
                break;
            case 3:
                DeactivateHands();
                break;
            default:
                SetActiveHandsDefault();
                break;
        }
    }
    public void ScrollbarValueChanged()
    {
        //GV._DebugWindow.writeDebugMessage("Scrollbar value : " + GV._HandFadeScrollbar.value, 0, "HandsColtroller.cs");
    }

    // Switches fade on and off
    public void HandActivateSphere()
    {
        if (activeHandSphere != true)
        {
            ActivateFade();
            HandChange();
        }
        else
        {
            DeactivateFade();
            HandChange();
        }

        activeHandSphere = !activeHandSphere;
    }
    void DeactivateHands()
    {
        GV._FullBodyAvatar.SetActive(false);
        GV.l_handMeshNode.SetActive(false);
        GV.r_handMeshNode.SetActive(false);

        DeactivateFade();
    }
    void SetActiveUpperBodyAvatar()
    {
        GV._FullBodyAvatar.SetActive(true);
        GV.l_handMeshNode.SetActive(false);
        GV.r_handMeshNode.SetActive(false);

        DeactivateFade();
    }
    void SetActiveHandsDefault()
    {
        GV._FullBodyAvatar.SetActive(false);
        GV.l_handMeshNode.SetActive(true);
        GV.r_handMeshNode.SetActive(true);

        _lRenderer = GV.l_handMeshNode.GetComponent<Renderer>();
        _rRenderer = GV.r_handMeshNode.GetComponent<Renderer>();

        if (_lRenderer != null)
        {
            Material[] materials = _lRenderer.materials;
            Material newMaterial = GV._HandMaterials[0];
            materials[0] = newMaterial;
            _lRenderer.materials = materials;
        }
        else
        {
            Debug.LogError("Renderer component not found on the GameObject.");
        }

        if (_rRenderer != null)
        {
            Material[] materials = _rRenderer.materials;
            Material newMaterial = GV._HandMaterials[0];
            materials[0] = newMaterial;
            _rRenderer.materials = materials;
        }
        else
        {
            Debug.LogError("Renderer component not found on the GameObject.");
        }

        // Make sure the fade is working
        ActivateFade();
        HandChange();
    }
    void SetActiveHandsBeige()
    {
        GV._FullBodyAvatar.SetActive(false);
        GV.l_handMeshNode.SetActive(true);
        GV.r_handMeshNode.SetActive(true);

        _lRenderer = GV.l_handMeshNode.GetComponent<Renderer>();
        _rRenderer = GV.r_handMeshNode.GetComponent<Renderer>();

        if (_lRenderer != null)
        {
            Material[] materials = _lRenderer.materials;
            Material newMaterial = GV._HandMaterials[1];
            materials[0] = newMaterial;
            _lRenderer.materials = materials;
        }
        else
        {
            Debug.LogError("Renderer component not found on the GameObject.");
        }

        if (_rRenderer != null)
        {
            Material[] materials = _rRenderer.materials;
            Material newMaterial = GV._HandMaterials[1];
            materials[0] = newMaterial;
            _rRenderer.materials = materials;
        }
        else
        {
            Debug.LogError("Renderer component not found on the GameObject.");
        }

        // Make sure the fade is working
        ActivateFade();
        HandChange();
    }

    void HandChange()
    {
        GV.l_handFade.OnHandChange();
        GV.r_handFade.OnHandChange();
    }

    void ActivateFade()
    {
        GV.l_handFade.Activate();
        GV.r_handFade.Activate();
    }
    void DeactivateFade()
    {
        GV.l_handFade.Deactivate();
        GV.r_handFade.Deactivate();
    }
}
