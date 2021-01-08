using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CodeStage.AntiCheat.Storage;

public class ModelHandler : MonoBehaviour
{
    public TextAsset _char;            /// 캐릭터 텍스트 파일
    public TextAsset _weapon;            /// 무기 텍스트 파일
    public TextAsset _Heart;            /// 유물 텍스트 파일
    public TextAsset ta;            /// 수집 텍스트 파일
    public TextAsset _Rune;            /// 룬 텍스트 파일
    public TextAsset _PET;            /// 펫 텍스트 파일
    public TextAsset SHPSHPSHOP;            /// 상점 텍스트 파일
    public TextAsset Misson;            /// 미션 텍스트 파일
    public TextAsset _mine;            /// 광산 텍스트 파일
    public TextAsset _Swamp;            /// 숨겨진 늪지 텍스트 파일



    private void Start()
    {
        /// 완전 초기화 후에 쌔삥 데이터로 갈아 끼워줌
        if (!ObscuredPrefs.HasKey("TEST_Key"))
        {
            /// 초반 초기화 완료 됐을때 키 초기화 TEST_Save 로 옮긴다
            //ObscuredPrefs.SetInt("TEST_Key", 525);
            
            if (ListModel.Instance.charatorList.Count == 0) TA_Parse_Charr();
            if (ListModel.Instance.supList.Count == 0) TA_Parser();
            if (ListModel.Instance.invisibleheartList.Count == 0) TA_Parser_HV();
            if (ListModel.Instance.invisibleruneList.Count == 0) TA_Parser_Rune();
            if (ListModel.Instance.weaponList.Count == 0) TA_Parser_Weapon();
            if (ListModel.Instance.petList.Count == 0) TA_Parser_Pet();
            if (ListModel.Instance.shopList.Count == 0) TA_Parser_Shop();
            if (ListModel.Instance.missionDAYlist.Count == 0) TA_Parser_Mission();
            if (ListModel.Instance.mineCraft.Count == 0) TA_Parser_Mine();
            if (ListModel.Instance.swampCaveData.Count == 0) TA_Parser_Swamp();

            // 유료 상품 구매 
            ListModel.Instance.mvpDataList.Add(new MVP
            {
                SuperUser = 0,
                buff_power_up = 0,
                buff_attack_speed_up = 0,
                buff_gold_earned_up = 0,
                buff_move_speed_up = 0,
                pack_06 = 0,
                pack_07 = 0,
                pack_08 = 0,
                pack_09 = 0,
                pack_10 = 0,
                daily_10 = 0,
                daily_11 = 0,
                daily_12 = 0,
                daily_13 = 0,
                weekend_14 = 0,
                weekend_15 = 0,
                weekend_16 = 0,
                weekend_17 = 0,
                mouth_18 = 0,
                mouth_19 = 0,
                mouth_20 = 0,
                mouth_21 = 0,
                mouth_22 = 0,
                mouth_23 = 0,
                weekend_Day = 0,
                mouth_Day = 0,
            });
            // 수정 동굴 초기 수치 세팅
            ListModel.Instance.axeDataList.Add(new AxeStat
            {
                Stack_EnStone = 0,
                Stack_AmaCystal = 0,
                Stack_Amber = 0,
                Axe_Power = 1,
                Axe_Speed = 1,
                Axe_Skill = 1,
            });

            /// 초반 초기화 완료 됐을때 키 초기화
            ObscuredPrefs.SetInt("TEST_Key", 525);

            /// 스트링[] 몽땅 저장
            PlayerPrefsManager.instance.TEST_SaveJson();
            // 초기 무기 1렙 짜리 장착
            Invoke(nameof(InvoFirstWeapon), 2.2f);
        }
        else
        {
            /// TODO :  임시 데이터 로드
            PlayerInventory.RecentDistance = double.Parse(ObscuredPrefs.GetString("RecentDistance"));
            PlayerInventory.Money_Gold = double.Parse(ObscuredPrefs.GetString("Money_Gold"));
            PlayerInventory.Money_Elixir = long.Parse(ObscuredPrefs.GetString("Money_Elixir"));
            PlayerInventory.Money_Dia = long.Parse(ObscuredPrefs.GetString("Money_Dia"));
            PlayerInventory.Money_Leaf = long.Parse(ObscuredPrefs.GetString("Money_Leaf"));
            PlayerInventory.Money_EnchantStone = long.Parse(ObscuredPrefs.GetString("Money_EnchantStone"));
            PlayerInventory.Money_AmazonCoin = long.Parse(ObscuredPrefs.GetString("Money_AmazonCoin"));
            PlayerInventory.AmazonStoneCount = long.Parse(ObscuredPrefs.GetString("AmazonStoneCount"));
            PlayerInventory.box_Coupon = int.Parse(ObscuredPrefs.GetString("box_Coupon"));
            PlayerInventory.box_E = int.Parse(ObscuredPrefs.GetString("box_E"));
            PlayerInventory.box_D = int.Parse(ObscuredPrefs.GetString("box_D"));
            PlayerInventory.box_C = int.Parse(ObscuredPrefs.GetString("box_C"));
            PlayerInventory.box_B = int.Parse(ObscuredPrefs.GetString("box_B"));
            PlayerInventory.box_A = int.Parse(ObscuredPrefs.GetString("box_A"));
            PlayerInventory.box_S = int.Parse(ObscuredPrefs.GetString("box_S"));
            PlayerInventory.box_L = int.Parse(ObscuredPrefs.GetString("box_L"));
            PlayerInventory.ticket_reinforce_box = int.Parse(ObscuredPrefs.GetString("ticket_reinforce_box"));
            PlayerInventory.ticket_leaf_box = int.Parse(ObscuredPrefs.GetString("ticket_leaf_box"));
            PlayerInventory.ticket_pvp_enter = int.Parse(ObscuredPrefs.GetString("ticket_pvp_enter"));
            PlayerInventory.ticket_cave_enter = int.Parse(ObscuredPrefs.GetString("ticket_cave_enter"));
            PlayerInventory.ticket_cave_clear = int.Parse(ObscuredPrefs.GetString("ticket_cave_clear"));
            PlayerInventory.S_reinforce_box = int.Parse(ObscuredPrefs.GetString("S_reinforce_box"));


            PlayerInventory.S_leaf_box = int.Parse(ObscuredPrefs.GetString("S_leaf_box"));
            
            
            PlayerInventory.mining = int.Parse(ObscuredPrefs.GetString("mining"));
            PlayerInventory.amber = int.Parse(ObscuredPrefs.GetString("amber"));
            PlayerPrefsManager.DailyCount_Cheak = ObscuredPrefs.GetInt("DailyCount_Cheak");
            PlayerPrefsManager.isDailyCheak = ObscuredPrefs.GetInt("isDailyCheak") != 0? true : false;
            PlayerPrefsManager.ZogarkMissionCnt = ObscuredPrefs.GetInt("ZogarkMissionCnt", 0);
            PlayerPrefsManager.AmaAdsTimer = ObscuredPrefs.GetInt("AmaAdsTimer", 0);
            PlayerPrefsManager.FreeDiaCnt = ObscuredPrefs.GetInt("FreeDiaCnt", 0);
            PlayerPrefsManager.FreeWeaponCnt = ObscuredPrefs.GetInt("FreeWeaponCnt", 0);
            PlayerPrefsManager.isTutoAllClear = ObscuredPrefs.GetInt("isTutoAllClear") != 0 ? true : false;

        }

        /// 파일에서 데이터 불러와서 리스트에 대입
        PlayerPrefsManager.instance.JObjectLoad();
    }

