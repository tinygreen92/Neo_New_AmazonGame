using UnityEngine;

using System.Collections;



public class DropGold : MonoBehaviour
{
    bool m_end;
    bool m_remove;

    Transform m_transform;
    Vector3 m_startPos;
    Vector3 m_targetPos;
    Vector3 m_centerPos;

    float m_currTime;
    float m_speed = 2f;
    float m_removeTime;

    string m_gold;

    public void Init(Vector3 startPos, Vector3 centerPos, Vector3 targetPos)
    {
        m_transform = gameObject.transform;
        m_startPos = startPos;
        m_targetPos = targetPos;

        m_centerPos = (m_startPos + m_targetPos) / 2;
        m_centerPos.y = centerPos.y;

        // 동전이 사라지는 시간을 랜덤하게 설정합니다.
        m_removeTime = Random.Range(0.2f, 0.5f);

    }

    public void Update()
    {
        if (m_remove)
            return;

        if (m_end)
        {
            m_currTime -= Time.deltaTime;
            if (m_currTime <= 0)
            {
                //AudioManager.instance.PlayAudio("Coin", "SE");
                m_remove = true;
                Destroy(gameObject);
            }
            return;
        }
        m_currTime += Time.deltaTime * m_speed;
        m_transform.position = Bezier3(m_startPos, m_centerPos, m_targetPos, m_currTime);

        if (m_currTime >= 1)
        {
            m_transform.position = m_targetPos;
            m_end = true;
            m_currTime = m_removeTime;
        }

    }






    static public Vector3 Bezier3(Vector3 p1, Vector3 p2, Vector3 p3, float mu)
    {
        float mum1, mum12, mu2;
        Vector3 p;
        //
        mu2 = mu * mu;
        mum1 = 1 - mu;
        mum12 = mum1 * mum1;

        p.x = p1.x * mum12 + 2 * p2.x * mum1 * mu + p3.x * mu2;
        p.y = p1.y * mum12 + 2 * p2.y * mum1 * mu + p3.y * mu2;
        p.z = p1.z * mum12 + 2 * p2.z * mum1 * mu + p3.z * mu2;

        return (p);

    }
}
