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

    // �˾���ư�� ������ ����Ʈ
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

    // PrevAutoClose : ������ �˾Ƽ� �ݱ�, ForcedOpen : ���� ���� �����־ ���� �ʰ� ����
    public void PopupOpen(EPopupType popupType, bool PrevAutoClose = true, bool instantClose = false)
    {
        // �������� �ϳ��� ���� ���
        if (popupStack.Count == 0)
        {
            popupStack.Push(popupType);
            popupDict[popupType].gameObject.SetActive(true);
            //popupDict[popupType].PopupEvent.Open();
            //ShowPopupButton(popupType, true);       // �ϴܹ�ư ���̱�
        }
        // �������� �ִ� ���
        else
        {
            // �ֱ� �ǰ� ���� ���� ���� ���
            if (popupStack.Peek() == popupType)
            {
                //OnOffBlockPanel();
                if (instantClose) { PopupClose(); }
                else { popupDict[popupType].PopupEvent.Close(); }
            }
            else // �ֱ� �ǰ� ���� ���� �ٸ� ���
            {
                // PrevAutoClose true�� �� �ݰ� ������ ����
                if (PrevAutoClose == true)
                {
                    PopupAllClear();
                }
                popupStack.Push(popupType);
                popupDict[popupType].transform.SetAsLastSibling();
                popupDict[popupType].gameObject.SetActive(true);
                popupDict[popupType].PopupEvent.Open();
                //ShowPopupButton(popupType, true);       // �ϴܹ�ư ���̱�
            }
        }
    }
    public GameObject PopupOpenReturnObj(EPopupType popupType, bool PrevAutoClose = true)
    {
        
        // �������� �ϳ��� ���� ���
        if (popupStack.Count == 0)
        {
            popupStack.Push(popupType);
            popupDict[popupType].gameObject.SetActive(true);
            return popupDict[popupType].gameObject;
        }
        // �������� �ִ� ���
        else
        {
            // �ֱ� �ǰ� ���� ���� ���� ���
            if (popupStack.Peek() == popupType)
            {
                //OnOffBlockPanel();
                popupDict[popupType].PopupEvent.Close();
                return null;
            }
            else // �ֱ� �ǰ� ���� ���� �ٸ� ���
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

    // ���� �˾� ���� ����
    public void PopupAllClear()
    {
        foreach (var header in popupDict.Values)
        {
            header.gameObject.SetActive(false);
        }
        popupStack.Clear();
    }


    // �ݱ� �ִϸ��̼� ������ �ٷ� ��Ȱ��ȭ ���� ����
    // PopupEvent���� DOTween �ִϸ��̼� ���� �� ȣ��� �Լ�
    public void PopupClose()
    {
        popupDict[popupStack.Peek()].gameObject.SetActive(false);
        popupStack.Pop();
    }

    public bool GetActivePopup()
    {
        return popupDict[popupStack.Peek()].gameObject.activeSelf;
    }

    // �ϴ� �˾���ư ���̱�/�����
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
