using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point_up : MonoBehaviour
{
    public GameObject Hand;
    private Vector3 Hup;

    void Start()
    {
        Hup = Hand.transform.up;    
    }

    // Update is called once per frame
    void Update()
    {
        Hup = Hand.transform.up;
        transform.LookAt(Hup);
    }
}
