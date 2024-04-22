using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject UI_Prefab;
    public float margin = 0.30f; // une marge de cos(pi/4)
    GameObject WristUI;


    // Start is called before the first frame update
    void Start()
    {
        // Starting position of the UI, moving it to be at the side of the wrist
        Vector3 startPosition = intersectionUI();

        WristUI = Instantiate(UI_Prefab, startPosition, Quaternion.identity);
        WristUI.transform.parent = this.transform;
        WristUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 HMDForward = MainCamera.transform.forward.normalized;
        WristUI.transform.position = intersectionUI();
        WristUI.transform.rotation = Quaternion.LookRotation(HMDForward, Vector3.up);

        Debug.Log("Première condition");
        Debug.Log(Vector3.Dot(-transform.right.normalized, MainCamera.transform.right.normalized) > 1 - margin);

        Debug.Log("Deuxième condition");
        Debug.Log(Vector3.Dot(-transform.up.normalized, HMDForward) > 1 - margin);

        // Condition pour afficher le UI
        if (true)//Vector3.Dot(-transform.right.normalized, MainCamera.transform.right.normalized) > 1 - margin && Vector3.Dot(transform.up.normalized, HMDForward) > 1 - margin)
        {
            WristUI.SetActive(true);
        }
        /*else
        {
            WristUI.SetActive(false);
        }*/
    }

    // Calcule l'intersection entre le casque et le plan de la main 
    Vector3 intersectionUI()
    {
        Vector3 PI;
        Vector3 HMDForward = MainCamera.transform.forward.normalized;
        Vector3 Ym = this.transform.up.normalized;

        float D = Vector3.Dot(Ym, transform.position); // Maybe invert the sign
        float t = -(D + Vector3.Dot(Ym, MainCamera.transform.position) / Vector3.Dot(Ym, HMDForward));

        PI = t * HMDForward + MainCamera.transform.position;

        return PI;
    }
}
