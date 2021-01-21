using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DistanceManager : MonoBehaviour
{
    public PetManager pm;
    [Header("- 거리 표시기 좌 중 우")]
    //public Text beforeDistance; // 좌측
    public Text mainDistance; // 중앙
    //public Text nextDistance; // 우측
    [Header("- 에너미 스포닝 풀 프리팹")]
    public Transform[] enemyNormal; // 25개
    public Transform[] enemyBoss;  // 25개
    public Transform enemyBox;      // 1개
    [Header("- 에너미 스포닝 풀 좌표")]
    public Transform EnemySpawnPos;
    public Transform EnemyTargetPos;
    [Header("- 백그라운드")]
    public BattleGndManager bgm;
    public Animator playerAnitor;

    /// <summary>
    /// 걷는 사운드 안 겹치게
    /// </summary>
    private bool isWalking;

    public static DistanceManager instance;
    void Start()
    {
        instance = this;
        StartCoroutine(Loading());
    }


    IEnumerator Loading()
    {
        yield return null;
        /// 로딩창 없어질 때까지 행동하지 않음.
        while (!PlayerPrefsManager.isNickNameComp)
        {
            yield return new WaitForFixedUpdate();
        }

        /// --------- 나누 포톤 플레이팹 로딩 끝-----
        TextDistDisplay(PlayerInventory.RecentDistance);


        /// 오프라인 보상 받기 전까지 행동하지 않음.
        while (!PlayerPrefsManager.isGetOfflineReword)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.25f);

        StartAmazonGame();
    }

    /// <summary>
    /// 해당 스테이지 만큼 거리 표시기
    /// </summary>
    /// <param name="stage">도전하는 스테이지 (최대 스테이지)</param>
    public void TextDistDisplay(double stage)
    {
        if (stage == 0)
        {
            mainDistance.text = "0.0km";
        }
        else
        {
            mainDistance.text = (stage * 0.1d).ToString("N1") + "km";
        }
    }


    /// <summary>
    /// 로딩 끝나면 외부에서 호출하기.
    /// </summary>
    public void StartAmazonGame()
    {
        // 튜토리얼 했니?
        if (PlayerPrefsManager.isTutorialClear)     /// 튜토리얼 완료함
        {
            ///*     첫 게임 로드라면 거리 증가는 하지 말기.
            //playerController.gameObject.SetActive(true);
            StartStage();
        }
        else /// 튜토리얼 완료 못함
        {
            /// TODO : 튜토리얼하다가 종료했던 부분 부터 호출
            /// 
            StartStage();
        }

    }

    void StartStage()
    {
        if (!isWalking)
        {
            //AudioManager.instance.PlayAudio("Walk", "SE");
            isWalking = true;
        }
        InitEnemy();
        //Invoke(nameof(InitEnemy), 2.0f);
    }

    /// <summary>
    /// 현재 거리
    /// </summary>
    private int reDist;
    private double Enemy_Hp_Full;
    private double Enemy_DropGold;

    /// <summary>
    /// 어떤 나쁜놈 호출할지 결정해줌
    /// </summary>
    public void InitEnemy()
    {
        /// 광산이나 늪지 입장할때 인보크로 불러지면 리턴
        if (PlayerPrefsManager.isEnterTheSwamp) return;

        float temp = Time.time * 525f;
        Random.InitState((int)temp);
        float randomseed = Random.Range(0, 100f);

        reDist = Mathf.RoundToInt((float)PlayerInventory.RecentDistance);
        TextDistDisplay(reDist);

        /// PlayerInventory.RecentDistance
        if (reDist == 0)            /// 거리 초기값.
        {
            Enemy_Hp_Full = 5d;
            Enemy_DropGold = 3d;
            CreateEnemyNormal();
        }
        else if (reDist % 10 == 0)          /// 10 스테이지 보스전
        {
            //Enemy_Hp_Full = 5d *  (5d * 1.55d * reDist) * PlayerInventory.Monster_Boss_HP;
            //Enemy_DropGold =  5d * (3d * 1.15d * reDist) * PlayerInventory.Player_Gold_Earned;
            //Enemy_Hp_Full = 5d * 5d * Mathf.Pow(1.07f, reDist - 1) * PlayerInventory.Monster_Boss_HP;
            //Enemy_DropGold = 3d * 3d * Mathf.Pow(1.05f, reDist - 1) * PlayerInventory.Player_Gold_Earned;
            Enemy_Hp_Full = (0.1d * (reDist * reDist) + 0.1d * reDist + 4.5d) * 3d * PlayerInventory.Monster_Boss_HP;
            Enemy_DropGold =  (3d * 1.15d * reDist) * 5d * PlayerInventory.Player_Gold_Earned;
            /// 1000 키로 마다 거리 체크해서
            reDist = reDist / 10000;
            /// 1.03 을 1000km 마다 
            for (int i = 0; i < reDist; i++)
            {
                Enemy_Hp_Full *= 1.03d;
                Enemy_DropGold *= 1.03d;
            }

            Debug.LogWarning("BOSS_Hp_Full : " + Enemy_Hp_Full + " BOSS_DropGold : " + Enemy_DropGold);

            CreateEnemyBoss();
        }
        else                                                /// 일반몹
        {
            //Enemy_Hp_Full = (5d * 1.55d * reDist) * PlayerInventory.Monster_Normal_HP;
            //Enemy_DropGold = (3d * 1.15d * reDist) * PlayerInventory.Player_Gold_Earned;
            //Enemy_Hp_Full = 0.25d * Mathf.Pow(1.07f, reDist) * PlayerInventory.Monster_Normal_HP;
            //Enemy_DropGold = 3d * Mathf.Pow(1.05f, reDist) * PlayerInventory.Player_Gold_Earned;
            Enemy_Hp_Full = (0.1d * (reDist * reDist) + 0.1d * reDist + 4.5d) * PlayerInventory.Monster_Normal_HP;
            Enemy_DropGold = (3d * 1.15d * reDist) * PlayerInventory.Player_Gold_Earned;
            /// 1000 키로 마다 거리 체크해서
            reDist = reDist / 10000;
            /// 1.03 을 1000km 마다 
            for (int i = 0; i < reDist; i++)
            {
                Enemy_Hp_Full *= 1.03d;
                Enemy_DropGold *= 1.03d;
            }

            Debug.LogWarning("Enemy_Hp_Full : " + Enemy_Hp_Full + " Enemy_DropGold : " + Enemy_DropGold);

            /// 1~9 스테이지 일반몹 or 박스
            /// 황금 상자 등장 확률 
            if (randomseed <= PlayerInventory.Superbox_Encounter)
            {
                Enemy_DropGold *= PlayerInventory.Superbox_Gold_Earned;
                PlayerPrefsManager.isSuperBox = true;
                //
                CreateEnemyBox();
            }
            else
            {
                CreateEnemyNormal();
            }
        }
    }

    static Transform enemyTransform;
    void SpawEnemyBase(Transform _target)
    {
        enemyTransform = Lean.Pool.LeanPool.Spawn(
                _target,
                Vector3.zero,
                Quaternion.identity);

        enemyTransform.SetParent(EnemySpawnPos); // 에너미 부모 위치에 생성
        enemyTransform.localPosition = Vector3.zero; // 뒤틀리는거 방지
        enemyTransform.localScale = new Vector3(1, 1, 1); // 스케일 값 1 고정
        /// 에너미 내부 값 정의
        enemyTransform.GetComponent<EnemyController>().SetEnemyStat(Enemy_Hp_Full, Enemy_DropGold);

    }
    void CreateEnemyNormal()
    {
        if (enemyTransform != null) return;
        /// 스폰 에너미
        int randomseed = Random.Range(0, 24);
        SpawEnemyBase(enemyNormal[randomseed]);
        //SpawEnemyBase(enemyNormal[0]);
        /// 무브 에너미
        enemyTransform.DOMove(EnemyTargetPos.position, 1f / PlayerInventory.Player_Move_Speed)
            .OnComplete(CallBackEnemyAttack).SetDelay(1f / PlayerInventory.Player_Move_Speed);
    }

    public void CreateEnemyBoss()
    {
        if (enemyTransform != null) return;
        /// 보스 등장시 늪지/동굴 입장 꺼줌
        PlayerPrefsManager.instance.BlockRightThings(false);
        /// 스폰 에너미
        int randomseed = Random.Range(10, 24);
        SpawEnemyBase(enemyBoss[randomseed]);
        /// 무브 에너미
        enemyTransform.DOMove(EnemyTargetPos.position, 1f / PlayerInventory.Player_Move_Speed)
            .OnComplete(CallBackEnemyBoss).SetDelay(1f / PlayerInventory.Player_Move_Speed);
    }

    /// <summary>
    /// DOMove 완료 되면 자동 공격
    /// </summary>
    private void CallBackEnemyBoss()
    {
        /// 보스 전에 입장팝업 켜놨다가  보스 존재할때 늪지로 들어가면 거리 보정 해줌
        if (PlayerPrefsManager.isEnterTheSwamp)
        {
            reDist = Mathf.RoundToInt((float)PlayerInventory.RecentDistance);
            if ((reDist % 10 == 0))
            {
                PlayerInventory.RecentDistance--;
            }
            /// 오른쪽 버튼 다시 열어줌.
            PlayerPrefsManager.instance.BlockRightThings(true);
            return;
        }

        /// 보스 등장하면 보스 시간이랑 버튼 켜줌.
        enemyTransform.GetComponent<EnemyController>().SetBossStat();
        /// 살짝 멈추고
        PlayserStop();
        /// 0.3초 뒤에 공격
        Invoke(nameof(CallBackEnemyAttack), 0.3f);

    }

    /// <summary>
    /// 황금 상자 소환
    /// </summary>
    void CreateEnemyBox()
    {
        if (enemyTransform != null) return;
        /// 스폰 에너미
        SpawEnemyBase(enemyBox);
        /// 무브 에너미
        enemyTransform.DOMove(EnemyTargetPos.position, 1f / PlayerInventory.Player_Move_Speed)
            .OnComplete(CallBackEnemyAttack).SetDelay(1f / PlayerInventory.Player_Move_Speed);
    }


    void PlayserStop()
    {
        /// 이전 애니 정지
        playerAnitor.StopPlayback();
        playerAnitor.speed = 0;
        /// 1. 플레이어 걷기 멈추기
        if (playerAnitor.transform.parent.parent.gameObject.activeSelf)
        {
            //playerAnitor.Play("Idle", -1, 0f);
            playerAnitor.Play("Player_Attack", -1, 0f);
        }
        AudioManager.instance.StopAudio("SE");
        isWalking = false;
        /// 2. 배경 정지
        bgm.isBGmovigPause = true;
        pm.PetAnimStop(true);
    }

    /// <summary>
    /// DOMove 완료되면 자동 공격
    /// </summary>
    void CallBackEnemyAttack()
    {
        /// 1+2. 걷기 멈추고 배경 정지
        PlayserStop();
        /// 3. 자동 공격 시작 -> 공격 히트 시 이벤트는 PlayerControllet.PlayerAttack()
        if (playerAnitor.transform.parent.parent.gameObject.activeSelf)
        {
            playerAnitor.speed = PlayerPrefsManager.isEnterTheMine ? 0 : PlayerInventory.Player_Attack_Speed;
            playerAnitor.Play("Player_Attack", -1, 0f);
        }
    }

    /// <summary>
    /// HBM에서 땡겨갈 지금 소환된 에너미에서 컨트롤러 추출
    /// </summary>
    /// <returns></returns>
    public EnemyController GetEnemyScript()
    {
        if (enemyTransform == null) return null;
        return enemyTransform.GetComponent<EnemyController>();
    }

    public void StopPlayer()
    {
        /// 이전 애니 정지
        playerAnitor.StopPlayback();
        playerAnitor.speed = 0;
        //playerAnitor.Play("Idle", -1, 0f);
        playerAnitor.Play("Player_Attack", -1, 0f);
        /// 딜레이 주고 시작
        Invoke(nameof(InvoStopPlayer), 0.02f);
    }

    public void CaveCancelPlayer()
    {
        /// 현재 잡고 있는 몹이 있으면 하지마
        if (enemyTransform != null) return;
        /// 이전 애니 정지
        playerAnitor.StopPlayback();
        playerAnitor.speed = 0;
        //playerAnitor.Play("Idle", -1, 0f);
        playerAnitor.Play("Player_Attack", -1, 0f);
        /// 딜레이 주고 시작
        Invoke(nameof(InvoStopPlayer), 0.02f);
    }

    void InvoStopPlayer()
    {
        playerAnitor.speed = PlayerInventory.Player_Move_Speed;
        //Debug.LogError("공속 : " + playerAnitor.speed);
        playerAnitor.Play("Player_Move", -1, 0f);
        if (!isWalking)
        {
            //AudioManager.instance.PlayAudio("Walk", "SE");
            isWalking = true;
        }
        /// 배경 흘러감
        bgm.isBGmovigPause = false;
        pm.PetAnimStop(false);

        if (enemyTransform != null)
        {
            enemyTransform.GetComponent<EnemyController>().InvoDestroy();
            enemyTransform = null;
        }
        // 바로 생성
        InitEnemy();
    }

    /// <summary>
    /// 멈칫.
    /// </summary>
    public void SwampStopPlayer()
    {
        if (enemyTransform != null)
        {
            enemyTransform.GetComponent<EnemyController>().InvoDestroy();
            enemyTransform = null;
        }
        /// 공격 정지 화면 정지
        PlayserStop();
    }

    /// <summary>
    /// 현재 몬스터 날려버리고 늪지 몬스터 소환
    /// </summary>
    public void SwampStart()
    {
        /// 이전 애니 정지
        playerAnitor.StopPlayback();
        playerAnitor.speed = 0;
        //playerAnitor.Play("Idle", -1, 0f);
        playerAnitor.Play("Player_Attack", -1, 0f);
        /// 딜레이 주고 시작
        Invoke(nameof(InvoSwampStart), 0.02f);
    }

    public void SwampDelay()
    {
        /// 이전 애니 정지
        playerAnitor.StopPlayback();
        playerAnitor.speed = 0;
        //playerAnitor.Play("Idle", -1, 0f);
        playerAnitor.Play("Player_Attack", -1, 0f);
        /// 몹 날려버리고
        if (enemyTransform != null)
        {
            enemyTransform.GetComponent<EnemyController>().InvoDestroy();
            enemyTransform = null;
        }
        /// 배경 흘러감
        bgm.isBGmovigPause = false;
        pm.PetAnimStop(false);

        playerAnitor.speed = PlayerInventory.Player_Move_Speed;
        playerAnitor.Play("Player_Move", -1, 0f);
    }

    void InvoSwampStart()
    {
        /// TODO : 선택한 스테이지 몇?
        playerAnitor.speed = PlayerInventory.Player_Move_Speed;
        Debug.LogError("늪지 공속 : " + playerAnitor.speed);
        playerAnitor.Play("Player_Move", -1, 0f);
        if (!isWalking)
        {
            //AudioManager.instance.PlayAudio("Walk", "SE");
            isWalking = true;
        }
        /// 흘러감
        bgm.isBGmovigPause = false;
        pm.PetAnimStop(false);

        if (enemyTransform != null)
        {
            enemyTransform.GetComponent<EnemyController>().InvoDestroy();
            enemyTransform = null;
        }
        /// 시간 초과 아직 아님. -> 몹 생성.
        if (!PlayerPrefsManager.isSwampTimeOver)
        {
            /// 늪지 몬스터 소환 
            Enemy_Hp_Full = ListModel.Instance.swampCaveData[PlayerPrefsManager.currentMyStage].monsterHP;          /// 일반몹
            Enemy_DropGold = -1;
            /// 조건 대로 소환
            /// 스폰 에너미
            int randomseed = Random.Range(0, 10);
            SpawEnemyBase(enemyBoss[randomseed]);
            //SpawEnemyBase(enemyNormal[0]);
            /// 무브 에너미
            enemyTransform.DOMove(EnemyTargetPos.position, 1f / PlayerInventory.Player_Move_Speed)
                .OnComplete(CallBackEnemyAttack).SetDelay(1f / PlayerInventory.Player_Move_Speed);

        }

    }
}
