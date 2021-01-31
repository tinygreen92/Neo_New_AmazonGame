using System;
using UnityEngine;
using UnityEngine.Purchasing;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

///
///     <캐릭터_스테이터스> <캐릭터_보유_재화> <캐릭터_기록용_변수(title_Statistics)>
/// 
///     get/set 을 통해 유효 데이터 제어하장
///     Newtonsoft.Json 사용 -> JArray, JObject
///     https://devstarsj.github.io/development/2016/06/11/CSharp.NewtonJSON/
/// 
public static class PlayerInventory
{
    #region 플레이어 스탯 

    // boss_
    public static ObscuredFloat boss_Time { get { return 30.1f; } }                                                                                               // 보스 잔류 시간
    public static ObscuredDouble weapon_equiped_power { get { return ListModel.Instance.CurrentEquiped * 0.01d; } }                                                                    // 무기 장착 공격력
    public static ObscuredDouble character_DPS
    {
        get { return 1.0d + Character_lv(0); }
    }                                                                               // 캐릭터 공격력
    public static ObscuredDouble character_HP
    {
        get { return 100.0d + ((Character_lv(0)) * 10.0d); }
    }                                                                         // 캐릭터 체력
    /// <summary>
    /// 
    /// </summary>
    public static ObscuredDouble stat_power
    {
        get { return Character_lv(1) * ListModel.Instance.charatorList[1].powerPer; }
    }                                                                       // 스탯 공격력
    public static ObscuredDouble stat_attack_speed
    {
        get { return Character_lv(2) == 0 ? 60.0d : 60.0d + (Character_lv(2) * ListModel.Instance.charatorList[2].powerPer); }
    }                                                                       // 스탯 공격속도
    public static ObscuredDouble stat_move_speed
    {
        get { return Character_lv(3) == 0 ? 90.0d : 90.0d + (Character_lv(3) * ListModel.Instance.charatorList[3].powerPer); }
    }                                                                           // 스탯 이동속도
    public static ObscuredDouble stat_cri_multi
    {
        get { return Character_lv(4) * ListModel.Instance.charatorList[4].powerPer; }
    }                                                                      // 스탯 치명타 확률
    public static ObscuredDouble stat_cri_dps
    {
        get { return Character_lv(5) * ListModel.Instance.charatorList[5].powerPer; }
    }                                                                                  // 스탯 치명타 대미지
    //public static ObscuredDouble stat_gold_earned
    //{
    //    get { return Character_lv(6) * ListModel.Instance.charatorList[6].powerPer; }
    //}                                                                          // 스탯 골드 획득량
    //public static ObscuredDouble stat_leaf_earned
    //{
    //    get { return Character_lv(7) * ListModel.Instance.charatorList[7].powerPer; }
    //}                                                             // 스탯 나뭇잎 획득량 증가
    /// <summary>
    /// 유물 해금 된 만큼만 본인의 imgIndex 저장해서, 0이 아닐때,  각 자리의 능력치 받아오기
    /// </summary>
    public static int[] heartIndexs = new int[30];
    /// <summary>
    /// 
    /// </summary>
    public static ObscuredDouble heart_equiped_attack_power
    {
        get { return heartIndexs[0] != 0 ? ListModel.Instance.heartList[heartIndexs[0]-1].powerToLvUP * Heart_lv(0) *0.01d : 0; }
    }                                                                       // 유물 자체 공격력 증가
    public static ObscuredDouble heart_equiped_attack_speed
    {
        get { return heartIndexs[1] != 0 ? ListModel.Instance.heartList[heartIndexs[1] - 1].powerToLvUP * Heart_lv(1) * 0.01d : 0; }
    }                                                            // 유물 공격속도
    public static ObscuredDouble heart_equiped_move_speed
    {
        get { return heartIndexs[2] != 0 ? ListModel.Instance.heartList[heartIndexs[2] - 1].powerToLvUP * Heart_lv(2) * 0.01d : 0; }
    }                                                                // 유물 이동속도
    public static ObscuredDouble heart_equiped_cri_multi
    {
        get { return heartIndexs[3] != 0 ? ListModel.Instance.heartList[heartIndexs[3] - 1].powerToLvUP * Heart_lv(3) * 0.01d : 0; }
    }                                                       // 유물 치명타 확률
    public static ObscuredDouble heart_equiped_cri_power
    {
        get { return heartIndexs[4] != 0 ? ListModel.Instance.heartList[heartIndexs[4] - 1].powerToLvUP * Heart_lv(4) * 0.01d : 0; }
    }                                                                 // 유물 치명타 대미지
    public static ObscuredDouble heart_equiped_monster_normal_HP
    {
        get { return heartIndexs[5] != 0 ? ListModel.Instance.heartList[heartIndexs[5] - 1].powerToLvUP * Heart_lv(5) * 0.01d : 0; }
    }                                                 // 유물 몬스터 HP 감소
    public static ObscuredDouble heart_equiped_monster_boss_HP
    {
        get { return heartIndexs[6] != 0 ? ListModel.Instance.heartList[heartIndexs[6] - 1].powerToLvUP * Heart_lv(6) * 0.01d : 0; }
    }                                               // 유물 보스 HP 감소
    public static ObscuredDouble heart_equiped_superbox_gold_earned
    {
        get { return heartIndexs[7] != 0 ? ListModel.Instance.heartList[heartIndexs[7] - 1].powerToLvUP * Heart_lv(7) * 0.01d : 0; }
    }                                                // 유물 황금상자 골드 획득 증가
    public static ObscuredDouble heart_equiped_superbox_encounter
    {
        get { return heartIndexs[8] != 0 ? ListModel.Instance.heartList[heartIndexs[8] - 1].powerToLvUP * Heart_lv(8) * 0.01d : 0; }
    }                                                 //  유물 황금상자 출현 확률 증가
    public static ObscuredDouble heart_equiped_soozip_powerup_gold
    {
        get { return heartIndexs[9] != 0 ? ListModel.Instance.heartList[heartIndexs[9] - 1].powerToLvUP * Heart_lv(9) * 0.01d : 0; }
    }                                                   //  유물 수집 강화 비용 감소
    public static ObscuredDouble heart_equiped_soozip_gold_earned
    {
        get { return heartIndexs[10] != 0 ? ListModel.Instance.heartList[heartIndexs[10] - 1].powerToLvUP * Heart_lv(10) * 0.01d : 0; }
    }                                                   //  유물 수집 보상 증가
    public static ObscuredDouble heart_equiped_soozip_time
    {
        get { return heartIndexs[11] != 0 ? ListModel.Instance.heartList[heartIndexs[11] - 1].powerToLvUP * Heart_lv(11) : 0; }
    }                                                     //  유물 수집 시간 감소
    public static ObscuredDouble heart_equiped_leaf_earned
    {
        get { return heartIndexs[12] != 0 ? ListModel.Instance.heartList[heartIndexs[12] - 1].powerToLvUP * Heart_lv(12) * 0.01d : 0; }
    }                                                           // 유물 나뭇잎 획득량 증가
    public static ObscuredDouble heart_equiped_leaf_cost
    {
        get { return heartIndexs[13] != 0 ? ListModel.Instance.heartList[heartIndexs[13] - 1].powerToLvUP * Heart_lv(13) * 0.01d : 0; }
    }                                                           //  유물 나뭇잎 비용 감소
    public static ObscuredDouble heart_equiped_enchantStone_earned
    {
        get { return heartIndexs[14] != 0 ? ListModel.Instance.heartList[heartIndexs[14] - 1].powerToLvUP * Heart_lv(14) * 0.01d : 0; }
    }                                                   //  유물 강화석 획득 증가
    public static ObscuredDouble heart_equiped_enchantStone_cost
    {
        get { return heartIndexs[15] != 0 ? ListModel.Instance.heartList[heartIndexs[15] - 1].powerToLvUP * Heart_lv(15) * 0.01d : 0; }
    }                                                 //  유물 강화석 비용 감소
    public static ObscuredDouble heart_equiped_cave_time
    {
        get { return heartIndexs[16] != 0 ? ListModel.Instance.heartList[heartIndexs[16] - 1].powerToLvUP * Heart_lv(16) : 0; }
    }                                                                 //  유물 숨겨진 동굴 시간 증가
    public static ObscuredDouble heart_equiped_gold_cost
    {
        get { return heartIndexs[17] != 0 ? ListModel.Instance.heartList[heartIndexs[17] - 1].powerToLvUP * Heart_lv(17) * 0.01d : 0; }
    }                                                           // 유물 골드 강화비용 감소
    public static ObscuredDouble heart_equiped_weapon_power
    {
        get { return heartIndexs[18] != 0 ? ListModel.Instance.heartList[heartIndexs[18] - 1].powerToLvUP * Heart_lv(18) * 0.01d : 0; }
    }                                                           // 유물 보유 무기 공격력 증가
    public static ObscuredDouble heart_equiped_hp_per
    {
        get { return heartIndexs[19] != 0 ? ListModel.Instance.heartList[heartIndexs[19] - 1].powerToLvUP * Heart_lv(19) * 0.01d : 0; }
    }                                                              // 유물 체력 %
    public static ObscuredDouble heart_equiped_fever_time
    {
        get { return heartIndexs[20] != 0 ? ListModel.Instance.heartList[heartIndexs[20] - 1].powerToLvUP * Heart_lv(20) : 0; }
    }                                                 //  유물 피버 지속시간 증가
    public static ObscuredInt heart_equiped_fever_kill_cost
    {
        get { return heartIndexs[21] != 0 ? (ObscuredInt)ListModel.Instance.heartList[heartIndexs[21] - 1].powerToLvUP * Heart_lv(21) : 0; }
    }                                                 //  유물 피버 게이지 감소
    public static ObscuredDouble heart_equiped_fever_power
    {
        get { return heartIndexs[22] != 0 ? ListModel.Instance.heartList[heartIndexs[22] - 1].powerToLvUP * Heart_lv(22) * 0.01d : 0; }
    }                                               //  유물 피버 공격력 증가
    public static ObscuredDouble heart_equiped_fever_attack_speed
    {
        get { return heartIndexs[23] != 0 ? ListModel.Instance.heartList[heartIndexs[23] - 1].powerToLvUP * Heart_lv(23) * 0.01d : 0; }
    }                                                 //  유물 피버 공격속도 증가
    public static ObscuredDouble heart_equiped_fever_move_speed
    {
        get { return heartIndexs[24] != 0 ? ListModel.Instance.heartList[heartIndexs[24] - 1].powerToLvUP * Heart_lv(24) * 0.01d : 0; }
    }                                               //  유물 피버 이동속도 증가
    public static ObscuredDouble heart_equiped_weapon_Lv_plus
    {
        get { return heartIndexs[25] != 0 ? ListModel.Instance.heartList[heartIndexs[25] - 1].powerToLvUP * Heart_lv(25) : 0; }
    }                                               //  유물 무기 레벨 증가
    public static ObscuredDouble heart_equiped_amazonpoint_cost
    {
        get { return heartIndexs[26] != 0 ? ListModel.Instance.heartList[heartIndexs[26]- 1].powerToLvUP * Heart_lv(26) * 0.01d : 0; }
    }                                                   //  유물 아마존 포인트 요구치 감소
    public static ObscuredDouble heart_equiped_amazonpoint_earned
    {
        get { return heartIndexs[27] != 0 ? ListModel.Instance.heartList[heartIndexs[27] - 1].powerToLvUP * Heart_lv(27) * 0.01d : 0; }
    }                                                 //  유물 아마존 포인트 획득확률 증가
    public static ObscuredDouble heart_equiped_offline_earned
    {
        get { return heartIndexs[28] != 0 ? Heart_lv(28) * 0.01d : 0; }
    }                                                             //  유물 오프라인 보상 증가
    public static ObscuredDouble heart_equiped_offline_time
    {
        get { return heartIndexs[29] != 0 ? ListModel.Instance.heartList[heartIndexs[29] - 1].powerToLvUP * Heart_lv(29) * 0.01d : 0; }
    }                                                               //  유물 오프라인 시간 증가
    /// <summary>
    /// 착용된 룬 수치 저장용
    /// </summary>
    public static ObscuredDouble[] RuneStat = new ObscuredDouble[12];
    /// <summary>
    /// 
    /// </summary>
    public static ObscuredDouble rune_equiped_power
    {
        get { return RuneStat[0] * 0.01d; }
    }                                                                       // 룬 장착 공격력
    public static ObscuredDouble rune_equiped_attack_speed
    {
        get { return RuneStat[1] * 0.01d; }
    }                                                  // 룬 공격속도
    public static ObscuredDouble rune_equiped_move_speed
    {
        get { return RuneStat[2] * 0.01d; }
    }                                                  // 룬 이동속도 
    public static ObscuredDouble rune_equiped_cri_multi
    {
        get { return RuneStat[3] * 0.01d; }
    }                                                            // 룬 치명타 확률
    public static ObscuredDouble rune_equiped_cri_power
    {
        get { return RuneStat[4] * 0.01d; }
    }                                                      // 룬 치명타 대미지
    public static ObscuredDouble rune_equiped_gold_earned
    {
        get { return RuneStat[5] * 0.01d; }
    }                                              // 룬 골드 획득량 증가
    public static ObscuredDouble rune_equiped_leaf_earned
    {
        get { return RuneStat[6] * 0.01d; }
    }                                                                 // 룬 나뭇잎 획득량 증가
    public static ObscuredDouble Rune_equiped_enchantStone_earned
    {
        get { return RuneStat[7] * 0.01d; }
    }                                                 /// 룬 강화석 획득량
    public static ObscuredDouble rune_equiped_soozip_gold_earned
    {
        get { return RuneStat[8] * 0.01d; }
    }                                                 // 룬 퀘스트 수집 보상 증가
    public static ObscuredDouble rune_equiped_hp_per
    {
        get { return RuneStat[9] * 0.01d; }
    }                                                                      // 룬 체력 %
    public static ObscuredDouble Rune_equiped_weapon_power
    {
        get { return RuneStat[10] * 0.01d; }
    }                                                   /// 룬 보유 무기 공격력
    public static ObscuredDouble rune_equiped_offline_earned
    {
        get { return RuneStat[11] * 0.01d; }
    }                                               //  룬 오프라인 보상 증가



