using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAReviewManager : MonoBehaviour
{
    /// <summary>
    /// 테스트 버튼에 붙여볼것
    /// </summary>
    public void ReviewReqStart()
    {
        ///ReviewManager 인스턴스를 사용하여 비동기 작업을 생성합니다.
        StartCoroutine(ReviewReq());
    }


    IEnumerator ReviewReq()
    {
        yield return null;
        // Create instance of ReviewManager
        ReviewManager _reviewManager = new ReviewManager();
        // ...
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        PlayReviewInfo _playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }


}
