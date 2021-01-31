using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CodeStage.AntiCheat.Storage;
using System.IO;

public class ModelHandler : MonoBehaviour
{
    //public TextAsset _char;            /// 캐릭터 텍스트 파일
    //public TextAsset _weapon;            /// 무기 텍스트 파일
    //public TextAsset _Heart;            /// 유물 텍스트 파일
    //public TextAsset ta;            /// 수집 텍스트 파일
    //public TextAsset _Rune;            /// 룬 텍스트 파일
    //public TextAsset _PET;            /// 펫 텍스트 파일
    //public TextAsset SHPSHPSHOP;            /// 상점 텍스트 파일
    //public TextAsset Misson;            /// 미션 텍스트 파일
    //public TextAsset _mine;            /// 광산 텍스트 파일
    //public TextAsset _Swamp;            /// 숨겨진 늪지 텍스트 파일

    //public void zTA_Parser_Swamp()
    //{
    //    ListModel.Instance.swampCaveData.Clear();

    //    string[] line = _Swamp.text.Substring(0, _Swamp.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.swampCaveData.Add(new SwampCave
    //        {
    //            stageLevel = float.Parse(row[0]),
    //            monsterHP = float.Parse(row[1]),
    //            rewordLeaf = float.Parse(row[2]),
    //            rewordEnchant = float.Parse(row[3]),
    //            killCount = row[4],
    //        });
    //    }
    //}


    /// <summary>
    /// 게임 기초 공사
    /// </summary>
    public void TunaDataLoad()
    {
        /// 210113 _ 추가 데이터 파일 초기화 - >nonSaveJsonMoney [0]
        ListModel.Instance.InitNonJsonData();
        /// 210115 _ 추가 데이터 파일 초기화 - >nonSaveJsonMoney [1]
        ListModel.Instance.InitNonJsonData();

        /// 나중에 추가 데이터 생기면 nonSaveJsonMoney [n] 생성해서 여기에 저장하면 됨
       // ListModel.Instance.InitNonJsonData(); 

        /// 완전 게임 처음 시작하거나
        /// 완전 초기화 후에 실행하거나
        if (!ObscuredPrefs.HasKey("tunamayo"))
        {
            /// 이전 데이터 다 적용
            ObscuredPrefs.SetInt("update210114", 214);
            ObscuredPrefs.SetInt("update210117", 956);
            ObscuredPrefs.SetInt("update210128", 455);
            ObscuredPrefs.Save();
            /// 파일에서 데이터 불러와서 리스트에 대입
            PlayerPrefsManager.instance.JObjectLoad(true);
            return;
        }


        ///update210114
        ///----------------------------------------------update210114---------------------------------------
        ///update210114
        if (!ObscuredPrefs.HasKey("update210114"))
        {
            /// 초반 초기화 완료 됐을때 키 초기화
            ObscuredPrefs.SetInt("update210114", 214);
            ObscuredPrefs.Save();
            string loadstring = File.ReadAllText(Application.persistentDataPath + "/_data_"); // string을 읽음 
            /// 깨진 파일이다.
            if (loadstring =="0ploWsGdZyF6rPHLBv8vIhpAa2lAnZAKThEqtD0iKfPCIfm2YG2CyfQ8lvKbXZQZhQCwBIQC+rLh3uVkTj2m+kdPxcx83eK+vvpRII+r0oIPeYbY12vSkQiV96LtybtNptckySL/rMdSuWQQal3Z0w==")
            {
                /// true 라면 유료 재화만 복구하고
                InitMoHa(true);
                return;
            }

        }

        ///update210128
        ///----------------------------------------------update210128---------------------------------------
        ///update210128
        if (!ObscuredPrefs.HasKey("update210128"))
        {
            /// 파일에서 데이터 불러와서 리스트에 대입
            PlayerPrefsManager.instance.SuperDataPatch();

            /// 초반 초기화 완료 됐을때 키 초기화
            ObscuredPrefs.SetInt("update210128", 455);
            ObscuredPrefs.Save();
            return;
        }



        /// ----------------------------------------- 통상 호출

        /// 로컬 데이터 로드
        InitMoHa(false);

    }


