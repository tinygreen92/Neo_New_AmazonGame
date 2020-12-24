using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.SceneManagement;

public class FakeSceneEscape : MonoBehaviour
{
    public void DisconectGame()
    {
        // Ask if user wants to exit
        NativeUI.AlertPopup alert = NativeUI.ShowTwoButtonAlert("게임 종료",
                                        "아마존을 종료하시겠습니까?",
                                        "넹",
                                        "아뇽");

        if (alert != null)
            alert.OnComplete += delegate (int button)
            {
                if (button == 0)
                    Application.Quit();
            };
    }
}
