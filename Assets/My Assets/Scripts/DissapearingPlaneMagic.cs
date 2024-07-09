using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingPlaneMagic : MonoBehaviour
{
    [SerializeField] private Material _Dissapeared;

    public void Dissapear()
    {
        this.gameObject.GetComponent<Renderer>().material = _Dissapeared;
        Color col = this.gameObject.GetComponent<Renderer>().material.color;
        col.a = 0;
        this.gameObject.GetComponent<Renderer>().material.color = col;        
    }
}
