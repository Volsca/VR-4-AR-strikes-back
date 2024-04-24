using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point_right : MonoBehaviour
{
    public GameObject Hand;
    private Vector3 Hrt;

    void Start()
    {
        Hrt = Hand.transform.right;    
    }

    // Update is called once per frame
    void Update()
    {
        Hrt = Hand.transform.right;
        transform.LookAt(Hrt);
    }
}
