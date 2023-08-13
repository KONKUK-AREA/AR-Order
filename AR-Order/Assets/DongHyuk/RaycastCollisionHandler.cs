using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCollisionHandler : MonoBehaviour
{
    private GameObject food;
    private ParticleSystem devour;
    public LayerMask targetLayer;
    private GameObject arCam;
    private bool myRoutin;
    // Start is called before the first frame update
    void Start()
    {
        myRoutin = true;
        arCam = GameObject.Find("AR Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if(food == null)
        {
            food = Findfood();
        }
        if(Physics.Raycast(transform.position, arCam.transform.forward, 100.0f, targetLayer) && myRoutin == true)
        {
            delayedDestroyer();
        }
        if(Physics.Raycast(transform.position, arCam.transform.forward * -1, 100.0f, targetLayer) && myRoutin == true)
        {
            delayedDestroyer();
        }
    }
    private GameObject Findfood()
    {
        GameObject obj = GameObject.FindWithTag("Character");
        if (obj != null)
        {
            devour = obj.GetComponent<ParticleSystem>();
        }
        return obj;
    }
    void delayedDestroyer()
    {
        myRoutin = false;
        devour.Play();
        food.GetComponent<AudioSource>().Play();
        Destroy(food, 1.8f);
    }
}
