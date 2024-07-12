using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;

public class ExperimentFadeController : MonoBehaviour
{
    #region Attributes //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private GlobalVariables GV;
    [SerializeField] private bool _LHanded;
    [SerializeField] private GameObject _Hand;

    // Distance values
    [SerializeField] private float epsilon; // Distance between the outside of the zone and max clamped distance value
    [SerializeField] private float omega; // Distance between e and 0.5f opacity 
    [SerializeField] private float _Rayon; // the radius of the zones

    private Vector3 handPosition;
    private GameObject zone;
    private bool fade; // If fade is set to true then the hand fades out
    private Material _HandMaterial;
    private bool stay;
    private float stick;

    #endregion

    #region Awake/Start/Update Callbacks ////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        handPosition = this.transform.position;
        fade = true;
        stay = false;
        stick = 1.0f;

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
        HandDetectionZone.FadeChanger += DeFadeAfterZoneExit;
    }

    private void Update()
    {
        handPosition = this.transform.position;

        // Opacity calculation and setting
        if (fade && !stay)
        {
            SetOpacity(CalculateOpacity(true));
        }
        else if (!stay)
        {
            SetOpacity(CalculateOpacity(false));
        }
        else
        {
            SetOpacity(stick);
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
        if (z == null)
        {
            NewDebugWindow.GetInstance().writeDebugMessage("FadeSwitch called with NULL zone", 1, "");
        }

        // Kind of crap duck tape to make opacity constant between same can zones
        if (fade == f)
        {
            stay = true;
            if (!f) { stick = 1.0f; }
            else { stick = 0.0f; }
        }

        zone = z;
        fade = f;
    }

    private void DeFadeAfterZoneExit()
    {
        if(true){} // TODO
    }

    // Opacity calculation as 0.5f in the middle, and 1.0f or 0.0f at the zone depending on fade. For details read explination
    private float CalculateOpacity(bool fade)
    {
        float op = 0.5f;

        float distance = Vector3.Magnitude(this.transform.position - zone.transform.position);
        float clampedDistance = Mathf.Clamp(distance, _Rayon + epsilon, _Rayon + epsilon + omega);
        float mappedDistance = (clampedDistance - (_Rayon + epsilon)) / (omega); // mapping it to [0, 1]

        // Linear interpolation for the opacity between r+e and r+o+e
        if (fade)
        {
            op = 0.0f + Mathf.Lerp(0.0f, 0.5f, mappedDistance);
        }
        else
        {
            op = 1.0f - Mathf.Lerp(0.0f, 0.5f, mappedDistance);
        }

        NewDebugWindow.GetInstance().writeDebugMessage("opacity, distances : " + op + ", " + mappedDistance + ", " + clampedDistance + ", " + distance, 0, "");
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


    private void SetOpacity(float op)
    {
        _HandMaterial.SetFloat("_Opacity", op);
        _HandMaterial.SetFloat("_OutlineOpacity", op);
    }
    #endregion
}
