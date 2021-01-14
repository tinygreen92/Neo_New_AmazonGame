using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// https://knightk.tistory.com/40
/// </summary>
public class InfiniteScroll : MonoBehaviour
{
    [Header("- 몇번째 스크롤임?")]
    public int NumOfSc;
    [SerializeField]
    private RectTransform m_ItemBase;
    [SerializeField]
    int m_instantiateItemCount = 9;
    [SerializeField]
    float gap = 0;

    float m_itemWidth;
    float m_itemHeight;
    public Direction direction;

    [System.NonSerialized]
    public List<RectTransform> m_itemList = new List<RectTransform>();
    protected float m_diffPreFramePosition = 0;

    [SerializeField]
    private int m_currentItemNo = 0;

    public ScrollRect scrollRect;
    public enum Direction { Vertical, Horizontal, }

    // cache component 
    public RectTransform m_Content;
    RectTransform m_ParntVeiport;
    private float AnchoredPosition { get { return (direction == Direction.Vertical) ? -m_Content.anchoredPosition.y : m_Content.anchoredPosition.x; } }
    private float ItemScale { get { return (direction == Direction.Vertical) ? m_itemHeight : m_itemWidth; } }

    /// <summary>
    /// 리스트 골라먹기
    /// </summary>
    int listCnt = 0;