    /// <summary>
    /// 
    /// </summary>
    public static ObscuredDouble pet_equiped_power 
    {
        get { return Pet_lv(1) * ListModel.Instance.petList[1].percentDam * 0.01d; }
    }                                                                                // 펫 공격력 버프
    public static ObscuredDouble pet_equiped_attack_speed
    {
        get { return Pet_lv(2) * ListModel.Instance.petList[2].percentDam * 0.01d; }
    }// 펫 공격속도
    public static ObscuredDouble pet_equiped_move_speed
    {
        get { return Pet_lv(3) * ListModel.Instance.petList[3].percentDam * 0.01d; }
    }                                                                   // 펫 이동속도
    public static ObscuredDouble pet_equiped_gold_earned
    {
        get { return Pet_lv(4) * ListModel.Instance.petList[4].percentDam * 0.01d; }
    }                                                                // 펫 골드 획득량 증가
    /// <summary>
    /// 
    /// </summary>
    public static ObscuredDouble buff_power_up = 1.0d;                                                                           // 공격력 버프 100%
    public static ObscuredDouble buff_attack_speed_up = 1.0d;                                                                 // 공격속도 버프 100%
    public static ObscuredDouble buff_move_speed_up = 1.0d;                                                                     // 이동속도 버프 100%
    public static ObscuredDouble buff_gold_earned_up = 1.0d;                                                                           // 골드 획득량 증가 버프 100%

