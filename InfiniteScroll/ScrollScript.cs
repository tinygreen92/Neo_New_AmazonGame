using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollScript : ScrollRect
{
    // 부모 스크롤뷰로 받을래?
    bool forParent;
    //
    NestedScrollManager nm;
    ScrollRect sc;

    protected override void Start()
    {
        nm = GameObject.FindWithTag("nm").GetComponent<NestedScrollManager>();
        sc = GameObject.FindWithTag("nm").GetComponent<ScrollRect>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if (forParent)
        {
            /// 부모 스크롤뷰 드래그 이벤트
            nm.OnBeginDrag(eventData);
            sc.OnBeginDrag(eventData);
        }
        else
        {
            /// 자식 스크롤뷰 드래그 이벤트
            base.OnBeginDrag(eventData);
        }

    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            /// 부모 스크롤뷰 드래그 이벤트
            nm.OnDrag(eventData);
            sc.OnDrag(eventData);
        }
        else
        {
            /// 자식 스크롤뷰 드래그 이벤트
            base.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            /// 부모 스크롤뷰 드래그 이벤트
            nm.OnEndDrag(eventData);
            sc.OnEndDrag(eventData);
        }
        else
        {
            /// 자식 스크롤뷰 드래그 이벤트
            base.OnEndDrag(eventData);
        }
    }
}
