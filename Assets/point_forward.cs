using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point_forward : MonoBehaviour
{
    public GameObject Hand;
    private Vector3 Hfw;

    void Start()
    {
        Hfw = Hand.transform.forward;    
    }

    // Update is called once per frame
    void Update()
    {
        Hfw = Hand.transform.forward;
        transform.LookAt(Hfw);
    }
}
