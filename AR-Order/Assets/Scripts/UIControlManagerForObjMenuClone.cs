using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using System.Security.Cryptography;
using TMPro;


public class UIControlManagerForObjMenuClone : MonoBehaviour
{
    public string menuName;
    public TextMeshProUGUI TotalPriceForEachMenu; // 메뉴별 총 금액
    public static int TotalPriceInCart=0;    // 장바구니 안 총 금액
    public static int TotalCount=0;
    private int menuPrice;
    [SerializeField]
    private TextMeshProUGUI TextMenuPrice;
    [SerializeField]
    private TextMeshProUGUI TextMenuCount;
    [SerializeField]
    private Image menuImg;
    private UIControlManager manager;
    

    private int currentValue = 1;
    
    
    private void Start() {

    }
    public void SetInit(string name,int price,Sprite img){
        menuName = name;
        menuPrice = price;
        menuImg.sprite = img;
        TextMenuPrice.text = menuPrice.ToString();
        TotalPriceForEachMenu.text = menuPrice.ToString();
        manager = GameObject.Find("Canvas").GetComponent<UIControlManager>();
        TotalPriceInCart += menuPrice;
        TotalCount++;
        UpdateCart();
    }
    public void ClickedCountUpButton()
    {

        currentValue++;

        Debug.Log("why");


        TextMenuCount.text = currentValue.ToString();

        TotalPriceInCart += menuPrice;
        TotalCount++;
        Debug.Log(manager);
        UpdateCart();
    }




    public void ClickedCountDownButton()
    {
        if(currentValue>1)
        {
            currentValue--;
            TotalCount--;
            Debug.Log("why");

            TextMenuCount.text = currentValue.ToString();
            TotalPriceInCart -= menuPrice;
            UpdateCart();
        }
        else
        {
            TotalCount--;
            TotalPriceInCart -= menuPrice;
            UpdateCart();
            manager.RemoveCart(menuName);
        }

    }
    

    private void UpdateCart(){
        TotalPriceForEachMenu.text= (menuPrice*currentValue).ToString();
        manager.UpdateTotal(TotalPriceInCart,TotalCount);
        manager.StartCartEvent();
    }

    
    








}
