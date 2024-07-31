using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;

// First version of the fade machine states //
//  - Disabled : constant 0.5f opacity,
//  - KeepOpacity : Keep the last opacity (just don't do anything)
//  - FadeTo1f : fade to 1.0f by function of the distance to the next object
//  - FadeTo0f : same with 0.0f instead
//  - DefadeFrom1f : Progressive defade from 1.0f to 0.5f
//  - DefadeFrom0f : Same from 0.0f to 0.5f
//
public enum FadeState
{
    Disabled,
    KeepOpacity,
    FadeTo1f,
    FadeTo0f,
    DefadeFrom1f,
    DefadeFrom0f
}

public class ExperimentFadeController : MonoBehaviour
{
    #region Attributes //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private GlobalVariables GV;

    [SerializeField]
    private bool _LHanded;

    [SerializeField]
    private GameObject _Hand;

    // Distance configurable values
    // Distance between the outside of the zone and max clamped distance value
    [SerializeField]
    private float epsilon;

    // Distance between e and 0.5f opacity
    [SerializeField]
    private float omega;

    // the radius of the zones
    [SerializeField]
    private float _Rayon;

    [SerializeField]
    private float _FadeTime;

    private FadeState currentState;

    private Vector3 handPosition;

    private GameObject zone;

    private bool DefadeStarted;

    // If fade is set to true then the hand fades out
    private bool fade;

    private Material _HandMaterial;

    private char currentCondition;

    #endregion

    #region Awake/Start/Update Callbacks ////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        currentState = FadeState.Disabled;
        handPosition = this.transform.position;
        fade = true;
        DefadeStarted = false;

        if (_LHanded)
        {
            _HandMaterial = GV.l_handMeshNode.GetComponent<Renderer>().material;
        }
        else
        {
            _HandMaterial = GV.r_handMeshNode.GetComponent<Renderer>().material;
        }

        _HandMaterial.SetOverrideTag("RenderType", "Fade");


