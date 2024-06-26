﻿using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CharatorContent
{
    public string index;                            // 인덱스
    public string charLevel;                    // 현재 레벨
    public string charMaxLevel;             // 만렙
    public float nextUpgradeCost;      // 필요 엘릭서
    public float powerPer;                     // 레벨당 증가치
    public string title;                             // 타이틀
    public string description;                             // 설명
}

[System.Serializable]
public struct WeaponContent
{
    public string index;                            // 인덱스
    public string headRank;                    // 중분류
    public string tailRank;                    // 소분류
    public string weaponLevel;                    // 현재 레벨
    public float startPower;                    ///  초기 장착 공격력 -> 보유 공격력은 여기에 레벨 곱한값의 1/10
    public float increedPower;                    // 레벨업당 공격력 증가치
    public float nextUpgradeCost;      // 업글  강화석
    public float rankUpDia;                     // 다이아 초월 비용
    public float rankUpENstone;                     // 강화석 초월 비용
    public float startPassFail;                     // 초기 강화성공률
    public float passFailPer;                     // 강화 성공 차감
    public string isEnable;                             // 황성화 여부
    public string weaAmount;                             // 보유 무기 갯수
}

[System.Serializable]
public struct HeartContent
{
    public string index;                                // 인덱스
    public string heartLevel;                       // 현재 레벨
    public string maxLevel;                         // 최대 레벨
    public float powerToLvUP;                   // 레벨업 당 증가치
    public double nextUpgradeNeed;     // 나뭇잎 시작값
    public double leafToLvUP;                   // 나뭇잎 증가 공식
    public float dropTable;                         // 뽑기 확률
    public string descHead;                         // 능력치 설명 앞쪽
    public string descTail;                             // 능력치 설명 증가 / 감소
    public string imgIndex;                               // 이미지 인덱스
    public string heartName;                               // 유물 이름
}

[System.Serializable]
public struct SupContent
{
    public string index;                   // 인덱스
    public string supporterLevel;          // 지원 임무 레벨
    public string supporterName;           // 지원 임무 이름
    public float maxTime;                 // 임무 수행 남은 시간
    public double currentEarnGold;         // 현재 골드 수익
    public double nextUpgradeNeed;         // 다음 레벨 업그레이드 비용
    public string isEnable;         // 활성화 여부
}

[System.Serializable]
public struct PetContent
{
    public string index;                   // 인덱스
    public string petLevel;                   // 현재 레벨
    public float needUpgrade;                   /// 레벨업 필요한 나뭇잎 초기값 (1.1.0 이후 사용 X)
    public float usingTimeDam;                   // 대미지 /. 지속시간
    public float percentDam;                   // 대미지 증가량
    public float coolTime;                   // 쿨 - 타임
    public string Desc;                   // 설명
    public string isEnable;                   // 인덱스
}

[System.Serializable]
public struct RuneContent            ///모든 룬 보관함
{
    public string index;                                // 인덱스
    public string imgIndex;                               // 이미지 인덱스
    public string rank;                                     ///  해당 유물의 랭크
    public string desc;                                     // 능력치 설명
    public float rank_1_MIN;                        // 능력치의 최소 최대 0.1 단위
    public float rank_1_MAX;                       // 능력치의 최소 최대 0.1 단위
    public float rank_2_MIN;                        // 능력치의 최소 최대 0.1 단위
    public float rank_2_MAX;                       // 능력치의 최소 최대 0.1 단위
    public float rank_3_MIN;                        // 능력치의 최소 최대 0.1 단위
    public float rank_3_MAX;                       // 능력치의 최소 최대 0.1 단위
    public float rank_4_MIN;                        // 능력치의 최소 최대 0.1 단위
    public float rank_4_MAX;                       // 능력치의 최소 최대 0.1 단위
    public float rank_5_MIN;                        // 능력치의 최소 최대 0.1 단위
    public float rank_5_MAX;                       // 능력치의 최소 최대 0.1 단위
    public string isEquip;                       // 능력치의 최소 최대 0.1 단위
}

[System.Serializable]
public struct RuneInventory                 ///실제 내가 획득한 룬
{
    public string imgIndex;                               // 이미지 인덱스
    public string rank;                                     ///  해당 유물의 랭크
    public string desc_1;                                     // 능력치 설명
    public string desc_2;                                     // 능력치 설명
    public string desc_3;                                     // 능력치 설명
    public string desc_4;                                     // 능력치 설명
    public float main_1;                        // 능력치의 최소 최대 0.1 단위
    public float main_2;                        // 능력치의 최소 최대 0.1 단위
    public float sub_1;                        // 능력치의 최소 최대 0.1 단위
    public float sub_2;                        // 능력치의 최소 최대 0.1 단위
    public string isEquip;
}

[System.Serializable]
public struct ShopPrice                 /// 상점 가격
{
    public string category;    
    public string korDesc;              
    public string korPrice;
    public string engDesc;
    public string engPrice;
    public string korTailDesc;
    public string engTailDesc;
}

[System.Serializable]
public struct MissonSchool                 /// 업적 / 튜토리얼
{
    public string category;
    public string korDesc;
    public string engDesc;
    public string reword;
    public string refreshMulti;
    public string maxValue;
    public string curentValue;
    public string rewordAmount;
}


[System.Serializable]
public struct MineCraft                 /// 광산맵
{
    public string stage;
    public float mine_hp;
    public string reword_es;
    public string reword_ama;
    public string unlockDia;
    public string isEnable;                   // true/false
}

[System.Serializable]
public class MVP                 /// 유료 결제 트리거 보관
{
    public int SuperUser;
    public int buff_power_up;
    public int buff_attack_speed_up;
    public int buff_gold_earned_up;
    public int buff_move_speed_up;
    // 한정 패키지 구매 여부
    public int pack_06;
    public int pack_07;
    public int pack_08;
    public int pack_09;
    public int pack_10;
    // 일간 패키지
    public int daily_10;
    public int daily_11;
    public int daily_12;
    public int daily_13;
    // 주간 패키지
    public int weekend_14;
    public int weekend_15;
    public int weekend_16;
    public int weekend_17;
    // 월간 패키지
    public int mouth_18;
    public int mouth_19;
    public int mouth_20;
    public int mouth_21;
    public int mouth_22;
    public int mouth_23;
    /// 주간/월간 패키지 체크 날짜
    public int weekend_Day;
    public int mouth_Day;

}

[System.Serializable]
public class AxeStat                 /// 저장소 + 곡괭이질 스탯
{
    public long Stack_EnStone;
    public long Stack_AmaCystal;
    public long Stack_Amber;
    public float Axe_Power;
    public float Axe_Speed;
    public float Axe_Skill;
}

[System.Serializable]
public class SwampCave                 /// 숨겨진 늪지
{
    public float stageLevel;
    public float monsterHP;
    public float rewordLeaf;
    public float rewordEnchant;
    public string killCount;
}

