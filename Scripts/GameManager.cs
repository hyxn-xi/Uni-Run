using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour 
{
    public static GameManager instance; // 싱글턴을 할당할 전역 변수

    public bool isGameover = false; // 게임 오버 상태
    public TextMeshProUGUI scoreText; // 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    private int score = 0; // 게임 점수

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake() 
    {
        // 싱글턴 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글턴 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");         // 경고 로그 출력
            Destroy(gameObject);                                         // 게임 오브젝트 파괴
        }
    }

    void Update() 
    {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (isGameover && Input.GetMouseButtonDown(0))                   // if문을 사용해 isGameover가 T인지 확인, 게임오버 상태인지 확인 && 마우스 왼쪽 버튼 눌렀는 지 확인 
        {
            // 게임오버 상태에서 마우스 왼쪽 버튼을 클릭하면 현재 씬 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // .name == 현재 씬의 이름 가져오기
        }
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) 
    {
        if(!isGameover)                                     // 게임오버가 아니라면
        {
            score += newScore;                              // 점수 증가
            scoreText.text = "Score : " + score;            // 증가된 점수 출력
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead() 
    {
        isGameover = true;                                  // 게임오버 활성화
        gameoverUI.SetActive (true);                        // 게임오버 UI 활성화
    }
}