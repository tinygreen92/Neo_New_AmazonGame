using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOutManager : MonoBehaviour
{
    public Image FadeImage;


    Color alpha;
    public void StartFadein()
    {
        // 선형보간 코루틴 ㄱ
        StartCoroutine(FadeFlow());
    }

    float currentTime = 0f;
    float lastTime = 1f;
    IEnumerator FadeFlow()
    {
        FadeImage.gameObject.SetActive(true);
        yield return null;

        currentTime = 0f;
        alpha = FadeImage.color;
        while(alpha.a < 1f)
        {
            currentTime += Time.deltaTime / lastTime;
            alpha.a = Mathf.SmoothStep(0,1,currentTime);
            FadeImage.color = alpha;
            yield return null;

        }

        currentTime = 0f;

        yield return new WaitForSeconds(0.25f);

        while (alpha.a > 0f)
        {
            currentTime += Time.deltaTime / lastTime;
            alpha.a = Mathf.SmoothStep(1, 0, currentTime);
            FadeImage.color = alpha;
            yield return null;

        }

        FadeImage.gameObject.SetActive(false);
    }

}