    // 0~7 부터 순서대로
    public static ObscuredBool isbuff_power_up;
    public static ObscuredBool isbuff_attack_speed_up;
    public static ObscuredBool isbuff_gold_earned_up;
    public static ObscuredBool isbuff_move_speed_up;
    // 이건 일시적 버프
    public static ObscuredBool dia_power_up;
    public static ObscuredBool dia_attack_speed_up;
    public static ObscuredBool dia_gold_earned_up;
    public static ObscuredBool dia_move_speed_up;
    //
    public static ObscuredBool ispet_equiped_power;
    public static ObscuredBool ispet_equiped_attack_speed;
    public static ObscuredBool ispet_equiped_move_speed;
    public static ObscuredBool ispet_equiped_gold_earned;

    public static ObscuredString buffStack;


    public static void SetBuffStack(ObscuredInt _index)
    {
        switch (_index)
        {
            case 1: isbuff_power_up = true;
                ListModel.Instance.mvpDataList[0].buff_power_up = 525;
                break;
            case 2: isbuff_attack_speed_up = true; 
                ListModel.Instance.mvpDataList[0].buff_attack_speed_up = 525;
                break;
            case 3: isbuff_move_speed_up = true; 
                ListModel.Instance.mvpDataList[0].buff_move_speed_up = 525;
                break;
            case 4: isbuff_gold_earned_up = true; 
                ListModel.Instance.mvpDataList[0].buff_gold_earned_up = 525;
                break;
            default:
                break;
        }
    }

    //public static string SaveBuffStackStatus()
    //{
    //    string result = "";
    //    if (isbuff_power_up) result += "1*"; else result += "0*";
    //    if (isbuff_attack_speed_up) result += "1*"; else result += "0*";
    //    if (isbuff_gold_earned_up) result += "1*"; else result += "0*";
    //    if (isbuff_move_speed_up) result += "1"; else result += "0";
    //    buffStack = result;

