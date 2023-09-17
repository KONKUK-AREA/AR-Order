using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<int> CharacterIndex = new List<int>();
    public bool isFoodTutorial;
    public bool isGameTutorial;
    public int Coupon;
    // Start is called before the first frame update
    private void Awake()
    {
        instance= this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
