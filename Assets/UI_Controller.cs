using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject UI_Prefab;
    public float margin = 0.2f;
    GameObject WristUI;


    // Start is called before the first frame update
    void Start()
    {
        WristUI = Instantiate(UI_Prefab, transform.position, Quaternion.identity);
        WristUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If controller Y axis alligned with camera.forward axis then show UI
        Vector3 ControllerY = this.transform.up.normalized;
        Vector3 HMDForward = MainCamera.transform.forward.normalized;

        if(Vector3.Dot(ControllerY, HMDForward) <= 1 - margin)
        {
            WristUI.SetActive(true);
        }
        else
        {
            WristUI.SetActive(false);
        }
    }
}
