using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerForced : MonoBehaviour
{
    public Vector3 scale;

    void Update()
    {

        this.transform.localScale = scale;    
    }
}