    int iTryResult;
    long lTryResult;
    double dTryResult;


    /// <summary>
    /// _isLocal = false 이면 로컬에서 불러오기
    /// _isLocal = true 이면 초기화 해버리기
    /// </summary>
    /// <param name="_isLocal"></param>
    void InitMoHa(bool _isLocal)
    {
        /// 파일에서 데이터 불러와서 리스트에 대입
        PlayerPrefsManager.instance.JObjectLoad(_isLocal);

        /// 뉴 데이터 -
        GameDataBox data = DataBoxCopy.instance.LoadBox();
        //
        PlayerInventory.RecentDistance = data.RecentDistance;
        PlayerInventory.Money_Gold = data.Money_Gold;
        PlayerInventory.Money_Elixir = data.Money_Elixir;
        PlayerInventory.Money_Dia = data.Money_Dia;
        PlayerInventory.Money_Leaf = data.Money_Leaf;
        PlayerInventory.Money_EnchantStone = data.Money_EnchantStone;
        PlayerInventory.Money_AmazonCoin = data.Money_AmazonCoin;
        PlayerInventory.AmazonStoneCount = data.AmazonStoneCount;
        PlayerInventory.CurrentAmaLV = data.CurrentAmaLV;
        PlayerInventory.box_Coupon = data.box_Coupon;
        PlayerInventory.box_E = data.box_E;
        PlayerInventory.box_D = data.box_D;
        PlayerInventory.box_C = data.box_C;
        PlayerInventory.box_B = data.box_B;
        PlayerInventory.box_A = data.box_A;
        PlayerInventory.box_S = data.box_S;
        PlayerInventory.box_L = data.box_L;
        PlayerInventory.ticket_leaf_box = data.ticket_leaf_box;
        PlayerInventory.ticket_pvp_enter = data.ticket_pvp_enter;
        PlayerInventory.ticket_cave_enter = data.ticket_cave_enter;
        PlayerInventory.ticket_cave_clear = data.ticket_cave_clear;
        PlayerInventory.S_leaf_box = data.S_leaf_box;
        PlayerInventory.mining = data.mining;
        PlayerInventory.amber = data.amber;
        //
        PlayerPrefsManager.DailyCount_Cheak = data.DailyCount_Cheak;
        PlayerPrefsManager.isTutoAllClear = data.isTutoAllClear != 0 ? true : false;
        PlayerPrefsManager.isDailyCheak = data.isDailyCheak != 0 ? true : false;
        PlayerPrefsManager.ZogarkMissionCnt = data.ZogarkMissionCnt;
        PlayerPrefsManager.AmaAdsTimer = data.AmaAdsTimer;
        PlayerPrefsManager.FreeDiaCnt = data.FreeDiaCnt;
        PlayerPrefsManager.FreeWeaponCnt = data.FreeWeaponCnt;
        PlayerPrefsManager.SwampyEnterCnt = data.SwampyEnterCnt;
        PlayerPrefsManager.SwampySkipCnt = data.SwampySkipCnt;
        //
    }

    ///// <summary>
    ///// 인보크로 불러와줄것
    ///// </summary>
    //void InvoFirstWeapon()
    //{
    //    ///기본무기 장착 Weapon_Add
    //    ListModel.Instance.Weapon_Add(0);
    //    ListModel.Instance.Weapon_Equip(0, true);

    //    /// 스트링[] 몽땅 저장
    //    PlayerPrefsManager.instance.TEST_SaveJson();
    //}


