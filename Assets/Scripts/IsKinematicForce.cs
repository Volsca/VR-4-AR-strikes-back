using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsKinematicForce : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().useGravity = false;
    }
}
