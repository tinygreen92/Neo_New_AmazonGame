using Lean.Transition.Extras;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [Header("-  대미지 폰트")]
    public Transform normalFont;
    public Transform criFont;

    private HpBarManager HBM;
    private Animator anim;

    private double dEnemy_Hp_Full;
    private double dEnemy_Hp_Current;
    private double dEnemy_DropGold;
    /// <summary>
    /// 에너미 젠 할때 트루 해주고
    /// </summary>
    private bool isAliveEnemy;
    private SpriteRenderer spre;

    private bool isFristClone;
    private void OnEnable()
    {
        spre = GetComponent<SpriteRenderer>();
        //
        HBM = GameObject.FindWithTag("HpBarManager").GetComponent<HpBarManager>();
        anim = GetComponent<Animator>();
        if (isFristClone) anim.Play(name + "_Idle", -1, 0f);
        else isFristClone = true;
    }

    public void SetEnemy_Hp_Current(double current)
    {
        dEnemy_Hp_Current -= current;
    }

    public void SetEnemyStat(double _hp, double _gold)
    {
        isAliveEnemy = true;
        dEnemy_Hp_Full = _hp;
        dEnemy_Hp_Current = _hp;
        dEnemy_DropGold = _gold;
        HBM.SetHpBarFill(1);
        /// 보스 죽기 전  화면 이동했으면 보스생존 상태 죽여
        //if(_gold < 0) HBM.isBossAlive = false;
    }

    public void SetBossStat()
    {
        HBM.SetHpBarFill(1);
        /// 타임바 활성화 + 늪지/동굴 입장 버튼 비활성화
        HBM.EnableBossColor(true);
        Invoke(nameof(InvoSetBossStat), 1.5f);
    }

    void InvoSetBossStat()
    {
        /// 포기 버튼 활성화.
        HBM.InvoGiveUP();
    }


    private double playerDPS;
    public void SubEnemyHP()
    {
        if (!isAliveEnemy) return;

        float randomseed = Random.Range(0, 100f);

        if (!PlayerPrefsManager.isIdleModeOn)
        {
            /// TODO : 크리 확률 계산
            if (PlayerInventory.Player_Critical_Multiplier > randomseed)
            {
                playerDPS = PlayerInventory.Player_Critical_DPS;
                criFont.GetComponent<DamageController>().Create(PlayerPrefsManager.instance.topCanvas, playerDPS, true);
            }
            else
            {
                playerDPS = PlayerInventory.Player_DPS;
                normalFont.GetComponent<DamageController>().Create(PlayerPrefsManager.instance.topCanvas, playerDPS, false);
            }
        }
        else
        {
            if (PlayerInventory.Player_Critical_Multiplier > randomseed)
            {
                playerDPS = PlayerInventory.Player_Critical_DPS;
            }
            else
            {
                playerDPS = PlayerInventory.Player_DPS;
            }
        }

        ///  피깎아줌
        dEnemy_Hp_Current -= playerDPS;
        /// TODO : 히트 애니메이션  오브젝트 네임_Hit 
        anim.speed = PlayerInventory.Player_Move_Speed;
        anim.Play(name +"_Hit", -1, 0f);
        /// 사망 처리 여부
        if (dEnemy_Hp_Current <= 0)     /// 죽었다.
        {
            /// 드랍 골드가 있을 경우에만, 사망 처리
            isAliveEnemy = false;
            /// 골드 포스 켜주기
            if (!PlayerPrefsManager.isGoldposOnAir && !HBM.cm.GoldDropPos.activeSelf)
            {
                HBM.cm.GoldDropPos.SetActive(true);
            }
            /// 없으면? 숨겨진 늪지다.
            if (dEnemy_DropGold > 0)
            {
                HBM.SetHpBarFill(0);
                HBM.DropGold();
                /// 근데 방금 죽인게 보스라면?
                if (PlayerInventory.RecentDistance != 0 && IsBossStage() && HBM.isBossAlive)
                {
                    /// 보스 죽었당 스위치
                    HBM.isBossAlive = false;
                    HBM.DisableBossFunc();
                    /// 보스 1회 처치하기
                    if (PlayerPrefsManager.currentTutoIndex == 22) ListModel.Instance.TUTO_Update(22);
                    if (PlayerPrefsManager.currentTutoIndex == 44) ListModel.Instance.TUTO_Update(44);
                    /// 아마존 결정 무조건 획득
                    PlayerInventory.Money_AmazonCoin++;
                    /// 아마존 결정 획득 팝업
                    //PopUpManager.instance.ShowGetPop(0, "1");
                    /// 나뭇잎 100% 드랍
                    HBM.DropLeaf();
                    /// 나뭇잎 기본 획득량 공식 (거리 비례)
                    double currentLeaf = 5.0d * (1.0d + (0.35d * PlayerInventory.RecentDistance));
                    PlayerInventory.Money_Leaf += Mathf.CeilToInt((float)(PlayerInventory.Player_Leaf_Earned * currentLeaf));
                    /// 나뭇잎 획득량 업적 올리기
                    ListModel.Instance.ALLlist_Update(4, Mathf.CeilToInt((float)(PlayerInventory.Player_Leaf_Earned * currentLeaf)));
                    /// 나뭇잎 주사위 굴리기 1% = 10*(1+(0.35*Lv))
                    randomseed = Random.Range(0, 100f);
                    if (randomseed < 10.1f)
                    {
                        /// TODO : 묶음 드랍 확률은 10% + @ 아이템 드랍 확률 추가
                        DropTheBox();
                    }

                }
                /// 몬스터 킬 카운터 +1
                HBM.fm.State_Monster_KillCount++;
                /// 아마존 결정 획득 주사위 굴리기 
                randomseed = Random.Range(0, 100f);
                if (randomseed < PlayerInventory.AmazonPoint_Earned)
                {
                    /// 아마존 결정 조각 1개 확률 획득
                    PlayerInventory.AmazonStoneCount++;
                    /// 결정조각  업적  카운트
                    ListModel.Instance.ALLlist_Update(2, 1);
                    ///// 아마존 결정 조각 1개 획득 팝업
                    //PopUpManager.instance.ShowPopUP(25);
                }
            }
            /// 공격 동작 멈추기
            StartCoroutine(WatingAnimEnd());
            /// 죽음 애니 재생
            anim.Play(name + "_Die", -1, 0f);
        }
        else  /// 안죽었다.
        {
            if (dEnemy_DropGold > 0) HBM.SetHpBarFill(dEnemy_Hp_Current / dEnemy_Hp_Full);
        }
    }

    /// <summary>
    /// 거리에 10을 나누어서 0이 되면 보스 스테이지
    /// </summary>
    /// <returns></returns>
    bool IsBossStage()
    {
        float tmp = (float)PlayerInventory.RecentDistance;
        tmp = Mathf.RoundToInt(tmp) % 10;
        ///
        if(tmp == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 죽음 애니메이션에서 이벤트.
    /// </summary>
    public void WinToEnemy()
    {
        /// ------- 숨겨진 늪지 트리거 ------
        if (dEnemy_DropGold < 0)
        {
            /// 늪지 킬카운터 +1
            HBM.cm.RefleshCurrentKill(++PlayerPrefsManager.swampMonKillCount);
            /// 늪지 뚜벅이
            if (!PlayerPrefsManager.isSwampTimeOver)
            {
                DistanceManager.instance.SwampStart();
            }
            else
            {
                DistanceManager.instance.SwampStopPlayer();
            }
            return;
        }

        /// ------- 일반 스테이지 등반 조건 ------
        PlayerInventory.Money_Gold += dEnemy_DropGold;
        ///  업적 카운트 올리기
        ListModel.Instance.ALLlist_Update(3, dEnemy_DropGold);
        /// 몬스터 5회 처치
        if (!PlayerPrefsManager.isTutoAllClear)
        {
            if (PlayerPrefsManager.currentTutoIndex == 0) ListModel.Instance.TUTO_Update(0);
            if (PlayerPrefsManager.currentTutoIndex == 4) ListModel.Instance.TUTO_Update(4);
            if (PlayerPrefsManager.currentTutoIndex == 8) ListModel.Instance.TUTO_Update(8);
            if (PlayerPrefsManager.currentTutoIndex == 13) ListModel.Instance.TUTO_Update(13);
            if (PlayerPrefsManager.currentTutoIndex == 17) ListModel.Instance.TUTO_Update(17);
            if (PlayerPrefsManager.currentTutoIndex == 35) ListModel.Instance.TUTO_Update(35);
        }

        /// 보스가 살아있지 않을때만 거리 증가.
        if (!HBM.isBossAlive)
        {
            PlayerInventory.RecentDistance++;
            /// 5km 돌파하기 = 50
            if (PlayerPrefsManager.currentTutoIndex == 18) ListModel.Instance.TUTO_Update(18);
            /// 10Km 돌파하기= 100
            if (PlayerPrefsManager.currentTutoIndex == 36) ListModel.Instance.TUTO_Update(36);
            /// 150km 인가?
            if (PlayerPrefsManager.currentTutoIndex == 40) ListModel.Instance.TUTO_Update(40);
        }
        /// 방금 황금박스 죽음?
        if (PlayerPrefsManager.isSuperBox)
        {
            /// 황금상자 처치하기 업적
            if (PlayerPrefsManager.currentTutoIndex == 27) ListModel.Instance.TUTO_Update(27);
            //
            PlayerPrefsManager.isSuperBox = false;
            PlayerPrefsManager.isGoldenDouble = false;

        }
        /// 뚜벅뚜벅이
        DistanceManager.instance.StopPlayer();
    }


    /// <summary>
    /// 강화석 묶음 + 나뭇잎 묶음 = 보스 처치시 10% + @의 확률로 드랍
    /// </summary>
    private void DropTheBox()
    {
        PlayerInventory.SetTicketCount("reinforce_box", 1);
        PlayerInventory.SetTicketCount("leaf_box", 1);
    }

    IEnumerator WatingAnimEnd()
    {
        while (true)
        {
            yield return null;
            if (!HBM.isAttatking) break;
        }
        DistanceManager.instance.playerAnitor.Play("Idle", -1, 0f);

    }



    public void InvoDestroy()
    {
        Lean.Pool.LeanPool.Despawn(gameObject);
    }


}