    /// <summary>
    /// 테스트 버튼에 붙이자 -> json 리셋후 종료
    /// </summary>
    public void TEST_RESTE_JSON()
    {
        //
        ObscuredPrefs.DeleteAll();
        PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    //#region 파서는 DB 최종판이 되면 일괄 삭제해준다.

    ///// <summary>
    ///// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    ///// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    ///// </summary>
    //void TA_Parser()
    //{
    //    string[] line = ta.text.Substring(0, ta.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.supList.Add(new SupContent
    //        {
    //            index = row[0],
    //            supporterLevel = row[1],
    //            supporterName = row[2],
    //            maxTime = float.Parse(row[3]),
    //            currentEarnGold = double.Parse(row[4]),
    //            nextUpgradeNeed = double.Parse(row[5]),
    //            isEnable = row[6],
    //        });
    //    }
    //}

    ///// <summary>
    ///// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    ///// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    ///// </summary>
    //void TA_Parser_HV()
    //{
    //    string[] line = _Heart.text.Substring(0, _Heart.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.invisibleheartList.Add(new HeartContent
    //        {
    //            index = row[0],
    //            heartLevel = row[1],
    //            maxLevel = row[2],
    //            powerToLvUP = float.Parse(row[3]),
    //            nextUpgradeNeed = double.Parse(row[4]),
    //            leafToLvUP = double.Parse(row[5]),
    //            dropTable = float.Parse(row[6]),
    //            descHead = row[7],
    //            descTail = row[8],
    //            imgIndex = row[9],
    //            heartName = row[10],
    //        });
    //    }
    //}

    ///// <summary>
    ///// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    ///// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    ///// </summary>
    //void TA_Parse_Charr()
    //{
    //    string[] line = _char.text.Substring(0, _char.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.charatorList.Add(new CharatorContent
    //        {
    //            index = row[0],
    //            charLevel = row[1],
    //            charMaxLevel = row[2],
    //            nextUpgradeCost = float.Parse(row[3]),
    //            powerPer = float.Parse(row[4]),
    //            title = row[5],
    //            description = row[6],
    //        });
    //    }
    //}

    ///// <summary>
    ///// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    ///// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    ///// </summary>
    //void TA_Parser_Rune()
    //{
    //    string[] line = _Rune.text.Substring(0, _Rune.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.invisibleruneList.Add(new RuneContent
    //        {
    //            index = row[0],
    //            imgIndex = row[1],
    //            rank = row[2],
    //            desc = row[3],
    //            rank_1_MIN = float.Parse(row[4]),
    //            rank_1_MAX = float.Parse(row[5]),
    //            rank_2_MIN = float.Parse(row[6]),
    //            rank_2_MAX = float.Parse(row[7]),
    //            rank_3_MIN = float.Parse(row[8]),
    //            rank_3_MAX = float.Parse(row[9]),
    //            rank_4_MIN = float.Parse(row[10]),
    //            rank_4_MAX = float.Parse(row[11]),
    //            rank_5_MIN = float.Parse(row[12]),
    //            rank_5_MAX = float.Parse(row[13]),
    //            isEquip = row[14],
    //        });
    //    }
    //}

    ///// <summary>
    ///// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    ///// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    ///// </summary>
    //void TA_Parser_Weapon()
    //{
    //    string[] line = _weapon.text.Substring(0, _weapon.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.weaponList.Add(new WeaponContent
    //        {
    //            index = row[0],
    //            headRank = row[1],
    //            tailRank = row[2],
    //            weaponLevel = row[3],
    //            startPower = float.Parse(row[4]),
    //            increedPower = float.Parse(row[5]),
    //            nextUpgradeCost = float.Parse(row[6]),
    //            rankUpDia = float.Parse(row[7]),
    //            rankUpENstone = float.Parse(row[8]),
    //            startPassFail = float.Parse(row[9]),
    //            passFailPer = float.Parse(row[10]),
    //            isEnable = row[11],
    //            weaAmount = row[12],
    //        });
    //    }
    //}

    ///// <summary>
    ///// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    ///// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    ///// </summary>
    //void TA_Parser_Pet()
    //{
    //    string[] line = _PET.text.Substring(0, _PET.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.petList.Add(new PetContent
    //        {
    //            index = row[0],
    //            petLevel = row[1],
    //            needUpgrade = float.Parse(row[2]),
    //            usingTimeDam = float.Parse(row[3]),
    //            percentDam = float.Parse(row[4]),
    //            coolTime = float.Parse(row[5]),
    //            Desc = row[6],
    //            isEnable = row[7],
    //        });
    //    }
    //}

    //void TA_Parser_Shop()
    //{
    //    string[] line = SHPSHPSHOP.text.Substring(0, SHPSHPSHOP.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        if (row[0] == "dia")
    //        {
    //            ListModel.Instance.shopList.Add(new ShopPrice
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                korPrice = row[2],
    //                engDesc = row[3],
    //                engPrice = row[4],
    //                korTailDesc = row[5],
    //                engTailDesc = row[6],
    //            });
    //        }
    //        else if (row[0] == "spec")
    //        {
    //            ListModel.Instance.shopListSPEC.Add(new ShopPrice
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                korPrice = row[2],
    //                engDesc = row[3],
    //                engPrice = row[4],
    //                korTailDesc = row[5],
    //                engTailDesc = row[6],
    //            });
    //        }
    //        else if (row[0] == "nor")
    //        {
    //            ListModel.Instance.shopListNOR.Add(new ShopPrice
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                korPrice = row[2],
    //                engDesc = row[3],
    //                engPrice = row[4],
    //                korTailDesc = row[5],
    //                engTailDesc = row[6],
    //            });
    //        }
    //        else if (row[0] == "pack")
    //        {
    //            ListModel.Instance.shopListPACK.Add(new ShopPrice
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                korPrice = row[2],
    //                engDesc = row[3],
    //                engPrice = row[4],
    //                korTailDesc = row[5],
    //                engTailDesc = row[6],
    //            });
    //        }
    //        else if (row[0] == "ama")
    //        {
    //            ListModel.Instance.shopListAMA.Add(new ShopPrice
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                korPrice = row[2],
    //                engDesc = row[3],
    //                engPrice = row[4],
    //                korTailDesc = row[5],
    //                engTailDesc = row[6],
    //            });
    //        }

    //    }

    //}

    //void TA_Parser_Mission()
    //{
    //    string[] line = Misson.text.Substring(0, Misson.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        if (row[0] == "day")
    //        {
    //            ListModel.Instance.missionDAYlist.Add(new MissonSchool
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                engDesc = row[2],
    //                reword = row[3],
    //                refreshMulti = row[4],
    //                maxValue = row[5],
    //                curentValue = row[6],
    //                rewordAmount = row[7],
    //            });
    //        }
    //        else if (row[0] == "all")
    //        {
    //            ListModel.Instance.missionALLlist.Add(new MissonSchool
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                engDesc = row[2],
    //                reword = row[3],
    //                refreshMulti = row[4],
    //                maxValue = row[5],
    //                curentValue = row[6],
    //                rewordAmount = row[7],
    //            });
    //        }
    //        else if (row[0] == "tuto")
    //        {
    //            ListModel.Instance.missionTUTOlist.Add(new MissonSchool
    //            {
    //                category = row[0],
    //                korDesc = row[1],
    //                engDesc = row[2],
    //                reword = row[3],
    //                refreshMulti = row[4],
    //                maxValue = row[5],
    //                curentValue = row[6],
    //                rewordAmount = row[7],
    //            });
    //        }

    //    }

    //}

    //void TA_Parser_Mine()
    //{
    //    string[] line = _mine.text.Substring(0, _mine.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.mineCraft.Add(new MineCraft
    //        {
    //            stage  = row[0],
    //            mine_hp = float.Parse(row[1]),
    //            reword_es = row[2],
    //            reword_ama = row[3],
    //            unlockDia = row[4],
    //            isEnable = row[5],
    //        });
    //    }
    //}


    //void TA_Parser_Swamp()
    //{
    //    string[] line = _Swamp.text.Substring(0, _Swamp.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        ListModel.Instance.swampCaveData.Add(new SwampCave
    //        {
    //            stageLevel = float.Parse(row[0]),
    //            monsterHP = float.Parse(row[1]),
    //            rewordLeaf = float.Parse(row[2]),
    //            rewordEnchant = float.Parse(row[3]),
    //            killCount = row[4],
    //        });
    //    }
    //}


    //#endregion





}
