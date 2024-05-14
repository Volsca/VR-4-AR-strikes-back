using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyntheticSize : MonoBehaviour
{
    public float size;
    private Vector3 _Scale = new Vector3(1, 1, 1);

    private void Start()
    {
        size = 1;
    }

    private void Update()
    {
        this.transform.localScale = _Scale * size;
    }
    
    public void SetSize(float s)
    {
        size = s;
    }
}
