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
    
    void Start()
    {
        _DataPath = Path.Combine(Application.persistentDataPath, "TEST.csv");
        file = new StreamWriter(new FileStream(_DataPath, FileMode.Create), Encoding.UTF8);
        // Write the header for the csv file
        file.WriteLine("TimeStamp, HMD.x, HMD.y, HMD.z, lHand Velocity, lHand Angular Velocity, rHand Velocity, " +
                       "rHand Andgular Velocity, lHand.x, lHand.y, lHand.z, rHand.x, rHand.y, rHand.z, lHand Angles, rHand Angles");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
