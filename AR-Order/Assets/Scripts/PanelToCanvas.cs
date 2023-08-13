using UnityEngine;
using UnityEngine.UI;

public class PanelToCanvas : MonoBehaviour
{
    void Start()
    {
        // 캔버스와 패널의 크기를 일치시킵니다.
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width,Screen.height);
    }

}

