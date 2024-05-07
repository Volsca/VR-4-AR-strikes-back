using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class HandsController : MonoBehaviour
{
    public GameObject UpperBodyAvatar;
    public GameObject lHandVisual;
    public GameObject rHandVisual;
    public Material HandsDefault;
    public Material HandsBeige;
    public HandsFade lHandFade;
    public HandsFade rHandFade;

    private Renderer _rRenderer;
    private Renderer _lRenderer;
    private bool activeHandSphere;

    public DebugWindow debug;

    public TMP_Dropdown dropdown;
    public Scrollbar scrollbar;

    void Start()
    {
        activeHandSphere = true;
        SetActiveHandsDefault();
        
        // Add a listener to the dropdown's onValueChanged event
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
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
        debug.writeDebugMessage("Scrollbar value : " + scrollbar.value, 0, "HandsColtroller.cs");
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
        UpperBodyAvatar.SetActive(false);
        lHandVisual.SetActive(false);
        rHandVisual.SetActive(false);

        DeactivateFade();
    }
    void SetActiveUpperBodyAvatar()
    {
        UpperBodyAvatar.SetActive(true);
        lHandVisual.SetActive(false);
        rHandVisual.SetActive(false);

        DeactivateFade();
    }
    void SetActiveHandsDefault()
    {
        UpperBodyAvatar.SetActive(false);
        lHandVisual.SetActive(true);
        rHandVisual.SetActive(true);

        _lRenderer = lHandVisual.GetComponent<Renderer>();
        _rRenderer = rHandVisual.GetComponent<Renderer>();

        if (_lRenderer != null)
        {
            Material[] materials = _lRenderer.materials;
            Material newMaterial = HandsDefault;
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
            Material newMaterial = HandsDefault;
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
        UpperBodyAvatar.SetActive(false);
        lHandVisual.SetActive(true);
        rHandVisual.SetActive(true);

        _lRenderer = lHandVisual.GetComponent<Renderer>();
        _rRenderer = rHandVisual.GetComponent<Renderer>();

        if (_lRenderer != null)
        {
            Material[] materials = _lRenderer.materials;
            Material newMaterial = HandsBeige;
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
            Material newMaterial = HandsBeige;
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
        lHandFade.OnHandChange();
        rHandFade.OnHandChange();
    }

    void ActivateFade()
    {
        lHandFade.Activate();
        rHandFade.Activate();
    }
    void DeactivateFade()
    {
        lHandFade.Deactivate();
        rHandFade.Deactivate();
    }
}
