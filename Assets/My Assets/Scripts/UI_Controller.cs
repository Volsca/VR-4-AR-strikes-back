using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject UI_Prefab;
    public GameObject UI_Follow;
    public float margin = 0.10f; // une marge de cos(pi/4)
    public float disMargin = 0.7f;
    public float distance = 0.2f;
    GameObject WristUI;


    // Start is called before the first frame update
    void Start()
    {
        // Starting position of the UI, moving it to be at the side of the wrist
        Vector3 HMDForward = MainCamera.transform.forward.normalized;
        Vector3 startPosition = MainCamera.transform.position + distance * HMDForward;
        WristUI = Instantiate(UI_Prefab, startPosition, Quaternion.identity);
        //WristUI.transform.parent = this.transform;
        WristUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 HMDForward = MainCamera.transform.forward.normalized;
        //Trop sensible au variations d'angle de la main
        //WristUI.transform.position = IntersectionUI();

        Vector3 HMDDirection = MainCamera.transform.position - WristUI.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(-HMDDirection);
        WristUI.transform.rotation = lookRotation;

        Debug.Log("Premi�re condition");
        Debug.Log(Vector3.Dot(-transform.right.normalized, MainCamera.transform.right.normalized) > 1 - margin);

        Debug.Log("Deuxi�me condition");
        Debug.Log(Vector3.Dot(-transform.up.normalized, HMDForward) > 1 - margin);

        // Condition pour afficher le UI
        if (Vector3.Dot(transform.up.normalized, HMDForward) > 1 - margin)//Vector3.Dot(-transform.right.normalized, MainCamera.transform.right.normalized) > 1 - margin && Vector3.Dot(transform.up.normalized, HMDForward) > 1 - margin)
        {
            WristUI.SetActive(true);
        }
        else if (Vector3.Dot(transform.up.normalized, HMDForward) < 1 - disMargin)
        {
            WristUI.transform.position = MainCamera.transform.position + distance * HMDForward;
            WristUI.SetActive(false);
        }
    }

    // Calcule l'intersection entre le casque et le plan de la main /////// DEPRECATED ////////
    Vector3 IntersectionUI()
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
