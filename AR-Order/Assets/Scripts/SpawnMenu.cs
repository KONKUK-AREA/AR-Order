using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnMenu : MonoBehaviour
{
    public GameObject KU;
    // Start is called before the first frame update
    GameObject plane;
    GameObject SpawnedObject;
    public ARSessionOrigin arSessionOrigin;
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isReady()
    {
        plane = GameObject.Find("ARPlane");
        return GetComponent<GetDataFromQR>().isGetInfo() && (plane != null);
    }
    public void spawnItem()
    {
        if(SpawnedObject!=null) Destroy(SpawnedObject);
        Vector3 Pos = arSessionOrigin.camera.transform.localPosition;
        Vector3 Rot = plane.transform.eulerAngles;
        Vector3 rePos = new Vector3(Pos.x,Pos.y,plane.transform.localPosition.z);
        Vector3 reRot = new Vector3(Rot.x + 90, Rot.y, Rot.z);
        SpawnedObject = Instantiate(KU, rePos, Quaternion.Euler(reRot));
    }
}
