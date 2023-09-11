using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffectsTest : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        // 파티클 시스템 컴포넌트 가져오기
        particleSystem = GetComponent<ParticleSystem>();
        
        // 파티클 시스템을 시작하지 않도록 설정
        particleSystem.Stop();
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 파티클 시스템을 시작 또는 중지
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
            else
            {
                particleSystem.Play();
            }
        }
    }
}