    //    return buffStack;
    //}
    //public static void LoadBuffStackStatus(string _Data)
    //{
    //    string[] sDataList = (_Data).Split('*');
    //    if (sDataList[0] == "1") isbuff_power_up = true; else isbuff_power_up = false;
    //    if (sDataList[1] == "1") isbuff_attack_speed_up = true; else isbuff_attack_speed_up = false;
    //    if (sDataList[2] == "1") isbuff_gold_earned_up = true; else isbuff_gold_earned_up = false;
    //    if (sDataList[3] == "1") isbuff_move_speed_up = true; else isbuff_move_speed_up = false;
    //}




    /// <summary>
    /// 캐릭터 - 스탯 레벨 따오기
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    static ObscuredInt Character_lv(ObscuredInt _index)
    {
        return int.Parse(ListModel.Instance.charatorList[_index].charLevel);
    }  
    /// <summary>
    /// 펫 레벨 따오기
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static ObscuredInt Pet_lv(ObscuredInt _index)
    {
        return int.Parse(ListModel.Instance.petList[_index].petLevel);
    }
    /// <summary>
    /// 유물(Heart) 레벨 따오기
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    static ObscuredInt Heart_lv(ObscuredInt _index)
    {
        return int.Parse(ListModel.Instance.heartList[heartIndexs[_index] - 1].heartLevel);
    }


    /*                                                                                                                                                                              */
    /*                                                                         실제로 쓰이는 스탯                                                                      */
    /*                                                                                                                                                                              */



    /// <summary>
    /// 2. 공격력
    /// 캐릭터 공격력 + 스탯 공격력
    /// * ( 무기 장착 공격력 % + 무기 보유 공격력 % + 유물 공격력 % + 룬 공격력 % + 공격력 버프 100% + 펫 공격력 버프 % )
    /// 
    /// </summary>
    public static ObscuredDouble Player_DPS
    {
        get
        {
            ObscuredDouble tmp = character_DPS + stat_power;

            ObscuredDouble tmp2 = 1.0d
                                    + weapon_equiped_power
                                    + Weapon_owned_power
                                    + heart_equiped_attack_power
                                    + rune_equiped_power;

            ObscuredDouble tmp3 = 1.0d;

            if (isbuff_power_up || dia_power_up) tmp3 += buff_power_up;
            if (ispet_equiped_power) tmp3 += pet_equiped_power;

            if (FeverManager.isFeverTime)
            {
                return tmp * tmp2 * tmp3 * Fever_Power;
            }
            else
            {
                return tmp * tmp2 * tmp3;
            }
        }
    }

    /// <summary>
    /// 3. 보유 무기 공격력 증가
    /// 보유 무기 공격력의 총 합 * ( 유물 보유 무기 공격력 증가% + 룬 보유무기 공격력 증가% )
    /// </summary>
    public static ObscuredDouble Weapon_owned_power
    {
        get
        {
            ObscuredDouble tmp = 0;

            for (ObscuredInt i = 0; i < ListModel.Instance.weaponList.Count; i++)
            {
                if (int.Parse(ListModel.Instance.weaponList[i].weaponLevel) != 0)
                {
                    tmp += 
                        ((double.Parse(ListModel.Instance.weaponList[i].weaponLevel) + Weapon_Lv_Plus - 1.0d) * ListModel.Instance.weaponList[i].increedPower)
                        + ListModel.Instance.weaponList[i].startPower;
                }
            }
            tmp *= 0.1d;

            ObscuredDouble tmp2 = 1.0d
                + heart_equiped_weapon_power
                + Rune_equiped_weapon_power ;

            return tmp * tmp2 * 0.01d;
        }
    }

    /// <summary>
    /// 4.체력
    /// 캐릭터 체력 * ( 유물 체력 % + 룬 체력 증가% )
    /// </summary>
    public static ObscuredDouble Player_HP
    {
        get
        {
            ObscuredDouble tmp = character_HP;

            ObscuredDouble tmp2 = heart_equiped_hp_per
                + rune_equiped_hp_per;

            return tmp * tmp2;
        }
    }

    /// <summary>
    /// 5. 공격속도
    ///  스탯 공격속도 * ( 유물 공격속도 % + 룬 공격속도 % + 공격속도 버프 100% + 펫 공격속도 버프 % )
    /// </summary>
    public static ObscuredFloat Player_Attack_Speed
    {
        get
        {
            var tmp = stat_attack_speed;

            var tmp2 = 1.0d
                + heart_equiped_attack_speed
                + rune_equiped_attack_speed;

            if (isbuff_attack_speed_up || dia_attack_speed_up) tmp2 += buff_attack_speed_up;
            if (ispet_equiped_attack_speed) tmp2 += pet_equiped_attack_speed;

            //Debug.LogWarning("스탯 공속 : " + tmp + "계수 : " + tmp2);

            if (FeverManager.isFeverTime)
            {
                return (ObscuredFloat)((tmp * tmp2 * Fever_Attack_Speed) + 30.0f) *0.01667f;
            }
            else
            {
                /// 60일때 이동속도 1배, 120일때 이동속도 2배
                return (ObscuredFloat)((tmp * tmp2) + 30.0f) * 0.01667f;
            }
        }
    }

    public static ObscuredFloat Player_STAT_Attack_Speed
    {
        get
        {
            var tmp = stat_attack_speed;

            var tmp2 = 1.0d
                + heart_equiped_attack_speed
                + rune_equiped_attack_speed;

            if (isbuff_attack_speed_up || dia_attack_speed_up) tmp2 += buff_attack_speed_up;
            if (ispet_equiped_attack_speed) tmp2 += pet_equiped_attack_speed;

            if (FeverManager.isFeverTime)
            {
                return (ObscuredFloat)((tmp * tmp2 * Fever_Attack_Speed));
            }
            else
            {
                /// 60일때 이동속도 1배, 120일때 이동속도 2배
                return (ObscuredFloat)((tmp * tmp2));
            }
        }
    }



