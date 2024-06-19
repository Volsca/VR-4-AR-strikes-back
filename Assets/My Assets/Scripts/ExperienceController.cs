using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    #region Variables
    public GlobalVariables GV;
    public lockY _lockY;
    public GameObject cube;
    public GameObject can;
    public GameObject HandDetectionZone;
    public float Rayon;
    public float sizeOfSide;
    public GameObject _CalibrationMenu;

    private List<HandDetection> hdList1;
    private List<HandDetection> hdList2;
    private bool _ExpStarted;
    private int _zone;

    #endregion
   
    private void Start()
    {
        _ExpStarted = false;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //Debug.Log("Button pressed");
            GV._DebugWindow.writeDebugMessage("Button pressed", 0, "");
            OnButtonPress();
        }

        // Stuff to do during the experiment
        if (_ExpStarted)
        {

        }
    }

    // Spawn the little menu that adjusts the height of the plane for the experiment
    void OnButtonPress()
    {
        _lockY.setOnTable = true;
        _CalibrationMenu.SetActive(true);
    }

    // Initial setup of the experiment
    public void ExperimentSetup()
    {
        // Spawn in two lists the detection zones, for n as one side (2n = number of zones)
        float angle = 2 * Mathf.PI / (2 * sizeOfSide);
        Vector3 newPointCoords = new Vector3(Rayon, 0, 0) + cube.transform.localPosition;
        Quaternion rot = Quaternion.identity;
        //rot.SetEulerRotation(-Mathf.PI / 2, 0, 0); // Rotating the cans to be upright
        
        // List 0ne 
        for (int i = 0; i < sizeOfSide; i++)
        {
            hdList1.Add(Instantiate(can, newPointCoords, Quaternion.identity).GetComponent<HandDetection>());

            // Execute rotation
            float tmpX = newPointCoords.x * Mathf.Cos(angle) - newPointCoords.z * Mathf.Sin(angle);
            float tmpZ = newPointCoords.x * Mathf.Sin(angle) + newPointCoords.z * Mathf.Cos(angle);
            newPointCoords.x = tmpX;
            newPointCoords.z = tmpZ;
        }
 
        // List 2wo
        for (int i = 0; i < sizeOfSide; i++)
        {
            hdList2.Add(Instantiate(can, newPointCoords, Quaternion.identity).GetComponent<HandDetection>());

            // Execute rotation
            float tmpX = newPointCoords.x * Mathf.Cos(angle) - newPointCoords.z * Mathf.Sin(angle);
            float tmpZ = newPointCoords.x * Mathf.Sin(angle) + newPointCoords.z * Mathf.Cos(angle);
            newPointCoords.x = tmpX;
            newPointCoords.z = tmpZ;
        }
    }

    void ExperimentStart()
    {

    }

    public void UpdateZone()
    {
        // Out of bounds
        if(_zone < 0 || _zone > sizeOfSide*2){
            Debug.LogError("Zone counter out of bounds ERROR. Value : " + _zone + " / Should be between " + 0 + " and " + 2*sizeOfSide);
        }

        // List 0ne
        if(_zone < sizeOfSide){
            
        }
        else{

        }
    }

    void TriggerZone(int zone){

    }
    void UnTriggerZone(int zone){
        
    }


    // Check if can is virtual, or if it is physical/empty
    bool IsPositionCanned(int i)
    {
        return true;
    }
}
