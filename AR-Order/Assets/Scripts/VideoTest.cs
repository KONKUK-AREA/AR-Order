using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.url = Path.Combine(Application.streamingAssetsPath, "CocktailFilter.webm");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
