using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour 
{
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수, 발판 3개가 돌아가며 사용하기에 충분하지 않다면 값을 늘려도 상관 없음

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격, 발판을 배치할 때 마다 매번 랜덤하게 값 변경

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들, 프리팹으로부터 생성한 발판 게임 오브젝트를 저장할 배열 변수
    private int currentIndex = 0; // 할당 된 발판 중에서 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치. 초반에 보여선 안되기 때문에 위치값을 (0, -25)로 할당
    private float lastSpawnTime; // 가장 최근에 발판을 재배치한 시점. 발판을 재배치할 때마다 매번 갱신


    void Start() 
    {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        
        // count만큼의 공간을 가지는 새로운 발판 배열 생성
        platforms = new GameObject[count];
        // count만큼 루프하면서 발판 생성
        for (int i = 0; i < count; i++)
        {
            // platformPrefab을 원본으로 새 발판을 poolPosition 위치에 복제 생성(Instantiate()함수 사용)
            // 생성된 발판을 platforms 배열에 할당
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);      // 생성할 위치는 poolPosition, 회전은 Quaternion.identity
        }

        lastSpawnTime = 0f;             // 마지막 배치 시점 초기화
        timeBetSpawn = 0f;              // 다음번 배치까지의 시간 간격을 0으로 초기화
    }

    void Update() 
    {
        // 순서를 돌아가며 주기적으로 발판을 배치
        // isGameover가 참인 경우 업데이트 함수 매번 종료, 거짓일 경우 업데이트 함수 진행
        if (GameManager.instance.isGameover){ return; }                     // 게임오버 상태에서는 밑의 if문이 동작하지 않음

        if (Time.time >= lastSpawnTime + timeBetSpawn)                      // 마지막 배치 시점에서 timeBetSpawn 이상 시간이 흘렀다면
        {
            lastSpawnTime = Time.time;                                      // 기록된 마지막 배치 시점을 현재 시점으로 갱신(Time.time 사용)
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);  // 다음 배치까지의 시간 간격을 timeBetSpawnMin, timeBetSpawnMax 사이에서 랜덤 설정
            float yPos = Random.Range(yMin, yMax);                          // 배치할 위치의 높이를 yMin, yMax 사이에서 랜덤 설정

            // 사용할 현재 순번의 발판 게임 오브젝트를 비활성화하고 즉시 다시 활성화
            // 이때 발판의 Platform 컴포넌트의 OnEnable 함수 실행됨
            platforms[currentIndex].SetActive(false);                       // 발판 오브젝트 상태 리셋
            platforms[currentIndex].SetActive(true);

            // 현재 순번의 발판을 화면 오른쪽에 재배치
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            currentIndex++;                                                 // 순번 넘기기

            if (currentIndex >= count) { currentIndex = 0; }                // 마지막 순번에 도달했다면 순번을 리셋
        }
    }
}