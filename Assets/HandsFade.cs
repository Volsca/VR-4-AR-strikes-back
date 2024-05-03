using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandsFade : MonoBehaviour
{
    public GameObject Hand;
    public float alpha;
    public GameObject DebugWindow;
    public DebugWindow DebugWindowScript;

    private bool isActive;
    private float collidersInsideTrigger;
    private List<GameObject> gList;
    private Material objectMaterial;

    private void Start()
    {
        alpha = 1.0f;
        isActive = true;
        //Hand.SetActive(false);
        collidersInsideTrigger = 0;

        DebugWindowScript = DebugWindow.GetComponent<DebugWindow>();

        Renderer handRenderer = Hand.GetComponent<Renderer>();
        objectMaterial = handRenderer.material;
    } 

    private void Update()
    {
        if (isActive)
        {
            if (collidersInsideTrigger == 0)
            {
                //Hand.SetActive(false);
                ChangeOpacity(0.0f);
            }
            else
            {
                DebugWindowScript.writeDebugMessage("Colliders within trigger : " + collidersInsideTrigger, 0, "HandsFade script");
                //Debug.Log("Colliders within trigger : " + collidersInsideTrigger);
                //Hand.SetActive(true);
                ChangeOpacity(setOpacity());
            }
        }
        else
        {
            ChangeOpacity(1.0f);
        }

    }

    private void ChangeOpacity(float opacity)
    {
        Debug.Log("Opacity was : " + objectMaterial.GetFloat("_Opacity"));
        objectMaterial.SetFloat("_Opacity", opacity);
        objectMaterial.SetFloat("_OutlineOpacity", opacity);
    }

    // Calculates an opacity level depending on the distance from the nearest object (base : between 1m and 0m) 
    // and a variable alpha (if alpha = 2 ; between 0.5m and 0m)
    private float setOpacity()
    {
        Debug.Log("Opacity calculation : " + Mathf.Clamp(Mathf.Lerp(0, 1, nearestObjectDistance() * alpha), 0, 1));
        return Mathf.Clamp(Mathf.Lerp(0, 1, nearestObjectDistance() * alpha), 0, 1);
    }

    private float nearestObjectDistance()
    {
        float distance = Mathf.Infinity;

        foreach (GameObject g in gList)
        {
            float temp = Vector3.Magnitude(g.transform.position - transform.position);
            distance = (distance > temp) ? temp : distance;
        }

        return distance;
    }

    public void SetActiveBool()
    {
        isActive = !isActive;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            //transform.gameObject.SetActive(true);
            collidersInsideTrigger++;
            gList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        // If there are still other colliders inside the trigger area
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            collidersInsideTrigger--;
            gList.Remove(other.gameObject);
        }

    }
}
