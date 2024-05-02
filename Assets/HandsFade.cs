using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandsFade : MonoBehaviour
{
    public GameObject Hand;
    public float alpha;

    private bool isActive;
    private float collidersInsideTrigger;
    private List<GameObject> gList;
    private Renderer _renderer; 
        
    private void Start()
    {
        alpha = 1.0f;
        isActive = true;
        //Hand.SetActive(false);
        collidersInsideTrigger = 0;

        _renderer = Hand.GetComponent<Renderer>();
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
        if (_renderer != null)
        {
            // Get the current material color
            Color currentColor = _renderer.material.color;
           
            // Set the alpha component of the color to the desired opacity
            currentColor.a = opacity;

            // Assign the modified color back to the material
            _renderer.material.color = currentColor;
        }
        else
        {
            Debug.LogError("Renderer component not found on the GameObject.");
        }
    }

    // Calculates an opacity level depending on the distance from the nearest object (base : between 1m and 0m) 
    // and a variable alpha (if alpha = 2 ; between 0.5m and 0m)
    private float setOpacity()
    {
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
