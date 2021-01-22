using UnityEngine;

public class BattleGndManager : MonoBehaviour
{
    public bool isBGmovigPause;
    [Header("-배그 부모 오브젝트")]
    public Transform bgMother;
    [Header("-매쉬 갯수만큼 할당 (None 맞음)")]
    public MeshRenderer[] render;
    [Header("-배경 스피드")]
    public float speed = 0.1f;

    // 매쉬 갯수?
    private int bgCnt;
    private void Start()
    {
        ChageBG_Render(0);
    }

    /// <summary>
    /// 배경 화면 바꿔주기
    /// </summary>
    /// <param name="_index"></param>
    public void ChageBG_Render(int _index)
    {
        bgCnt = render.Length;
        /// 0번 베이스 스테이지만 5장이다.
        if (_index == 0) bgCnt--;

        for (int i = 0; i < bgCnt; i++)
        {
            //랜더러 교체
            render[i] = bgMother.GetChild(_index).GetChild(i).GetComponent<MeshRenderer>();
        }
        for (int i = 0; i < bgMother.childCount; i++)
        {
            //모든 스테이지 오브젝트 꺼주기
            bgMother.GetChild(i).gameObject.SetActive(false);
        }
        // 교체한 스테이지 켜주기
        bgMother.GetChild(_index).gameObject.SetActive(true);
    }



    // 움직일 오프셋
    private float bgOffset;
    private void Update()
    {
        if (isBGmovigPause || !PlayerPrefsManager.isJObjectLoad) return;

        bgOffset += Time.deltaTime * speed * PlayerInventory.Player_Move_Speed ;

        for (int i =0; i< bgCnt; i++)
        {
            render[i].material.mainTextureOffset = new Vector2((bgOffset * (1f + i)), 0);
        }
    }

}
 