
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderTester : MonoBehaviour
{
    public Image Buff_01;


    bool isSwich;
    public void GrayScale()
    {
        isSwich = !isSwich;
        if (isSwich) Buff_01.material.SetFloat("_EffectAmount", 1.0f);
        else Buff_01.material.SetFloat("_EffectAmount", 0.0f);
    }

}
