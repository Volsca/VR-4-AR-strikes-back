using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public GlobalVariables GV;
    public lockY _lockY;
    public GameObject cube;
    public GameObject can;
    public float Rayon;
    public float nbPts;

    public GameObject _CalibrationMenu;

    private bool _ExpStarted;

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

    }

    void ExperimentStart()
    {
        // Logic spawning the cans at the right positions
        float angle = 2 * Mathf.PI / nbPts;
        Vector3 newPointCoords = new Vector3(Rayon, 0, 0) + cube.transform.localPosition;
        Quaternion rot = Quaternion.identity;
        rot.SetEulerRotation(-Mathf.PI / 2, 0, 0);

        for (int i = 0; i < nbPts; i++)
        {
            // Spawn can if virtual
            if (isPositionCanned(i))
            {
                Instantiate(can, newPointCoords, rot);

                // Execute rotation
                float tmpX = newPointCoords.x * Mathf.Cos(angle) - newPointCoords.z * Mathf.Sin(angle);
                float tmpZ = newPointCoords.x * Mathf.Sin(angle) + newPointCoords.z * Mathf.Cos(angle);
                newPointCoords.x = tmpX;
                newPointCoords.z = tmpZ;
            }
            
        }
    }

    // Check if can is virtual, or if it is physical/empty
    bool isPositionCanned(int i)
    {
        return true;
    }
}
