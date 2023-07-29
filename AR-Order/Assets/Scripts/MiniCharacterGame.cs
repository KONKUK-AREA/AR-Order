using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MiniCharacterGame : MonoBehaviour
{
    private bool isAnim = false;
    private ARSessionOrigin arSessionOrigin;

    private RaycastHit hitLayerChar;
    private LayerMask charLayerMask;
    private bool isLookingCamera = false;
    private float rotationSpeed = 5f;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        arSessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0 || isAnim) return;
        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            charLayerMask = LayerMask.GetMask("Character");
            Ray ray = arSessionOrigin.camera.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hitLayerChar, Mathf.Infinity, charLayerMask))
            {
                isAnim = true;
                StartCoroutine("StartAnim");
            }
        }
    }
    IEnumerator StartAnim()
    {
        while (!isLookingCamera)
        {
            // 카메라 쪽으로 고개를 돌리는 회전을 계산합니다.
            Vector3 lookDir = arSessionOrigin.camera.transform.position - transform.position;
            lookDir.y = 0f; // y 축은 회전하지 않도록 설정합니다.
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);

            // 부드러운 회전을 위해 Slerp 함수를 사용하여 오브젝트의 회전을 업데이트합니다.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 카메라 방향과 오브젝트의 방향 벡터 사이의 각도를 계산합니다.
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            // 각도가 일정 값 미만이면 반복문 종료
            if (angle < 5f)
            {
                isLookingCamera = true;
            }

            // 코루틴을 1프레임 대기합니다.
            yield return null;
        }
        anim.SetTrigger("Click");
        isAnim = false;
        isLookingCamera= false;
    }
}
