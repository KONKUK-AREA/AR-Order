using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderButtonHandler : MonoBehaviour
{
    public GameObject buttonItSelf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeButton()
    {
        buttonItSelf.transform.GetChild(0).gameObject.SetActive(true);
        buttonItSelf.transform.GetChild(1).gameObject.SetActive(false);
        buttonItSelf.transform.GetChild(2).gameObject.SetActive(false);
    }

}
