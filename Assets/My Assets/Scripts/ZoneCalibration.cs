using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is in charge of all the calibration code, needing
/// only the order to spawn zones, and should return a list of 
/// all the zones in the ExperimentMain script
/// </summary>
public class ZoneCalibration : MonoBehaviour, ICalibrator
{
    private List<GameObject> zones = new List<GameObject>();
    [SerializeField] private GameObject _CalibrationMenu;
    [SerializeField] private lockY _LockY;
    [SerializeField] private GameObject _ZonePrefab;
    private float angle = -2 * Mathf.PI / (2 * 6);
    Vector3 newPointCoords = new Vector3(0.28f, 0, 0) + new Vector3(0, -0.04f, 0);   

    public void Calibrate()
    {
        Debug.Log("Button pressed");
        _LockY.setOnTable = true;
        _CalibrationMenu.SetActive(true);
    }

    public List<GameObject> GetHandDetectionZones()
    {
        return zones;
    }

    public void SpawnZones()
    {
        for (int i = 0; i < 6 * 2; i++)
        {
            CreateZoneInstance(i);
            IncrementRotationAngle();
        }
    }

    private void CreateZoneInstance(int i)
    {
        GameObject tmp = Instantiate(_ZonePrefab, newPointCoords + this.transform.position, Quaternion.identity);
        tmp.SetActive(true);
        tmp.GetComponent<HandDetectionZone>().SetZoneNumber(i);
        //NewDebugWindow.GetInstance().writeDebugMessage("Created Zone N°" + i, 0, "");

        zones.Add(tmp);
        if (!(zones[i] == tmp))
        {
            //NewDebugWindow.GetInstance().writeDebugMessage("List instertion error at" + i, 1, "");
        }
        tmp.transform.parent = this.transform; // In case calibration is to be done after spawning the objects
    }

    // Deportation of basic ExpSetup Logic
    private void IncrementRotationAngle() // Execute rotation by angle around the center of the cube
    {
        float tmpX = newPointCoords.x * Mathf.Cos(angle) - newPointCoords.z * Mathf.Sin(angle);
        float tmpZ = newPointCoords.x * Mathf.Sin(angle) + newPointCoords.z * Mathf.Cos(angle);
        newPointCoords.x = tmpX;
        newPointCoords.z = tmpZ;
    }
}