using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private bool isPunch = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        arSessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        transform.localScale = Vector3.one * 0.1f;

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
                if (isPunch)
                {
                    Punch();
                }
                else
                {
                    isAnim = true;
                    StartCoroutine("StartAnim");
                }
            }
        }
    }
    IEnumerator StartAnim()
    {
        Vector3 charVec = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 cameraVec = transform.position - arSessionOrigin.camera.transform.position;
        cameraVec.y = 0f;

        bool Right = onRight(-1*cameraVec, charVec);
        while (!isLookingCamera)
        {
            charVec = new Vector3(transform.forward.x, 0, transform.forward.z);
            cameraVec = transform.position - arSessionOrigin.camera.transform.position;
            cameraVec.y = 0f; 
            float angleWithVector= GetAngle(-1*cameraVec, charVec);
            //Debug.Log("��Ÿ�� ����� : " + angleWithVector);
            if (angleWithVector <= 5f)
            {
                isLookingCamera= true;
            }
           
            transform.Rotate(0f, ((Right)? 1f : -1f) * 150f * Time.deltaTime, 0f);
            yield return null;
        }
        anim.SetTrigger("Click");
        isAnim = false;
        isLookingCamera= false;
    }
    public float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
       
        float Angle =  Mathf.Acos( Vector3.Dot(vStart,vEnd) / (Vector3.Magnitude(vStart) * Vector3.Magnitude(vEnd)));
        Debug.Log("��Ÿ�� ����� : " + Angle*Mathf.Rad2Deg);
        return Angle*Mathf.Rad2Deg;
        //return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
    public bool onRight(Vector3 cameraV, Vector3 charV)
    {
        Vector3 cross = Vector3.Cross(charV.normalized, cameraV.normalized);
        return (cross.y) >= 0f;
    }
    public void ChangeReact()
    {
        isPunch = !isPunch;
    }
    public void Punch()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = false;
        rigid.useGravity = true;

        // ī�޶� ����� ������Ʈ ������ ���͸� ���մϴ�.
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        directionToCamera.y = 0; // Y �� �̵��� ������� ����
        directionToCamera.Normalize();

        // ī�޶� �ݴ� �������� ��ġ�� ���ϴ� ���� ���մϴ�.
        Vector3 forceDir = -directionToCamera + transform.up;
        forceDir.Normalize();

        // ��ġ�� ���� ������ �� �ִ� ������ �߰��ϰ�, ���� �����մϴ�.
        float punchForce = 10f;
        rigid.AddForce(forceDir * punchForce, ForceMode.Impulse);

        StartCoroutine("DestroyAfterPunch");

    }
    IEnumerator DestroyAfterPunch()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
