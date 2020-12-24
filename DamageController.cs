/**
 * @author : tinygreen92 
 * @date   : 2019-11-04
 * @desc   : 대미지 프리팹 세팅해서 일반 대미지 / 크리티컬 / 버프상자 대미지 주는거
 *           
 *           
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{
    public Transform normalFont;

    /// <summary>
    /// 대미지 폰트 오브젝트 생성
    /// </summary>
    /// <param name="tfPosition">생성 위치 트랜스폼 포지션</param>
    /// <param name="damageAmount">출력할 내용 대미지</param>
    /// <returns></returns>
    public DamageController Create(Transform tfPosition, double damageAmount, bool isCriticalHit)
    {
        //프리팹 일단 생성하고
        Transform damagePopupTransform = Lean.Pool.LeanPool.Spawn(normalFont, Vector3.zero, Quaternion.identity);
        //부모에 달아줌
        damagePopupTransform.SetParent(tfPosition);
        damagePopupTransform.localPosition = Vector3.zero;
        damagePopupTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        transform.GetComponent<Text>().material.DOFade(1, 0);

        // 거기서 컨트롤러 스크립트 떼온다.
        DamageController damageController = damagePopupTransform.GetComponent<DamageController>();
        damageController.Setup(tfPosition, damageAmount, isCriticalHit);

        return damageController;
    }

    /// <summary>
    /// 표시할 대미지 수치 셋업
    /// </summary>
    /// <param name="damegeAmount"></param>
    public void Setup(Transform tfPosition, double damageAmount, bool isCriticalHit)
    {
        //대미지 출력
        transform.GetComponent<Text>().text = PlayerPrefsManager.instance.DoubleToStringNumber(damageAmount);
        //
        if (isCriticalHit)
        {
            transform.DOScale((Vector3.one *1.5f), 0.5f);
            transform.GetComponent<Text>().material.DOFade(0, 1f).SetEase(Ease.InBack);
            transform.DOMove(transform.parent.GetChild(0).position, 0.9f).OnComplete(CallBackEnemyAttack);
            /// 카메라 쉐이크
            if (tfPosition.GetComponent<CameraShaker>().isShake) return;
            tfPosition.GetComponent<CameraShaker>().isShake = true;
            tfPosition.GetComponent<CameraShaker>().monsterCanvas.DOShakePosition(0.3f, 0.6f, 10, 90f, false, false);
            tfPosition.GetComponent<CameraShaker>().battleCanvas.DOShakePosition(0.35f, 0.6f, 10, 90f,false,false).OnComplete(CallBackShake);
        }
        else
        {
            transform.DOScale((Vector3.one), 0.5f);
            transform.GetComponent<Text>().material.DOFade(0, 1f).SetEase(Ease.InBack);
            transform.DOMove(transform.parent.GetChild(0).position, 0.9f).OnComplete(CallBackEnemyAttack);
            /// 카메라 쉐이크
            if (tfPosition.GetComponent<CameraShaker>().isShake) return;
            tfPosition.GetComponent<CameraShaker>().isShake = true;
            //tfPosition.GetComponent<CameraShaker>().monsterCanvas.DOShakePosition(0.3f, 0.6f, 10, 90f, false, false);
            //tfPosition.GetComponent<CameraShaker>().battleCanvas.DOShakePosition(0.35f, 0.6f, 10, 90f, false, false).OnComplete(CallBackShake);
        }
    }

    void CallBackShake()
    {
        transform.parent.GetComponent<CameraShaker>().SetPositionInit();
    }

    void CallBackEnemyAttack()
    {
        Lean.Pool.LeanPool.Despawn(gameObject);
    }

}