    /// <summary>
    /// 인보크로 불러와줄것
    /// </summary>
    void InvoFirstWeapon()
    {
        ///기본무기 장착 Weapon_Add
        ListModel.Instance.Weapon_Add(0);
        ListModel.Instance.Weapon_Equip(0, true);

        /// 스트링[] 몽땅 저장
        PlayerPrefsManager.instance.TEST_SaveJson();
    }


    /// <summary>
    /// 테스트 버튼에 붙이자 -> json 리셋후 종료
    /// </summary>
    public void TEST_RESTE_JSON()
    {
        ListModel.Instance.supList.Clear();
        ListModel.Instance.charatorList.Clear();
        ListModel.Instance.invisibleheartList.Clear();
        ListModel.Instance.invisibleruneList.Clear();
        ListModel.Instance.weaponList.Clear();
        ListModel.Instance.petList.Clear();
        ListModel.Instance.shopList.Clear();
        ListModel.Instance.shopListSPEC.Clear();
        ListModel.Instance.shopListNOR.Clear();
        ListModel.Instance.mineCraft.Clear();
        //
        ObscuredPrefs.DeleteAll();
        PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    #region 파서는 DB 최종판이 되면 일괄 삭제해준다.

    /// <summary>
    /// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    /// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    /// </summary>
    void TA_Parser()
    {
        string[] line = ta.text.Substring(0, ta.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.supList.Add(new SupContent
            {
                index = row[0],
                supporterLevel = row[1],
                supporterName = row[2],
                maxTime = float.Parse(row[3]),
                currentEarnGold = double.Parse(row[4]),
                nextUpgradeNeed = double.Parse(row[5]),
                isEnable = row[6],
            });
        }
    }

    /// <summary>
    /// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    /// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    /// </summary>
    void TA_Parser_HV()
    {
        string[] line = _Heart.text.Substring(0, _Heart.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.invisibleheartList.Add(new HeartContent
            {
                index = row[0],
                heartLevel = row[1],
                maxLevel = row[2],
                powerToLvUP = float.Parse(row[3]),
                nextUpgradeNeed = double.Parse(row[4]),
                leafToLvUP = double.Parse(row[5]),
                dropTable = float.Parse(row[6]),
                descHead = row[7],
                descTail = row[8],
                imgIndex = row[9],
                heartName = row[10],
            });
        }
    }

    /// <summary>
    /// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    /// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    /// </summary>
    void TA_Parse_Charr()
    {
        string[] line = _char.text.Substring(0, _char.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.charatorList.Add(new CharatorContent
            {
                index = row[0],
                charLevel = row[1],
                charMaxLevel = row[2],
                nextUpgradeCost = float.Parse(row[3]),
                powerPer = float.Parse(row[4]),
                title = row[5],
                description = row[6],
            });
        }
    }

    /// <summary>
    /// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    /// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    /// </summary>
    void TA_Parser_Rune()
    {
        string[] line = _Rune.text.Substring(0, _Rune.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.invisibleruneList.Add(new RuneContent
            {
                index = row[0],
                imgIndex = row[1],
                rank = row[2],
                desc = row[3],
                rank_1_MIN = float.Parse(row[4]),
                rank_1_MAX = float.Parse(row[5]),
                rank_2_MIN = float.Parse(row[6]),
                rank_2_MAX = float.Parse(row[7]),
                rank_3_MIN = float.Parse(row[8]),
                rank_3_MAX = float.Parse(row[9]),
                rank_4_MIN = float.Parse(row[10]),
                rank_4_MAX = float.Parse(row[11]),
                rank_5_MIN = float.Parse(row[12]),
                rank_5_MAX = float.Parse(row[13]),
                isEquip = row[14],
            });
        }
    }

    /// <summary>
    /// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    /// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    /// </summary>
    void TA_Parser_Weapon()
    {
        string[] line = _weapon.text.Substring(0, _weapon.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.weaponList.Add(new WeaponContent
            {
                index = row[0],
                headRank = row[1],
                tailRank = row[2],
                weaponLevel = row[3],
                startPower = float.Parse(row[4]),
                increedPower = float.Parse(row[5]),
                nextUpgradeCost = float.Parse(row[6]),
                rankUpDia = float.Parse(row[7]),
                rankUpENstone = float.Parse(row[8]),
                startPassFail = float.Parse(row[9]),
                passFailPer = float.Parse(row[10]),
                isEnable = row[11],
                weaAmount = row[12],
            });
        }
    }

    /// <summary>
    /// 파서는 DB 최종판이 되면 일괄 삭제해준다.
    /// 파서에 사용된 txt 데이터도 일괄 삭제해준다.
    /// </summary>
    void TA_Parser_Pet()
    {
        string[] line = _PET.text.Substring(0, _PET.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.petList.Add(new PetContent
            {
                index = row[0],
                petLevel = row[1],
                needUpgrade = float.Parse(row[2]),
                usingTimeDam = float.Parse(row[3]),
                percentDam = float.Parse(row[4]),
                coolTime = float.Parse(row[5]),
                Desc = row[6],
                isEnable = row[7],
            });
        }
    }

    void TA_Parser_Shop()
    {
        string[] line = SHPSHPSHOP.text.Substring(0, SHPSHPSHOP.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            if (row[0] == "dia")
            {
                ListModel.Instance.shopList.Add(new ShopPrice
                {
                    category = row[0],
                    korDesc = row[1],
                    korPrice = row[2],
                    engDesc = row[3],
                    engPrice = row[4],
                    korTailDesc = row[5],
                    engTailDesc = row[6],
                });
            }
            else if (row[0] == "spec")
            {
                ListModel.Instance.shopListSPEC.Add(new ShopPrice
                {
                    category = row[0],
                    korDesc = row[1],
                    korPrice = row[2],
                    engDesc = row[3],
                    engPrice = row[4],
                    korTailDesc = row[5],
                    engTailDesc = row[6],
                });
            }
            else if (row[0] == "nor")
            {
                ListModel.Instance.shopListNOR.Add(new ShopPrice
                {
                    category = row[0],
                    korDesc = row[1],
                    korPrice = row[2],
                    engDesc = row[3],
                    engPrice = row[4],
                    korTailDesc = row[5],
                    engTailDesc = row[6],
                });
            }
            else if (row[0] == "pack")
            {
                ListModel.Instance.shopListPACK.Add(new ShopPrice
                {
                    category = row[0],
                    korDesc = row[1],
                    korPrice = row[2],
                    engDesc = row[3],
                    engPrice = row[4],
                    korTailDesc = row[5],
                    engTailDesc = row[6],
                });
            }
            else if (row[0] == "ama")
            {
                ListModel.Instance.shopListAMA.Add(new ShopPrice
                {
                    category = row[0],
                    korDesc = row[1],
                    korPrice = row[2],
                    engDesc = row[3],
                    engPrice = row[4],
                    korTailDesc = row[5],
                    engTailDesc = row[6],
                });
            }

        }

    }

    void TA_Parser_Mission()
    {
        string[] line = Misson.text.Substring(0, Misson.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            if (row[0] == "day")
            {
                ListModel.Instance.missionDAYlist.Add(new MissonSchool
                {
                    category = row[0],
                    korDesc = row[1],
                    engDesc = row[2],
                    reword = row[3],
                    refreshMulti = row[4],
                    maxValue = row[5],
                    curentValue = row[6],
                    rewordAmount = row[7],
                });
            }
            else if (row[0] == "all")
            {
                ListModel.Instance.missionALLlist.Add(new MissonSchool
                {
                    category = row[0],
                    korDesc = row[1],
                    engDesc = row[2],
                    reword = row[3],
                    refreshMulti = row[4],
                    maxValue = row[5],
                    curentValue = row[6],
                    rewordAmount = row[7],
                });
            }
            else if (row[0] == "tuto")
            {
                ListModel.Instance.missionTUTOlist.Add(new MissonSchool
                {
                    category = row[0],
                    korDesc = row[1],
                    engDesc = row[2],
                    reword = row[3],
                    refreshMulti = row[4],
                    maxValue = row[5],
                    curentValue = row[6],
                    rewordAmount = row[7],
                });
            }

        }

    }

    void TA_Parser_Mine()
    {
        string[] line = _mine.text.Substring(0, _mine.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.mineCraft.Add(new MineCraft
            {
                stage  = row[0],
                mine_hp = float.Parse(row[1]),
                reword_es = row[2],
                reword_ama = row[3],
                unlockDia = row[4],
                isEnable = row[5],
            });
        }
    }


    void TA_Parser_Swamp()
    {
        string[] line = _Swamp.text.Substring(0, _Swamp.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            ListModel.Instance.swampCaveData.Add(new SwampCave
            {
                stageLevel = float.Parse(row[0]),
                monsterHP = float.Parse(row[1]),
                rewordLeaf = float.Parse(row[2]),
                rewordEnchant = float.Parse(row[3]),
                killCount = row[4],
            });
        }
    }


    #endregion





}
