using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailRoatation : MonoBehaviour
{
    public float rotationSpeed = 60.0f; // 회전 속도 (초당 60도)
    public GameObject Cocktail;
    // Update 함수는 프레임마다 호출됩니다.
    void Update()
    {
        // 로컬 축을 중심으로 회전합니다.
        Cocktail.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
