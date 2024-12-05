using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour {
    public float speed = 10f; // 이동 속도

    private void Update() 
    {
        // 게임 오브젝트를 왼쪽으로 일정 속도로 평행 이동하는 처리
        // 게임오버가 아니라면
        if(!GameManager.instance.isGameover)
        {
        // 초당 speed의 속도로 왼쪽으로 평행이동
        transform.Translate(Vector3.left*speed*Time.deltaTime);     // 게임 오브젝트를 초당 (-speed, 0, 0) 만큼 이동
        // Vector3.left의 값 -> (-1, 0, 0), 따라서 Vector3.left*speed = (-speed, 0, 0)
        }
    }
}