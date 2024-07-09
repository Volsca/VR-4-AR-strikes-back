using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CSVLogWriter : MonoBehaviour
{
    public GlobalVariables GV;

    private string _DataPath;
    private StreamWriter file;
    private Vector3 lHandPreviousposition;
    private Quaternion lHandPreviousAngle;
    private float[] lHandPreviousVel;
    private Vector3 rHandPreviousposition;
    private Quaternion rHandPreviousAngle;
    private float[] rHandPreviousVel;

    void Start()
    {
        lHandPreviousVel = new float[2];
        lHandPreviousVel[0] = 0.0f;
        lHandPreviousVel[1] = 0.0f;
        rHandPreviousVel = new float[2];
        rHandPreviousVel[0] = 0.0f;
        rHandPreviousVel[1] = 0.0f;
        lHandPreviousposition = new Vector3(0, 0, 0);
        lHandPreviousAngle = new Quaternion();
        rHandPreviousposition = new Vector3(0, 0, 0);
        rHandPreviousAngle = new Quaternion();

        _DataPath = Path.Combine(Application.persistentDataPath, "TEST.csv");
        file = new StreamWriter(new FileStream(_DataPath, FileMode.Create), Encoding.UTF8);
        // Write the header for the csv file
        file.WriteLine("TimeStamp; HMD.x; HMD.y; HMD.z; lHand_Velocity; lHand_Angular_Velocity; rHand_Velocity; " +
                       "rHand_Angular_Velocity; lHand.x; lHand.y; lHand.z; rHand.x; rHand.y; rHand.z; lHand_Angles; rHand_Angles;" +
                       "lHand_Distance_From_HMD; rHand_Distance_From_HMD");
        file.Flush(); // Ensure the header is written immediately
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float[] lhandVel = CalculateVelocity(ref lHandPreviousposition, ref lHandPreviousAngle, ref lHandPreviousVel,GV.l_handMeshNode);
        float[] rhandVel = CalculateVelocity(ref rHandPreviousposition, ref rHandPreviousAngle, ref rHandPreviousVel,GV.r_handMeshNode);
        // Example of logging data every frame, make sure to replace with actual data collection logic
        string logEntry = $"{Time.time}; "+ GV._HMD.transform.position.x +"; "+ GV._HMD.transform.position.y + "; "+ GV._HMD.transform.position.z + "; " + // HMD pos
                            // lHand Velocity
                            lhandVel[0] + "; " + lhandVel[1] + "; " +
                            // rHand velocity
                            rhandVel[0] + "; " + rhandVel[1] + "; " +
                            // lHand pos
                            GV.l_handMeshNode.transform.position.x + "; " + GV.l_handMeshNode.transform.position.y + "; " + GV.l_handMeshNode.transform.position.z + "; " +
                            // rHand pos    
                            GV.r_handMeshNode.transform.position.x + "; " + GV.r_handMeshNode.transform.position.y + "; " + GV.r_handMeshNode.transform.position.z + "; " +
                            // Hand angles
                            GV.l_HandAngleAggregator.GetComponent<AngleAggregator>().AverageAngleCalculation() + "; " + 
                            GV.r_HandAngleAggregator.GetComponent<AngleAggregator>().AverageAngleCalculation() + "; " + 
                            // Hand distance from HMD
                            (GV._HMD.transform.position - GV.l_handMeshNode.transform.position).magnitude + "; " + (GV._HMD.transform.position - GV.r_handMeshNode.transform.position).magnitude;

        logEntry.Replace(".", ",");

        file.WriteLine(logEntry);
        file.Flush(); // Ensure each log entry is written immediately
    }

    float[] CalculateVelocity(ref Vector3 pp, ref Quaternion pa, ref float[] pre,GameObject hand)
    {
        float[] ret = new float[2];

        // Velocity
        ret[0] = (hand.transform.position - pp).magnitude / Time.deltaTime;
        ret[1] = Quaternion.Angle(pa, hand.transform.rotation) / Time.deltaTime;

        // Attribute previous values
        pp = hand.transform.position;
        pa = hand.transform.rotation;

        return ret;
    }
}