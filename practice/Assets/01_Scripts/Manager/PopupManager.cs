using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; } = null;
    private PlayerData Player => GameDataManager.PlayerData;
    private Dictionary<EPopupType, PopupHeader> popupDict = new();
    private Stack<EPopupType> popupStack = new();
    private Queue<EPopupType> reservationQueue = new();
    //[SerializeField] private Image blockPanel;
    //private Coroutine blockPanelRoutine;
    //private WaitForSeconds blockPanelWaitForSeconds = new WaitForSeconds(0.33f);
    public GameObject GetPopupObject(EPopupType type) => popupDict[type].gameObject;

    // 팝업버튼을 보여줄 리스트
    List<EPopupType> ShowButtonList = new List<EPopupType> {
            EPopupType.PopupPlayerInfo,
            //EPopupType.PopupUpgrade,
            //EPopupType.PopupEquip,
            //EPopupType.PopupSkill,
            //EPopupType.PopupDungeon,
            //EPopupType.PopupShop,
            //EPopupType.PopupCollection
        };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out PopupHeader header) == true)
            {
                var type = header.Type;
                popupDict[type] = header;
            }
        }
        //popupDict[EPopupType.PopupLimitedBoxItemResultDisplay].transform.SetParent(limitedBoxPopupParent);
    }

    public bool HasPopupStack() => popupStack.Count > 0;

    public void CloseCurrent()
    {
        var current = popupStack.Peek();
        PopupOpen(current);
    }

    // PrevAutoClose : 기존탭 알아서 닫기, ForcedOpen : 같은 것이 열려있어도 닫지 않고 유지
    public void PopupOpen(EPopupType popupType, bool PrevAutoClose = true, bool instantClose = false)
    {
        // 열린탭이 하나도 없는 경우
        if (popupStack.Count == 0)
        {
            popupStack.Push(popupType);
            popupDict[popupType].gameObject.SetActive(true);
            //popupDict[popupType].PopupEvent.Open();
            //ShowPopupButton(popupType, true);       // 하단버튼 보이기
        }
        // 열린탭이 있는 경우
        else
        {
            // 최근 탭과 현재 탭이 같을 경우
            if (popupStack.Peek() == popupType)
            {
                //OnOffBlockPanel();
                if (instantClose) { PopupClose(); }
                else { popupDict[popupType].PopupEvent.Close(); }
            }
            else // 최근 탭과 현재 탭이 다를 경우
            {
                // PrevAutoClose true면 다 닫고 현재탭 열기
                if (PrevAutoClose == true)
                {
                    PopupAllClear();
                }
                popupStack.Push(popupType);
                popupDict[popupType].transform.SetAsLastSibling();
                popupDict[popupType].gameObject.SetActive(true);
                popupDict[popupType].PopupEvent.Open();
                //ShowPopupButton(popupType, true);       // 하단버튼 보이기
            }
        }
    }
    public GameObject PopupOpenReturnObj(EPopupType popupType, bool PrevAutoClose = true)
    {
        
        // 열린탭이 하나도 없는 경우
        if (popupStack.Count == 0)
        {
            popupStack.Push(popupType);
            popupDict[popupType].gameObject.SetActive(true);
            return popupDict[popupType].gameObject;
        }
        // 열린탭이 있는 경우
        else
        {
            // 최근 탭과 현재 탭이 같을 경우
            if (popupStack.Peek() == popupType)
            {
                //OnOffBlockPanel();
                popupDict[popupType].PopupEvent.Close();
                return null;
            }
            else // 최근 탭과 현재 탭이 다를 경우
            {
                if (PrevAutoClose == true)
                {
                    PopupAllClear();
                }

                popupStack.Push(popupType);
                popupDict[popupType].transform.SetAsLastSibling();
                popupDict[popupType].gameObject.SetActive(true);
                popupDict[popupType].PopupEvent.Open();
                return popupDict[popupType].gameObject;
            }
        }
    }
    //private void OnOffBlockPanel()
    //{
    //    blockPanel.enabled = true;
    //    if(blockPanelRoutine != null)
    //    {
    //        StopCoroutine(blockPanelRoutine);
    //        blockPanelRoutine = null;
    //    }
    //    blockPanelRoutine = StartCoroutine(BlockPanelRout());
    //}
    //IEnumerator BlockPanelRout()
    //{
    //    yield return blockPanelWaitForSeconds;
    //    blockPanel.enabled = false;
    //}
    private void Update()
    {
        if (popupStack.Count > 0) return;

        if (reservationQueue.Count == 0) return;

        var target = reservationQueue.Dequeue();
        PopupOpen(target);
    }

    public void Reserve(EPopupType popupType)
    {
        if (reservationQueue.Contains(popupType) == false)
        {
            reservationQueue.Enqueue(popupType);
        }
    }


    public GameObject GetTopPopup() => popupDict[popupStack.Peek()].gameObject;


    public EPopupType GetTopPopupType()
    {
        if (popupStack.Count == 0)
        {
            return 0;
        }
        else
        {
            return popupStack.Peek();
        }
    }

    // 열린 팝업 전부 삭제
    public void PopupAllClear()
    {
        foreach (var header in popupDict.Values)
        {
            header.gameObject.SetActive(false);
        }
        popupStack.Clear();
    }


    // 닫기 애니메이션 때문에 바로 비활성화 하지 못함
    // PopupEvent에서 DOTween 애니메이션 끝난 후 호출될 함수
    public void PopupClose()
    {
        popupDict[popupStack.Peek()].gameObject.SetActive(false);
        popupStack.Pop();
    }

    public bool GetActivePopup()
    {
        return popupDict[popupStack.Peek()].gameObject.activeSelf;
    }

    // 하단 팝업버튼 보이기/숨기기
    /*
    private void ShowPopupButton(EPopupType Type, bool IsShow)
    {
        if (ShowButtonList.Contains(Type) == true)
        {
            OnShowPopupButton?.Invoke(IsShow, Type);
        }
    }*/

    [ContextMenu("Log Stack")]
    public void StackLog()
    {
        foreach (var item in popupStack)
        {
            print(item);
        }
    }


}
