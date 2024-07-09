using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExperimentFadeController : MonoBehaviour
{
    #region Attributes //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private GlobalVariables GV;
    [SerializeField] private bool _LHanded;
    [SerializeField] private GameObject _Hand;

    private Vector3 handPosition;
    private GameObject zone;
    private bool fade; // If fade is set to true then the hand fades out
    private float lastOpacity;
    private Material _HandMaterial;
    #endregion

    #region Awake/Start/Update Callbacks ////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        handPosition = this.transform.position;
        fade = true;

        if (_LHanded)
        {
            _HandMaterial = GV.l_handMeshNode.GetComponent<Renderer>().material;
        }
        else
        {
            _HandMaterial = GV.r_handMeshNode.GetComponent<Renderer>().material;
        }

        _HandMaterial.SetOverrideTag("RenderType", "Fade");


        // Add callbacks to FadeSwitch()
        ExperienceController._FadeHands += FadeSwitch;



    }

    private void Update()
    {
        handPosition = this.transform.position;

        // Fade calculation
        if (fade)
        {
            SetOpacity(CalculateOpacity(0.0f));
        }
        else
        {
            SetOpacity(CalculateOpacity(0.9f));
        }

        float outSize = Mathf.Lerp(0.0f, 0.005f, Mathf.Clamp(GV.handOutlineSize, 0, 1));
        _HandMaterial.SetFloat("_OutlineWidth", 0.003f);

        if (_LHanded)
        {
            GV.l_Wrist.transform.localScale = new Vector3(1, 1, 1) * 1.05f;
        }
        else
        {
            GV.r_Wrist.transform.localScale = new Vector3(1, 1, 1) * 1.05f;
        }

        _Hand.transform.localScale = new Vector3(1, 1, 1) * 1.05f;
    }
    #endregion

    #region Methods /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void FadeSwitch(GameObject z, bool f)
    {
        zone = z;
        fade = f;
    }

    // Calculate the linear interpolation between the last Opacity and the Target
    /*private float CalculateOpacity(float target)
    {
        float op;
        float distance = Vector3.Magnitude(handPosition - zone.transform.position);

        float mappedValue = (Mathf.Clamp(distance, 0.3f, 0.65f) - 0.3f) / (0.65f - 0.3f);

        // Determins if it should be inverted or not (if tagets = to 0 or not)
        if (target > 0.0f)
        {
            op = Mathf.Clamp(Mathf.Lerp(lastOpacity, target, mappedValue), 0, 1);
        }
        else
        {
            op = 1 - Mathf.Clamp(Mathf.Lerp(lastOpacity, target, mappedValue), 0, 1);
        }

        return op;
    }*/ // og function, works with linear interpolation between a target and current value


    // Should work using the new zone as a target and a 0.5f middle point ////// Needs some serious mind power
    private float CalculateOpacity(float target)
    { 
        return 0.0f;
    }

    private void SetOpacity(float op)
    {
        _HandMaterial.SetFloat("_Opacity", op);
        _HandMaterial.SetFloat("_OutlineOpacity", op);
    }
    #endregion
}
