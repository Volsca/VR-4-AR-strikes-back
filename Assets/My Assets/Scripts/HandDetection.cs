using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDetection : MonoBehaviour
{
    public Material unHighlightedMaterial;
    public Material highlightedMaterial;
    public bool canGrab;
    public bool canGrab2;

    // TODO switch layer detection to the hand's layer

    private void Awake()
    {
        canGrab = false;
        canGrab2 = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("Something entered zone" + other.gameObject.layer);
        if (other.CompareTag("Hands") || other.gameObject.layer == 13)
        {
            this.GetComponent<Renderer>().material = highlightedMaterial;

            // Both hands edge case
            if (canGrab)
            {
                canGrab2 = true;
            }
            else
            {
                canGrab = true;
            }
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.LogError("Something exited zone" + other.gameObject.layer);
        if (other.CompareTag("Hands") || other.gameObject.layer == 13)
        {
            this.GetComponent<Renderer>().material = unHighlightedMaterial;

            // Both hands edge case
            if (canGrab2)
            {
                canGrab2 = false;
            }
            else
            {
                canGrab = false;
            }
        }
    }

    public bool getCanGrab()
    {
        return canGrab || canGrab2;
    }
}
