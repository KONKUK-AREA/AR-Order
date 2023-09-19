using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public GameObject bladeTrail;
    public float minCuttingVelocity = .001f;

    bool isCutting = false;
    Vector3 prevoiusPosition;
    GameObject currentBladeTrail;

    Rigidbody rb;
    SphereCollider sphereCollider;
    Camera cam;
 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        cam = Camera.main;
    }


    // Update is called once per frame
    void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if(isCutting)
        {
            UpdateCut();
        }
	}


    void UpdateCut()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 newPosition = hit.point;
            rb.position = newPosition;

            float velocity = (newPosition - prevoiusPosition).magnitude * Time.deltaTime;

            sphereCollider.enabled = velocity > minCuttingVelocity;
            prevoiusPosition = newPosition;
        }
    }


    void StartCutting()
    {
        isCutting = true;
        currentBladeTrail = Instantiate(bladeTrail, transform);
        sphereCollider.enabled = false;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            prevoiusPosition = hit.point;
        }
    }


    void StopCutting()
    {
        isCutting = false;
        currentBladeTrail.transform.SetParent(null);
        sphereCollider.enabled = false;

        Destroy(currentBladeTrail, 1f);
    }
}
