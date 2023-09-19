using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

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
    private Sprite[] AceMenuSprite_brunchCafeKKU;
    [SerializeField]
    private Sprite[] MenuSprite_CockTail;
    [SerializeField]
    private GameObject[] MenuGameObject_CockTail;
    [SerializeField]
    private string[] MenuDescription_CockTail;
    [SerializeField]
    private Sprite[] AceMenuSprite_CockTail;
    [SerializeField]
    private VideoClip AceMenuFilter_CockTail;
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
            case "Metamong": // Metamong
                menuIndex = 0;
                Menu[] menus = { new Menu("�Ƹ޸�ī��", 4000), new Menu("���ָ� �ٰ�", 5000), new Menu("Ʈ����Į �����", 5500),new Menu("�ñ״�ó ���ν����", 6000), new Menu("����̵� ����", 6000),new Menu("�������� ��", 6500),
                                new Menu("���� ������ ũ����", 8500),new Menu("�Ƹ޸�ĭ �귺�۽�Ʈ", 9500),new Menu("��ö� ����Ƹ� ������ġ", 8500),
                                new Menu("����ƾ �Ƚ� ������ũ", 25000),new Menu("�丶�� ��Ʈ�� �Ľ�Ÿ", 18000), new Menu("�����̽� ī����Ÿ �� �Ľ�Ÿ", 19000),
                                new Menu("�Ƹ޸�ĭ �극�� �÷���Ʈ ",12000),new Menu("�� ��ũ�м�Ʈ",13000),new Menu("�극�� �� �ٱ���",11000),
                                new Menu("�׸����� & ������ ��Ʈ",12000),new Menu("������ž ������ũ",12000),new Menu("����",10000), };
                //Menu[] menus = {new Menu("Ŀ��1", 5000), new Menu("Ŀ��2",6500), new Menu("Ŀ��3",7500), new Menu("Ŀ��4",5000),
                //new Menu("�ɟ�1",7500), new Menu("�ɟ�2",6500), new Menu("����1",8500)};
                for (int i = 0; i < menus.Length; i++)
                {
                    menus[i].Img = MenuSprite_brunchCafeKKU[i];
                    menus[i].menuPrefab = MenuGameObject_brunchCafeKKU[i];
                    menus[i].Description = MenuDescription_brunchCafeKKU[i];
                }
                int[] info = { 5, 6, 3, 3, 3, 3 };
                AceMenu[] AceMetamong = { new AceMenu(menus[16],1,4,2), new AceMenu(menus[11],2,2,2)};
                for(int i = 0; i<AceMetamong.Length; i++)
                {
                    AceMetamong[i].aceImage = AceMenuSprite_brunchCafeKKU[i];
                }
                AceMetamong[0].filter = AceMenuFilter_CockTail;
                string[] BrunchCafeType = { "Coffee & Drinks", "Brunch", "Dinner", "Bread", "Side" };
                //int[] info = { 4, 4, 2, 1 };
                restaurant = new Restaurant("�귱ġī�� �Ǵ��Ա���", info, menus,AceMetamong,BrunchCafeType,0);
                break;
            case "Cocktail"://Cocktail
                Menu[] Cocktails = {new Menu("������",15000), new Menu("��ų�� ��������",13000), new Menu("�� ��ƺ��", 12000),new Menu("��ũ���̵�", 15000), new Menu("�ǳ��ݶ��",15000) ,
                new Menu("��� ���� ���̽�Ƽ", 12000), new Menu("�Ͽ��̾� �����̾�", 17000), new Menu("��Ƽ��", 15000), new Menu("������Ÿ", 14000), new Menu("����ź", 16000),
                new Menu("��Ʈ�κ��� ����ī", 9000), new Menu("�õ� �мǵ�",16000), new Menu("�Ŀ콺Ʈ", 10000)};

                for(int i = 0; i< Cocktails.Length; i++)
                {
                    Cocktails[i].Img = MenuSprite_CockTail[i];
                    Cocktails[i].menuPrefab = MenuGameObject_CockTail[i];
                    Cocktails[i].Description = MenuDescription_CockTail[i];
                }
                int[] InfoCocktails = { 4,3,4,3,3};
                AceMenu[] AceCocktails = { new AceMenu(Cocktails[1], 1,0,1), new AceMenu(Cocktails[6], 2, 1, 3) };
                for(int i = 0; i< AceCocktails.Length; i++)
                {
                    AceCocktails[i].aceImage = AceMenuSprite_CockTail[i];
                }
                AceCocktails[0].filter = AceMenuFilter_CockTail;
                string[] CocktailType = { "û������ ���� �� �ִ�", "�������� ���� �޴�", "����ϰ� ���� ���� ���� ����", "���� ���� ������ ����" };
                restaurant = new Restaurant("Ĭ���Ϲ� �Ǵ��Ա���", InfoCocktails, Cocktails, AceCocktails, CocktailType,1);
                break;
        
        }
        return restaurant;
    }
}
public class Restaurant
{
    public Menu[][] totalMenu;
    public AceMenu[] aceMenus;
    public string[] MenuType;
    public string Name;
    public int CharacterIdx;
    public int ListLength = 0;
    public int Length=0;
    public Restaurant(string Name, int[] split, Menu[] menus, AceMenu[] Ace, string[] type, int Character) // split[0] = �޴� ��, split[1],split[2].... = �� �ٴ� �޴�����
    {
        ListLength = split[0];
        this.Name = Name;
        totalMenu = new Menu[split[0]][];
        for(int i = 0; i < split[0]; i++)
        {//43433
            totalMenu[i] = new Menu[split[i+1]];
            for(int j = 0; j < split[i+1]; j++)
            {
                totalMenu[i][j] = menus[Length++];
                Debug.Log("��Ÿ�� ����� : " + totalMenu[i][j].name);
            }
        }
        CharacterIdx = Character;
        aceMenus = new AceMenu[Ace.Length];
        for(int i = 0; i<aceMenus.Length; i++)
        {
            aceMenus[i] = Ace[i];
        }
        MenuType = new string[type.Length];
        for(int i = 0; i<MenuType.Length; i++)
        {
            MenuType[i] = type[i];
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
    public Sprite aceImage;
    public VideoClip filter;
    public int type;
    public int[] index = new int[2];
    public AceMenu(Menu menu, int type, int idx1, int idx2) // 1 ���� 2 ��ƼŬ
    {
        baseMenu = menu;
        this.type = type;
        index[0] = idx1; index[1]= idx2;
    }
}
