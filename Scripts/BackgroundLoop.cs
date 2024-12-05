using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour 
{
    private float width; // 배경의 가로 길이

    private void Awake() // Start보다 먼저. 게임이 실행될 때 최초 1번 실행
    {
        // 가로 길이를 측정하는 처리
        // BoxCollider2D 컴포넌트의 Size 필드의 x 값을 가로 길이로 사용
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();      // backgroundColider에 BoxCollider2D를 할당
        width = backgroundCollider.size.x;                   // 배경의 가로 길이에 backgroundCollider의 x축 사이즈를 할당
    }

    private void Update() 
    {
        // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을때 위치를 재배치
        if(transform.position.x <= width)                   // 현재 x축 위치가 배경의 가로길이보다 작거나 같다면 
        {
            Reposition();                                   // 위치 재배치
        }
    }

    // 위치를 리셋하는 메서드
    private void Reposition() // 재배치를 실제로 실행하는 함수, Update함수에 의해 실행됨
    {
        // 현재 위치에서 오른쪽으로 가로 길이 *2 만큼 이동
        Vector2 offset = new Vector2(width * 2f, 0);       // offset에 가로길이 * 2 할당
        transform.position = (Vector2) transform.position + offset;         // 현재 위치에 offset을 더함 ( 현재 위치에 가로길이 *2 )
    }
}