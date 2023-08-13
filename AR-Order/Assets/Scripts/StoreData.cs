using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class StoreData : MonoBehaviour
{
    private static StoreData instance = null;

    [Header("Metamong")]
    [SerializeField]
    private Sprite[] MenuSprite_brunchCafeKKU;
    [SerializeField]
    private GameObject[] MenuGameObject_brunchCafeKKU;

    private Sprite[][] StoreSprites = new Sprite[10][];
    private GameObject[][] StoreGameObjects = new GameObject[10][];
    private int menuIndex = 0;
    private void Start()
    {
        StoreSprites[0] = MenuSprite_brunchCafeKKU;
        StoreGameObjects[0] = MenuGameObject_brunchCafeKKU;


    }


    public Restaurant GetMenu(string name)
    {
        Restaurant restaurant=null;
        //���߿� DB�� �ؾ��ҵ�
        switch (name)
        {
            case "Metamong":
                menuIndex = 0;
                Menu[] menus = { new Menu("Ŀ��1", 3000), new Menu("Ŀ��2", 5000), new Menu("�ɟ�1", 4000), new Menu("����1",5000),new Menu("������ġ1",3000) };
                //Menu[] menus = {new Menu("Ŀ��1", 5000), new Menu("Ŀ��2",6500), new Menu("Ŀ��3",7500), new Menu("Ŀ��4",5000),
                //new Menu("�ɟ�1",7500), new Menu("�ɟ�2",6500), new Menu("����1",8500)};
                for(int i =0; i < menus.Length; i++)
                {
                    menus[i].Img = MenuSprite_brunchCafeKKU[i];
                    menus[i].menuPrefab = MenuGameObject_brunchCafeKKU[i];
                }
                int[] info = { 5, 2, 1,1,1,0 };
                //int[] info = { 4, 4, 2, 1 };
                restaurant = new Restaurant("�귱ġī�� �Ǵ��Ա���", info, menus);
                break;
        
        }
        return restaurant;
    }
}
public class Restaurant
{
    public Menu[][] totalMenu;
    public string Name;
    public int ListLength = 0;
    public int Length=0;
    public Restaurant(string Name, int[] split, Menu[] menus) // split[0] = �޴� ��, split[1],split[2].... = �� �ٴ� �޴�����
    {
        ListLength = split[0];
        this.Name = Name;
        totalMenu = new Menu[split[0]][];
        for(int i = 0; i < split[0]; i++)
        {
            totalMenu[i] = new Menu[split[i+1]];
            for(int j = 0; j < split[i+1]; j++)
            {
                totalMenu[i][j] = menus[Length++];
            }
        }
        Debug.Log("��Ÿ�� ����� : " + Name + " " + ListLength + " " + Length);
    }
    
}

public class Menu
{
    public string name;
    public int price;
    public Sprite Img=null;
    public GameObject menuPrefab=null;

    public Menu(string name, int price)
    {
        this.name = name;
        this.price = price;
    
    }

}
