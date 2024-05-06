using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandsFade : MonoBehaviour
{
    public GameObject Hand;
    public float alpha;
    public GameObject DebugWindow;
    public DebugWindow DebugWindowScript;
    //public Material handMaterial;

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
        gList = new List<GameObject>();
        //DebugWindowScript = DebugWindow.GetComponent<DebugWindow>();

        Renderer handRenderer = Hand.GetComponent<Renderer>();
        objectMaterial = handRenderer.material;
        objectMaterial.SetOverrideTag("RenderType", "Fade");
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
        DebugWindowScript.writeDebugMessage("Opacity to be : " + opacity + " ----- Nearest object : " + nearestObjectDistance(), 0, "HandsFade script");
        //Color myColor = objectMaterial.color;
        //myColor.a = opacity;
        //objectMaterial.color = myColor;
        Debug.Log("Opacity was : " + objectMaterial.GetFloat("_Opacity"));
        objectMaterial.SetFloat("_Opacity", opacity);
        objectMaterial.SetFloat("_OutlineOpacity", opacity);
    }

    // Calculates an opacity level depending on the distance from the nearest object (base : between 1m and 0m) 
    // and a variable alpha (if alpha = 2 ; between 0.5m and 0m)
    private float setOpacity()
    {
        //float clampedValue = Mathf.Clamp(nearestObjectDistance(), 0.2f, 0.5f);
        float mappedValue = (Mathf.Clamp(nearestObjectDistance(), 0.4f, 0.8f) - 0.4f) / (0.8f - 0.4f);
        float op = Mathf.Clamp(Mathf.Lerp(1, 0, mappedValue * alpha), 0, 1);
        //DebugWindowScript.writeDebugMessage("Nearest object distance : " + nearestObjectDistance(), 0, "HandsFade script");
        //DebugWindowScript.writeDebugMessage("Opacity calculation : " + op, 0, "HandsFade script");
        return op;
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
        //DebugWindowScript.writeDebugMessage("Entered Trigger", 0, "HandsFade script");
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            //transform.gameObject.SetActive(true);
            collidersInsideTrigger++;
            gList.Add(other.gameObject);

            //DebugWindowScript.writeDebugMessage("Grabbables within trigger : " + collidersInsideTrigger, 0, "HandsFade script");
        }
        else
        {
            //DebugWindowScript.writeDebugMessage("Not Grabbable", 0, "HandsFade script");
        }
    }

    private void OnTriggerExit(Collider other)
    {

        // If there are still other colliders inside the trigger area
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            collidersInsideTrigger--;
            gList.Remove(other.gameObject);

            //DebugWindowScript.writeDebugMessage("Grabbables within trigger : " + collidersInsideTrigger, 0, "HandsFade script");
        }

    }
}
