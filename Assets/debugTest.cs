using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugTest : MonoBehaviour
{
    public DebugWindow dw;

    // Update is called once per frame
    void Update()
    {
        dw.writeDebugMessage("message", 0, "test");
    }
}
