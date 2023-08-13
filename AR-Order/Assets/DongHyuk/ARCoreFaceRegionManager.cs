using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

public class ARCoreFaceRegionManager : MonoBehaviour
{
    public GameObject nosePrefab;

    ARFaceManager arFaceManager;
    ARSessionOrigin sessionOrigin;

    NativeArray<ARCoreFaceRegionData> faceRegions;

    private GameObject noseObject;

    // Start is called before the first frame update
    void Start()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        sessionOrigin = GetComponent<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        ARCoreFaceSubsystem subsystem = (ARCoreFaceSubsystem)arFaceManager.subsystem;

        foreach(ARFace face in arFaceManager.trackables)
        {
            subsystem.GetRegionPoses(face.trackableId, Unity.Collections.Allocator.Persistent, ref faceRegions);

            foreach (ARCoreFaceRegionData faceRegion in faceRegions)
            {
                ARCoreFaceRegion regionType = faceRegion.region;

                if (regionType == ARCoreFaceRegion.NoseTip)
                {
                    
                    if (!noseObject)
                    {
                        noseObject = Instantiate(nosePrefab, sessionOrigin.trackablesParent);
                    }
                    

                    noseObject.transform.localPosition = faceRegion.pose.position + new Vector3(0.0f, -0.035f, 0.0f);
                    noseObject.transform.localRotation = faceRegion.pose.rotation;
                }
            }
        }
    }
}
