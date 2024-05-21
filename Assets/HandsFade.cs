using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandsFade : MonoBehaviour
{

    public GlobalVariables GV;

    public GameObject Hand;

    private bool isActive;
    private float collidersInsideTrigger;
    private List<GameObject> gList;
    private Material[] objectMaterial;
    private float size;
    private Vector3 scale;
    public bool rightHanded;

    private void Start()
    {
        GV.handOutlineSize = 1;
        scale = new Vector3(1, 1, 1);
        size = 1.0f;
        GV.alpha = 1.0f;
        isActive = true;
        collidersInsideTrigger = 0;
        gList = new List<GameObject>();

        Renderer handRenderer;
        // Left handed
        if (rightHanded == false)
        {
            handRenderer = GV.l_handMeshNode.GetComponent<Renderer>();
        }
        else
        {
            handRenderer = GV.r_handMeshNode.GetComponent<Renderer>();
        }

        objectMaterial = handRenderer.materials;
        objectMaterial[0].SetOverrideTag("RenderType", "Fade");
    } 

    private void Update()
    {
        if (isActive)
        {
            if (collidersInsideTrigger == 0)
            { 
                ChangeOpacity(0.0f);
            }
            else
            {
                ChangeOpacity(setOpacity());
            }
        }
        else
        {
            ChangeOpacity(1.0f);
        }
        float outSize = Mathf.Lerp(0.0f, 0.005f, Mathf.Clamp(GV.handOutlineSize, 0, 1));
        objectMaterial[0].SetFloat("_OutlineWidth", outSize);

        // Applying scale
        if (rightHanded == false)
        {
            GV.l_Wrist.transform.localScale = scale * size;
        }
        else
        {
            GV.r_Wrist.transform.localScale = scale * size;
        }
        Hand.transform.localScale = scale * size;

        /*_SynthHand.GetComponent<SyntheticSize>().SetSize(size);
        HandParent.transform.localScale = scale * size;
        HandPoke.transform.localScale = this.transform.localScale;
        HandGrab.transform.localScale = this.transform.localScale;
        
        HandRay.transform.localScale = this.transform.localScale;
        HandLoco.transform.localScale = this.transform.localScale;
        HandTouch.transform.localScale = this.transform.localScale;
        HandUse.transform.localScale = this.transform.localScale;
        */
    }

    public void ChangeHandColor(Color c)
    {
        objectMaterial[0].SetColor("_ColorTop", c);
    }

    private void ChangeOpacity(float opacity)
    {

        // Methode fonctionelle pour changer l'opacité des matériaux Oculus (ne pas oublier de changer le render type à fade)
        
        //objectMaterial[0].SetColor("_ColorTop", new Color(0.1f, 0.1f, 0.1f, 0));
        objectMaterial[0].SetFloat("_Opacity", opacity);
        objectMaterial[0].SetFloat("_OutlineOpacity", opacity);
        
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

    public void OnHandChange()
    {
        Renderer handRenderer;
        // Left handed
        if (rightHanded == false)
        {
            handRenderer = GV.l_handMeshNode.GetComponent<Renderer>();
        }
        else
        {
            handRenderer = GV.r_handMeshNode.GetComponent<Renderer>();
        }

        objectMaterial = handRenderer.materials;
        objectMaterial[0].SetOverrideTag("RenderType", "Fade");
    }
    public void Activate()
    {
        isActive = true;
    }
    public void Deactivate()
    {
        isActive = false;
    }
    public void SetAlpha(float a)
    {
        GV.alpha = a*2.0f + 0.5f;
    }
    public void SetActiveBool()
    {
        isActive = !isActive;
    }
    // Calculates an opacity level depending on the distance from the nearest object (base : between 1m and 0m) 
    // and a variable alpha (if alpha = 2 ; between 0.5m and 0m)
    private float setOpacity()
    {
        //float clampedValue = Mathf.Clamp(nearestObjectDistance(), 0.2f, 0.5f);
        float mappedValue = (Mathf.Clamp(nearestObjectDistance(), 0.3f, 0.65f) - 0.3f) / (0.65f - 0.3f);
        float op = Mathf.Clamp(Mathf.Lerp(1, 0, mappedValue * GV.alpha), 0, 1);
        return op;
    }
    public void SetSize(float s)
    {
        size = s;
    }
    public void SetOutlineSize(float s)
    {
        GV.handOutlineSize = s;
    }
}
