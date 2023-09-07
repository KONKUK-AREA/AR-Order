using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class StoreData : MonoBehaviour
{
    private static StoreData instance = null;

    [Header("Metamong")]
    [SerializeField]
    private Sprite[] MenuSprite_brunchCafeKKU;
    [SerializeField]
    private GameObject[] MenuGameObject_brunchCafeKKU;
    [SerializeField]
    private string[] MenuDescription_brunchCafeKKU;
    [SerializeField]
    private Sprite[] MenuSprite_CockTail;
    [SerializeField]
    private GameObject[] MenuGameObject_CockTail;
    [SerializeField]
    private string[] MenuDescription_CockTail;
    [SerializeField]
    private Sprite AceMenuSprite_CockTail;
    [SerializeField]
    private GameObject[] AceMenuChilds_CockTail;

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
                Menu[] menus = { new Menu("�Ƹ޸�ī��", 4000), new Menu("���ָ� �ٰ�", 5000), new Menu("Ʈ����Į �����", 5500),new Menu("�ñ״�ó ���ν����", 6000), new Menu("����̵� ����", 6000),new Menu("�������� ��", 6500),
                                new Menu("���� ������ ũ����", 8500),new Menu("�Ƹ޸�ĭ �귺�۽�Ʈ", 9500),new Menu("��ö� ����Ƹ� ������ġ", 8500),
                                new Menu("����ƾ �Ƚ� ������ũ", 25000),new Menu("�丶�� ��Ʈ�� �Ľ�Ÿ", 18000), new Menu("�����̽� ī����Ÿ �� �Ľ�Ÿ", 19000),
                                new Menu("�Ƹ޸�ĭ �극�� �÷���Ʈ ",12000),new Menu("�� ��ũ�м�Ʈ",13000),new Menu("�극�� �� �ٱ���",11000),
                                new Menu("�׸����� & ������ ��Ʈ",12000),new Menu("������ž ������ũ",12000),new Menu("����",10000), };
                //Menu[] menus = {new Menu("Ŀ��1", 5000), new Menu("Ŀ��2",6500), new Menu("Ŀ��3",7500), new Menu("Ŀ��4",5000),
                //new Menu("�ɟ�1",7500), new Menu("�ɟ�2",6500), new Menu("����1",8500)};
                for(int i =0; i < menus.Length; i++)
                {
                    menus[i].Img = MenuSprite_brunchCafeKKU[i];
                    menus[i].menuPrefab = MenuGameObject_brunchCafeKKU[i];
                    menus[i].Description = MenuDescription_brunchCafeKKU[i];
                }
                int[] info = { 5, 6, 3,3,3,3 };
                AceMenu[] AceMetamong = { };
                //int[] info = { 4, 4, 2, 1 };
                restaurant = new Restaurant("�귱ġī�� �Ǵ��Ա���", info, menus,AceMetamong);
                break;
            case "Cocktail":
                Menu[] Cocktails = { };

                for(int i = 0; i< Cocktails.Length; i++)
                {

                }
                int[] InfoCocktails = { };
                AceMenu[] AceCocktails = { };
                break;
        
        }
        return restaurant;
    }
}
public class Restaurant
{
    public Menu[][] totalMenu;
    public AceMenu[] aceMenus;
    public string Name;
    public int ListLength = 0;
    public int Length=0;
    public Restaurant(string Name, int[] split, Menu[] menus, AceMenu[] Ace) // split[0] = �޴� ��, split[1],split[2].... = �� �ٴ� �޴�����
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
        aceMenus = Ace;
        Debug.Log("��Ÿ�� ����� : " + Name + " " + ListLength + " " + Length);
    }
    
}

public class Menu
{
    public string name;
    public int price;
    public Sprite Img=null;
    public GameObject menuPrefab=null;
    public string Description;

    public Menu(string name, int price)
    {
        this.name = name;
        this.price = price;

    }

}
public class AceMenu
{
    public Menu baseMenu;
    public Sprite filter;
    public GameObject[] childs;
    public int type;
    public AceMenu(Menu menu, int type)
    {
        baseMenu = menu;
        this.type = type;
    }
}
