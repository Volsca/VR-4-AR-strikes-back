using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    public GameObject Projectile;
    public float shootSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if(Projectile == null)
        {
            Debug.Log("ERR : Projectile undef");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
            //GameObject SpawnedObject = Instantiate(Projectile, transform.position, Quaternion.identity);
            //SpawnedObject.GetComponent<Rigidbody>().velocity = transform.forward * shootSpeed;
        
    }
}
