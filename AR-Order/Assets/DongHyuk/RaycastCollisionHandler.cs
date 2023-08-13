using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCollisionHandler : MonoBehaviour
{
    public GameObject food;
    public ParticleSystem devour;
    public LayerMask targetLayer;
    public GameObject arCam;
    private bool myRoutin;
    // Start is called before the first frame update
    void Start()
    {
        myRoutin = true;
    }

    // Update is called once per frame
    void Update()
    {        
        if(Physics.Raycast(transform.position, arCam.transform.forward, 100.0f, targetLayer) && myRoutin == true)
        {
            delayedDestroyer();
        }
        if(Physics.Raycast(transform.position, arCam.transform.forward * -1, 100.0f, targetLayer) && myRoutin == true)
        {
            delayedDestroyer();
        }
    }

    void delayedDestroyer()
    {
        myRoutin = false;
        devour.Play();
        food.GetComponent<AudioSource>().Play();
        Destroy(food, 1.8f);
    }
}