        // Add callbacks to FadeSwitch() and SetCurrentCondition()
        //ExperienceController._FadeHands += FadeSwitch;
        RoundController.FadeHands += FadeSwitch;
        ExperienceController._FadeConditionChange += SetCurrentCondition;
    }

    private void Update()
    {
        handPosition = this.transform.position;

        switch (currentCondition)
        {
            case 'r':
                SetOpacity(0.0f);
                break;

            case 'v':
                SetOpacity(1.0f);
                break;

            case 'h':
                Opacity();
                break;

            case 'd':
            default:
                SetOpacity(0.5f);
                break;
        }

        SetSizesToBeCorrectCauseOtherwiseItsAllBroken();
    }
    #endregion

    #region Methods /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetCurrentCondition(char c)
    {
        switch (c)
        {
            // Real
            case 'r':
            case 'R':
                currentCondition = 'r';
                break;

            case 'v':
            case 'V':
                currentCondition = 'v';
                break;

            case 'h':
            case 'H':
                currentCondition = 'h';
                break;
            default:
                NewDebugWindow.GetInstance().writeDebugMessage("Error in SetCurrentCondition", 1, "");
                break;
        }
    }

    // Callback for when the next zone changes to determine what state to be in
    private void FadeSwitch(GameObject z, bool f)
    {
        fade = f;
        zone = z;

        switch (currentState)
        {
            // Should be the case only for the start of the experiment so just set the currentState to what the next zone is
            case FadeState.Disabled:
                if (fade) { currentState = FadeState.FadeTo0f; }
                else { currentState = FadeState.FadeTo1f; }
                break;

            // Should be the case for when a progressive defade is needed
            case FadeState.KeepOpacity:
                if (fade) { currentState = FadeState.DefadeFrom1f; }
                else { currentState = FadeState.DefadeFrom0f; }
                break;

            // Should only lead to KeepOpacity
            case FadeState.FadeTo1f:
                if (!fade) { currentState = FadeState.KeepOpacity; }
                else // Catching errors
                {
                    currentState = FadeState.FadeTo0f;
                    NewDebugWindow.GetInstance().writeDebugMessage("ERROR STATE DISCONTINUITY (FadeTo1f)", 1, "");
                }
                break;

            case FadeState.FadeTo0f:
                if (fade) { currentState = FadeState.KeepOpacity; }
                else // Catching errors
                {
                    currentState = FadeState.FadeTo1f;
                    NewDebugWindow.GetInstance().writeDebugMessage("ERROR STATE DISCONTINUITY (FadeTo0f)", 1, "");
                }
                break;

            // Should never be in the next two, but can never be too cautious
            case FadeState.DefadeFrom1f:
                NewDebugWindow.GetInstance().writeDebugMessage("ERROR STATE DISCONTINUITY (DefadeFrom1f)", 1, "");
                if (fade) { currentState = FadeState.FadeTo0f; }
                else { currentState = FadeState.FadeTo1f; }
                break;

            case FadeState.DefadeFrom0f:
                NewDebugWindow.GetInstance().writeDebugMessage("ERROR STATE DISCONTINUITY (DefadeFrom0f)", 1, "");
                if (fade) { currentState = FadeState.FadeTo0f; }
                else { currentState = FadeState.FadeTo1f; }
                break;

            default:
                NewDebugWindow.GetInstance().writeDebugMessage("ERROR STATE DISCONTINUITY (Default)", 1, "");
                break;
        }
    }

    // Main loop of the opacity calculation, to be called in the Update() funciton
    private void Opacity()
    {
        switch (currentState)
        {
            case FadeState.Disabled:
                SetOpacity(0.5f);
                break;

            case FadeState.KeepOpacity:
                if (fade) { SetOpacity(0.0f); }
                else { SetOpacity(1.0f); }
                break;

            case FadeState.FadeTo1f:
                SetOpacity(CalculateOpacity());
                break;

            case FadeState.FadeTo0f:
                SetOpacity(CalculateOpacity());
                break;

            case FadeState.DefadeFrom1f:
                if (!DefadeStarted)
                {
                    StartCoroutine(DefadeCoroutine());
                    DefadeStarted = true;
                }
                break;

            case FadeState.DefadeFrom0f:
                if (!DefadeStarted)
                {
                    StartCoroutine(DefadeCoroutine());
                    DefadeStarted = true;
                }
                break;

            default:
                SetOpacity(0.5f);
                break;
        }
    }

    // Opacity calculation as 0.5f in the middle, and 1.0f or 0.0f at the zone depending on fade. For details read explination
    private float CalculateOpacity()
    {
        float op = 0.5f;

        // Calculation of a parametered distance between 0 and 1 for the lerp
        float distance = Vector3.Magnitude(this.transform.position - zone.transform.position);
        float clampedDistance = Mathf.Clamp(distance, _Rayon + epsilon, _Rayon + epsilon + omega);
        // mapping it to [0, 1]
        float mappedDistance = (clampedDistance - (_Rayon + epsilon)) / (omega);

        switch (currentState)
        {
            case FadeState.FadeTo1f:
                op = 1.0f - Mathf.Lerp(0.0f, 0.5f, mappedDistance);
                break;

            case FadeState.FadeTo0f:
                op = 0.0f + Mathf.Lerp(0.0f, 0.5f, mappedDistance);
                break;

            default:
                NewDebugWindow.GetInstance().writeDebugMessage("Error default in CalculateOpacity()", 1, "");
                break;
        }

        return op;
    }
    // Functionality explanation : Calculates an opacity between 0.5f and a target value of either 0.0f or 1.0f (Depending on fade)
    //      - if fade is true then target is 0.0f, otherwise it's 1.0f
    //      - _Rayon is the radius of the HandDetectionZone
    //      - epsilon is the distance between the exterior of the zones hit box and the line where opacity reaches it's target value 
    //      - omega is the distance between the epsilon line and the line defining the start of the interpolation (Before omega the value is 0.5f)
    //
    //                                          \                         \                               \
    //                                           \                         \                               \
    //      - it goes like this :       zone - - | - target.float - epsilon| - [0.5f, target.float] - omega| - 0.5f - - ...
    //                                           /                         /                               /
    //                                          /                         /                               /

    IEnumerator DefadeCoroutine()
    {
        float op;
        switch (currentState)
        {
            case FadeState.DefadeFrom1f:
                for (op = 1f; op >= 0.5f; op -= 0.1f)
                {
                    SetOpacity(op);
                    yield return new WaitForSeconds(.1f);
                }
                break;

            case FadeState.DefadeFrom0f:
                for (op = 0.0f; op <= 0.5f; op += 0.05f)
                {
                    SetOpacity(op);
                    yield return new WaitForSeconds(.05f);
                }
                break;

            default:
                NewDebugWindow.GetInstance().writeDebugMessage("ERROR : in DefadeCoroutine", 1, "");
                break;
        }

        NewDebugWindow.GetInstance().writeDebugMessage("Coroutine supposed to be over", 0, "");
        if (fade) { currentState = FadeState.FadeTo0f; }
        else { currentState = FadeState.FadeTo1f; }

        DefadeStarted = false;
    }

    // Sets the opacity directly to deport code from other functions
    private void SetOpacity(float op)
    {
        _HandMaterial.SetFloat("_Opacity", op);
        _HandMaterial.SetFloat("_OutlineOpacity", op);
    }

    // Necessary because i'm shit at coding (left a size override somewhere that I need to over-override)
    private void SetSizesToBeCorrectCauseOtherwiseItsAllBroken()
    {
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

    private void OnDestroy()
    {
        // Unsubscribe from events
        ExperienceController._FadeHands -= FadeSwitch;
        ExperienceController._FadeConditionChange -= SetCurrentCondition;
        //HandDetectionZone.FadeChanger -= DeFadeAfterZoneExit;
    }
    #endregion
}