    void Start()
    {
        m_ParntVeiport = transform.parent.GetComponent<RectTransform>();

        m_itemWidth = m_ItemBase.rect.width;
        m_itemHeight = m_ItemBase.rect.height;

        /// 데이터 리스트 세팅
        switch (NumOfSc)
        {

            case 1:
                listCnt = ListModel.Instance.charatorList.Count;
                break;

            case 2:
                listCnt = ListModel.Instance.weaponList.Count;
                break;

            case 3:
                listCnt = ListModel.Instance.heartList.Count + 1;
                break;

            case 4:
                listCnt = ListModel.Instance.supList.Count;
                break;

            case 5:
                listCnt = ListModel.Instance.petList.Count;
                break;

            case 6:
                listCnt = ListModel.Instance.runeList.Count;
                break;

            case 7:
                listCnt = ListModel.Instance.shopList.Count;
                break;

            case 8:
                listCnt = ListModel.Instance.mineCraft.Count;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 상점 리프레쉬
    /// </summary>
    public void RRefreshShop()
    {
        m_itemList.Clear();

        for (int i = 1; i < m_Content.childCount; i++)
        {
            m_Content.GetChild(m_Content.childCount - i).GetComponent<ShopItemManager>().enabled = false;
            Destroy(m_Content.GetChild(m_Content.childCount - i).gameObject);
        }
    }
    public void SetLonWizard()
    {
        switch (PlayerPrefsManager.storeIndex)
        {
            case 1:                             /// 다이아 상점 dia
                listCnt = ListModel.Instance.shopList.Count;
                break;

            case 10:                             /// 특별 상점 spec
                listCnt = ListModel.Instance.shopListSPEC.Count;
                break;

            case 100:                             /// 일반 상점 nor
                listCnt = ListModel.Instance.shopListNOR.Count;
                break;

            default:
                break;
        }
        m_instantiateItemCount = 9;
        ListStart();
    }

    /// <summary>
    /// 리스트 추가될때 유물 뷰 새로고침
    /// </summary>
    public void SetLinsCntForHeart()
    {
        if (listCnt > 30) return;

        m_itemList.Clear();

        for (int i = 1; i < m_Content.childCount; i++)
        {
            m_Content.GetChild(m_Content.childCount - i).GetComponent<HeartItem>().DestroyChain();
            m_Content.GetChild(m_Content.childCount - i).GetComponent<HeartItem>().enabled = false;
            Destroy(m_Content.GetChild(m_Content.childCount - i).gameObject);
        }
    }
    /// <summary>
    /// 그 다음에 리스트 추가한 만큼 오브젝트 추가하기
    /// </summary>
    public void SetCompHeartLins()
    {
        listCnt = ListModel.Instance.heartList.Count + 1;
        m_instantiateItemCount = 9;
        ListStart();
    }


    /// <summary>
    /// 일단 오브젝트 싹 비우기
    /// </summary>
    public void SetLinsCntForRune()
    {
        m_itemList.Clear();

        for (int i = 1; i < m_Content.childCount; i++)
        {
            m_Content.GetChild(m_Content.childCount - i).GetComponent<RuntItem>().enabled = false;
            Destroy(m_Content.GetChild(m_Content.childCount - i).gameObject);
        }

        /// <랜덤으로_룬_뽑기> -  해당 코드 룬 매니저로 이동
    }

    /// <summary>
    /// 그 다음에 리스트 추가한 만큼 오브젝트 추가하기
    /// </summary>
    public void SetCompRuneLins()
    {
        listCnt = ListModel.Instance.runeList.Count;
        Debug.LogWarning("listCnt : " + listCnt);
        m_instantiateItemCount = 9;
        ListStart();
    }




    public void ListStart()
    {
        scrollRect.verticalScrollbar.value = 1f;

        if (listCnt < m_instantiateItemCount)
        {
            m_instantiateItemCount = listCnt;
        }
        else
        {
            m_instantiateItemCount = (direction == Direction.Vertical) ? Mathf.RoundToInt(m_ParntVeiport.rect.height / ItemScale) + 3 : Mathf.RoundToInt(Screen.width / ItemScale) + 3;
            //m_instantiateItemCount = 9;
            if (name == "SHOP_INFINI_Content" || name == "Heat_INFINI_Content" || name == "Rune_INFINI_Content") m_instantiateItemCount = listCnt;
        }

        // create items 
        scrollRect.horizontal = direction == Direction.Horizontal;
        scrollRect.vertical = direction == Direction.Vertical;

        if (direction == Direction.Vertical)
        {
            //m_Content.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, (ItemScale + gap) * (listData.Count - 1) + gap);
            //m_Content.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, m_itemWidth + gap * 2); 
            m_Content.sizeDelta = new Vector2(0, (ItemScale + gap) * (listCnt) + gap);

            //Debug.LogWarning("m_itemWidth : " + m_itemWidth);
        }
        else
        {
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (ItemScale + gap) * (listCnt) + gap);
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_itemHeight + gap * 2);
        }

        m_ItemBase.gameObject.SetActive(false);

        for (int i = 0; i < m_instantiateItemCount; i++)
        {
            RectTransform item = Instantiate(m_ItemBase);
            item.name = string.Format("{0}", i);
            item.SetParent(transform, false);
            item.localPosition = Vector3.zero;
            item.localScale = new Vector3(1, 1, 1);

            if (direction == Direction.Vertical)
            {
                //item.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, m_itemWidth - gap * 2);
                //item.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, m_itemHeight);
                //item.sizeDelta = new Vector2(0, m_itemWidth - gap * 2);
            }
            else
            {
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_itemWidth);
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_itemHeight);
            }

            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -gap - (ItemScale + gap) * i) : new Vector2((ItemScale + gap) * i + gap, -gap);
            m_itemList.Add(item);

            item.gameObject.SetActive(true);
            item.GetComponent<Item>().UpdateItem(i);
        }

        if (name == "Heat_INFINI_Content" || name == "Rune_INFINI_Content")
        {
            if (listCnt > m_instantiateItemCount)
            {
                scrollRect.onValueChanged.AddListener(valueChange);
            }
            else
            {
                scrollRect.onValueChanged.RemoveListener(valueChange);
            }
            /// 스크롤뷰 아래로 부드럽게 스크롤
            if (gameObject.activeSelf && name == "Heat_INFINI_Content")
            {
                /// 스크롤 다 내려가는 중
                PlayerPrefsManager.isRefreshHeart = true;
                StartCoroutine(MoveDownSc());
            }
            else if (gameObject.activeSelf && name == "Rune_INFINI_Content") StartCoroutine(MoveDownSc());
        }
        else if(name == "SHOP_INFINI_Content")
        {
            scrollRect.onValueChanged.RemoveListener(valueChange);
        }
        else
        {
            scrollRect.onValueChanged.AddListener(valueChange);
        }

    }

    IEnumerator MoveDownSc()
    {
        yield return null;

        while (true)
        {
            scrollRect.verticalScrollbar.value = Mathf.Lerp(scrollRect.verticalScrollbar.value, 0, 0.1f);
            yield return null;
            if (scrollRect.verticalScrollbar.value <= 0.001f) break;
        }

    }

    private void valueChange(Vector2 _pos)
    {
        // scroll up, item attach bottom or right 
        while (AnchoredPosition - m_diffPreFramePosition < -(ItemScale + gap) * 2)
        {
            m_diffPreFramePosition -= (ItemScale + gap);
            RectTransform item = m_itemList[0]; m_itemList.RemoveAt(0); m_itemList.Add(item);
            float pos = (ItemScale + gap) * m_instantiateItemCount + (ItemScale + gap) * m_currentItemNo;

            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -pos - gap) : new Vector2(pos + gap, -gap);
            m_currentItemNo++;

            if (m_currentItemNo + m_instantiateItemCount < listCnt + 1)
            {
                item.GetComponent<Item>().UpdateItem(m_currentItemNo + m_instantiateItemCount - 1);
            }
            else
            {
                item.GetComponent<Item>().UpdateItem(-100);
            }
        }

        // scroll down, item attach top or left 
        while (AnchoredPosition - m_diffPreFramePosition > 0)
        {


            m_diffPreFramePosition += (ItemScale + gap);
            int itemListLastCount = m_instantiateItemCount - 1;
            RectTransform item = m_itemList[itemListLastCount];
            m_itemList.RemoveAt(itemListLastCount);
            m_itemList.Insert(0, item);

            m_currentItemNo--;

            float pos = (ItemScale + gap) * m_currentItemNo + gap;
            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, -gap);

            if (m_currentItemNo > -1)
            {
                item.GetComponent<Item>().UpdateItem(m_currentItemNo);
            }
            else
            {
                item.GetComponent<Item>().UpdateItem(-100);
            }
        }
    }
}