    /// <summary>
    /// 6. 이 동 속 도
    ///  스탯 이동속도 * ( 유물 이동속도 % + 룬 이동속도 % + 이동속도 버프 100% + 펫 이동속도 버프 % )
    /// </summary>
    public static ObscuredFloat Player_Move_Speed
    {
        get
        {
            var tmp = stat_move_speed;

            var tmp2 = 1.0d
                + heart_equiped_move_speed
                + rune_equiped_move_speed;

            if (isbuff_move_speed_up || dia_move_speed_up) tmp2 += buff_move_speed_up;
            if (ispet_equiped_move_speed) tmp2 += pet_equiped_move_speed;

            if (FeverManager.isFeverTime)
            {
                return (ObscuredFloat)((tmp * tmp2 * Fever_Move_Speed) +10f) * 0.01667f;
            }
            else
            {
                return (ObscuredFloat)((tmp * tmp2) + 10f) * 0.01667f;
            }
        }
    }

    public static ObscuredFloat Player_STAT_Move_Speed
    {
        get
        {
            var tmp = stat_move_speed;

            var tmp2 = 1.0d
                + heart_equiped_move_speed
                + rune_equiped_move_speed;

            if (isbuff_move_speed_up || dia_move_speed_up) tmp2 += buff_move_speed_up;
            if (ispet_equiped_move_speed) tmp2 += pet_equiped_move_speed;

            if (FeverManager.isFeverTime)
            {
                return (ObscuredFloat)((tmp * tmp2 * Fever_Move_Speed));
            }
            else
            {
                return (ObscuredFloat)((tmp * tmp2));
            }
        }
    }

    /// <summary>
    /// 7. 치명타 확률
    ///   스탯 치명타 확률 * ( 유물 치명타 확률 + 룬 치명타 확률 )
    /// </summary>
    public static ObscuredFloat Player_Critical_Multiplier
    {
        get
        {
            var tmp = stat_cri_multi;

            var tmp2 = 1.0d + heart_equiped_cri_multi + rune_equiped_cri_multi;
            //Debug.LogWarning("크리 확률 : " + (tmp * tmp2));
            if (FeverManager.isFeverTime)
            {
                return (ObscuredFloat)(tmp * tmp2 * Fever_Critical_Multiplier);
            }
            else
            {
                return (ObscuredFloat)(tmp * tmp2);
            }

        }
    }

    /// <summary>
    /// 8. 치명타 대미지
    /// 스탯 치명타 대미지 * ( 유물 치명타 대미지 + 룬 치명타 대미지 )
    /// </summary>
    public static ObscuredDouble Player_Critical_DPS
    {
        get
        {
            var tmp = 1.0d + (stat_cri_dps * 0.01d);

            var tmp2 = 1.0d + heart_equiped_cri_power + rune_equiped_cri_power;

            if (FeverManager.isFeverTime)
            {
                return Player_DPS * (tmp * tmp2 * Fever_Critical_DPS);
            }
            else
            {
                return Player_DPS * (tmp * tmp2);

            }

        }
    }

    /// <summary>
    /// 스탯상으로 표기할 퍼센테이지
    /// </summary>
    public static ObscuredDouble Player_STAT_Critical_DPS
    {
        get
        {
            var tmp = 1.0d +(stat_cri_dps * 0.01d);

            var tmp2 = 1.0d + heart_equiped_cri_power + rune_equiped_cri_power;

            if (FeverManager.isFeverTime)
            {
                return  tmp * tmp2 * Fever_Critical_DPS;
            }
            else
            {
                return  tmp * tmp2;

            }

        }
    }

    /// <summary>
    /// 9. 골드 획득량 증가
    /// 스탯 골드 획득량 증가 * ( 룬 골드 획득량 증가 + 골드 버프 100% + 펫 골드 획득량 증가 버프 % )
    /// </summary>
    public static ObscuredDouble Player_Gold_Earned
    {
        get
        {
            //var tmp = 1.0d + stat_gold_earned;

            var tmp2 = 1.0d + rune_equiped_gold_earned;

            if (isbuff_gold_earned_up || dia_gold_earned_up) tmp2 += buff_gold_earned_up;
            if (ispet_equiped_gold_earned) tmp2 += pet_equiped_gold_earned;
            return tmp2;
        }
    }

    /// <summary>
    /// 10. 나뭇잎 획득량
    ///  스탯 나뭇잎 획득량 증가 * ( 유물 나뭇잎 획득량 증가 + 룬 나뭇잎 획득량 증가 )
    /// </summary>
    public static ObscuredDouble Player_Leaf_Earned
    {
        get
        {
            /// 얘는 1.00 이상이어야한다
            ObscuredDouble tmp2 = 1.0d
                + heart_equiped_leaf_earned
                + rune_equiped_leaf_earned;

            return tmp2;
        }
    }

    /// <summary>
    /// 11. 몬스터 HP 감소
    /// 해당 스테이지의 몬스터 HP - 유물 몬스터 HP 감소 %
    /// </summary>
    public static ObscuredDouble Monster_Normal_HP
    {
        get
        {
            return (1.0d - heart_equiped_monster_normal_HP);
        }
    }

    /// <summary>
    /// 12. 보스 HP 감소
    /// 해당 스테이지의 보스 몬스터 HP - 유물 보스몬스터 HP 감소 %
    /// </summary>
    public static ObscuredDouble Monster_Boss_HP
    {
        get
        {
            return 1.0d - heart_equiped_monster_boss_HP;
        }
    }

    /// <summary>
    /// 13. 황금상자 골드 획득
    ///  황금상자 골드 획득률 500% * 유물 미믹 골드 획득 증가 %
    /// </summary>
    public static ObscuredDouble Superbox_Gold_Earned
    {
        get
        {
            return 5.0d * (1.0d + heart_equiped_superbox_gold_earned);
        }
    }

