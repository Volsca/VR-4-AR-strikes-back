using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtHMD : MonoBehaviour
{
    [SerializeField] private GameObject HMD;

    // Update is called once per frame
    void Update()
    {
        if (HMD != null)
        {
            transform.LookAt(HMD.transform);
            transform.Rotate(0, 180, 0);
        }
        else
        {
            Debug.LogWarning("HMD GameObject is not assigned in the inspector.");
        }
    }
}
