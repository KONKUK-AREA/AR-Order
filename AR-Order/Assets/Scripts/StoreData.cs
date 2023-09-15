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
    private Sprite[] MenuSprite_CockTail;
    [SerializeField]
    private GameObject[] MenuGameObject_CockTail;
    [SerializeField]
    private string[] MenuDescription_CockTail;
    [SerializeField]
    private Sprite[] AceMenuSprite_CockTail;
    [SerializeField]
    private GameObject[] AceMenuChilds_CockTail;
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
        //나중에 DB로 해야할듯
        switch (name)
        {
            case "Metamong":
                menuIndex = 0;
                Menu[] menus = { new Menu("아메리카노", 4000), new Menu("우주를 줄게", 5000), new Menu("트로피칼 썬샤인", 5500),new Menu("시그니처 아인슈페너", 6000), new Menu("토네이도 초코", 6000),new Menu("퐁당퐁당 라떼", 6500),
                                new Menu("숲속 프루츠 크로플", 8500),new Menu("아메리칸 브렉퍼스트", 9500),new Menu("루꼴라 잠봉뵈르 샌드위치", 8500),
                                new Menu("오스틴 안심 스테이크", 25000),new Menu("토마토 미트볼 파스타", 18000), new Menu("스파이시 카포나타 라구 파스타", 19000),
                                new Menu("아메리칸 브레드 플레이트 ",12000),new Menu("봄 피크닉세트",13000),new Menu("브레드 한 바구니",11000),
                                new Menu("그린스프 & 샐러드 세트",12000),new Menu("메이플탑 팬케이크",12000),new Menu("레옹",10000), };
                //Menu[] menus = {new Menu("커피1", 5000), new Menu("커피2",6500), new Menu("커피3",7500), new Menu("커피4",5000),
                //new Menu("케잌1",7500), new Menu("케잌2",6500), new Menu("피자1",8500)};
                for (int i = 0; i < menus.Length; i++)
                {
                    menus[i].Img = MenuSprite_brunchCafeKKU[i];
                    menus[i].menuPrefab = MenuGameObject_brunchCafeKKU[i];
                    menus[i].Description = MenuDescription_brunchCafeKKU[i];
                }
                int[] info = { 5, 6, 3, 3, 3, 3 };
                AceMenu[] AceMetamong = { };
                string[] BrunchCafeType = { "Coffee & Drinks", "Brunch", "Dinner", "Bread", "Side" };
                //int[] info = { 4, 4, 2, 1 };
                restaurant = new Restaurant("브런치카페 건대입구점", info, menus,AceMetamong,BrunchCafeType,0);
                break;
            case "Cocktail":
                Menu[] Cocktails = {new Menu("모히또 (Mojito)",15000), new Menu("데킬라 선라이즈 (Tequila Sunrise",13000), new Menu("엘 디아블로 (El Diablo)", 12000),new Menu("핑크레이디 (Pink Lady)", 15000), new Menu("피냐콜라다 (Pina Colada)",15000) ,
                new Menu("도쿄 아이스티 (Tokyo Iced Tea)", 12000), new Menu("하와이안 사파이어 (Hawaiian Sapphire)", 17000), new Menu("마티니 (Martini)", 15000), new Menu("마가리타 (Margarita)", 14000), new Menu("멘하탄 (Manhattan)", 16000),
                new Menu("베일리스 밀크 (Baileys Milk)", 9000), new Menu("올드 패션드 (Old Fashioned)",16000), new Menu("파우스트 (Fause)", 10000)};

                for(int i = 0; i< Cocktails.Length; i++)
                {
                    Cocktails[i].Img = MenuSprite_CockTail[i];
                    Cocktails[i].menuPrefab = MenuGameObject_CockTail[i];
                    Cocktails[i].Description = MenuDescription_CockTail[i];
                }
                int[] InfoCocktails = { 4,3,4,3,3};
                AceMenu[] AceCocktails = {new AceMenu(Cocktails[6],1), new AceMenu(Cocktails[1],2)};
                for(int i = 0; i< AceCocktails.Length; i++)
                {
                    AceCocktails[i].aceImage = AceMenuSprite_CockTail[i];
                }
                AceCocktails[0].filter = AceMenuFilter_CockTail;
                AceCocktails[1].childs = AceMenuChilds_CockTail;
                string[] CocktailType = { "청량함을 느낄 수 있는", "여성들을 위한 메뉴", "깔끔하고 독한 맛을 즐기고 싶은", "깊은 맛을 느끼고 싶은" };
                restaurant = new Restaurant("세타몽 칵테일바", InfoCocktails, Cocktails, AceCocktails, CocktailType,1);
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
    public Restaurant(string Name, int[] split, Menu[] menus, AceMenu[] Ace, string[] type, int CharacterIdx) // split[0] = 메뉴 줄, split[1],split[2].... = 각 줄당 메뉴개수
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
        this.CharacterIdx = CharacterIdx;
        aceMenus = Ace;
        MenuType = type;
        Debug.Log("메타몽 디버깅 : " + Name + " " + ListLength + " " + Length);
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
    public GameObject[] childs;
    public int type;
    public AceMenu(Menu menu, int type) // 1 필터 2 파티클
    {
        baseMenu = menu;
        this.type = type;
    }
}