    /// <summary>
    /// 14. 황금상자 출현 확률
    /// 황금상자 출현 확률 1% * 유물 미믹 출현 확률 증가 %
    /// </summary>
    public static ObscuredDouble Superbox_Encounter
    {
        get
        {
            if (PlayerPrefsManager.isGoldenDouble)
            {
                return (90.0d * (1.0d + heart_equiped_superbox_encounter));
            }
            else
            {
                return (3.0d * (1.0d + heart_equiped_superbox_encounter));
            }
        }
    }

    /// <summary>
    /// 15. 퀘스트 강화 비용 감소
    /// 퀘스트 강화 비용 - 유물 퀘스트 강화 비용 감소 %
    /// </summary>
    public static ObscuredDouble Soozip_Powerup_Gold
    {
        get
        {
            return 1.0d - heart_equiped_soozip_powerup_gold;
        }
    }

    public static ObscuredDouble STAT_Soozip_Powerup_Gold
    {
        get
        {
            return heart_equiped_soozip_powerup_gold;
        }
    }

    /// <summary>
    /// 16. 퀘스트 보상 증가
    /// 퀘스트 보상 * ( 유물 퀘스트 보상 증가 % + 룬 퀘스트 보상 증가 % )
    /// </summary>
    public static ObscuredDouble Soozip_Gold_Earned
    {
        get
        {
            var tmp2 = heart_equiped_soozip_gold_earned
                + rune_equiped_soozip_gold_earned;

            return 1.0d + tmp2;
        }
    }

    /// <summary>
    /// 17. 퀘스트 시간 감소
    /// 퀘스트 시간 - 유물 퀘스트 시간 감소
    /// </summary>
    public static ObscuredDouble Soozip_Time
    {
        get
        {
            return heart_equiped_soozip_time;
        }
    }

    /// <summary>
    /// 18. 숨겨진 늪지 동굴 시간 증가
    /// 시작값 30초 + 유물 숨겨진 동굴 시간 증가
    /// </summary>
    public static ObscuredDouble Cave_Time
    {
        get
        {
            return (30.0d + heart_equiped_cave_time);
        }
    }

    /// <summary>
    /// 19. 골드 강화 비용 감소	
    /// 골드 강화 비용 - 유물 골드 강화 비용 감소 
    /// </summary>
    public static ObscuredDouble Gold_Cost
    {
        get
        {
            return 1.0d - heart_equiped_gold_cost;
        }
    }

    public static ObscuredDouble STAT_Gold_Cost
    {
        get
        {
            return heart_equiped_gold_cost;
        }
    }

    /// <summary>
    /// 20. 강화석 강화 비용 감소	
    /// 강화석 강화 비용 - 유물 강화석 강화 비용 감소
    /// </summary>
    public static ObscuredDouble EnchantStone_Cost
    {
        get
        {
            return 1.0d - heart_equiped_enchantStone_cost;
        }
    }

    public static ObscuredDouble STAT_EnchantStone_Cost
    {
        get
        {
            return heart_equiped_enchantStone_cost;
        }
    }

    /// <summary>
    /// 21. 나뭇잎 강화 비용 감소	
    /// 강화석 강화 비용 - 유물 강화석 강화 비용 감소
    /// </summary>
    public static ObscuredDouble Leaf_Cost
    {
        get
        {
            return 1.0d - heart_equiped_leaf_cost;
        }
    }

    public static ObscuredDouble STAT_Leaf_Cost
    {
        get
        {
            return heart_equiped_leaf_cost;
        }
    }

    /// <summary>
    /// 22. 피버타임 지속 시간 증가	
    /// 시작값 5초 + 유물 피버타임 지속시간 증가
    /// </summary>
    public static ObscuredDouble Fever_Time
    {
        get
        {
            return (5.0d + heart_equiped_fever_time);
        }
    }

    /// <summary>
    /// 23. 피버타임 게이지 감소	
    /// 시작값 몬스터 1000회 처치 - 유물 피버타임 요구 처치수 감소
    /// </summary>
    public static ObscuredFloat Kill_Cost
    {
        get
        {
            return (1000 - heart_equiped_fever_kill_cost);
        }
    }

    /// <summary>
    /// 24. 피버타임 공격력 증가	
    /// 시작값 50% ( 1.5배 ) * 유물 피버타임 공격력 증가
    /// </summary>
    public static ObscuredDouble Fever_Power
    {
        get
        {
            return 1.5d * (1.0d + heart_equiped_fever_power);
        }
    }

    /// <summary>
    /// 25. 피버타임 공격속도 증가 
    /// 시작값 50% ( 1.5배 ) * 유물 피버타임 공격력 증가
    /// </summary>
    public static ObscuredDouble Fever_Attack_Speed
    {
        get
        {
            return 1.5d * (1.0d + heart_equiped_fever_attack_speed);
        }
    }

    /// <summary>
    /// 26. 피버타임 이동속도 증가 
    /// 시작값 50% ( 1.5배 ) * 유물 피버타임 이동속도 증가
    /// </summary>
    public static ObscuredDouble Fever_Move_Speed
    {
        get
        {
            return 1.5d * (1.0d + heart_equiped_fever_move_speed);
        }
    }

    /// <summary>
    /// 27. 무기 레벨 증가 
    /// 보유중인 무기의 레벨 + 유물 무기레벨 증가
    /// </summary>
    public static ObscuredDouble Weapon_Lv_Plus
    {
        get
        {
            return (heart_equiped_weapon_Lv_plus);
        }
    }

    /// <summary>
    /// 28. 아마존 결정 조각 획득 확률 증가 
    /// 시작값 5% + 유물 아마존 포인트 획득 확률 증가
    /// </summary>
    public static ObscuredDouble AmazonPoint_Earned
    {
        get
        {
            return 5.0d +heart_equiped_amazonpoint_earned;
        }
    }

