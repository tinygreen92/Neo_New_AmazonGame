using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallChatBox : MonoBehaviour
{
    public Image profileImg;
    public Text userName;
    public Text chatBody;

    /// <summary>
    /// 채팅창 불러올때 호출
    /// 1. 닉네임
    /// 2. 대화 내용
    /// ( 3. 사용자 이미지? )
    /// </summary>
    public void SpawnThisObject(string _head, string _body)
    {
        userName.text = _head;
        chatBody.text = _body;
    }


}
