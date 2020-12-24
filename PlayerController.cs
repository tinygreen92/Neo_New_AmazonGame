using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public HpBarManager HBM;
    [Header("-에너미 리젠 장소 / 이펙트 표기 레이어")]
    public LeanGameObjectPool effectPool;

    /// <summary>
    /// 공격 애니메이션 재생시 Event로 불러오는 메소드
    /// </summary>
    public void PlayerAttack()
    {
        /// 공속 적용
        DistanceManager.instance.playerAnitor.speed = PlayerPrefsManager.isEnterTheMine? 0 : PlayerInventory.Player_Attack_Speed;
        /// 공격중이다.
        HBM.isAttatking = true;
        /// 몬스터 HP 감소
        HBM.SubEnemyHP();

        if (!PlayerPrefsManager.isIdleModeOn)
        {
            /// 이펙트 효과
            effectPool.Spawn();
            AudioManager.instance.PlayAudio("Attack", "SE");
        }
    }

    public void StopAttack() => HBM.isAttatking = false;


}
