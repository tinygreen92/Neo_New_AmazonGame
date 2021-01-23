using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using UnityEngine.SceneManagement;

public class FakeSceneEscape : MonoBehaviour
{
    [Header("- 로딩 이미지")]
    public Image loadingBar;

    public void DisconectGame()
    {

    }

    private void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;
        float currentTime = 0;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(1);
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log("로딩 얼마??" + asyncScene.progress);

            currentTime += Time.deltaTime / 5f;
            loadingBar.fillAmount = Mathf.SmoothStep(0, 0.3f, currentTime);

            if (asyncScene.progress >= 0.9f)
            {
                asyncScene.allowSceneActivation = true;
            }
        }

    }

}