    /// <summary>
    /// 29. 아마존 결정 조각 요구치 감소 
    /// 필요 아마존 요구 결정 조각 - 유물 아마존 결정 조각 요구치 감소
    /// </summary>
    public static ObscuredDouble AmazonPoint_Cost
    {
        get
        {
            return (1.0d - heart_equiped_amazonpoint_cost);
        }
    }

    public static ObscuredDouble STAT_AmazonPoint_Cost
    {
        get
        {
            return (heart_equiped_amazonpoint_cost);
        }
    }

    /// <summary>
    /// 30. 오프라인 보상 증가 
    /// 오프라인 보상* (유물 오프라인 보상 증가 + 룬 오프라인 보상 증가% )
    /// </summary>
    public static ObscuredDouble Offline_Earned
    {
        get
        {
            /// 210126 유물 보상 5% -> 1% 너프
            if (heart_equiped_offline_earned != 0)
            {
                ListModel.Instance.Heart_myNeaf(heartIndexs[28] - 1, 1f);
            }

            return 1.0d +  (heart_equiped_offline_earned + rune_equiped_offline_earned);
        }
    }



    /// <summary>
    /// 31. 오프라인 시간 증가 
    /// 오프라인 시간 240분(14400초) + 유물 오프라인 시간 증가
    /// </summary>
    public static ObscuredDouble Offline_Time
    {
        get
        {
            return 14400.0d * (1.0d +  heart_equiped_offline_time);
        }
    }

    /// <summary>
    /// 32. 강화석 획득량 증가  heart_equiped_enchantStone_earned
    /// 강화석 획득량* 룬 강화석 획득량 증가
    /// </summary>
    public static ObscuredDouble EnchantStone_Earned
    {
        get
        {
            return 1.0d + (heart_equiped_enchantStone_earned + Rune_equiped_enchantStone_earned);
        }
    }
    /// <summary>
    /// 나뭇잎 뽑기권에 쓰일 공식
    /// </summary>
    public static ObscuredDouble RandomLeaf_Earned
    {
        get
        {
            //var tmp = 1.0f + (stat_leaf_earned * 0.01f);

            /// 얘는 1.00 이상이어야한다
            var tmp2 = 1.0f
                + heart_equiped_leaf_earned
                + rune_equiped_leaf_earned;

            return  tmp2;
        }
    }

    /// <summary>
    /// 33. 피버타임 치명타 확률 증가
    /// 시작값 50% ( 1.5배 )
    /// </summary>
    public static ObscuredDouble Fever_Critical_Multiplier
    {
        get
        {
            return 1.5d;
        }
    }

    /// <summary>
    /// 33. 피버타임 치명타 피해량 증가
    /// 시작값 50% ( 1.5배 )
    /// </summary>
    public static ObscuredDouble Fever_Critical_DPS
    {
        get
        {
            return 1.5d;
        }
    }





    #endregion

    #region 플레이어 재화 

    public static ObscuredInt box_Coupon;             // 무기 상자 뽑기권
    public static ObscuredInt box_E;             // 무기 상자 
    public static ObscuredInt box_D;             // 무기 상자 
    public static ObscuredInt box_C;             // 무기 상자 
    public static ObscuredInt box_B;             // 무기 상자 
    public static ObscuredInt box_A;             // 무기 상자 
    public static ObscuredInt box_S;             // 무기 상자 
    public static ObscuredInt box_L;             // 무기 상자 

    public static ObscuredInt ticket_reinforce_box;             // 강화석 묶음
    public static ObscuredInt ticket_leaf_box;             // 나뭇잎 묶음
    public static ObscuredInt ticket_pvp_enter;             // 결투 입장권
    public static ObscuredInt ticket_cave_enter;             // 늪지 동굴 입장권
    public static ObscuredInt ticket_cave_clear;             // 늪지 동굴 소탕권
    /// <summary>
    /// 대박 나뭇잎 묶음에서 아마존 코인으로 변경
    /// </summary>
    public static ObscuredInt S_leaf_box;             // 대박 나뭇잎 묶음에서 아마존 코인으로 변경
    //
    public static ObscuredInt S_reinforce_box;    // 대박 강화석 묶음
    public static ObscuredInt mining;             // 채굴권
    public static ObscuredInt amber;             // 호박석

    public static ObscuredInt Crazy_dia;             // 대박 다이아 뽑기
    public static ObscuredInt Crazy_elixr;             // 대박 나뭇잎 뽑기


    static ObscuredDouble money_Gold;               // 골드
    static ObscuredLong money_Elixir;             // 엘릭서

    static ObscuredLong money_Dia;                // 다이아
    static ObscuredDouble money_Leaf;               // 나뭇잎
    static ObscuredLong money_EnchantStone;            // 강화석
    static ObscuredLong money_AmazonStone;            // 아마존 결정



    ///--------------------------------  로컬 저장 무료  재화  -------------------------------------///

    public static void SetTicketCount(string _index, ObscuredInt _amount)
    {
        switch (_index)
        {
            case "reinforce_box": ticket_reinforce_box += _amount; break;
            case "leaf_box": ticket_leaf_box += _amount; break;
            case "pvp": ticket_pvp_enter += _amount; break;
            case "cave_enter": ticket_cave_enter += _amount; break;
            case "cave_clear": ticket_cave_clear += _amount; break;
            /// 아마존 포션
            case "S_leaf_box": S_leaf_box += _amount; break;
            //
            case "S_reinforce_box": S_reinforce_box += _amount; break;
            case "mining": mining += _amount; break;
            case "amber": amber += _amount; break;
            //
            case "Crazy_dia": Crazy_dia += _amount; break;
            case "Crazy_elixr": Crazy_elixr += _amount; break;

            default: break;
        }
        /// 화폐 표시기 업데이트 : UpdateBoxs() InvenTicketTextBoxs[]
        MoneyManager.instance.UpdateTicket();
    }


