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
    [SerializeField]
    private string[] MenuDescription_brunchCafeKKU;

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
                Menu[] menus = { 
                                new Menu("칵테일1", 4000), new Menu("칵테일2", 5000), 
                                new Menu("칵테일3", 5500),new Menu("칵테일4", 6000),new Menu("칵테일5", 5500),new Menu("칵테일6", 6000),
                                new Menu("칵테일7", 5500),new Menu("칵테일8", 6000),new Menu("칵테일9", 5500),new Menu("칵테일10", 6000),
                                new Menu("칵테일11", 5500),new Menu("칵테일12", 6000),new Menu("칵테일13", 5500),new Menu("칵테일14", 6000),
                                new Menu("칵테일15", 5500),new Menu("칵테일16", 6000),new Menu("칵테일17", 5500),new Menu("칵테일18", 6000),
                                new Menu("칵테일19", 5500),new Menu("칵테일20", 6000),new Menu("칵테일21", 5500),new Menu("칵테일22", 6000),
                    
                                // new Menu("아메리카노", 4000), new Menu("우주를 줄게", 5000), new Menu("트로피칼 썬샤인", 5500),new Menu("시그니처 아인슈페너", 6000), new Menu("토네이도 초코", 6000),new Menu("퐁당퐁당 라떼", 6500),
                                // new Menu("숲속 프루츠 크로플", 8500),new Menu("아메리칸 브렉퍼스트", 9500),new Menu("루꼴라 잠봉뵈르 샌드위치", 8500),
                                // new Menu("오스틴 안심 스테이크", 25000),new Menu("토마토 미트볼 파스타", 18000), new Menu("스파이시 카포나타 라구 파스타", 19000),
                                // new Menu("아메리칸 브레드 플레이트 ",12000),new Menu("봄 피크닉세트",13000),new Menu("브레드 한 바구니",11000),
                                // new Menu("그린스프 & 샐러드 세트",12000),new Menu("메이플탑 팬케이크",12000),new Menu("레옹",10000), 
                                
                                
                                };
                //Menu[] menus = {new Menu("커피1", 5000), new Menu("커피2",6500), new Menu("커피3",7500), new Menu("커피4",5000),
                //new Menu("케잌1",7500), new Menu("케잌2",6500), new Menu("피자1",8500)};
                for(int i =0; i < menus.Length; i++)
                {
                    menus[i].Img = MenuSprite_brunchCafeKKU[i];
                    menus[i].menuPrefab = MenuGameObject_brunchCafeKKU[i];
                    menus[i].Description = MenuDescription_brunchCafeKKU[i];
                }
                int[] info = { 5,       2,3,4,3,3};     //행 갯수, 행 별 버튼 갯수
                //int[] info = { 4, 4, 2, 1 };
                restaurant = new Restaurant("브런치카페 건대입구점", info, menus);
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
    public Restaurant(string Name, int[] split, Menu[] menus) // split[0] = 메뉴 줄, split[1],split[2].... = 각 줄당 메뉴개수
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
