using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataLogger : MonoBehaviour
{
    [SerializeField] private GlobalVariables GV;
    private string _DataPath;
    private StreamWriter _File;

    private int _RoundNumber;

    void Start()
    {
        //ExperienceController._StepEnd += NextRound;

        _RoundNumber = -1;
        _DataPath = Path.Combine(Application.persistentDataPath, "DataLog" + 0 + ".csv"); // Room for identification of test user
        _File = new StreamWriter(new FileStream(_DataPath, FileMode.Create), Encoding.UTF8);

        _File.WriteLine("TimeStamp; HMD.x; HMD.y; HMD.z; lHand_Velocity; rHand_Velocity; " +
                        "lHand.x; lHand.y; lHand.z; rHand.x; rHand.y; rHand.z; Round_Number");
    }

    void FixedUpdate()
    {
        // All the necessary info
        string logEntry = $"{Time.time};"
                        + GV._HMD.transform.position.x + "; "
                        + GV._HMD.transform.position.y + "; "
                        + GV._HMD.transform.position.z + "; "
                        + CalulateVelocity(true) + "; "
                        + CalulateVelocity(false) + "; "
                        + GV.l_handMeshNode.transform.position.x + "; "
                        + GV.l_handMeshNode.transform.position.y + "; "
                        + GV.l_handMeshNode.transform.position.z + "; "
                        + GV.r_handMeshNode.transform.position.x + "; "
                        + GV.r_handMeshNode.transform.position.y + "; "
                        + GV.r_handMeshNode.transform.position.z + "; "
                        + _RoundNumber;

        _File.WriteLine(logEntry);
        _File.Flush(); // Ensure each log entry is written immediately
    }

    void NextRound()
    {
        _RoundNumber++;
    }

    float CalulateVelocity(bool isitthelefthandperchance)
    {
        return 0.0f;
    }
}
