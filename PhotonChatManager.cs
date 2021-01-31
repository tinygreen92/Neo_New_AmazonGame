using ExitGames.Client.Photon;
using Photon.Chat;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    [Header("-채팅창 업글 1.0.5")]
    public GameObject special_L;
    public GameObject special_R;

    [Header("-확장된 채팅 패널")]
    public GameObject chatInnerPanel;
    [Header("-바깥 채팅 박스 텍스트")]
    public Image ArrowIcon;
    public Sprite[] ArroIcons;
    public Text outterChatBoxText;
    public Scrollbar innerChatScbar;
    public Transform innerContent;

    [Header("-한계치 설정")]
    public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context
    public int MaxLengthLog;
    /// <summary>
    /// 채팅 말 풍선이 붙을 오브젝트
    /// </summary>
    public Transform ChatContent;

    public GameObject inChatBox_L;
    public GameObject inChatBox_R;

    public InputField InputMesseageBox;

    public ChatSettings chatAppSettings;
    public ChatClient chatClient;


    public string UserName { get; set; }
    public string[] ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.


    private string selectedChannelName; // mainly used for GUI/input


    /// <summary>
    /// 누르면 채팅창 껐다 켰다
    /// </summary>
    public void ChattingBoxOnOff()
    {
        if (chatInnerPanel.activeSelf)
        {
            ArrowIcon.sprite = ArroIcons[0];
            chatInnerPanel.SetActive(false);
        }
        else
        {
            ArrowIcon.sprite = ArroIcons[1];
            chatInnerPanel.SetActive(true);
            Invoke(nameof(ScrollReset), 0.02f);
        }
    }

    public void PhotonStart(string _name)
    {
        // 테스트 닉네임
        if (string.IsNullOrEmpty(this.UserName))
        {
            this.UserName = _name;
        }

#if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
#else
        if (this.chatAppSettings == null)
        {
            this.chatAppSettings = ChatSettings.Instance;
        }
#endif

        bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppId);

        if (!appIdPresent)
        {
            Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
        }

        Connect();
    }

    /// <summary>
    /// 포톤 커넥트 시도
    /// </summary>
    public void Connect()
    {
        chatClient = new ChatClient(this);

#if !UNITY_WEBGL
        chatClient.UseBackgroundWorkerForSending = true;
#endif

        chatClient.Connect(chatAppSettings.AppId, "1.0", new AuthenticationValues(UserName));

        Debug.LogWarning("포톤 챗 Connecting as: " + this.UserName);
    }

    /// <summary>
    /// 접속 잘 되었다면? 채널 리스트에 전부 subscribe
    /// </summary>
    public void OnConnected()
    {
        if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
        {
            this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
        }
    }

    public void OnDestroy()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }


    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
        }
    }


    /// <summary>
    /// 외부에서 고의로 연결 끊어줌
    /// </summary>
    public void ExDisconnect()
    {
        this.chatClient.Disconnect();
        chatClient = null;
        /// 채팅 지워!
        int mc = ChatContent.childCount;
        for (int i = 0; i < mc; i++)
        {
            Lean.Pool.LeanPool.Despawn(innerContent.GetChild(0));
        }
    }
    public void ExReconnect()
    {
        Connect();
    }

    /// <summary>
    /// InPut 필드에 OnEndEdit 에 붙여준다.
    /// </summary>
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            SendChatMessage(InputMesseageBox.text);
            /// 활성화 상태니까 -> 비활성화
            InputMesseageBox.Select();
            InputMesseageBox.ActivateInputField();
            InputMesseageBox.text = "";
        }
    }

    /// <summary>
    /// 보내기 버튼에 붙여준다.
    /// </summary>
    public void OnClickSend()
    {
        if (InputMesseageBox != null)
        {
            SendChatMessage(InputMesseageBox.text);
            /// 활성화 상태니까 -> 비활성화
            InputMesseageBox.Select();
            InputMesseageBox.ActivateInputField();
            InputMesseageBox.text = "";
        }
    }

    /// <summary>
    /// 보내기 누르거나 엔터 입력시 input필드 메세지 날아감
    /// </summary>
    /// <param name="inputLine"></param>
    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine)) return;
        // 서버에 배포
        this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
    }


    /// <summary>
    /// 메세지 수신
    /// 
    /// 모든 공개 메시지는 
    /// Dictionary<string, ChatChannel> PublicChannels
    /// 로 캐시되므로 별도로 추적관리할 필요는 없습니다. 
    /// PrivateChannels 에 대한 키는 채널 이름 입니다.
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="senders"></param>
    /// <param name="messages"></param>
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(this.selectedChannelName))
        {
            // update text
            this.ShowChannel(this.selectedChannelName);
        }

        for (int i = 0; i < senders.Length; i++)
        {
            // 내가 보낸 메세지면 오른쪽에 표시
            if (UserName == senders[i])
            {
                /// 특별 메시지
                if (senders[i].Contains("★"))
                {
                    GameObject inCB = Lean.Pool.LeanPool.Spawn(special_R, ChatContent);
                    inCB.GetComponent<CallChatBox>().SpawnThisObject(senders[i], messages[i].ToString());
                }
                else
                {
                    GameObject inCB = Lean.Pool.LeanPool.Spawn(inChatBox_R, ChatContent);
                    inCB.GetComponent<CallChatBox>().SpawnThisObject(senders[i], messages[i].ToString());
                }

            }
            else
            {
                /// 특별 메시지
                if (senders[i].Contains("★"))
                {
                    GameObject inCB = Lean.Pool.LeanPool.Spawn(special_L, ChatContent);
                    inCB.GetComponent<CallChatBox>().SpawnThisObject(senders[i], messages[i].ToString());
                }
                else
                {
                    // 다른 사람 메세지는 왼쪽에 표시
                    GameObject inCB = Lean.Pool.LeanPool.Spawn(inChatBox_L, ChatContent);
                    inCB.GetComponent<CallChatBox>().SpawnThisObject(senders[i], messages[i].ToString());
                }


            }
        }

        // 50개 넘어가면 과거 대화 버림
        if (innerContent.childCount > MaxLengthLog)
        {
            Lean.Pool.LeanPool.Despawn(innerContent.GetChild(0));
        }

        // 바깥 채팅 창에 텍스트 표시
        outterChatBoxText.text = StringTransfer(senders[senders.Length - 1] + " : " + messages[messages.Length - 1], 28);

        // 스크롤
        Invoke(nameof(ScrollReset), 0.2f);
    }

    /// <summary>
    /// 스크롤 밑으로 내리기 인보크 용
    /// </summary>
    void ScrollReset() => innerChatScbar.value = 0f;

    /// <summary>
    /// 문자열 크기 변환 메소드
    /// 예) 대한민국 -> 대한민...
    /// </summary>
    /// <param name="inputString">입력 문자열</param>
    /// <param name="stringLength">자를 길이</param>
    /// <returns></returns>
    string StringTransfer(string inputString, int stringLength)
    {
        string msg = inputString;

        if (msg.Length > stringLength)
        {
            msg = msg.Substring(0, stringLength) + "...";
        }

        return msg;
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            // TODO : 포톤 끊겼을때 채팅 치면 이곳으로
            outterChatBoxText.text = "<채팅 서버 연결 끊김>";
            Debug.LogError(message);
            /// 1초 뒤에 재접속 시도
            Invoke(nameof(Connect), 1.0f);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogError(message);
        }
        else
        {
            // TODO : 포톤 정상 연결되면 여기로
            outterChatBoxText.text = "<채팅 서버 연결됨>";
            Debug.LogError(message);
        }
    }

    public void OnChatStateChange(ChatState state) => Debug.LogWarning("포톤 " + state);

    public void OnDisconnected()
    {
        outterChatBoxText.text = "<채팅 서버 연결 끊김>";
        Debug.LogError("포톤 OnDisconnected");
    }

    public void OnPrivateMessage(string sender, object message, string channelName) => Debug.Log("포톤 OnPrivateMessage");

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) => Debug.Log("포톤 OnStatusUpdate");

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //// in this demo, we simply send a message into each channel. This is NOT a must have!
        //foreach (string channel in channels)
        //{
        //    this.chatClient.PublishMessage(channel, "님이 채널에 입장하셨습니다."); // you don't HAVE to send a msg on join but you could.
        //}

        // Switch to the first newly created channel
        ShowChannel(channels[0]);
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName)) return;

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        selectedChannelName = channelName;

        Debug.Log("ShowChannel: " + this.selectedChannelName);
    }

    public void OnUnsubscribed(string[] channels) => Debug.Log("포톤 메서드");

    public void OnUserSubscribed(string channel, string user) => Debug.Log("포톤 메서드");

    public void OnUserUnsubscribed(string channel, string user) => Debug.Log("포톤 메서드");
}
