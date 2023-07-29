using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using ZXing;
using TMPro;


public class GetDataFromQR : MonoBehaviour
{
    private Rect screenRect;
    static string strQRCodeRead;
    public float waitTime = 1.5f;
    public TextMeshProUGUI Name;
    Texture2D qrTexture;
    [Header("Feedbacks")]
    public UnityEvent feedBack;
   

    private bool isGetQR = false;
    private JsonInfo data;

    private void Start()
    {
        qrTexture = new Texture2D(Screen.width, Screen.height);
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        
    }
    
    private void Update()
    {

    }
    public void GetQR()
    {
        if (!isGetQR)
        {
            try
            {
                StartCoroutine("CaptureScreen");
                //qrTexture.ReadPixels(screenRect, 0, 0);
                IBarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(qrTexture.GetPixels32(), qrTexture.width, qrTexture.height);
                if (result != null)
                {
                    Debug.Log(result.Text);
                    strQRCodeRead = result.Text;
                    data = JsonLoader.loadJson(result.Text);
                    Debug.Log(data.name);
                    isGetQR = true;
                    Name.text = data.name;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
    }
    IEnumerator CaptureScreen()
    {
        yield return new WaitForEndOfFrame();
        qrTexture.ReadPixels(screenRect, 0, 0);
    }
    
    public bool isGetInfo()
    {
        return isGetQR;
    }
    public JsonInfo marketInfo()
    {
        return data;
    }
}
