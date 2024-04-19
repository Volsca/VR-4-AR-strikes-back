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
        // Starting position of the UI, moving it to be at the side of the wrist
        Vector3 startPosition = transform.position;
        //startPosition.x += 0.3f;
        startPosition.z += -0.3f;

        WristUI = Instantiate(UI_Prefab, startPosition, Quaternion.identity);
        WristUI.transform.parent = this.transform;
        WristUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If controller Y axis alligned with camera.forward axis then show UI
        Vector3 ControllerY = this.transform.right.normalized;
        Vector3 HMDForward = MainCamera.transform.forward.normalized;
        WristUI.transform.rotation = Quaternion.LookRotation(HMDForward, Vector3.up);

        if(Vector3.Dot(HMDForward, ControllerY) >= 1 - margin)
        {
            WristUI.SetActive(true);
        }
        else
        {
            WristUI.SetActive(false);
        }
    }
}
