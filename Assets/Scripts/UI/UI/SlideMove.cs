using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideMove : MonoBehaviour,IBeginDragHandler,IEndDragHandler {
    private float contentLength;
    private float beginMousePosX;
    private float endMousePosX;
    private ScrollRect scrollRect;
    private float lastProportion;//上一个位置比例

    private Vector2 contentTransSize;//Content初始大小

    public int CellLength;//每个单元格长度
    public int Spacing;//间隔
    public int LeftOffset;//左偏移量
    private float upperLimit;
    private float lowerLimit;
    private float firstItemLength;//移动第一个单元格的距离
    private float oneItemLength;//移动一个单元格的距离
    private float oneItemProportion;//滑动一个单元格的比例

    public int TotalItemNum;
    private int currentIndex;//当前单元格索引

    public Text PageText;

    public bool needSendMessage;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentLength = scrollRect.content.rect.xMax - 2 * LeftOffset - CellLength;
        firstItemLength = CellLength / 2 + LeftOffset;
        oneItemLength = CellLength + Spacing;
        oneItemProportion = oneItemLength / contentLength;
        lowerLimit = firstItemLength / contentLength;
        upperLimit = 1 - firstItemLength / contentLength;
        currentIndex = 1;
        scrollRect.horizontalNormalizedPosition = 0;

        contentTransSize = scrollRect.content.sizeDelta;

        if (PageText !=null)
        {
            PageText.text = currentIndex.ToString() + " / " + TotalItemNum;
        }
    }

    public void Init()
    {
        lastProportion = 0;
        currentIndex = 1;
        if (scrollRect != null)
        {
            scrollRect.horizontalNormalizedPosition = 0;
        }
        if (PageText != null)
        {
            PageText.text = currentIndex.ToString() + " / " + TotalItemNum;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePosX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float offSetX = 0;
        endMousePosX = Input.mousePosition.x;
        offSetX = (beginMousePosX - endMousePosX) * 2;
        if (Mathf.Abs(offSetX)>firstItemLength)
        {
            if (offSetX > 0)//右滑
            {
                if (currentIndex>=TotalItemNum)
                {
                    return;
                }
                int moveCount = (int)((offSetX - firstItemLength) / oneItemLength) + 1;
                currentIndex += moveCount;
                if (currentIndex>=TotalItemNum)
                {
                    currentIndex = TotalItemNum;
                }
                lastProportion += oneItemProportion * moveCount;
                if (lastProportion>=upperLimit)
                {
                    lastProportion = 1;
                }
                if (needSendMessage)
                {
                    UpdatePanel(true);
                }
            }
            else//
            {
                if (currentIndex <= 1)
                {
                    return;
                }
                int moveCount = (int)((offSetX + firstItemLength) / oneItemLength) - 1;
                currentIndex += moveCount;
                if (currentIndex <= 1)
                {
                    currentIndex = 1;
                }
                lastProportion += oneItemProportion * moveCount;
                if (lastProportion <= lowerLimit)
                {
                    lastProportion = 0;
                }
                if (needSendMessage)
                {
                    UpdatePanel(false);
                }
            }
            if (PageText != null)
            {
                PageText.text = currentIndex.ToString() + " / " + TotalItemNum;
            }
        }

        DOTween.To(() =>
        scrollRect.horizontalNormalizedPosition, lerpValue =>
         scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.3f).SetEase(Ease.Linear);
    }

    public void ToNextPage()
    {
        if (currentIndex>=TotalItemNum)
        {
            return;

        }
        lastProportion += oneItemProportion;
        currentIndex++;
        if (PageText != null)
        {
            PageText.text = currentIndex.ToString() + " / " + TotalItemNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(true);
        }
        DOTween.To(() =>
        scrollRect.horizontalNormalizedPosition, lerpValue =>
        scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.3f).SetEase(Ease.Linear);

    }
    public void ToLastPage()
    {
        if (currentIndex <= 1)
        {
            return;

        }
        lastProportion -= oneItemProportion;
        currentIndex--;
        if (PageText != null)
        {
            PageText.text = currentIndex.ToString() + " / " + TotalItemNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(false);
        }
        DOTween.To(() =>
        scrollRect.horizontalNormalizedPosition, lerpValue =>
        scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.3f).SetEase(Ease.Linear);

    }

    //设置Content的大小
    public void SetContentLength(int itemNum)
    {
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x + (CellLength + Spacing) * (itemNum - 1), scrollRect.content.sizeDelta.y);
        TotalItemNum = itemNum;
    }
    //初始化Content的大小
    public void InitContentLength()
    {
        scrollRect.content.sizeDelta = contentTransSize;
    }

    //发送翻页信息的方法
    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            //向父级发送消息
            gameObject.SendMessageUpwards("ToNextLevel");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastLevel");
        }
    }
}
