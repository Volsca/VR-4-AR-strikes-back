using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.GrabAPI;

public class UI_Place : MonoBehaviour
{
    public GameObject UIprefab              ;
    public float Angle = 30.0f;
    public float height = 1.2f;
    //private HandGrabInteractable handGrab;
    private Oculus.Interaction.Grabbable GrabComponent;
    private HandGrabInteractor handGrabInt;
    private IHandGrabInteractable handGrab;
    private HandGrabInteractor inter;

    private void Start()
    {
        GrabComponent = GetComponent<Oculus.Interaction.Grabbable>();
        handGrab = GetComponent<HandGrabInteractable>();
        handGrabInt = GetComponent<HandGrabInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newX = transform.localPosition;
        newX.y = height;

        UIprefab.transform.position = transform.localPosition;
        
        

        //if (GrabComponent.) //--then an object is grabbed
        //{
        //
        //}

    }
}
