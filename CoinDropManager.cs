using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropManager : MonoBehaviour
{
    public Transform eneObj;
    public Transform leafObj;
    public Transform zogarkObj;
    public Transform potionObj;

    public Transform[] m_goldPosList;
    public static CoinDropManager instance;

    void Awake()
    {
        instance = this;
    }

    int randomValue;
    public void DropGold(GameObject deadEnemy, GameObject coinPos, bool isSuperBox)
    {
        if (isSuperBox) /// 황금 상자냐?
        {
            ///  골드 박스 업적 카운트 올리기
            ListModel.Instance.ALLlist_Update(10, 1);
            /// 골드 박스 일일 업적
            ListModel.Instance.DAYlist_Update(8);
            // 골드 드랍 오브젝트 갯수 맥스 30개.
            randomValue = Random.Range(15, m_goldPosList.Length);
        }
        else
        {
            // 골드 드랍 오브젝트 갯수 맥스 15개.
            randomValue = Random.Range(3, 15);
        }

        for (int i = 0; i < randomValue; i++)
        {
            Transform coinObject = Instantiate(eneObj, Vector3.zero, Quaternion.identity); // 프리팹 생성

            coinObject.SetParent(gameObject.transform); // 에너미 부모 위치에 생성
            coinObject.localScale = new Vector3(1.5f, 1.5f, 1.5f); // 스케일 값 1 고정
            coinObject.localPosition = Vector3.zero; // 뒤틀리는거 방지

            int randomPos = Random.Range(0, m_goldPosList.Length); // 리스트에 올려둔 위치값.

            coinObject.GetComponent<DropGold>().Init(deadEnemy.transform.position, coinPos.transform.position, m_goldPosList[randomPos].position);
        }

    }


    public void DropLeaf(GameObject deadEnemy, GameObject coinPos)
    {
        // 골드 드랍 오브젝트 갯수 맥스 15개.
        randomValue = Random.Range(3, 15);

        for (int i = 0; i < randomValue; i++)
        {
            Transform coinObject = Instantiate(leafObj, Vector3.zero, Quaternion.identity); // 프리팹 생성

            coinObject.SetParent(gameObject.transform); // 에너미 부모 위치에 생성
            coinObject.localScale = new Vector3(1.5f, 1.5f, 1.5f); // 스케일 값 1 고정
            coinObject.localPosition = Vector3.zero; // 뒤틀리는거 방지

            int randomPos = Random.Range(0, m_goldPosList.Length); // 리스트에 올려둔 위치값.

            coinObject.GetComponent<DropGold>().Init(deadEnemy.transform.position, coinPos.transform.position, m_goldPosList[randomPos].position);
        }



    }


    public void DropAmaCoin(GameObject deadEnemy, GameObject coinPos)
    {
        Transform coinObject = Instantiate(zogarkObj, Vector3.zero, Quaternion.identity); // 프리팹 생성

        coinObject.SetParent(gameObject.transform); // 에너미 부모 위치에 생성
        coinObject.localScale = new Vector3(1f, 1f, 1f); // 스케일 값 1 고정
        coinObject.localPosition = Vector3.zero; // 뒤틀리는거 방지

        int randomPos = Random.Range(0, m_goldPosList.Length); // 리스트에 올려둔 위치값.

        coinObject.GetComponent<DropGold>().Init(deadEnemy.transform.position, coinPos.transform.position, m_goldPosList[randomPos].position);
    }


    public void DropPotion(GameObject deadEnemy, GameObject coinPos)
    {
        Transform coinObject = Instantiate(potionObj, Vector3.zero, Quaternion.identity); // 프리팹 생성

        coinObject.SetParent(gameObject.transform); // 에너미 부모 위치에 생성
        coinObject.localScale = new Vector3(1.2f, 1.2f, 1.2f); // 스케일 값 1 고정
        coinObject.localPosition = Vector3.zero; // 뒤틀리는거 방지

        int randomPos = Random.Range(0, m_goldPosList.Length); // 리스트에 올려둔 위치값.

        coinObject.GetComponent<DropGold>().Init(deadEnemy.transform.position, coinPos.transform.position, m_goldPosList[randomPos].position);
    }



}
