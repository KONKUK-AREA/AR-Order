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
            // ī�޶� ������ ���� ������ ȸ���� ����մϴ�.
            Vector3 lookDir = arSessionOrigin.camera.transform.position - transform.position;
            lookDir.y = 0f; // y ���� ȸ������ �ʵ��� �����մϴ�.
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);

            // �ε巯�� ȸ���� ���� Slerp �Լ��� ����Ͽ� ������Ʈ�� ȸ���� ������Ʈ�մϴ�.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // ī�޶� ����� ������Ʈ�� ���� ���� ������ ������ ����մϴ�.
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            // ������ ���� �� �̸��̸� �ݺ��� ����
            if (angle < 5f)
            {
                isLookingCamera = true;
            }

            // �ڷ�ƾ�� 1������ ����մϴ�.
            yield return null;
        }
        anim.SetTrigger("Click");
        isAnim = false;
        isLookingCamera= false;
    }
}
