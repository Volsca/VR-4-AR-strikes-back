using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsFade : MonoBehaviour
{
    public GameObject Hand;

    private float collidersInsideTrigger;

    private void Start()
    {
        Hand.SetActive(false);
        collidersInsideTrigger = 0;
    }

    private void Update()
    {

        if (collidersInsideTrigger == 0)
        {
            Hand.SetActive(false);
        }
        else
        {
            Hand.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            //transform.gameObject.SetActive(true);
            collidersInsideTrigger++;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        // If there are still other colliders inside the trigger area
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            collidersInsideTrigger--;
        }

    }
}
