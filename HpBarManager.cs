using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarManager : MonoBehaviour
{
    public GoldTextPool goldText;
    public GameObject GoldDropPos;
    public CaveManager cm;
    public FeverManager fm;
    [Header("- HP 바 fill Amount 이미지")]
    public Image hp_fill;
    [Header("- 타임 바")]
    public Transform timeBar;
    public Image timeBarFill;
    [Header("- 보스 버튼")]
    public Transform bossBtn;
    public Sprite[] bossSprit;
    [Header("- 코인 드랍 관련")]
    public GameObject enemyPos;
    public GameObject coinCentPos;

    /// <summary>
    /// PlayerController에서 호출해서 어택에니메이션 끝날때 false
    /// </summary>
    public bool isAttatking;

    /// <summary>
    /// true 면 보스 살아 있으므로 9 스테이지에서 거리 증가 X
    /// </summary>
    [HideInInspector]
    public bool isBossAlive;
    /// <summary>
    /// DistanceManager 에서 소환된 트랜스폼에서 땡겨올거야
    /// </summary>
    private EnemyController ecScript;

    /// <summary>
    /// 코인 드랍
    /// </summary>
    public void DropGold()
    {
        CoinDropManager.instance.DropGold(enemyPos, coinCentPos, PlayerPrefsManager.isSuperBox);
    }


    public void DropLeaf()
    {
        CoinDropManager.instance.DropLeaf(enemyPos, coinCentPos);
    }

    public void DropAmaCoin()
    {
        CoinDropManager.instance.DropAmaCoin(enemyPos, coinCentPos);
    }

    public void DropPotion()
    {
        CoinDropManager.instance.DropPotion(enemyPos, coinCentPos);
    }


    public void SubEnemyHP()
    {
        ecScript = DistanceManager.instance.GetEnemyScript();     /// 스크립트 땡겨오고
        if(ecScript != null) ecScript.SubEnemyHP();
    } 

    /// <summary>
    /// 0 혹은 1 => 플레이어 걷기 멈추기
    /// </summary>
    /// <param name="_amount">0 이면 죽었음 / 1이면 풀피임</param>
    public void SetHpBarFill(double _amount)
    {
        hp_fill.fillAmount = (float)_amount;
    }




    /// <summary>
    /// 보스 버튼이 비 활성화 된 상태라면 활성화 후,
    /// true 라면 보스버튼 이미지 색 + 텍스트 변화
    /// </summary>
    /// <param name="_switch"></param>
    public void EnableBossColor(bool _switch)
    {
        if (_switch)
        {
            /// 타임바 활성화
            InitBossTime();
            /// 보스 포기 버튼이 떠있는 상태
            PlayerPrefsManager.isBossBtnAlive = false;
            /// 보스 나오면 오른쪽 비활성화
            PlayerPrefsManager.instance.BlockRightThings(false);
        }
        else
        {
            /// 타임바  비 활성화
            timeBar.gameObject.SetActive(false);
            /// 보스 도전 버튼이 떠있는 상태
            PlayerPrefsManager.isBossBtnAlive = true;
            /// 오른쪽 버튼 풀어주기
            PlayerPrefsManager.instance.BlockRightThings(true);
        }
    }

    /// <summary>
    /// 포기 이미지 활성화
    /// </summary>
    public void InvoGiveUP()
    {
        var reDist = Mathf.RoundToInt((float)PlayerInventory.RecentDistance);
        /// 10 스테이지 보스전이 아닌 상황에서 호출 되면 리턴
        if (reDist % 10 != 0)         
        {
            return;
        }
        bossBtn.GetComponent<Image>().sprite = bossSprit[0];            /// 포기 이미지 활성화
        bossBtn.GetChild(0).GetComponent<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Battle_Scene_BossGiveUP"); ;            /// 포기 텍스트 활성화
        bossBtn.gameObject.SetActive(true);
    }
    /// <summary>
    /// 전투시작 이미지 활성화
    /// </summary>
    void InvoReFight()
    {
        // 2초 안에 늪지에 입장했을 경우 리턴
        if (PlayerPrefsManager.isGoldposOnAir) return;

        var reDist = Mathf.RoundToInt((float)PlayerInventory.RecentDistance);
        /// 9 스테이지 보스전이 아닌 상황에서 호출 되면
        if (reDist % 10 != 9)
        {
            return;
        }
        bossBtn.GetComponent<Image>().sprite = bossSprit[1];            /// 전투시작 이미지 활성화
        bossBtn.GetChild(0).GetComponent<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Battle_Scene_BossBtn"); ;            /// 전투시작 텍스트 활성화
        bossBtn.gameObject.SetActive(true);
    }

    Coroutine bossCo;
    public void InitBossTime()
    {
        timeBarFill.fillAmount = 1.0f;
        timeBar.gameObject.SetActive(true);
        isBossAlive = true;
        if (bossCo != null) StopCoroutine(bossCo);
        bossCo = StartCoroutine(BossTimer());
    }


    float currentTime;
    IEnumerator BossTimer()
    {
        yield return null;

        float Maxcnt = PlayerInventory.boss_Time;
        currentTime = Maxcnt;

        //TimerFillamount.text = "남은 시간 : " + string.Format("{0:f1}", Maxcnt);

        while (currentTime > 0)
        {
            yield return new WaitForFixedUpdate();
            /// 타이머는 보스 살아있을때만 
            if (isBossAlive)
            {
                currentTime -= Time.deltaTime;
                timeBarFill.fillAmount = currentTime / Maxcnt;
            }
        }

        /// 타임오버라면? 보스 증발 시킴
        PlayerInventory.RecentDistance--;
        Debug.LogWarning("타임 오버 보스 증발 : " + PlayerInventory.RecentDistance);
        EnableBossColor(false);
        /// 현재 에너미 날려버림 && 내부에서 새 에너미 생성까지
        DistanceManager.instance.StopPlayer();
        bossBtn.gameObject.SetActive(false);
        /// 전투시작 버튼 활성화.
        Invoke(nameof(InvoReFight), 2.0f);
        //
        if (bossCo != null) StopCoroutine(bossCo);
        bossCo = null;
    }



    /// <summary>
    /// 보스 버튼 클릭시 반응. 
    /// </summary>
    public void ClickedBossBtn()
    {
        /// 포기 이미지 클릭
        if (!PlayerPrefsManager.isBossBtnAlive)
        {
            /// 보스 버튼 비활성화
            bossBtn.gameObject.SetActive(false);
            //
            if (bossCo != null) StopCoroutine(bossCo);
            bossCo = null;
            //
            PlayerInventory.RecentDistance--;
            Debug.LogWarning("포기 이미지 클릭 : " + PlayerInventory.RecentDistance);
            SetHpBarFill(1);
            EnableBossColor(false);
            Invoke(nameof(InvoReFight), 2.0f);
        }
        else    /// 전투시작 이미지 클릭
        {
            /// 보스 버튼 비활성화
            bossBtn.gameObject.SetActive(false);
            //
            if (bossCo != null) StopCoroutine(bossCo);
            bossCo = null;
            //
            PlayerInventory.RecentDistance++;
            Debug.LogWarning("전투시작 이미지 클릭 : " + PlayerInventory.RecentDistance);
        }

        /// 현재 에너미 날려버림 && 내부에서 새 에너미 생성까지
        DistanceManager.instance.StopPlayer();
    }

    /// <summary>
    /// 보스 전 관련  UI 숨김
    /// </summary>
    public void DisableBossFunc()
    {
        if (bossCo != null) StopCoroutine(bossCo);
        bossCo = null;
        bossBtn.gameObject.SetActive(false);
        timeBar.gameObject.SetActive(false);
        /// 보스가 죽었당 늪지 다시 입장
        PlayerPrefsManager.instance.BlockRightThings(true);
    }
    
    /// <summary>
    /// 보스 떠있는데 늪지 들어가면 거리 한칸 없애줌.
    /// </summary>
    public void CheatGetOut()
    {
        if (bossCo != null)
        {
            StopCoroutine(bossCo);
            isBossAlive = false;
            PlayerInventory.RecentDistance--;
        }
        bossCo = null;
        /// 보스가 죽었당 늪지 다시 입장
        PlayerPrefsManager.instance.BlockRightThings(true);
    }


}


