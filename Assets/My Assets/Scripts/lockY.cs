using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockY : MonoBehaviour
{
    public GameObject plane;
    public GlobalVariables GV;
    private bool initialized = false;
    public bool setOnTable;

    private void Start()
    {
        setOnTable = false;
    }

    void Update()
    {
        if (!initialized)
        {
            if (GV != null && GV.r_Controller != null)
            {
                initialized = true;
            }
        }
        else if(!setOnTable)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.position = GV.r_Controller.transform.position;
            GV._DebugWindow.writeDebugMessage("Plane position set : " + plane.transform.position + " : " + GV.r_Controller.transform.position, 0, "1010");
        }
    }
}