    public static void SetBoxsCount(string _index, ObscuredInt _amount)
    {
        switch (_index)
        {
            case "weapon_coupon":                 box_Coupon+= _amount;                break;
            case "E":                box_E += _amount;                 break;
            case "D":                box_D+= _amount;                 break;
            case "C":                box_C+= _amount;                 break;
            case "B":                box_B+= _amount;                 break;
            case "A":                box_A += _amount;                 break;
            case "S":                box_S+= _amount;                 break;
            case "L":                 box_L+= _amount;                 break;
            default:                break;
        }
        MoneyManager.instance.UpdateBoxs();
    }




    public static ObscuredDouble Money_Gold    {
        get { if (money_Gold > 0) return money_Gold; else return 0; } 
        set
        { 
            money_Gold =  Math.Truncate(value); 
            if (money_Gold > 9.99E+300) money_Gold = 9.99E+300;
            MoneyManager.instance.DisplayGold();
        } 
    }
    public static ObscuredLong Money_Elixir
    {
        get { if (money_Elixir > 0) return money_Elixir; else return 0; }
        set 
        { 
            money_Elixir = value; 
            if (money_Elixir > 2000000000) money_Elixir = 2000000000;
            MoneyManager.instance.DisplayElixir(); 
        }
    }


    public static ObscuredLong Money_Dia
    {
        get { if (money_Dia > 0) return money_Dia; else return 0; }
        set
        {
            money_Dia = value;
            if (money_Dia > 2000000000) money_Dia = 2000000000;
            MoneyManager.instance.DisplayDia();
        }
    }

    /// <summary>
    /// 나뭇잎 상한 해제 
    /// </summary>
    public static ObscuredDouble Money_Leaf    {
        get { if (money_Leaf > 0) return money_Leaf; else return 0; }
        set
        {
            money_Leaf = Math.Truncate(value);
            if (money_Leaf > 9.99E+300) money_Leaf = 9.99E+300;
            MoneyManager.instance.DisplayLeaf();
        }

    }
    public static ObscuredLong Money_EnchantStone {
        get { if (money_EnchantStone > 0) return money_EnchantStone; else return 0; }
        set
        { 
            money_EnchantStone = value; 
            if (money_EnchantStone > 2000000000) money_EnchantStone = 2000000000;
            MoneyManager.instance.DisplayEnchantStone();
        } 
    }
    public static ObscuredLong Money_AmazonCoin
    {
        get { if (money_AmazonStone > 0) return money_AmazonStone; else return 0; }
        set 
        { 
            money_AmazonStone = value; 
            if (money_AmazonStone > 2000000000) money_AmazonStone = 2000000000;
            MoneyManager.instance.DisplayAmazonStone();
        }
    }


    #endregion

    #region 상태정보 저장

    /// <summary>
    /// 광고제거 구매했으면 플레이팹 로딩 중에 이거 활성화
    /// </summary>
    public static ObscuredInt isSuperUser;
    /// <summary>
    /// 튜토리얼 퀘스트에서 쓰일 아무 무기 레벨 10이상 체크용 
    /// </summary>
    public static ObscuredInt MAX_WEAPON_LV;

    static ObscuredDouble recentDistance;                                           // 최근 거리
    public static ObscuredDouble RecentDistance
    {
        get { if (recentDistance > 0) return recentDistance; else return 0; }
        set { recentDistance = Math.Truncate(value); }
    }


    ///-------------------------------- getter/setter  -------------------------------------///
    /// <summary>
    /// 아마존 레벨 단독 저장 -> 경험치 총량도 관여
    /// </summary>    
    public static ObscuredInt CurrentAmaLV;
    /// <summary>
    /// 아마존 포션으로 얻은 경험치에 연결된 본체
    /// </summary>
    static ObscuredLong amazonStoneCount;
    public static ObscuredInt MaxGage { get; set; }
    /// <summary>
    /// 아마존 포션으로 얻은 경험치
    /// </summary>
    public static ObscuredLong AmazonStoneCount
    {
        get 
        {
            return amazonStoneCount;
        }
        set 
        {
            Debug.LogError("호출 어디야");
            amazonStoneCount = value;
            /// 유물 경험치 요구 유물 적용
            MaxGage = (int)Math.Truncate((CurrentAmaLV + 1) * 100d * AmazonPoint_Cost);
            /// TODO : 증가 시켰는데 맥스 요구 보다 높으면 이월 시키고 결정지급 / 게이지 돌파하면 이월 시키고 게이지 증가
            if (amazonStoneCount >= MaxGage && amazonStoneCount != 0)
            {
                if (!ObscuredPrefs.HasKey("isFirstPotion"))
                {
                    Debug.LogError("첫번째 포션 변환 ");
                    ObscuredPrefs.SetInt("isFirstPotion", 525);
                    ObscuredPrefs.Save();
                    /// 포션 인벤토리로 넣어
                    SetTicketCount("S_leaf_box", (int)amazonStoneCount);
                    amazonStoneCount -= amazonStoneCount;
                    /// 게이지 올려
                    ExpManager.instance.UpdateExpGage(MaxGage);
                    return;
                }

                Debug.LogError("호출 어디야" + amazonStoneCount + " / " + MaxGage);

                CurrentAmaLV++;
                amazonStoneCount -= MaxGage;
                Money_AmazonCoin += CurrentAmaLV;
                PopUpManager.instance.ShowGetPop(5, CurrentAmaLV.ToString());
                /// 다시 갱신
                MaxGage = Mathf.CeilToInt(((CurrentAmaLV + 1) * 100 * (float)AmazonPoint_Cost));
                if (!ObscuredPrefs.HasKey("isFirstPotion"))
                {
                    Debug.LogError("없어져라 ");
                    ObscuredPrefs.SetInt("isFirstPotion", 525);
                    ObscuredPrefs.Save();
                }

            }

            /// 게이지 올려
            ExpManager.instance.UpdateExpGage(MaxGage);
            //MoneyManager.instance.DisplayCostZogak();
        }

    }




    #endregion







}
