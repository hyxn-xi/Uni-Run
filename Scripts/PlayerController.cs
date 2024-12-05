using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour 
{
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수 -> 바닥에 닿을 때 마다 매번 0으로 리셋, 2단 점프나 3단 점프 등을 구현할 때 캐릭터가 현재 몇 회까지 점프했는 지 검사
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄   기본값 -> false
   private bool isDead = false; // 사망 상태        기본값 -> false, isDead사용 시 죽은 상태에서 다시 죽는 비논리적 상황을 막을 수 있음

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() 
   {
       // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
       playerRigidbody = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       playerAudio = GetComponent<AudioSource>();
   }

   private void Update() 
   {
       // 사용자 입력을 감지하고 점프하는 처리
       if(isDead)
       {
            return;             // 사망 시 처리를 더 이상 진행하지 않고 종료
       }

        // 마우스 왼쪽 버튼을 눌렀으며 && 최대 점프 횟수(2)에 도달하지 않았다면
       if(Input.GetMouseButtonDown(0) && jumpCount < 2)
       {
            jumpCount++;        // 점프 횟수 증가
            playerRigidbody.velocity = Vector2.zero;        // 점프 직전에 속도를 순간적으로 (0,0)으로 변경
            playerRigidbody.AddForce(new Vector2(0, jumpForce));        // 리지드바디에 위쪽으로 힘 주기
            playerAudio.Play();                                           // 오디오 소스 재생
       }
       // 마우스 왼쪽 버튼에서 손을 뗴는 순간 && 속도의 y 값이 양수라면(위로 상승 중)
       else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
       {
        // 현재 속도를 절반으로 변경
        playerRigidbody.velocity = playerRigidbody.velocity*0.5f;
       }
       animator.SetBool("Grounded", isGrounded);            // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신
   }

   private void Die() 
   {
       // 사망 처리
       // 애니메이터의 Die 트리거 파라미터 셋
       animator.SetTrigger("Die");
       // 오디오 소스에 할당된 오디오 클립을 deathClip으로 변경
       playerAudio.clip = deathClip;
       // 사망 효과음 재생
       playerAudio.Play();

       // 속도를 제로(0,0)로 변경
       playerRigidbody.velocity = Vector2.zero;
       // 사망 상태를 true로 변경
       isDead = true;

       // 게임 매니저 스크립트의 게임오버 함수 처리 실행
       GameManager.instance.OnPlayerDead();
   }

   private void OnTriggerEnter2D(Collider2D other) 
   {
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
       if(other.tag == "Dead" && !isDead)
       {
            // 충돌한 상대방의 태그가 Dead이며 아직 사망하지 않았다면 Die()함수 실행
            Die();
       }
   }

   private void OnCollisionEnter2D(Collision2D collision) 
   {
       // 바닥에 닿았음을 감지하는 처리
       // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
       if(collision.contacts[0].normal.y > 0.7f)
       {
            // isGrounded를 true로 변경하고, 누적 점프 횟수를 0으로 리셋
            isGrounded = true ;
            jumpCount = 0 ;
       }
   }

   private void OnCollisionExit2D(Collision2D collision) 
   {
       // 바닥에서 벗어났음을 감지하는 처리
       // 어떤 콜라이더에서 떼어진 경우 isGrounded를 false로 변경
       isGrounded = false ;
   }
}