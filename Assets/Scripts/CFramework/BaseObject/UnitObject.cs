using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;
using DG.Tweening;
using UnityEngine.Events;

public class UnitObject : MonoBehaviour, IPathFinding, IClickEvent
{
    BillboardSprite sprite;
    Seeker seeker;
    public float speed = 1;
    Vector3 _destination;
    public List<Vector3> PathList;

    EventTrigger mouseEventTrigger;

    private void Start()
    {
        mouseEventTrigger = GetComponent<EventTrigger>();
        mouseEventTrigger.triggers = new List<EventTrigger.Entry>();
        //实例化一个EventTrigger.Entry对象
        EventTrigger.Entry[] enter = new EventTrigger.Entry[3];
        enter[0] = new EventTrigger.Entry();
        enter[1] = new EventTrigger.Entry();
        enter[2] = new EventTrigger.Entry();
        //根据方法需要的参数依次写
        //3.指定事件触发的类型
        enter[0].eventID = EventTriggerType.PointerClick;
        enter[1].eventID = EventTriggerType.PointerEnter;
        enter[2].eventID = EventTriggerType.PointerExit;
        //4.指定事件触发的方法
        enter[0].callback = new EventTrigger.TriggerEvent();
        enter[1].callback = new EventTrigger.TriggerEvent();
        enter[2].callback = new EventTrigger.TriggerEvent();
        //5.需要添加命名空间using UnityEngine.Events;,并传入一个要调用的方法名称,这句可以先写，因为这里初始化只需要一个待调用的方法
        UnityAction<BaseEventData> MouseClick = new UnityAction<BaseEventData>(MouseClickEvent);
        UnityAction<BaseEventData> MouseEnter = new UnityAction<BaseEventData>(MouseEnterEvent);
        UnityAction<BaseEventData> MouseExit = new UnityAction<BaseEventData>(MouseExitEvent);
        //6.添加
        enter[0].callback.AddListener(MouseClick);
        enter[1].callback.AddListener(MouseEnter);
        enter[2].callback.AddListener(MouseExit);
        mouseEventTrigger.triggers.Add(enter[0]);
        mouseEventTrigger.triggers.Add(enter[1]);
        mouseEventTrigger.triggers.Add(enter[2]);
    }
    public void StartMove(Path P_path)
    {
        PathList = new List<Vector3>();
        transform.DOKill(false);
        Vector3[] pos = new Vector3[P_path.path.Count];
        for (int a = 0; a < pos.Length; a++)
        {
            PathList.Add((Vector3)P_path.path[a].position);
        }

        StepCall();
    }

    void StepCall()
    {
        PathList.Remove(PathList[0]);
        if (PathList.Count < 1)
            return;
        Vector3 look = PathList[0];
        look.y = transform.position.y;
        transform.LookAt(look);
        //sprite.
        if (PathList.Count >= 0)
        {
            transform.DOMove(PathList[0], speed).SetSpeedBased().OnComplete(StepCall).SetEase(Ease.Linear);
        }
    }

    public void GetPath()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (PathList != null && PathList.Count > 0)
        {
            Gizmos.DrawLine(transform.position, PathList[0]);
            for (int a = 0; a < PathList.Count - 1; a++)
                Gizmos.DrawLine(PathList[a], PathList[a + 1]);
        }
    }
    public void MouseClickEvent(BaseEventData pd)
    {
        VCamControl._instance.SelectTarget(this);
    }
    public void MouseEnterEvent(BaseEventData pd)
    {
        print(pd);
    }
    public void MouseExitEvent(BaseEventData pd)
    {
        print(pd);
    }
}
