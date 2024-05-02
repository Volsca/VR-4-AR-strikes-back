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

    public TMP_Dropdown dropdown;

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

    // Button functions
    public void HandActivateSphere()
    {
       if (activeHandSphere == true)
       {
           lHandFade.gameObject.SetActive(false);
           rHandFade.gameObject.SetActive(false);

           lHandVisual.SetActive(true);
           rHandVisual.SetActive(true);
       
           activeHandSphere = false;
       }
        else
        {
            lHandFade.gameObject.SetActive(true);
            rHandFade.gameObject.SetActive(true);

            lHandVisual.SetActive(false);
            rHandVisual.SetActive(false);

            activeHandSphere = true;
        }
    }
    void DeactivateHands()
    {
        UpperBodyAvatar.SetActive(false);
        lHandVisual.SetActive(false);
        rHandVisual.SetActive(false);
    }
    void SetActiveUpperBodyAvatar()
    {
        UpperBodyAvatar.SetActive(true);
        lHandVisual.SetActive(false);
        rHandVisual.SetActive(false);
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
    }
}
