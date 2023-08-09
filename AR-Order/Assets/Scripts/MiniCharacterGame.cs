using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MiniCharacterGame : MonoBehaviour
{
    private bool isAnim = false;
    private bool isClick = false;
    private int isMoving = 0;
    private Coroutine MoveCoroutine = null;
    private ARSessionOrigin arSessionOrigin;

    private RaycastHit hitLayerChar;
    private LayerMask charLayerMask;
    private bool isLookingCamera = false;
    private float rotationSpeed = 5f;
    private float touchTime = 0f;
    private Animator anim;

    private bool isPunch = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        arSessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        transform.localScale = Vector3.one * 0.1f;

    }
    Vector3 prePos = Vector3.zero;
    Transform hitTransform;
    float dist;
    Vector3 offset = Vector3.zero;
    Vector3 hitVec = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 0)
        {

            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                if (isAnim) return;
                charLayerMask = LayerMask.GetMask("Character");
                Ray ray = arSessionOrigin.camera.ScreenPointToRay(pos);
                if (Physics.Raycast(ray, out hitLayerChar, Mathf.Infinity, charLayerMask))
                {
                    prePos = touch.position;
                    isClick = true;
                    touchTime = 0;
                    hitTransform = hitLayerChar.transform;
                    dist = hitTransform.position.z - arSessionOrigin.camera.transform.position.z;
                    hitVec = new Vector3(prePos.x, prePos.y, dist);
                    hitVec = arSessionOrigin.camera.ScreenToWorldPoint(hitVec);
                    offset = hitTransform.position - hitVec;
                    if (isPunch)
                    {
                        StopMoveCoroutine();
                        Punch();
                    }
                    else
                    {
                        isAnim = true;
                        StopMoveCoroutine();
                        StartCoroutine("StartAnim");
                    }
                }
            }
            if(isClick && touch.phase == TouchPhase.Moved)
            {
                touchTime += Time.deltaTime;
                if(touchTime > 0.2f)
                {
                    hitVec = new Vector3(Input.mousePosition.x,Input.mousePosition.y, dist);
                    hitVec = arSessionOrigin.camera.ScreenToWorldPoint(hitVec);
                    hitTransform.position = hitVec - offset;
                }
            }
            if(isClick && touch.phase == TouchPhase.Ended)
            {
                Vector3 lastPos = touch.position;
                Vector3 dragVector = lastPos - prePos;
                Vector3 standardDir = new Vector3(0, 1, 0) - Vector3.zero;
                float dragSpeed = Vector3.Magnitude(dragVector) / touchTime;
                float dragAngle = Vector3.Angle(standardDir, dragVector);
                Debug.Log("메타몽 디버깅 : " + dragSpeed + " " + dragAngle);
                if(dragSpeed  <3500f)
                {

                }
                else
                {
                    if(dragAngle < 30f)
                    {
                        Punch();
                    }
                }
                isClick= false;

            }
        }
        /*
        if (!isClick)
        {
            if (isMoving == 0)
            {
                StartMoveCoroutine();
            }
            if (isMoving == 2)
            {
                CheckFront();
            }

            return;
        }
        */

    }
    void StartMoveCoroutine()
    {
        if (MoveCoroutine == null)
        {
            MoveCoroutine = StartCoroutine(MoveChar());
        }
    }
    void StopMoveCoroutine()
    {
        if (MoveCoroutine != null)
        {
            StopCoroutine(MoveCoroutine);
            MoveCoroutine = null;
        }
    }
    IEnumerator MoveChar(){
        isMoving = 1;
        int Angle = Random.Range(0, 180);
        int randomDir = Random.Range(0, 2);
        Vector3 fromVector = transform.forward;
        Vector3 toVector = transform.forward;
        bool dir = (randomDir == 0) ? true : false;
        while (GetAngle(fromVector,toVector) < Angle)
        {
            transform.Rotate(0f, (dir ? 1f : -1f) * 60f * Time.deltaTime, 0f);
            fromVector = transform.forward;
            yield return null;
        }
        isMoving = 2;
        float countTime = 0;
        while (countTime <= 2f)
        {
            transform.Translate(Vector3.forward *0.5f* Time.deltaTime, Space.Self);
            countTime += Time.deltaTime;
            yield return null;
        }
        MoveCoroutine = null;
        isMoving = 0;
    }
    void CheckFront()
    {
        Vector3 checkFront = Quaternion.Euler(30f, 0f, 0f) * transform.forward - transform.forward;
        Ray ray = new Ray(transform.position, checkFront);
        if (Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("Plane")))
        {
            StopMoveCoroutine();
            isMoving = 0;
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
            if (angleWithVector <= 5f)
            {
                isLookingCamera= true;
            }
           
            transform.Rotate(0f, ((Right)? 1f : -1f) * 150f * Time.deltaTime, 0f);
            yield return null;
        }
        anim.SetTrigger("Click");
        StartCoroutine(isEndAnim());
    }
    IEnumerator isEndAnim()
    {
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("attack")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }
        isAnim = false;
        isLookingCamera = false;
    }
    public float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
       
        float Angle =  Mathf.Acos( Vector3.Dot(vStart,vEnd) / (Vector3.Magnitude(vStart) * Vector3.Magnitude(vEnd)));
        //Debug.Log("메타몽 디버깅 : " + Angle*Mathf.Rad2Deg);
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

        // 카메라 방향과 오브젝트 사이의 벡터를 구합니다.
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        directionToCamera.y = 0; // Y 축 이동은 고려하지 않음
        directionToCamera.Normalize();

        // 카메라 반대 방향으로 펀치를 가하는 힘을 구합니다.
        Vector3 forceDir = -directionToCamera + transform.up;
        forceDir.Normalize();

        // 펀치할 힘을 조절할 수 있는 변수를 추가하고, 힘을 적용합니다.
        float punchForce = 10f;
        rigid.AddForce(forceDir * punchForce, ForceMode.Impulse);

        StartCoroutine("DestroyAfterPunch");

    }
    IEnumerator DestroyAfterPunch()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