[System.Serializable]
public class NonJson                 /// 계속 추가 수정 가능한 돈
{
    public string RecentDistance;
    public string Money_Gold;
    public string Money_Elixir;
    public string Money_AmazonCoin;
    public string AmazonStoneCount;
    ///
    public string CurrentAmaLV;
    public string box_Coupon;
    public string box_E;
    public string box_D;
    public string box_C;
    public string box_B;
    public string box_A;
    public string box_S;
    public string box_L;
    //
    public string ticket_reinforce_box;
    public string ticket_leaf_box;
    //
    public string ticket_pvp_enter;
    public string ticket_cave_enter;
    public string ticket_cave_clear;
    // 
    public string S_reinforce_box;
    /// <summary>
    /// 아마존 포션
    /// </summary>
    public string S_leaf_box;
    //
    public string mining;
    public string amber;
    /// <summary>
    ///  [int] 튜토리얼 클리어 트리거
    /// </summary>
    public int isTutoAllClear;

}


public class ListModel : MonoBehaviour
{

    [Header("- 최근 장착 무기 대미지 배율")]
    public double CurrentEquiped = 0;

    #region 싱글톤
    private static ListModel _instance = null;
    public static ListModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameObject.Find("InfiniteScManager")).AddComponent<ListModel>();
            }

            return _instance;
        }
    }
    #endregion

    public List<PetContent> petList = new List<PetContent>();



    /// <summary>
    /// 펫 레벨업 
    /// </summary>
    public void Pet_LevelUp(int _index, int _level)
    {
        PetContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = petList[_index].index;
        sc.petLevel = _level.ToString();
        sc.needUpgrade = petList[_index].needUpgrade;
        sc.usingTimeDam = petList[_index].usingTimeDam;
        sc.percentDam = petList[_index].percentDam;
        sc.coolTime = petList[_index].coolTime;
        sc.Desc = petList[_index].Desc;
        sc.isEnable = petList[_index].isEnable;

        petList[_index] = sc;
    }


    /// <summary>
    /// 펫 활성화 갱신
    /// </summary>
    /// <param name="_index"></param>
    public void Pet_Unlock(int _index)
    {
        PetContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = petList[_index].index;
        sc.petLevel = petList[_index].petLevel;
        sc.needUpgrade = petList[_index].needUpgrade;
        sc.usingTimeDam = petList[_index].usingTimeDam;
        sc.percentDam = petList[_index].percentDam;
        sc.coolTime = petList[_index].coolTime;
        sc.Desc = petList[_index].Desc;
        sc.isEnable = "TRUE";

        petList[_index] = sc;
    }



    public void Pet_NeafThis(int _index, int _Neaf)
    {
        PetContent sc;
        sc.needUpgrade = _Neaf;
        // 주석 눈에 잘띄는 색
        sc.index = petList[_index].index;
        sc.petLevel = petList[_index].petLevel;
        sc.usingTimeDam = petList[_index].usingTimeDam;
        sc.percentDam = petList[_index].percentDam;
        sc.coolTime = petList[_index].coolTime;
        sc.Desc = petList[_index].Desc;
        sc.isEnable = petList[_index].isEnable;

        petList[_index] = sc;
    }



    ///--------------------------------  캐릭터  관련  -------------------------------------/// 



    public List<CharatorContent> charatorList = new List<CharatorContent>();



    public void Chara_LvUP(int _index, int _level)
    {
        CharatorContent sc;

        sc.index = charatorList[_index].index;
        sc.charLevel = _level.ToString();                                       /// <- 해당 레벨로 바꿔주면 다른 조건은 따라온다
        sc.charMaxLevel = charatorList[_index].charMaxLevel;
        sc.nextUpgradeCost = charatorList[_index].nextUpgradeCost;
        sc.powerPer = charatorList[_index].powerPer;
        sc.title = charatorList[_index].title;
        sc.description = charatorList[_index].description;
        charatorList[_index] = sc;
    }

    /// <summary>
    /// 210126 수정 charatorList[5] 스탯 치명타 대미지 5% -> 2.5%로 조정
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_level"></param>
    public void Chara_NerfThis(int _index, float _level)
    {
        CharatorContent sc;

        sc.index = charatorList[_index].index;
        sc.charLevel = charatorList[_index].charLevel;                                   /// <- 해당 레벨로 바꿔주면 다른 조건은 따라온다
        sc.charMaxLevel = charatorList[_index].charMaxLevel;
        sc.nextUpgradeCost = charatorList[_index].nextUpgradeCost;
        sc.powerPer = _level;
        sc.title = charatorList[_index].title;
        sc.description = charatorList[_index].description;
        charatorList[_index] = sc;
    }



    ///--------------------------------  무기  관련  -------------------------------------/// 




    public List<WeaponContent> weaponList = new List<WeaponContent>();


    /// <summary>
    /// 무기 활성화.
    /// </summary>
    /// <param name="_index"></param>
    public void Weapon_Equip(int _index, bool _isEquip)
    {
        WeaponContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = weaponList[_index].index;
        sc.headRank = weaponList[_index].headRank;
        sc.tailRank = weaponList[_index].tailRank;
        sc.weaponLevel = weaponList[_index].weaponLevel;
        sc.startPower = weaponList[_index].startPower;
        sc.increedPower = weaponList[_index].increedPower;
        sc.nextUpgradeCost = weaponList[_index].nextUpgradeCost;
        sc.rankUpDia = weaponList[_index].rankUpDia;
        sc.rankUpENstone = weaponList[_index].rankUpENstone;
        sc.startPassFail = weaponList[_index].startPassFail;
        sc.passFailPer = weaponList[_index].passFailPer;
        sc.weaAmount = weaponList[_index].weaAmount;

        if (_isEquip)
        {
            /// 무기 애니메이션 바꿔줌
            PlayerPrefsManager.instance.SetPlayerWeaponAnim(_index);
            /// 스탯에서 가져올 부분
            CurrentEquiped = sc.startPower + ((double.Parse(sc.weaponLevel) + PlayerInventory.Weapon_Lv_Plus) * sc.increedPower);
            sc.isEnable = "TRUE";
        }
        else sc.isEnable = "FALSE";


        weaponList[_index] = sc;
    }

    /// <summary>
    /// 무기 갯수 수정
    /// </summary>
    /// <param name="_index"></param>
    public void Weapon_Add(int _index)
    {
        WeaponContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = weaponList[_index].index;
        sc.headRank = weaponList[_index].headRank;
        sc.tailRank = weaponList[_index].tailRank;
        if (int.Parse(weaponList[_index].weaponLevel) == 0)
        {
            sc.weaponLevel = "1";
        }
        else
        {
            sc.weaponLevel = weaponList[_index].weaponLevel;
        }
        sc.startPower = weaponList[_index].startPower;
        sc.increedPower = weaponList[_index].increedPower;
        sc.nextUpgradeCost = weaponList[_index].nextUpgradeCost;
        sc.rankUpDia = weaponList[_index].rankUpDia;
        sc.rankUpENstone = weaponList[_index].rankUpENstone;
        sc.startPassFail = weaponList[_index].startPassFail;
        sc.passFailPer = weaponList[_index].passFailPer;
        sc.isEnable = weaponList[_index].isEnable;
        sc.weaAmount = (int.Parse(weaponList[_index].weaAmount) + 1).ToString();

        weaponList[_index] = sc;
    }

    /// <summary>
    /// 무기 합성 시 5개 제거
    /// </summary>
    /// <param name="_index"></param>
    public void Weapon_Sub(int _index)
    {
        WeaponContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = weaponList[_index].index;
        sc.headRank = weaponList[_index].headRank;
        sc.tailRank = weaponList[_index].tailRank;
        sc.weaponLevel = weaponList[_index].weaponLevel;
        sc.startPower = weaponList[_index].startPower;
        sc.increedPower = weaponList[_index].increedPower;
        sc.nextUpgradeCost = weaponList[_index].nextUpgradeCost;
        sc.rankUpDia = weaponList[_index].rankUpDia;
        sc.rankUpENstone = weaponList[_index].rankUpENstone;
        sc.startPassFail = weaponList[_index].startPassFail;
        sc.passFailPer = weaponList[_index].passFailPer;
        sc.isEnable = weaponList[_index].isEnable;
        sc.weaAmount = (int.Parse(weaponList[_index].weaAmount) - 10).ToString();

        weaponList[_index] = sc;
    }

    /// <summary>
    /// 레벨럽
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_thisLv"></param>
    public void Weapon_LvUP(int _index, int _thisLv)
    {
        WeaponContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = weaponList[_index].index;
        sc.headRank = weaponList[_index].headRank;
        sc.tailRank = weaponList[_index].tailRank;
        sc.weaponLevel = _thisLv.ToString();
        sc.startPower = weaponList[_index].startPower;
        sc.increedPower = weaponList[_index].increedPower;
        sc.nextUpgradeCost = weaponList[_index].nextUpgradeCost;
        sc.rankUpDia = weaponList[_index].rankUpDia;
        sc.rankUpENstone = weaponList[_index].rankUpENstone;
        sc.startPassFail = weaponList[_index].startPassFail;
        sc.passFailPer = weaponList[_index].passFailPer;
        sc.isEnable = weaponList[_index].isEnable;
        sc.weaAmount = weaponList[_index].weaAmount;

        if (sc.isEnable == "TRUE")
        {
            /// 스탯에서 가져올 부분
            CurrentEquiped = sc.startPower + ((double.Parse(sc.weaponLevel) + PlayerInventory.Weapon_Lv_Plus) * sc.increedPower);
        }

        weaponList[_index] = sc;
    }



    ///--------------------------------  END 무기  관련  -------------------------------------/// 





    ///--------------------------------  룬  관련  -------------------------------------/// 
    string[] tmpRuneDesc = new string[4];
    /// TODO : 장착 될때 한 번 실행
    /// 1. 장착 룬의 랭크 판단
    /// 2. 해당 랭크 만큼의 Desc 갯수 만큼 읽어오기 "한국어" 
    public void SetEquipedRuneEffect()
    {
        int descCnt = 0;

        /// 초기화
        for (int i = 0; i < PlayerInventory.RuneStat.Length; i++)
        {
            PlayerInventory.RuneStat[i] = 0;
        }

        /// 연산
        for (int i = 0; i < equipRuneList.Count; i++)
        {
            switch (equipRuneList[i].rank)
            {
                case "B":
                    descCnt = 1;
                    tmpRuneDesc[0] = equipRuneList[i].desc_1;

                    ReleaseRuneStat(descCnt, i);
                    break;

                case "A":
                    descCnt = 1;
                    tmpRuneDesc[0] = equipRuneList[i].desc_1;

                    ReleaseRuneStat(descCnt, i);
                    break;

                case "S":
                    descCnt = 2;
                    tmpRuneDesc[0] = equipRuneList[i].desc_1;
                    tmpRuneDesc[1] = equipRuneList[i].desc_2;

                    ReleaseRuneStat(descCnt, i);
                    break;

                case "L":
                    descCnt = 3;
                    tmpRuneDesc[0] = equipRuneList[i].desc_1;
                    tmpRuneDesc[1] = equipRuneList[i].desc_2;
                    tmpRuneDesc[2] = equipRuneList[i].desc_3;

                    ReleaseRuneStat(descCnt, i);
                    break;

                case "R":
                    descCnt = 4;
                    tmpRuneDesc[0] = equipRuneList[i].desc_1;
                    tmpRuneDesc[1] = equipRuneList[i].desc_2;
                    tmpRuneDesc[2] = equipRuneList[i].desc_3;
                    tmpRuneDesc[3] = equipRuneList[i].desc_4;

                    ReleaseRuneStat(descCnt, i);
                    break;

                default:
                    break;
            }
        }
    }

    void ReleaseRuneStat(int _dc, int equipIndex)
    {
        /// 선택된 스탯 적용 수 만큼 더해줌.
        switch (_dc)
        {
            case 0:
                SelectMainSubStat(tmpRuneDesc[0], equipRuneList[equipIndex].main_1);
                break;

            case 1:
                SelectMainSubStat(tmpRuneDesc[0], equipRuneList[equipIndex].main_1);
                break;

            case 2:
                SelectMainSubStat(tmpRuneDesc[0], equipRuneList[equipIndex].main_1);
                SelectMainSubStat(tmpRuneDesc[1], equipRuneList[equipIndex].main_2);
                break;

            case 3:
                SelectMainSubStat(tmpRuneDesc[0], equipRuneList[equipIndex].main_1);
                SelectMainSubStat(tmpRuneDesc[1], equipRuneList[equipIndex].main_2);
                SelectMainSubStat(tmpRuneDesc[2], equipRuneList[equipIndex].sub_1);
                break;

            case 4:
                SelectMainSubStat(tmpRuneDesc[0], equipRuneList[equipIndex].main_1);
                SelectMainSubStat(tmpRuneDesc[1], equipRuneList[equipIndex].main_2);
                SelectMainSubStat(tmpRuneDesc[2], equipRuneList[equipIndex].sub_1);
                SelectMainSubStat(tmpRuneDesc[3], equipRuneList[equipIndex].sub_2);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 각 Desc에 해당하는 스탯을 플레이어 인벤토리에 직접 적용 해줌
    /// </summary>
    /// <returns></returns>
    void SelectMainSubStat(string _Desc, double _StatValue)
    {
        switch (_Desc)
        {
            case "공격력": PlayerInventory.RuneStat[0] += _StatValue; break;
            case "공격속도": PlayerInventory.RuneStat[1] += _StatValue; break;
            case "이동 속도": PlayerInventory.RuneStat[2] += _StatValue; break;
            case "치명타 확률": PlayerInventory.RuneStat[3] += _StatValue; break;
            case "치명타 피해": PlayerInventory.RuneStat[4] += _StatValue; break;
            case "골드 획득량": PlayerInventory.RuneStat[5] += _StatValue; break;
            case "나뭇잎 획득량": PlayerInventory.RuneStat[6] += _StatValue; break;
            case "강화석 획득량": PlayerInventory.RuneStat[7] += _StatValue; break;
            case "수집 보상": PlayerInventory.RuneStat[8] += _StatValue; break;
            case "체력": PlayerInventory.RuneStat[9] += _StatValue; break;
            case "보유무기 공격력": PlayerInventory.RuneStat[10] += _StatValue; break;
            case "오프라인 보상": PlayerInventory.RuneStat[11] += _StatValue; break;

            default:
                break;
        }

        string tmpddfasd = "";
        for (int i = 0; i < PlayerInventory.RuneStat.Length; i++)
        {
            tmpddfasd += PlayerInventory.RuneStat[i] + "/";
        }

        Debug.Log("장착된 룬~~ : " + tmpddfasd);
    }



    /// <summary>
    /// 착용한 룬 저장되는 리스트.
    /// </summary>
    public List<RuneInventory> equipRuneList = new List<RuneInventory>();

    /// <summary>
    ///  실제 내가 획득한 룬
    /// </summary>
    public List<RuneInventory> runeList = new List<RuneInventory>();

    /// <summary>
    /// 얻을 수 있는 룬 모든 정보 (안보임)
    /// </summary>
    public List<RuneContent> invisibleruneList = new List<RuneContent>();

    /// <summary>
    /// 룬  활성화 갱신 + 리스트에 add 까지 다 해줌
    /// </summary>
    /// <param name="_index"></param>
    public void Rune_Unlock(string _rank, int i1, int i2, int i3, int i4)
    {
        float random1 = Random.Range(invisibleruneList[i1].rank_1_MIN, invisibleruneList[i1].rank_1_MAX);
        float random2 = Random.Range(invisibleruneList[i2].rank_1_MIN, invisibleruneList[i2].rank_1_MAX);
        float random3 = Random.Range(invisibleruneList[i3].rank_1_MIN, invisibleruneList[i3].rank_1_MAX);
        float random4 = Random.Range(invisibleruneList[i4].rank_1_MIN, invisibleruneList[i4].rank_1_MAX);

        RuneInventory sc;
        // 주석 눈에 잘띄는 색
        sc.rank = _rank;
        sc.imgIndex = i1.ToString();

        sc.desc_1 = invisibleruneList[i1].desc;
        sc.desc_2 = invisibleruneList[i2].desc;
        sc.desc_3 = invisibleruneList[i3].desc;
        sc.desc_4 = invisibleruneList[i4].desc;

        sc.main_1 = random1;
        sc.main_2 = random2;
        sc.sub_1 = random3;
        sc.sub_2 = random4;

        sc.isEquip = "FALSE";

        /// 신규로 추가해준다.
        runeList.Add(sc);
    }
    public void Rune_Unlock(string _rank, int i1, int i2, int i3)
    {
        float random1 = Random.Range(invisibleruneList[i1].rank_2_MIN, invisibleruneList[i1].rank_2_MAX);
        float random2 = Random.Range(invisibleruneList[i2].rank_2_MIN, invisibleruneList[i2].rank_2_MAX);
        float random3 = Random.Range(invisibleruneList[i3].rank_2_MIN, invisibleruneList[i3].rank_2_MAX);

        RuneInventory sc;
        // 주석 눈에 잘띄는 색
        sc.rank = _rank;
        sc.imgIndex = i1.ToString();

        sc.desc_1 = invisibleruneList[i1].desc;
        sc.desc_2 = invisibleruneList[i2].desc;
        sc.desc_3 = invisibleruneList[i3].desc;
        sc.desc_4 = null;

        sc.main_1 = random1;
        sc.main_2 = random2;
        sc.sub_1 = random3;
        sc.sub_2 = 0;

        sc.isEquip = "FALSE";

        /// 신규로 추가해준다.
        runeList.Add(sc);
    }
    public void Rune_Unlock(string _rank, int i1, int i2)
    {
        float random1 = Random.Range(invisibleruneList[i1].rank_3_MIN, invisibleruneList[i1].rank_3_MAX);
        float random2 = Random.Range(invisibleruneList[i2].rank_3_MIN, invisibleruneList[i2].rank_3_MAX);
        RuneInventory sc;
        // 주석 눈에 잘띄는 색
        sc.rank = _rank;
        sc.imgIndex = i1.ToString();

        sc.desc_1 = invisibleruneList[i1].desc;
        sc.desc_2 = invisibleruneList[i2].desc;
        sc.desc_3 = null;
        sc.desc_4 = null;

        sc.main_1 = random1;
        sc.main_2 = random2;
        sc.sub_1 = 0;
        sc.sub_2 = 0;

        sc.isEquip = "FALSE";

        /// 신규로 추가해준다.
        runeList.Add(sc);
    }
    public void Rune_Unlock(string _rank, int i1)
    {
        float random = 0;
        if (i1 < 5) random = Random.Range(invisibleruneList[i1].rank_4_MIN, invisibleruneList[i1].rank_4_MAX);
        else random = Random.Range(invisibleruneList[i1].rank_5_MIN, invisibleruneList[i1].rank_5_MAX);

        RuneInventory sc;
        // 주석 눈에 잘띄는 색
        sc.rank = _rank;
        sc.imgIndex = i1.ToString();

        sc.desc_1 = invisibleruneList[i1].desc;
        sc.desc_2 = null;
        sc.desc_3 = null;
        sc.desc_4 = null;

        sc.main_1 = random;
        sc.main_2 = 0;
        sc.sub_1 = 0;
        sc.sub_2 = 0;

        sc.isEquip = "FALSE";

        /// 신규로 추가해준다.
        runeList.Add(sc);
    }

    /// <summary>
    /// 룬 장착 / 해제
    /// </summary>
    /// <param name="_index"></param>
    public void Rune_Equip(int _index, bool _isEquip)
    {
        RuneInventory sc;
        // 주석 눈에 잘띄는 색
        sc.rank = runeList[_index].rank;
        sc.imgIndex = runeList[_index].imgIndex;

        sc.desc_1 = runeList[_index].desc_1;
        sc.desc_2 = runeList[_index].desc_2;
        sc.desc_3 = runeList[_index].desc_3;
        sc.desc_4 = runeList[_index].desc_4;

        sc.main_1 = runeList[_index].main_1;
        sc.main_2 = runeList[_index].main_2;
        sc.sub_1 = runeList[_index].sub_1;
        sc.sub_2 = runeList[_index].sub_2;

        if (_isEquip) sc.isEquip = "TRUE";
        else sc.isEquip = "FALSE";

        runeList[_index] = sc;
    }

    /// <summary>
    /// 룬 장착 / 해제
    /// </summary>
    /// <param name="_index"></param>
    public void Rune_ResetFromFussion(int _index, bool _isEquip)
    {
        RuneInventory sc;
        // 주석 눈에 잘띄는 색
        sc.rank = runeList[_index].rank;
        sc.imgIndex = runeList[_index].imgIndex;

        sc.desc_1 = runeList[_index].desc_1;
        sc.desc_2 = runeList[_index].desc_2;
        sc.desc_3 = runeList[_index].desc_3;
        sc.desc_4 = runeList[_index].desc_4;

        sc.main_1 = runeList[_index].main_1;
        sc.main_2 = runeList[_index].main_2;
        sc.sub_1 = runeList[_index].sub_1;
        sc.sub_2 = runeList[_index].sub_2;

        if (_isEquip) sc.isEquip = "FUSION";
        else sc.isEquip = "FALSE";

        runeList[_index] = sc;
    }




    /// <summary>
    /// 5개 넣고 퓨전하면 5개 삭제되고 신규 룬 하나 생성
    /// 장착된 룬은 제거 안되게 
    /// </summary>
    /// <param name="_indexs"></param>
    //public void Rune_Fusion(int[] _indexs)
    public void Rune_Fusion(List<RuneInventory> _hangman)
    {
        if (runeList.Count == 0) return;
        /// 5개 지워준다.
        for (int i = 0; i < 5; i++)
        {
            runeList.Remove(_hangman[i]);
        }
    }

    ///--------------------------------  END 룬  관련  관련  END -------------------------------------/// 




    ///--------------------------------  유물(Heart)  관련  -------------------------------------/// 

    /// <summary>
    /// 실제 내가 획득한 유물 목록 (유저 눈에 보임)
    /// </summary>
    public List<HeartContent> heartList = new List<HeartContent>();
    /// <summary>
    /// 얻을 수 있는 유물 목록 30개 (눈에 안보임)
    /// </summary>
    public List<HeartContent> invisibleheartList = new List<HeartContent>();


    #region HeartContent 유물(Heart) 수집 heartList

    /// <summary>
    /// 레벨업 시 json 갱신
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_level"></param>
    public void Heart_LvUP(int _index, int _level)
    {
        HeartContent sc;
        sc.index = heartList[_index].index;
        sc.heartLevel = _level.ToString();                                       /// <- 해당 레벨로 바꿔주면 다른 조건은 따라온다
        sc.maxLevel = heartList[_index].maxLevel;
        sc.powerToLvUP = heartList[_index].powerToLvUP;
        sc.nextUpgradeNeed = heartList[_index].nextUpgradeNeed;
        sc.leafToLvUP = heartList[_index].leafToLvUP;
        sc.dropTable = heartList[_index].dropTable;
        sc.descHead = heartList[_index].descHead;
        sc.descTail = heartList[_index].descTail;
        sc.imgIndex = heartList[_index].imgIndex;
        sc.heartName = heartList[_index].heartName;

        heartList[_index] = sc;
    }

    /// <summary>
    /// 유물  활성화 갱신
    /// </summary>
    /// <param name="_index"></param>
    public void Heart_Unlock(int _index)
    {
        HeartContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = heartList.Count.ToString();                                   /// <- 인벤토리에 들어온 순서대로 인덱스 지정해줌
        sc.heartLevel = invisibleheartList[_index].heartLevel;
        sc.maxLevel = invisibleheartList[_index].maxLevel;
        sc.powerToLvUP = invisibleheartList[_index].powerToLvUP;
        sc.nextUpgradeNeed = invisibleheartList[_index].nextUpgradeNeed;
        sc.leafToLvUP = invisibleheartList[_index].leafToLvUP;
        sc.dropTable = invisibleheartList[_index].dropTable;
        sc.descHead = invisibleheartList[_index].descHead;
        sc.descTail = invisibleheartList[_index].descTail;
        sc.imgIndex = invisibleheartList[_index].imgIndex;
        sc.heartName = invisibleheartList[_index].heartName;


        /// 신규로 추가해준다.
        heartList.Add(sc);
    }


    /// <summary>
    /// 210126 수정 invisibleheartList[28] 오프라인 보상 5% -> 1%
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_value"></param>
    public void Heart_invisNeaf(int _index, float _value)
    {
        /// 추적하기
        for (int i = 0; i < invisibleheartList.Count; i++)
        {
            if(invisibleheartList[i].imgIndex == "29")
            {
                HeartContent sc = invisibleheartList[i];
                sc.powerToLvUP = 1f;
                /// 갱신
                invisibleheartList[i] = sc;
                return;
            }
        }
    }

    /// <summary>
    ///  210126 유물 보상 5% -> 1% 너프
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_value"></param>
    public void Heart_myNeaf(int _index, float _value)
    {
        HeartContent sc = heartList[_index];
        /// 이미 값이 바뀌어있으면 return;
        if (sc.powerToLvUP == _value)
            return;
        sc.powerToLvUP = _value;
        // 갱신
        heartList[_index] = sc;
    }


    #endregion

    ///--------------------------------  End 유물(Heart)  관련   -------------------------------------///




    ///--------------------------------  수집 관련  -------------------------------------///


    /// 1. 레벨           supporterLevel           0 ~ 1000 가변
    /// 2. 활성화 여부    isEnable                 TRUE / FALSE
    public List<SupContent> supList = new List<SupContent>();

    #region SupContent 서포터 수집 supList

    /// <summary>
    /// 레벨업 시 json 갱신
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_level"></param>
    public void Supporter_LvUP(int _index, int _level)
    {
        SupContent sc;

        sc.index = supList[_index].index;
        sc.supporterLevel = _level.ToString();
        sc.supporterName = supList[_index].supporterName;
        sc.maxTime = supList[_index].maxTime;
        sc.currentEarnGold = supList[_index].currentEarnGold;
        sc.nextUpgradeNeed = supList[_index].nextUpgradeNeed;
        sc.isEnable = supList[_index].isEnable;

        supList[_index] = sc;
    }

    /// <summary>
    /// 수집 활성화 갱신
    /// </summary>
    /// <param name="_index"></param>
    public void Supporter_Unlock(int _index)
    {
        SupContent sc;
        // 주석 눈에 잘띄는 색
        sc.index = supList[_index].index;
        sc.supporterLevel = supList[_index].supporterLevel;
        sc.supporterName = supList[_index].supporterName;
        sc.maxTime = supList[_index].maxTime;
        sc.currentEarnGold = supList[_index].currentEarnGold;
        sc.nextUpgradeNeed = supList[_index].nextUpgradeNeed;
        sc.isEnable = "TRUE";

        supList[_index] = sc;
    }

    #endregion

    ///--------------------------------  End 수집 관련   -------------------------------------///



    /// 상점
    public List<ShopPrice> shopList = new List<ShopPrice>();
    public List<ShopPrice> shopListSPEC = new List<ShopPrice>();
    public List<ShopPrice> shopListNOR = new List<ShopPrice>();
    //
    public List<ShopPrice> shopListPACK = new List<ShopPrice>();
    public List<ShopPrice> shopListAMA = new List<ShopPrice>();

    /// <summary>
    ///  저려미 패키지 추가
    /// </summary>
    public List<ShopPrice> shopCheepPack = new List<ShopPrice>();



    /// 튜토리얼이요 MissonSchool
    public List<MissonSchool> missionDAYlist = new List<MissonSchool>();
    public List<MissonSchool> missionALLlist = new List<MissonSchool>();
    public List<MissonSchool> missionTUTOlist = new List<MissonSchool>();

    /// <summary>
    /// 해당 일일퀘스트 진행 사항 갱신해준다.
    /// </summary>
    /// <param name="_index"></param>
    public void DAYlist_Update(int _index)
    {
        if (!PlayerPrefsManager.isLoadingComp) return;
        
        /// Max값 못 넘게 예외처리 || 이미 보상 받아서 -1 상태면 리턴
        if (missionDAYlist[_index].maxValue == missionDAYlist[_index].curentValue 
            || missionDAYlist[_index].curentValue == "-1") return;

        MissonSchool sc;
        sc.category = missionDAYlist[_index].category;
        sc.korDesc = missionDAYlist[_index].korDesc;
        sc.engDesc = missionDAYlist[_index].engDesc;
        sc.reword = missionDAYlist[_index].reword;
        sc.refreshMulti = missionDAYlist[_index].refreshMulti;
        sc.maxValue = missionDAYlist[_index].maxValue;
        sc.curentValue = (int.Parse(missionDAYlist[_index].curentValue) +1).ToString();
        sc.rewordAmount = missionDAYlist[_index].reword;

        missionDAYlist[_index] = sc;

        /// 레드닷 체크
        if (int.Parse(sc.curentValue) >= int.Parse(sc.maxValue))
        {
            RedDotManager.instance.RedDot[5].SetActive(true);
        }
    }
    public void DAYlist_Update(int _index, int _value)
    {
        MissonSchool sc;
        sc.category = missionDAYlist[_index].category;
        sc.korDesc = missionDAYlist[_index].korDesc;
        sc.engDesc = missionDAYlist[_index].engDesc;
        sc.reword = missionDAYlist[_index].reword;
        sc.refreshMulti = missionDAYlist[_index].refreshMulti;
        sc.maxValue = missionDAYlist[_index].maxValue;
        sc.curentValue = _value.ToString();
        sc.rewordAmount = missionDAYlist[_index].rewordAmount;

        missionDAYlist[_index] = sc;

        /// 레드닷 체크
        if (int.Parse(sc.curentValue) >= int.Parse(sc.maxValue))
        {
            RedDotManager.instance.RedDot[5].SetActive(true);
        }
    }

    /// <summary>
    /// 업적 퀘스트 갱신해준다.
    /// </summary>
    /// <param name="_index"></param>
    public void ALLlist_Update(int _index, long _value)
    {
        MissonSchool sc;
        sc.category = missionALLlist[_index].category;
        sc.korDesc = missionALLlist[_index].korDesc;
        sc.engDesc = missionALLlist[_index].engDesc;
        sc.reword = missionALLlist[_index].reword;
        sc.refreshMulti = missionALLlist[_index].refreshMulti;
        sc.maxValue = missionALLlist[_index].maxValue;
        if (_value == -1)
        {
            sc.curentValue = "0";
        }
        else
        {
            sc.curentValue = (long.Parse(missionALLlist[_index].curentValue) + _value).ToString();
        }

        sc.rewordAmount = missionALLlist[_index].rewordAmount;
        missionALLlist[_index] = sc;
        /// 레드닷 체크
        if(long.Parse(sc.curentValue) >= long.Parse(sc.maxValue))
        {
            RedDotManager.instance.RedDot[5].SetActive(true);
        }
    }
    public void ALLlist_Update(int _index, double _value)
    {
        MissonSchool sc;
        sc.category = missionALLlist[_index].category;
        sc.korDesc = missionALLlist[_index].korDesc;
        sc.engDesc = missionALLlist[_index].engDesc;
        sc.reword = missionALLlist[_index].reword;
        sc.refreshMulti = missionALLlist[_index].refreshMulti;
        sc.maxValue = missionALLlist[_index].maxValue;
        sc.curentValue = (double.Parse(missionALLlist[_index].curentValue) + _value).ToString();
        sc.rewordAmount = missionALLlist[_index].rewordAmount;

        missionALLlist[_index] = sc;

        /// 레드닷 체크
        if (double.Parse(sc.curentValue) >= double.Parse(sc.maxValue))
        {
            RedDotManager.instance.RedDot[5].SetActive(true);
        }
    }

    /// <summary>
    /// 업적 퀘스트 <최대값> 갱신해준다.
    /// </summary>
    /// <param name="_index"></param>
    public void ALLlist_Max_Update(int _index)
    {

        /// MaxValue 갱신
        MissonSchool sc;
        sc.category = missionALLlist[_index].category;
        sc.korDesc = missionALLlist[_index].korDesc;
        sc.engDesc = missionALLlist[_index].engDesc;
        sc.reword = missionALLlist[_index].reword;
        sc.refreshMulti = missionALLlist[_index].refreshMulti;
        sc.rewordAmount = missionALLlist[_index].rewordAmount;
        /// -1 은 Max값 변함 없음
        if (missionALLlist[_index].refreshMulti == "-1")
        {
            /// 갱신 전에 커런트 빼줌.
            ALLlist_Sub(_index);
            sc.maxValue = missionALLlist[_index].maxValue;
        }
        /// 0은 10배수로 증가함
        else if (missionALLlist[_index].refreshMulti == "0")
        {
            sc.maxValue = (double.Parse(missionALLlist[_index].maxValue) * 10d).ToString("F0");
        }
        /// 나머지는 해당 값만큼 더해줌
        else
        {
            sc.maxValue = (long.Parse(missionALLlist[_index].maxValue) + int.Parse(missionALLlist[_index].refreshMulti)).ToString();
        }
        sc.curentValue = missionALLlist[_index].curentValue;

        missionALLlist[_index] = sc;
    }
    void ALLlist_Sub(int _index)
    {
        MissonSchool sc;
        sc.category = missionALLlist[_index].category;
        sc.korDesc = missionALLlist[_index].korDesc;
        sc.engDesc = missionALLlist[_index].engDesc;
        sc.reword = missionALLlist[_index].reword;
        sc.refreshMulti = missionALLlist[_index].refreshMulti;
        sc.maxValue = missionALLlist[_index].maxValue;
        sc.rewordAmount = missionALLlist[_index].rewordAmount;
        sc.curentValue =
                (long.Parse(missionALLlist[_index].curentValue)
                - long.Parse(missionALLlist[_index].maxValue)
                ).ToString();
        missionALLlist[_index] = sc;
    }


    /// <summary>
    /// 튜토리얼 정보 갱신
    /// </summary>
    /// <param name="_index"></param>
    public void TUTO_Update(int _index)
    {
        /// 보상 받아서 -1 상태 || 현재 표기된 튜토리얼 인덱스 다르면 리턴
        if (missionTUTOlist[_index].curentValue == "-1" 
            || PlayerPrefsManager.currentTutoIndex != _index) return;

        MissonSchool sc;
        sc.category = missionTUTOlist[_index].category;
        sc.korDesc = missionTUTOlist[_index].korDesc;
        sc.engDesc = missionTUTOlist[_index].engDesc;
        sc.reword = missionTUTOlist[_index].reword;
        sc.refreshMulti = missionTUTOlist[_index].refreshMulti;
        sc.maxValue = missionTUTOlist[_index].maxValue;
        sc.rewordAmount = missionTUTOlist[_index].rewordAmount;
        sc.curentValue = (int.Parse(missionTUTOlist[_index].curentValue) + 1).ToString();

        /// 변경사항 저장
        missionTUTOlist[_index] = sc;
        /// 업데이트 했는데 완료 해버리면 ? 
        if (int.Parse(sc.curentValue) >= int.Parse(sc.maxValue))
        {
            PlayerPrefsManager.instance.tm.CompliteThisTuto();
        }
        /// 스트링 새로고침
        PlayerPrefsManager.instance.tm.SetTutoText(_index);
    }
    public void TUTO_Update(int _index, int _Amount)
    {
        /// 보상 받아서 -1 상태 || 현재 표기된 튜토리얼 인덱스 다르면 리턴
        if (missionTUTOlist[_index].curentValue == "-1"
            || PlayerPrefsManager.currentTutoIndex != _index) return;

        MissonSchool sc;
        sc.category = missionTUTOlist[_index].category;
        sc.korDesc = missionTUTOlist[_index].korDesc;
        sc.engDesc = missionTUTOlist[_index].engDesc;
        sc.reword = missionTUTOlist[_index].reword;
        sc.refreshMulti = missionTUTOlist[_index].refreshMulti;
        sc.maxValue = missionTUTOlist[_index].maxValue;
        sc.rewordAmount = missionTUTOlist[_index].rewordAmount;
        sc.curentValue = (int.Parse(missionTUTOlist[_index].curentValue) + _Amount).ToString();

        /// 변경사항 저장
        missionTUTOlist[_index] = sc;
        /// 업데이트 했는데 완료 해버리면 ? 
        if (int.Parse(sc.curentValue) >= int.Parse(sc.maxValue))
        {
            PlayerPrefsManager.instance.tm.CompliteThisTuto();
        }
        /// 스트링 새로고침
        PlayerPrefsManager.instance.tm.SetTutoText(_index);
    }




    /// <summary>
    /// 해당 시점에서 초과 레벨인 경우 즉시 해금
    /// </summary>
    /// <param name="_index"></param>
    public void TUTO_BeforeComp(int _index, int _amount)
    {
        MissonSchool sc;
        sc.category = missionTUTOlist[_index].category;
        sc.korDesc = missionTUTOlist[_index].korDesc;
        sc.engDesc = missionTUTOlist[_index].engDesc;
        sc.reword = missionTUTOlist[_index].reword;
        sc.refreshMulti = missionTUTOlist[_index].refreshMulti;
        sc.maxValue = missionTUTOlist[_index].maxValue;
        sc.rewordAmount = missionTUTOlist[_index].rewordAmount;
        sc.curentValue = _amount.ToString();
        /// 변경사항 저장
        missionTUTOlist[_index] = sc;
        /// 업데이트 했는데 완료 해버리면 ? 
        if (int.Parse(sc.curentValue) >= int.Parse(sc.maxValue))
        {
            PlayerPrefsManager.instance.tm.CompliteThisTuto();
        }
    }


    /// <summary>
    /// 버튼 클릭했을때 처리해준다.
    /// </summary>
    /// <param name="_index"></param>
    public void TUTO_Complite(int _index)
    {
        MissonSchool sc;
        sc.category = missionTUTOlist[_index].category;
        sc.korDesc = missionTUTOlist[_index].korDesc;
        sc.engDesc = missionTUTOlist[_index].engDesc;
        sc.reword = missionTUTOlist[_index].reword;
        sc.refreshMulti = missionTUTOlist[_index].refreshMulti;
        sc.maxValue = missionTUTOlist[_index].maxValue;
        sc.rewordAmount = missionTUTOlist[_index].rewordAmount;
        sc.curentValue = "-1";
        /// 변경사항 저장
        missionTUTOlist[_index] = sc;
    }

    ///--------------------------------  End 일일 퀘스트 누적 미션   -------------------------------------///

    public List<MineCraft> mineCraft = new List<MineCraft>();
    /// <summary>
    /// 수집 활성화 갱신
    /// </summary>
    /// <param name="_index"></param>
    public void Mine_Unlock(int _index, string _string)
    {
        MineCraft sc;
        // 주석 눈에 잘띄는 색
        sc.stage = mineCraft[_index].stage;
        sc.mine_hp = mineCraft[_index].mine_hp;
        sc.reword_es = mineCraft[_index].reword_es;
        sc.reword_ama = mineCraft[_index].reword_ama;
        sc.unlockDia = mineCraft[_index].unlockDia;
        sc.isEnable = _string;

        mineCraft[_index] = sc;
    }


    ///--------------------------------  유료 구매 데이터 저장용 -------------------------------------///

    public List<MVP> mvpDataList = new List<MVP>();
    public List<AxeStat> axeDataList = new List<AxeStat>();


    ///--------------------------------  숨겨진 늪지 데이터 저장용   -------------------------------------///

    public List<SwampCave> swampCaveData = new List<SwampCave>();


    /// <summary>
    ///     /// 덮어쓰기용 재화 저장용 
    ///     nonSaveJsonMoney[0]은 21-01-13 에 사용
    ///     nonSaveJsonMoney[1]은 -- -- -- 에 사용 하는 식으로?
    ///     nonSaveJsonMoney[1]은 -- -- -- 에 사용 하는 식으로?
    /// </summary>
    public List<NonJson> nonSaveJsonMoney = new List<NonJson>();

    public void InitNonJsonData()
    {
        nonSaveJsonMoney.Add(new NonJson
        {
            RecentDistance = "0",
            Money_Gold = "0",
            Money_Elixir = "0",
            Money_AmazonCoin = "0",
            AmazonStoneCount = "0",
            CurrentAmaLV = "0",
            box_Coupon = "0",
            box_E = "0",
            box_D = "0",
            box_C = "0",
            box_B = "0",
            box_A = "0",
            box_S = "0",
            box_L = "0",
            ticket_reinforce_box = "0",
            ticket_leaf_box = "0",
            ticket_pvp_enter = "0",
            ticket_cave_enter = "0",
            ticket_cave_clear = "0",
            S_reinforce_box = "0",
            S_leaf_box = "0",
            mining = "0",
            amber = "0",
            isTutoAllClear = 0,
        });
    }



    /// <summary>
    ///-- 서버에서 불러온 다음  DataBopCopy 데이터 저장용   -------------------///
    /// </summary>
    /// <param name="data"></param>
    public void Sector5LoadBox(SeverDataBox data, string _107jsonString)
    {
        // 펫  0 
        for (int i = 0; i < data.petLevel.Length; i++)
        {
            Pet_UnBoxing(i, data.petLevel[i], data.petIsEnable[i]);
        }
        // 캐릭터 1 
        for (int i = 0; i < data.charLevel.Length; i++)
        {
            Chara_UnBoxing(i, data.charLevel[i]);
        }
        //  무기  2 
        for (int i = 0; i < data.weaponLevel.Length; i++)
        {
            Weapon_UnBoxing(i, data.weaponLevel[i], data.weaAmount[i], data.weaIsEnable[i]);
        }
        // 수집 8 
        for (int i = 0; i < data.supporterLevel.Length; i++)
        {
            Supporter_UnBoxing(i, data.supporterLevel[i], data.supIsEnable[i]);
        }
        // 일일 미션  15
        for (int i = 0; i < data.curentDayValue.Length; i++)
        {
            DAYlist_UnBoxing(i, data.curentDayValue[i]);
        }
        // 업적  16
        for (int i = 0; i < data.maxAllValue.Length; i++)
        {
            ALLlist_UnBoxing(i, data.maxAllValue[i], data.curentAllValue[i]);
        }
        // 튜토리얼 17
        for (int i = 0; i < data.curentTutoValue.Length; i++)
        {
            TUTO_UnBoxing(i, data.curentTutoValue[i]);
        }
        // 광산 해금18
        for (int i = 0; i < data.mineIsEnable.Length; i++)
        {
            Mine_UnBoxing(i, data.mineIsEnable[i]);
        }
        // 늪지 킬카운터 20
        for (int i = 0; i < data.killCount.Length; i++)
        {
            swampCaveData[i].killCount = data.killCount[i].ToString();
        }

        /// 로컬 파일로 저장
        PlayerPrefsManager.instance.NewNewSector5();
    }
    void Pet_UnBoxing(int _index, int _lv, bool _isTrue)
    {
        PetContent sc;
        sc.petLevel = _lv.ToString();
        sc.isEnable = _isTrue  ? "TRUE" : "FALSE";
        // 주석 눈에 잘띄는 색
        sc.index = petList[_index].index;
        sc.needUpgrade = petList[_index].needUpgrade;
        sc.usingTimeDam = petList[_index].usingTimeDam;
        sc.percentDam = petList[_index].percentDam;
        sc.coolTime = petList[_index].coolTime;
        sc.Desc = petList[_index].Desc;

        petList[_index] = sc;
    }
    void Chara_UnBoxing(int _index, int _level)
    {
        CharatorContent sc;
        sc.charLevel = _level.ToString();                                       /// <- 해당 레벨로 바꿔주면 다른 조건은 따라온다

        sc.index = charatorList[_index].index;
        sc.charMaxLevel = charatorList[_index].charMaxLevel;
        sc.nextUpgradeCost = charatorList[_index].nextUpgradeCost;
        sc.powerPer = charatorList[_index].powerPer;
        sc.title = charatorList[_index].title;
        sc.description = charatorList[_index].description;
        charatorList[_index] = sc;
    }
    void Weapon_UnBoxing(int _index, int _Lv, int _Amont,  bool _isTrue)
    {
        WeaponContent sc;
        sc.weaponLevel = _Lv.ToString();
        sc.weaAmount = _Amont.ToString();
        sc.isEnable = _isTrue ? "TRUE" : "FALSE";
        // 주석 눈에 잘띄는 색
        sc.index = weaponList[_index].index;
        sc.headRank = weaponList[_index].headRank;
        sc.tailRank = weaponList[_index].tailRank;
        sc.startPower = weaponList[_index].startPower;
        sc.increedPower = weaponList[_index].increedPower;
        sc.nextUpgradeCost = weaponList[_index].nextUpgradeCost;
        sc.rankUpDia = weaponList[_index].rankUpDia;
        sc.rankUpENstone = weaponList[_index].rankUpENstone;
        sc.startPassFail = weaponList[_index].startPassFail;
        sc.passFailPer = weaponList[_index].passFailPer;

        weaponList[_index] = sc;
    }
    void Supporter_UnBoxing(int _index, int _level, bool _isTrue)
    {
        SupContent sc;
        sc.supporterLevel = _level.ToString();
        sc.isEnable = _isTrue ? "TRUE" : "FALSE";

        sc.index = supList[_index].index;
        sc.supporterName = supList[_index].supporterName;
        sc.maxTime = supList[_index].maxTime;
        sc.currentEarnGold = supList[_index].currentEarnGold;
        sc.nextUpgradeNeed = supList[_index].nextUpgradeNeed;

        supList[_index] = sc;
    }
    void DAYlist_UnBoxing(int _index, int _value)
    {
        MissonSchool sc;
        sc.curentValue = _value.ToString();

        sc.category = missionDAYlist[_index].category;
        sc.korDesc = missionDAYlist[_index].korDesc;
        sc.engDesc = missionDAYlist[_index].engDesc;
        sc.reword = missionDAYlist[_index].reword;
        sc.refreshMulti = missionDAYlist[_index].refreshMulti;
        sc.maxValue = missionDAYlist[_index].maxValue;
        sc.rewordAmount = missionDAYlist[_index].rewordAmount;

        missionDAYlist[_index] = sc;
    }
    void ALLlist_UnBoxing(int _index, string _maxValue, string _curValue)
    {
        MissonSchool sc;
        sc.maxValue = _maxValue;
        sc.curentValue = _curValue;

        sc.category = missionALLlist[_index].category;
        sc.korDesc = missionALLlist[_index].korDesc;
        sc.engDesc = missionALLlist[_index].engDesc;
        sc.reword = missionALLlist[_index].reword;
        sc.refreshMulti = missionALLlist[_index].refreshMulti;
        sc.rewordAmount = missionALLlist[_index].rewordAmount;

        missionALLlist[_index] = sc;
    }
    void TUTO_UnBoxing(int _index, int _Amount)
    {
        MissonSchool sc;
        sc.curentValue = _Amount.ToString();

        sc.category = missionTUTOlist[_index].category;
        sc.korDesc = missionTUTOlist[_index].korDesc;
        sc.engDesc = missionTUTOlist[_index].engDesc;
        sc.reword = missionTUTOlist[_index].reword;
        sc.refreshMulti = missionTUTOlist[_index].refreshMulti;
        sc.maxValue = missionTUTOlist[_index].maxValue;
        sc.rewordAmount = missionTUTOlist[_index].rewordAmount;

        missionTUTOlist[_index] = sc;
    }
    void Mine_UnBoxing(int _index, string _string)
    {
        MineCraft sc;
        sc.isEnable = _string;
        /// 1.0.7 추가 - 채굴 진행 시간 불러오기
        sc.mine_hp = mineCraft[_index].mine_hp;
        // 주석 눈에 잘띄는 색
        sc.stage = mineCraft[_index].stage;
        sc.reword_es = mineCraft[_index].reword_es;
        sc.reword_ama = mineCraft[_index].reword_ama;
        sc.unlockDia = mineCraft[_index].unlockDia;

        mineCraft[_index] = sc;
    }

}

