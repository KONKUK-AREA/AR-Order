using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZXing;


public class GetDataFromQR : MonoBehaviour
{
    private Rect screenRect;
    static string strQRCodeRead;
    public RawImage image;
    public float waitTime = 1.5f;
    Texture2D qrTexture;
    [Header("Feedbacks")]
    public UnityEvent feedBack;

    private bool isGetQR = false;

    private void Start()
    {
        qrTexture = new Texture2D(Screen.width, Screen.height);
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        
    }
    private void Update()
    {
        if (!isGetQR)
        {
            try
            {
                qrTexture.ReadPixels(screenRect, 0, 0);
                IBarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(qrTexture.GetPixels32(), qrTexture.width, qrTexture.height);
                if (result != null)
                {
                    Debug.Log(result.Text);
                    strQRCodeRead = result.Text;
                    JsonInfo data = JsonLoader.loadJson(result.Text);
                    isGetQR= true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
    }
}
