using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideMoveDan : MonoBehaviour,IBeginDragHandler,IEndDragHandler {
    private RectTransform contentTrans;
    private float beginMousePosX;
    private float endMousePosX;
    private ScrollRect scrollRect;

    public int CellLength;
    public int Spacing;
    public int leftOffset;
    private float moveOneItemLength;

    private Vector3 currentContentLocalPos;
    private Vector3 contentInitPos;
    private Vector3 contentTransSize;

    public int TotalItemNum;
    private int currentIndex;

    public Text PageText;

    public bool needSendMassage;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        moveOneItemLength = CellLength + Spacing;
        currentContentLocalPos = contentTrans.localPosition;
        contentTransSize = contentTrans.sizeDelta;
        contentInitPos = contentTrans.localPosition;
        currentIndex = 1;
        if(PageText != null)
        {
            PageText.text = currentIndex.ToString() + "/" + TotalItemNum;
        }
    }

    public void Init()
    {
        currentIndex = 1;
        if (contentTrans!=null)
        {
            contentTrans.localPosition = contentInitPos;
            currentContentLocalPos = contentInitPos;
            if (PageText != null)
            {
                PageText.text = currentIndex.ToString() + "/" + TotalItemNum;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePosX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endMousePosX = Input.mousePosition.x;
        float offSetX = 0;
        float moveDistance = 0;
        offSetX = beginMousePosX - endMousePosX;
        if (offSetX>0)//右滑
        {
            if (currentIndex>TotalItemNum)
            {
                return;
            }
            if (needSendMassage)
            {
                UpdatePanel(true);
            }

            moveDistance = -moveOneItemLength;
            currentIndex++;
        }
        else
        {
            if (currentIndex<=1)
            {
                return;
            }
            if (needSendMassage)
            {
                UpdatePanel(false);
            }
            moveDistance = moveOneItemLength;
            currentIndex--;

        }
        if (PageText != null)
        {
            PageText.text = currentIndex.ToString() + "/" + TotalItemNum;
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void ToNextPage()
    {
        float moveDistance = 0;
        if (currentIndex>=TotalItemNum)
        {
            return;
        }
        moveDistance = -moveOneItemLength;
        currentIndex++;
        if (PageText != null)
        {
            PageText.text = currentIndex.ToString() + "/" + TotalItemNum;
        }
        if (needSendMassage)
        {
            UpdatePanel(true);
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void ToLastPage()
    {
        float moveDistance = 0;
        if (currentIndex <=1)
        {
            return;
        }
        moveDistance = moveOneItemLength;
        currentIndex--;
        if (PageText != null)
        {
            PageText.text = currentIndex.ToString() + "/" + TotalItemNum;
        }
        if (needSendMassage)
        {
            UpdatePanel(true);
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void SetContentLength(int itemNum)
    {

        contentTrans.sizeDelta = new Vector2(contentTrans.sizeDelta.x + (CellLength + Spacing) * (itemNum - 1), contentTrans.sizeDelta.y);
        TotalItemNum = itemNum;
    }

    public void InitScrollLength()
    {
        contentTrans.sizeDelta = contentTransSize;
    }

    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            gameObject.SendMessageUpwards("ToNextLevel");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastLevel");
        }
    }
}
