﻿using EasyMobile;
using PlayNANOO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CodeStage.AntiCheat.Storage;

public class NanooManager : MonoBehaviour
{
    private static string NanooRankTable = "amazon-RANK-92E2A6C2-7EE8CE7E";
    private static string userID = "Unknown"; // 유저 ID 저장하라.
    private static string userNickname = "Unranked"; // 닉네임 랭킹에 표기



    [Header("- 나누 재화 테스트")]
    public Text text1;
    public Text text2;
    public Text text3;

    [Header("- 플레이어 닉네임 표기")]
    public Text outterNameText;
    public Text innerNameText;
    [Header("- 선물함 관련")]
    public GameObject IncorectPanel;
    public GameObject GetPanel;
    public Image IconImg;
    public Text IconText;
    public PresentManager pm;
    [Header("- 쿠폰 입력 칸")]
    public Text InputText;
    private string[,] tmpUID = new string[128, 4];


    Plugin plugin;
    public PlayFabManage playfabm;
    public RankManager rm;


    void Start()
    {
        StartCoroutine(NickUpd());
    }


    public void CurrencyAll()
    {
        plugin.CurrencyAll((status, errorMessage, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                string textitem = "";
                foreach (Dictionary<string, object> item in (ArrayList)values["items"])
                {
                    textitem += item["currency"] + " : " + item["amount"] + System.Environment.NewLine;
                    Debug.LogWarning(item["currency"] + " : " + item["amount"]);
                }
                /// 테스트 텍스트 표기
                text1.text = textitem;
            }
            else
            {
                Debug.LogWarning("CurrencyAll Fail");
            }
        });
    }

    /// <summary>
    /// 재화 조회
    /// </summary>
    /// <param name="_code"></param>
    public void CurrencyGet(string _code)
    {
        plugin.CurrencyGet(_code, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning(values["amount"]);
            }
            else
            {
                Debug.LogWarning("Fail");
            }
        });
    }

    public void CurrencySet()
    {
        CurrencySet("ES", 1);
        CurrencySet("LF", 1);
        CurrencySet("DM", 1);
        // 갱신
        Invoke(nameof(CurrencyAll), 0.6f);
    }

    /// <summary>
    /// 재화 등록
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_amount"></param>
    public void CurrencySet(string _code, long _amount)
    {
        plugin.CurrencySet(_code, _amount, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning(values["amount"]);
                CurrencySubtract(_code, 1);
            }
            else
            {
                Debug.LogWarning("Fail");
                text1.text = "Fail";
            }
        });
    }

    public void CurrencyCharge(string _code)
    {
        CurrencyCharge(_code, 10);
    }
    /// <summary>
    /// 재화 충전
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_amount"></param>
    public void CurrencyCharge(string _code, long _amount)
    {
        plugin.CurrencyCharge(_code, _amount, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning(values["amount"]);
                //text2.text = _code + " : " + values["amount"];
                CurrencyAll();
            }
            else
            {
                Debug.LogWarning("Fail");
                text1.text = "Fail";
            }
        });
    }


    public void CurrencySubtract(string _code)
    {
        CurrencySubtract(_code, 10);
    }
    /// <summary>
    /// 재화 차감
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_amount"></param>
    public void CurrencySubtract(string _code, long _amount)
    {
        plugin.CurrencySubtract(_code, _amount, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning(values["amount"]);
                //text3.text = _code + " : " + values["amount"];
                CurrencyAll();
            }
            else
            {
                Debug.LogWarning("Fail");
                text1.text = "Fail";
            }
        });
    }

    
    /// <summary>
    /// 이벤트 로그 작성
    /// </summary>
    public void LogWrite()
    {
        var messages = new PlayNANOO.Monitor.LogMessages();
        messages.Add(Configure.PN_LOG_DEBUG, "Message PN_LOG_DEBUG");
        //messages.Add(Configure.PN_LOG_INFO, "Message PN_LOG_INFO");
        //messages.Add(Configure.PN_LOG_ERROR, "Message PN_LOG_ERROR");

        plugin.LogWrite(new PlayNANOO.Monitor.LogWrite()
        {
            EventCode = "EVENT_TEST_CODE",
            EventMessages = messages
        });
    }




    /// <summary>
    /// 구글 로그인 되면 바로 닉네임 설정하게
    /// </summary>
    /// <returns></returns>
    private IEnumerator NickUpd()
    {
        yield return new WaitForSeconds(1f);

        if (!Social.localUser.authenticated)
        {
            GameServices.Init();
        } 

        yield return new WaitForSeconds(2.0f);

        if (StartManager.instance.isDebugMode)
        {
            playfabm.InitPlayfab(userID);
            Debug.LogWarning("디버그 모드임.");
            yield break;
        }

        if (Social.localUser.authenticated)
        {
            userID = Social.localUser.id;
            /// 구글 아이디 바탕으로 플레이팹도 초기화.
            playfabm.InitPlayfab(userID);
            // 로그인 됐으면 로딩바 올려준다.
            //isUserLogin = true;
        }
        else
        {
            StartManager.instance.headChatTxt.text = "";
            retryCnt++;
            StartManager.instance.headChatTxt.DOText("로그인 재시도 " + retryCnt + "회...", 1f);

            StartCoroutine(NickUpd());
        }
    }
    int retryCnt;

    /// <summary>
    /// 닉네임 설정하면 다시 설정
    /// </summary>
    /// <param name="_name"></param>
    public  void SetReNickName(string _name)
    {
        userNickname = _name;
        plugin = Plugin.GetInstance();
        plugin.SetUUID(userID);
        plugin.SetNickname(_name);
        /// 언어셋팅
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            plugin.SetLanguage(Configure.PN_LANG_KO);
        }
        else
        {
            plugin.SetLanguage(Configure.PN_LANG_EN);
        }
        /// 텍스트 갱신
        outterNameText.text = _name;
        innerNameText.text = _name;
        /// 개인 기록 관련
        StartCoroutine(MyRankiner());
    }

    IEnumerator MyRankiner()
    {
        while (!PlayerPrefsManager.isLoadingComp)
        {
            yield return new WaitForFixedUpdate();
        }

        if (PlayerInventory.RecentDistance > 0)
        {
            /// 랭킹 기록
            RecordRankDistance(Mathf.RoundToInt((float)PlayerInventory.RecentDistance) - 1);
            /// 개인 랭킹 갱신
            Invoke(nameof(ShowRankingPersonal), 3.0f);
        }
    }


    /// <summary>
    /// Open Forum
    /// </summary>
    public void OpenForum()
    {
        plugin.OpenForum();
    }


    /// <summary>
    /// Open HelpDesk
    /// </summary>
    public void OpenHelpDesk()
    {
        plugin.SetHelpDeskOptional("OptionTest1", "ValueTest1");
        plugin.SetHelpDeskOptional("OptionTest2", "ValueTest2");
        plugin.OpenHelpDesk();
    }

    /// <summary>
    /// Open Forum Banner
    /// </summary>
    public void OpenBanner()
    {
        plugin.OpenBanner();
        /// 서버타임
        ServerTime();

        /// 나누 재화 set
        //CurrencySet("DM", 100);

        CurrencyAll();
    }



    /// <summary>
    /// 쿠폰 입력 버튼에 연결
    /// </summary>
    public void Coupon()
    {
        string inputTmp = InputText.text;

        plugin.Coupon(inputTmp, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                string item_code = dictionary["item_code"].ToString();
                item_code = item_code.Replace("\"", "");
                string item_count = dictionary["item_count"].ToString();
                item_count = item_count.Replace("\"", "");

                InputText.transform.parent.GetComponent<InputField>().text = "";

                IconText.text = "";
                IconImg.sprite = null;
                /// 팝업 생성
                isCouponPost = true;
                PostboxItemSend(item_code, int.Parse(item_count), "");
                //
            }
            else
            {
                Debug.Log("Coupon Fail");
                InputText.transform.parent.GetComponent<InputField>().text = "";

                /// 쿠폰 에러 팝업 SetActive
                IncorectPanel.SetActive(true);
            }
        });
    }

    /// <summary>
    /// 쿠폰 입력해서 받을때만 활성화.
    /// </summary>
    bool isCouponPost;




    /// <summary>
    /// 우편함에 뭐 들어와있나? 빨간점 체크
    /// </summary>
    public void PostboxCheck()
    {
        AccessEvent();
        //
        plugin.PostboxItem((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                /// 우편함에 아이템 없음
                RedDotManager.instance.RedDot[1].SetActive(false);
                RedDotManager.instance.RedDot[2].SetActive(false);
                //
                ArrayList items = (ArrayList)dictionary["item"];
                foreach (Dictionary<string, object> item in items)
                {
                    //Debug.LogWarning(item["uid"]);
                    //Debug.LogWarning(item["message"]);
                    //Debug.LogWarning(item["item_code"]);
                    //Debug.LogWarning(item["item_count"]);
                    //Debug.LogWarning(item["expire_sec"]);
                    /// 우편함에 아이템 있다
                    RedDotManager.instance.RedDot[1].SetActive(true);
                    RedDotManager.instance.RedDot[2].SetActive(true);
                }
            }
            else
            {
                Debug.Log("PostboxItem Fail");
            }
        });


    }

    /// <summary>
    /// 선물함 팝업창 열 때 호출. Item (5)
    /// 서버에서 데이터 우편 불러와서 프리팹 생성
    /// </summary>
    public void Postbox()
    {
        SystemPopUp.instance.LoopLoadingImg();
        Invoke(nameof(InvoStopLoop), 5.0f);
        int _index = 0;
        //
        string uid = string.Empty;
        string item_code = string.Empty;
        string item_count = string.Empty;
        string _message = string.Empty;
        //
        plugin.PostboxItem((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList items = (ArrayList)dictionary["item"];
                foreach (Dictionary<string, object> item in items)
                {
                    uid = item["uid"].ToString();
                    tmpUID[_index, 0] = uid.Replace("\"", "");

                    item_code = item["item_code"].ToString();
                    tmpUID[_index, 1] = item_code.Replace("\"", "");

                    item_count = item["item_count"].ToString();
                    tmpUID[_index, 2] = item_count.Replace("\"", "");

                    _message = item["message"].ToString();
                    tmpUID[_index, 3] = _message.Replace("\"", "");

                    PostSendAddItem(_index);

                    _index++;

                    /// 50개 까지만 인게임 표현 , 그 이상은 서버에 저장
                    if (_index > 49) break;
                }
                /// 포문 돌았는데 0개다 = 모두받기 회색.
                if (_index < 1)
                {
                    pm.SetBtnBlueAllcept(false);
                }
                else
                {
                    pm.SetBtnBlueAllcept(true);
                }

                Debug.LogError("PostboxItem Sucess");
                SystemPopUp.instance.StopLoopLoading();
            }
            else
            {
                Debug.LogError("PostboxItem Fail");
                SystemPopUp.instance.StopLoopLoading();
            }
        });


    }

    void InvoStopLoop()
    {
        SystemPopUp.instance.StopLoopLoading();
    }


    /// <summary>
    /// 우편함에 추가하기.
    /// </summary>
    /// <param name="_indx"></param>
    void PostSendAddItem(int _indx)
    {
        //Debug.LogWarning("_indx : " + _indx);

        string uid = tmpUID[_indx, 0];
        string item_code = tmpUID[_indx, 1];
        string item_count = tmpUID[_indx, 2];
        string message = tmpUID[_indx, 3];
        //Debug.LogWarning("message : " + message);
        //
        pm.AddPresent(item_code, item_count, uid, message);
    }

    /// <summary>
    /// 선물함으로 이동되었습니다! 팝업
    /// </summary>
    public void PostboxItemSend(string _code, int _amount, string _msg)
    {
        plugin.PostboxItemSend(_code, _amount, 365, _msg, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                if (isCouponPost)
                {
                    IconText.text = "x" + _amount.ToString("N0");
                    switch (_code)
                    {
                        case "weapon_coupon": IconImg.sprite = pm.IconSprs[0]; break;
                        case "reinforce_box": IconImg.sprite = pm.IconSprs[1]; break;
                        case "leaf_box": IconImg.sprite = pm.IconSprs[2]; break;
                        case "E_box": IconImg.sprite = pm.IconSprs[3]; break;
                        case "D_box": IconImg.sprite = pm.IconSprs[4]; break;
                        case "C_box": IconImg.sprite = pm.IconSprs[5]; break;
                        case "B_box": IconImg.sprite = pm.IconSprs[6]; break;
                        case "A_box": IconImg.sprite = pm.IconSprs[7]; break;
                        case "S_box": IconImg.sprite = pm.IconSprs[8]; break;
                        case "L_box": IconImg.sprite = pm.IconSprs[9]; break;
                        case "pvp": IconImg.sprite = pm.IconSprs[10]; break;
                        case "cave": IconImg.sprite = pm.IconSprs[11]; break;

                        /// 아마존 포션 crystal / S_leaf_box

                        case "crystal": IconImg.sprite = pm.IconSprs[12]; break;
                        /// 얘는 아마존 결정
                        case "stone": IconImg.sprite = pm.IconSprs[13]; break;
                        
                        case "reinforce": IconImg.sprite = pm.IconSprs[14]; break;
                        case "gold": IconImg.sprite = pm.IconSprs[15]; break;
                        case "leaf": IconImg.sprite = pm.IconSprs[16]; break;
                        case "diamond": IconImg.sprite = pm.IconSprs[17]; break;
                        case "cave_clear": IconImg.sprite = pm.IconSprs[18]; break;
                        case "elixr": IconImg.sprite = pm.IconSprs[19]; break;
                        /// 아마존 포션
                        case "S_leaf_box": IconImg.sprite = pm.IconSprs[12]; break;
                            //
                        case "S_reinforce_box": IconImg.sprite = pm.IconSprs[21]; break;
                        case "mining": IconImg.sprite = pm.IconSprs[22]; break;
                        case "amber": IconImg.sprite = pm.IconSprs[23]; break;
                        //

                        case "Crazy_dia": IconImg.sprite = pm.IconSprs[24]; break;
                        case "Crazy_elixr": IconImg.sprite = pm.IconSprs[25]; break;
                        default: break;
                    }
                    /// 획득팝업 SetActive
                    GetPanel.SetActive(true);
                    /// 쿠폰 입력 false
                    isCouponPost = false;
                }
                Debug.Log("PostboxItemSend Pass");
                //
                PostboxCheck();
            }
            else
            {
                Debug.Log("PostboxItemSend Fail");
            }
        });
    }


    /// <summary>
    /// 팝업 없이 바로 우편함 들어가는 코드
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_amount"></param>
    public void PostboxDailySend(string _code, int _amount)
    {
        plugin.PostboxItemSend(_code, _amount, 365, "", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                PostboxCheck();
            }
            else
            {
                Debug.Log("PostboxItemSend Fail");
            }
        });
    }



    /// <summary>
    /// Item Use in InBox
    /// </summary>
    public void PostboxItemUse(string _UID)
    {
        plugin.PostboxItemUse(_UID, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                string item_code = dictionary["item_code"].ToString();
                item_code = item_code.Replace("\"", "");
                string item_count = dictionary["item_count"].ToString();
                item_count = item_count.Replace("\"", "");
                //
                CouponCheak(item_code, item_count);
                Debug.Log("PostboxItemUse Pass");
                // 우편함 물건 다 없어졌나 확인.
                PostboxCheck();
            }
            else
            {
                Debug.Log("PostboxItemUse Fail");
            }
        });
    }

    public void PostboxRedDot()
    {
        pm.RedotAssemble();
    }

    /// <summary>
    /// PostboxItemUse에 서 호출해서 인벤토리로 습득
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_count"></param>
    public void CouponCheak(string _code, string _count)
    {
        switch (_code)
        {
            case "weapon_coupon":
                PlayerInventory.SetBoxsCount("weapon_coupon", int.Parse(_count));
                break;
            case "reinforce_box":
                PlayerInventory.SetTicketCount("reinforce_box", int.Parse(_count));
                break;
            case "leaf_box":
                PlayerInventory.SetTicketCount("leaf_box", int.Parse(_count));
                break;
            case "E_box":
                CheckTutoWeapon();
                PlayerInventory.SetBoxsCount("E", int.Parse(_count));
                break;
            case "D_box":
                CheckTutoWeapon();
                PlayerInventory.SetBoxsCount("D", int.Parse(_count));
                break;
            case "C_box":
                CheckTutoWeapon();
                PlayerInventory.SetBoxsCount("C", int.Parse(_count));
                break;
            case "B_box":
                CheckTutoWeapon();
                PlayerInventory.SetBoxsCount("B", int.Parse(_count));
                break;
            case "A_box":
                CheckTutoWeapon();
                PlayerInventory.SetBoxsCount("A", int.Parse(_count));
                break;
            case "S_box":
                CheckTutoWeapon();
                PlayerInventory.SetBoxsCount("S", int.Parse(_count));
                break;
            case "L_box":
                CheckTutoWeapon();
                PlayerInventory.SetBoxsCount("L", int.Parse(_count));
                break;
            case "pvp":
                PlayerInventory.SetTicketCount("pvp", int.Parse(_count));
                break;
            case "cave":
                PlayerInventory.SetTicketCount("cave_enter", int.Parse(_count));
                break;



            case "crystal":
                //PlayerInventory.AmazonStoneCount += long.Parse(_count);
                ///// 결정조각  업적  카운트
                //ListModel.Instance.ALLlist_Update(2, long.Parse(_count));
                PlayerInventory.SetTicketCount("S_leaf_box", int.Parse(_count));

                break;



            case "stone":
                PlayerInventory.Money_AmazonCoin += long.Parse(_count);
                break;
            case "reinforce":
                PlayerInventory.Money_EnchantStone += double.Parse(_count);
                ///  강화석 업적 카운트 올리기
                ListModel.Instance.ALLlist_Update(5, double.Parse(_count));
                break;
            case "gold":
                PlayerInventory.Money_Gold += double.Parse(_count);
                ///  골드 업적 카운트 올리기
                ListModel.Instance.ALLlist_Update(3, double.Parse(_count));
                break;
            case "leaf":
                PlayerInventory.Money_Leaf += double.Parse(_count);
                /// 나뭇잎 획득량 업적 올리기
                ListModel.Instance.ALLlist_Update(4, double.Parse(_count));
                break;
            case "diamond":
                PlayerInventory.Money_Dia += long.Parse(_count);
                break;
            case "cave_clear":
                PlayerInventory.SetTicketCount("cave_clear", int.Parse(_count));
                break;
            case "elixr":
                PlayerInventory.Money_Elixir += long.Parse(_count);
                break;
                //
            case "S_leaf_box":
                PlayerInventory.SetTicketCount("S_leaf_box", int.Parse(_count));
                break;
            case "S_reinforce_box":
                PlayerInventory.SetTicketCount("S_reinforce_box", int.Parse(_count));
                break;
            case "mining":
                PlayerInventory.SetTicketCount("mining", int.Parse(_count));
                break;
            case "amber":
                PlayerInventory.SetTicketCount("amber", int.Parse(_count));
                break;
            //
            case "Crazy_dia":
                PlayerInventory.SetTicketCount("Crazy_dia", int.Parse(_count));
                break;
            case "Crazy_elixr":
                PlayerInventory.SetTicketCount("Crazy_elixr", int.Parse(_count));
                break;

            default: break;
        }
        /// 로컬 저장
        PlayerPrefsManager.instance.TEST_SaveJson();
    }

    /// <summary>
    /// 튜토리얼 미션의 무기상자 획득하기 체크
    /// </summary>
    void CheckTutoWeapon()
    {
        /// 무기 상자 획득하기
        if (PlayerPrefsManager.currentTutoIndex == 34) ListModel.Instance.TUTO_Update(34);
        else if (PlayerPrefsManager.currentTutoIndex == 39) ListModel.Instance.TUTO_Update(39);
        else if (PlayerPrefsManager.currentTutoIndex == 43) ListModel.Instance.TUTO_Update(43);
        else if (PlayerPrefsManager.currentTutoIndex == 49) ListModel.Instance.TUTO_Update(49);
    }

    public void PostboxDelete()
    {
        plugin.PostboxClear((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                PostboxCheck();
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// Android Verification 안드로이드 영수증 검증
    /// </summary>
    public void IapReceiptionAndroid(string _PRODUCT_ID, string _RECEIPT, string _SIGNATURE, string _CURRENCY, double _Price)
    {

        plugin.ReceiptVerificationAOS(_PRODUCT_ID, _RECEIPT, _SIGNATURE, _CURRENCY, _Price, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning(dictionary["package"]);
                Debug.LogWarning(dictionary["product_id"]);
                Debug.LogWarning(dictionary["order_id"]);
                Debug.LogWarning("IapReceiptionAndroid Issue Item");

            }
            else
            {
                Debug.LogWarning("IapReceiptionAndroid Fail");
            }
        });
    }
    

    /// <summary>
    /// 랭킹에 기록
    /// </summary>
    /// <param name="_score"></param>
    public void RecordRankDistance(int _score)
    {
        plugin.RankingRecord(NanooRankTable, _score, _score.ToString(), (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else Debug.Log("Fail");
        });
    }


    /// <summary>
    /// 랭킹 호출 (내부에 개인 기록 호출까지 동시에)
    /// </summary>
    public void ShowRankDistance()
    {
        /// 개인 기록 보여줌
        ShowRankingPersonal();
        
        plugin.Ranking(NanooRankTable, 50, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList list = (ArrayList)dictionary["list"];
                foreach (Dictionary<string, object> rank in list)
                {
                    Debug.Log(rank["nickname"]);
                    Debug.Log(rank["score"]);
                    Debug.Log(rank["data"]);

                    rm.rankList.Add(new RankData
                    {
                        _nickname = rank["nickname"].ToString(),
                        _score = rank["score"].ToString(),
                    });
                }
                /// 포문 다 돌면 디스플레이
                rm.InitRankPage();
            }
            else
            {
                /// 랭킹 호출 실패하면 뺑뺑이 꺼줌
                SystemPopUp.instance.StopLoopLoading();
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// 나의 나누 랭킹
    /// </summary>
    public void ShowRankingPersonal()
    {
        rm.personalText[1].text = userNickname;
        plugin.RankingPersonal(NanooRankTable, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["nickname"]);
                Debug.Log(dictionary["ranking"]);
                Debug.Log(dictionary["score"]);
                Debug.Log(dictionary["total_player"]);

                rm.result[0] = dictionary["ranking"].ToString() ;
                rm.result[1] = userNickname;
                rm.result[2] = dictionary["score"].ToString();

                rm.ShowPersonal();
            }
            else
            {
                /// 랭킹 호출 실패하면 뺑뺑이 꺼줌
                SystemPopUp.instance.StopLoopLoading();
                Debug.LogError(" ShowRankingPersonal Fail");
            }
        });
    }



    /// <summary>
    /// Server Time
    /// </summary>
    public void ServerTime()
    {
        plugin.ServerTime((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning ("timezone : " + dictionary["timezone"]);
                Debug.LogWarning("timezone : " + dictionary["timestamp"]);
                Debug.LogWarning("timezone : " + dictionary["ISO_8601_date"]);
                Debug.LogWarning("timezone : " + dictionary["date"]);
                //
                ConvertFromUnixTimestamp(dictionary["timestamp"].ToString());
            }
            else
            {
                Debug.Log("Fail");
                ConvertFromUnixTimestamp("fail");
            }
        });
    }

    void ConvertFromUnixTimestamp(string timestamp)
    {
        if (timestamp == "fail")
        {
            /// TODO 서버시간 못 받아오면 처리 어케?
        }
        else
        {
            System.DateTime origin = new System.DateTime(1970, 1, 1, 9, 0, 0, 0);
            Debug.LogError("나누 서버 타임 : " + origin.AddSeconds(double.Parse(timestamp)));
        }
    }




    private void OnApplicationFocus(bool focus)
    {
        if (plugin != null && focus)
        {
            AccessEvent();
        }
    }

    private void AccessEvent()
    {
        plugin.AccessEvent((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                if (dictionary.ContainsKey("server_timestamp"))
                {
                    Debug.Log(dictionary["server_timestamp"]);
                }

                if (dictionary.ContainsKey("postbox_subscription"))
                {
                    foreach (Dictionary<string, object> subscription in (ArrayList)dictionary["postbox_subscription"])
                    {
                        Debug.Log(subscription["product"]);
                        Debug.Log(subscription["ttl"]);
                    }
                }

                if (dictionary.ContainsKey("invite_rewards"))
                {
                    foreach (Dictionary<string, object> invite in (ArrayList)dictionary["invite_rewards"])
                    {
                        Debug.Log(invite["item_code"]);
                        Debug.Log(invite["item_count"]);
                    }
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }






}
