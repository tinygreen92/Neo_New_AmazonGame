﻿using CodeStage.AntiCheat.Storage;
using Lean.Localization;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayFabManage : MonoBehaviour
{
    [Header("- 버전 닉네임")]
    public Text versionTxt;
    public Text uidTxt;
    public Text versionConfigTxt;
    public Text uidConfigTxt;
    [Header("- 매니저 놈들")]
    public ModelHandler mh;
    public IntroManager im;
    public NanooManager nm;
    public PhotonChatManager pcm;
    [Header("- 닉네임 설정 자식 원투")]
    public InputField nickInputText;

    private string URL = "com.lab525.new_amazon";

    /// <summary>
    ///  플레이 팹에서 사용하는 숫자+영문
    /// </summary>
    private string myPlayFabId;
    /// <summary>
    /// 유저가 지정한 플레이팹 닉네임
    /// </summary>
    private string myDisplayName;           // GPGSManager.GetLocalUserName()


    //public void UpdateUserReadOnlyData()
    //{
    //    PlayFabServerAPI.UpdateUserReadOnlyData(new UpdateUserDataRequest()
    //    {
    //        PlayFabId = "user PlayFabId here - obtained from any successful LoginResult",
    //        Data = new Dictionary<string, string>() {
    //        {"Father", "Fred"},
    //        {"Mother", "Alice"},
    //        {"Sister", "Lucy"},
    //        {"Brother", "Doug"}
    //    },
    //        Permission = UserDataPermission.Public
    //    },
    //    result => Debug.Log("Set read-only user data successful"),
    //    error => {
    //        Debug.Log("Got error updating read-only user data:");
    //        Debug.Log(error.GenerateErrorReport());
    //    });
    //}


    #region <플레이팹 로그인 관련>

    // Start is called before the first frame update
    public void InitPlayfab(string _cusId)
    {
        /// 플래이팹 로그인 확인되면 ModelHandler 로딩
        PlayerPrefsManager.CursedId = _cusId;
        /// 회원정보 표기
        uidTxt.text = "uid : " + _cusId;
        uidConfigTxt.text = "uid : " + _cusId;
        versionTxt.text = "ver : " + Application.version;
        versionConfigTxt.text = "ver : " + Application.version;
        /// 로컬 데이터 준비
        mh.TunaDataLoad();

        /// 플레이팹 로그인
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "947D2";
        }
        /// TODO : CustomId 는 구글 ID 형식 :  a_10220022002392
        var request = new LoginWithCustomIDRequest { CustomId = _cusId, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request,
                                           OnLoginSuccess,
                                           (error) =>
                                           {
                                               Debug.LogWarning("Something went wrong with your first API call.  :(");
                                               Debug.LogWarning("Here's some debug information:");
                                               Debug.LogWarning(error.GenerateErrorReport());
                                               /// 차단 계정 메시지는 여기로
                                               StartManager.instance.headChatTxt.text = error.GenerateErrorReport();
                                           });
    }

    private void OnLoginSuccess(LoginResult _result)
    {
        /// result.PlayFabId 플레이팹 아이디 = 구글 랭킹용
        myPlayFabId = _result.PlayFabId;
        /// 섹터 5 없는 사람 예외처리
        //GetUserSector5();
        // ------------- 테스트 --------------------
        /// 아마존 게임내 공통 데이터 호출  - 버전 코드 불러오기용 테스트
        ClientGetTitleData();
        /// 서버 인벤토리 열어서 유료화폐 보여 주기
        //GetVirtualCurrency("");
        // ------------- 테스트 --------------------
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = myPlayFabId },
                                        GetAccountSuccess,
                                        (error) => Debug.LogError(" GetAccountInfo error"));
    }

    /// <summary>
    /// 정상적으로 플레이팹 접근했으면 얘가 디스플레이 네임을 지정했는지 여부 조사
    /// </summary>
    /// <param name="result"></param>
    private void GetAccountSuccess(GetAccountInfoResult obj)
    {
        myDisplayName = obj.AccountInfo.TitleInfo.DisplayName;

        if (myDisplayName == null || myDisplayName == myPlayFabId)          /// 닉네임 설정이 안되었다 ||  혹은 도중에 취소했다 (임시로 구글 아이디로 저장)
        {
            PopUpManager.instance.ShowPopUP(0);                     /// 개인정보 이용약관
            PopUpManager.instance.ShowPopUP(5);                     /// 닉네임 설정 팝업창.
        }
        else
        {
            /// 플레이팹 로딩 완료
            PlayerPrefsManager.isLoadingComp = true;
            /// 닉네임 설정되어 있네? 바로 접속
            pcm.PhotonStart(myDisplayName);
            nm.SetReNickName(myDisplayName);
            PlayerPrefsManager.isNickNameComp = true;
        }
    }

    /// <summary>
    /// 이용약관
    /// </summary>
    public void OpenE_Dragon()
    {
        Application.OpenURL("https://docs.google.com/document/d/1UN7PaKHUl12bNxyPvG-JQ6Saevz4HxYJVSPXO60UsBs/edit?usp=sharing");
    }

    /// <summary>
    /// 이용약관
    /// </summary>
    public void OpenDOG_DRAGON()
    {
        Application.OpenURL("https://docs.google.com/document/d/1HeDnju0OqyWmVDlB5R5w1W-md6hvrhPPlGqcTEWE2AA/edit?usp=sharing");
    }


    /// <summary>
    ///5.SetNickNameCanvas 에서 확인 누르면 닉네임 중복확인 해보기
    /// </summary>
    public void CheckSameName()
    {
        /// 유저 디스플레이 네임 세팅 nickInputText
        UpdateUserName(nickInputText.text);
    }

    /// <summary>
    /// 팝업에서 닉네임 확정
    /// </summary>
    public void OkayMyNick()
    {
        /// 플레이팹 로딩 완료
        PlayerPrefsManager.isLoadingComp = true;
        /// 닉네임 세팅하고 게임 접속
        pcm.PhotonStart(myDisplayName);
        nm.SetReNickName(myDisplayName);
        /// TODO : 인트로 만화 재생.
        im.StartIntro();
    }

    /// <summary>
    /// 으로 하시겠습니까? -> 취소 누르면 다시 입력 기회 줌
    /// </summary>
    public void NoThisName()
    {
        /// 널 되돌려 달라고
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = myPlayFabId },
        (result) => myDisplayName = myPlayFabId,
        (error) => Debug.LogError(error.GenerateErrorReport()));
    }

    /// <summary>
    /// 닉네임 중복확인
    /// </summary>
    /// <param name="_dpName">방금 입력한 닉네임</param>
    void UpdateUserName(string _dpName)
    {
        /// 유저 디스플레이 네임 세팅
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = _dpName },
        (result) =>
        {
            Debug.LogWarning("유저 디스플레이 네임 : " + result.DisplayName);
            myDisplayName = _dpName;
            /// 이걸로 할래? -> 생성하시겠습니까?
            PopUpManager.instance.SetNickCheckTxt(_dpName);
            PopUpManager.instance.ShowNickPanel(2);
        },
        (error) =>
        {
            Debug.LogError(error.GenerateErrorReport());
            /// 중복 !! 경고 팝업 호출 -> 중복된 닉네임입니다.
            PopUpManager.instance.ShowNickPanel(1);
        });
    }



    #endregion

    /// <summary>
    /// 타이틀 데이터 조회 // GetUserData
    /// 모든 게임 계정에 공통으로 불러와지는 값 -> 웹에서 수정 가능 (공지 ?) // 버전 코드 체크!!
    /// </summary>
    public void ClientGetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                /// 클라이언트 버전 체크용으로 사용
                if (result.Data == null || !result.Data.ContainsKey("CurrentVersion"))
                {
                    Debug.LogWarning("No CurrentVersion");
                }
                /// 데이터 호출 성공시.
                else
                {
                    /// 최신 버전이면 패스.
                    if (Application.version == result.Data["CurrentVersion"] || Application.version == result.Data["TEST_Mode"])
                    {
                        PlayerPrefsManager.isMissingFive = true;
                        /// 공지사항 넣어주고
                        PlayerPrefsManager.instance.CH_NOTICE = result.Data["ChatNotice"];
                    }
                    else
                    {
                        PopUpManager.instance.ShowPopUP(31);
                    }

                }
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }


    /// <summary>
    /// 로그인 할때 버전 체크 말고 채팅창 공지사항 불러올때
    /// 포톤에서 사용해야 하나?
    /// </summary>
    /// <param name="_Func"></param>
    public void ClientGetTitleData(string _Func)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                /// 예외처리
                if (result.Data == null || !result.Data.ContainsKey("ChatNotice"))
                {
                    Debug.LogWarning("No ChatNotice");
                }
                /// 데이터 호출 성공시.
                else
                {
                    PlayerPrefsManager.instance.CH_NOTICE = result.Data["ChatNotice"];
                }
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }




    ///----------------------------------------------------------------------------------------------------------------------
    ///----------------------------------------------------------------------------------------------------------------------
    ///----------------------                                        데이터 수정 / 호출 부분                                               ---------------------
    ///----------------------------------------------------------------------------------------------------------------------
    ///----------------------------------------------------------------------------------------------------------------------






    ///// <summary>
    ///// 유료 재화를 서버에서 불러오기
    ///// </summary>
    ///// <param name="_CurrCode">공백""하면 모든 재화 다 불러서 저장.</param>
    //public int GetVirtualCurrency(string _CurrCode)
    //{
    //    int iResult = 0;

    //    PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
    //    (result) =>
    //    {
    //        switch (_CurrCode)
    //        {
    //            case "DA":
    //                iResult = result.VirtualCurrency["DA"];
    //                break;

    //            case "LF":
    //                iResult = result.VirtualCurrency["LF"];
    //                break;

    //            case "ES":
    //                iResult = result.VirtualCurrency["ES"];
    //                break;

    //            default:
    //                /// 최초 로그인시 서버 저장된 유료 재화 싹다 긁어옴
    //                //PlayerInventory.Money_Dia = result.VirtualCurrency["DA"];
    //                //PlayerInventory.Money_Leaf = result.VirtualCurrency["LF"];
    //                //PlayerInventory.Money_EnchantStone = result.VirtualCurrency["ES"];
    //                iResult = -1;
    //                //Debug.LogError("가상화폐 로딩 이 제일 느려 ");
    //                break;
    //        }
    //    },
    //    (error) =>
    //    {
    //        Debug.LogError(error.GenerateErrorReport());
    //        SceneManager.LoadScene(0);
    //    }

    //    );

    //    return iResult;
    //}


    ///// <summary>
    ///// 가상화폐 재고는 충분한지?
    ///// </summary>
    ///// <returns></returns>
    //public bool IsEnoughVC(string _CurrCode, int _amount)
    //{
    //    if (GetVirtualCurrency(_CurrCode) >= _amount)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}


    ///// <summary>
    ///// 유료 화폐 추가하기
    ///// </summary>
    ///// <param name="_CurrCode">DA / LF / ES</param>
    //public void AddVirtualCurrency(string _CurrCode, int _Amount)
    //{
    //    PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest() { VirtualCurrency = _CurrCode, Amount = _Amount },
    //                                            (result) => 
    //                                            { 
    //                                                Debug.LogWarning("돈 획득");
    //                                                /// 재화  표시기에 표기
    //                                                GetVirtualCurrency("");
    //                                            },
    //                                            (error) => 
    //                                            {
    //                                                Debug.LogWarning("돈 추가 에러");
    //                                                SceneManager.LoadScene(0);
    //                                            }
    //                                            );
    //}

    ///// <summary>
    ///// 유료화폐 소모 -> 상점 연동하는게 더 안전할지도
    ///// </summary>
    //public void SubVirtualCurrency(string _CurrCode, int _Amount)
    //{
    //    if (!IsEnoughVC(_CurrCode, _Amount))
    //    {
    //        Debug.LogError(_Amount + "보다 재화가 적음! 강제 return");
    //        return;
    //    }
    //    /// 서버에서 돈 소모
    //    PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = _CurrCode, Amount = _Amount },
    //                                            (result) =>
    //                                            {
    //                                                Debug.LogWarning("돈 소모");
    //                                                /// 재화  표시기에 표기
    //                                                GetVirtualCurrency("");
    //                                            },
    //                                            (error) =>
    //                                            {
    //                                                Debug.LogWarning("돈 소모 에러");
    //                                                SceneManager.LoadScene(0);
    //                                            }
    //                                            );
    //}

    /// <summary>
    /// JObjectSave 에서 넘어옴
    /// </summary>
    public void SetUserData(string dataBox, string realMayo, string _107jsonString)
    {
        /// 여기는 인터넷 연결 해서 처리하는 구간
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                /// 알파벳 순서 enemyAllData 에 차곡 차곡 10개씩 cnt / 10 == index 0 부터 시작
                /// { "SECTOR_0", PlayerPrefsManager.instance.LoadStringJsonn("isilpus")}, 
                { "SECTOR_0", Application.version},
                { "SECTOR_1", "1.0.7" },
                { "SECTOR_2", "1.0.7"},
                { "SECTOR_3", _107jsonString },
                { "SECTOR_4", PlayerPrefsManager.instance.ZZoGGoMiDataSave() },              /// 쪼꼬미 데이터
                { "SECTOR_5", dataBox},                                                                                            /// 인벤토리 데이터 박스
                { "SECTOR_6", PlayerInventory.isSuperUser.ToString() },                                         ///  광고 제거 구매 여부 저장
                { "SECTOR_7",  (PlayerInventory.RecentDistance -1d).ToString("F0") },                   /// 거리
                { "SECTOR_8", PlayerPrefsManager.instance.NonJsonDataOutput() },                /// 확장 가능한 NonJson 리스트 로컬 저장된거
                { "SECTOR_9", realMayo},                                                                                                    /// 실전압축 데이터 유물 / 룬 json
            },
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request,
            (result) =>
            {
                Debug.LogError("SECTOR_데이터 저장 성공!! " + myPlayFabId);
                /// 유저가 임의로 서버 저장
                if (PlayerPrefsManager.instance.isResetAferSave)
                {
                    /// 파일 덮어쓰고 종료
                    File.WriteAllText(Application.persistentDataPath + "/_data_", "n1u2l3l" + PlayerPrefsManager.CursedId);
                    /// 로컬 데이터 리셋 후 종료
                    ObscuredPrefs.DeleteAll();
                    PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                        #else
                                Application.Quit(); // 어플리케이션 종료
                        #endif
                }
                /// 시스템에서 서버 저장
                else
                {
                    SystemPopUp.instance.StopLoopLoading();
                }
            },
            (error) =>
            {
                Debug.LogError("SECTOR_데이터 저장 실패");
                SystemPopUp.instance.StopLoopLoading();
            }
            );
    }

    //void GetUserSector5()
    //{
    //    var request = new GetUserDataRequest() { PlayFabId = myPlayFabId };
    //    PlayFabClientAPI.GetUserData(request, (result) =>
    //    {
    //        /// 섹터 5 만 없을 경우 처리
    //        if (result.Data != null || !result.Data.ContainsKey("SECTOR_5"))        /// 예외처리
    //        {
    //            PlayerPrefsManager.isMissingFive = true;
    //            Debug.LogError("섹터 5 불러오기 성공");
    //        }
    //    },
    //    (error) =>
    //    {
    //        SystemPopUp.instance.StopLoopLoading();
    //        Debug.LogError("섹터 5 불러오기 실패");
    //    }
    //     );
    //}

    /// <summary>
    /// 서버 데이터에 남아있는 클라이언트 버전
    /// </summary>
    string curentVersion ="";

    /// <summary>
    ///  true - 쪼꼬미만 떼오기
    /// false - 서버 데이터로 덮어씌우고 재실행
    /// </summary>
    /// <param name="_isZZOgomi"></param>
    public void GetUserData(bool _isZZOgomi)
    {
        /// 새로운 영혼 씌우기
        //long tryResult = 0;
        double dtryResult = 0;
        int tryResultt = 0;
        SystemPopUp.instance.LoopLoadingImg();
        
        /// 플레이팹 연결
        var request = new GetUserDataRequest() { PlayFabId = myPlayFabId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            //if (result.Data == null || !result.Data.ContainsKey("SECTOR_0"))        /// 예외처리
            if (result.Data == null || !result.Data.ContainsKey("SECTOR_4"))        /// 예외처리
            {
                Debug.LogError("쪼꼬미 데이터 없음");
                PlayerPrefsManager.instance.ZZoGGoMiDataLoad(null);
                PlayerPrefsManager.isLoadingComp = true;
                SystemPopUp.instance.StopLoopLoading();
            }
            else  /// 실제로 실행할 동작 !!! 여기 리스트 형식 바뀌면 실행 오류 남(PC에서 안 잡힘)
            {
                if (_isZZOgomi)
                {
                    Debug.LogError("쪼꼬미 데이터 로드");
                    //Debug.LogError("SECTOR_4 (isSuperUser): " + result.Data["SECTOR_4"].Value);
                    PlayerPrefsManager.instance.ZZoGGoMiDataLoad(result.Data["SECTOR_4"].Value);
                    SystemPopUp.instance.StopLoopLoading();
                }
                else
                {
                    /// 남아있는 찌꺼기 제거
                    //ObscuredPrefs.DeleteAll();
                    //PlayerPrefs.DeleteAll();

                    /// TODO : 불러오기 할때 _data_ 데이터 있을 때만 덮어쓰기 1.0.1 이슈 
                    if (result.Data.ContainsKey("SECTOR_5") && result.Data.ContainsKey("SECTOR_0"))
                    {
                        curentVersion = result.Data["SECTOR_0"].Value;
                        //Debug.LogError("SECTOR_5 (JsonData): " + result.Data["SECTOR_5"].Value); // 제이와피
                        if (curentVersion == "1.0.1" || curentVersion == "1.0.2" || curentVersion == "1.0.3" || curentVersion == "1.0.4" || curentVersion == "1.0.5")
                        {
                            Debug.LogError(" 해당 1.0.6 이전 버전 데이터 처리 서버 데이터 파일로 써줌. ");
                            /// 1.0.6 이전 버전 데이터 처리
                            File.WriteAllText(Application.persistentDataPath + "/_data_", result.Data["SECTOR_5"].Value);
                        }
                        else
                        {
                            /// 1.0.6 이후 데이터 쪼개기 16개 짜리 실전 압축 데이터 이다.
                            PlayerPrefsManager.instance.NewSector5SaveJson(result.Data["SECTOR_5"].Value, result.Data["SECTOR_3"].Value);
                        }

                    }

                    ///---------------------------------------------------------------------------------------

                    //Debug.LogError("SECTOR_1 (Money_Dia): " + result.Data["SECTOR_1"].Value);
                    //if (long.TryParse(result.Data["SECTOR_1"].Value, out tryResult)) PlayerInventory.Money_Dia = tryResult;
                    //else PlayerInventory.Money_Dia = 0;
                    //tryResult = 0;
                    //Debug.LogError("SECTOR_2 (Money_Leaf): " + result.Data["SECTOR_2"].Value);
                    //if (double.TryParse(result.Data["SECTOR_2"].Value, out dtryResult)) PlayerInventory.Money_Leaf = dtryResult;
                    //else PlayerInventory.Money_Leaf = 0;
                    //tryResult = 0;
                    //Debug.LogError("SECTOR_3 (Money_EnchantStone): " + result.Data["SECTOR_3"].Value);
                    //if (long.TryParse(result.Data["SECTOR_3"].Value, out tryResult)) PlayerInventory.Money_EnchantStone = tryResult;
                    //else PlayerInventory.Money_EnchantStone = 0;
                    //tryResult = 0;

                    ///---------------------------------------------------------------------------------------

                    Debug.LogError("SECTOR_6 (isSuperUser): " + result.Data["SECTOR_6"].Value);
                    if(int.TryParse(result.Data["SECTOR_6"].Value, out tryResultt)) PlayerInventory.isSuperUser = tryResultt;
                    else PlayerInventory.isSuperUser = 0;

                    Debug.LogError("SECTOR_7 (RecentDistance): " + result.Data["SECTOR_7"].Value);
                    if (double.TryParse(result.Data["SECTOR_7"].Value, out dtryResult)) PlayerInventory.RecentDistance = dtryResult;
                    else PlayerInventory.RecentDistance = 0;

                    Debug.LogError("SECTOR_8 (nonSaveJsonMoney): " + result.Data["SECTOR_8"].Value);
                    PlayerPrefsManager.instance.NonJsonDataLoad(result.Data["SECTOR_8"].Value);

                    Debug.LogError("SECTOR_9 (NonDataBoxList): " + result.Data["SECTOR_9"].Value);
                    PlayerPrefsManager.instance.NonDataBoxList(result.Data["SECTOR_9"].Value);

                    ///// 파일에서 데이터 불러와서 리스트에 대입
                    //PlayerPrefsManager.isLoadingComp = true;
                    /// 초반 초기화 완료 됐을때 오프라인 보상 안 받게.
                    ObscuredPrefs.SetInt("isSeverDataLoad", 609);
                    /// 서버 로드 후 최초 실행때 초기화 막아주기
                    ObscuredPrefs.SetInt("tunamayo", 22);
                    /// 이전 업데이트 다 적용
                    ObscuredPrefs.SetInt("update210114", 214);
                    ObscuredPrefs.SetInt("update210117", 956);
                    ObscuredPrefs.Save();

                    /// 파일에서 데이터 불러와서 리스트에 대입
                    PlayerPrefsManager.instance.SeverLoadMaser(curentVersion);
                    // 팝업 뜰거
                }
            }

        },
        (error) =>
        {
            SystemPopUp.instance.StopLoopLoading();
            Debug.LogError ("데이터 불러오기 실패");
            //SceneManager.LoadScene(0);
        }
         );
    }

    /// <summary>
    /// 게임 버전 낮으면 이곳으로 이동
    /// </summary>
    public void GameUpdatePlz()
    {
        Application.OpenURL("market://details?id=" + URL);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }



}
