using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorOpen : GoalBase
{
    Vector3 endPos;

    [HideInInspector]
    public Vector3 moveDistance;

    [System.Serializable,HideInInspector]
    public class MoveDistanceSetting
    {
        public bool x, y, z;
    }
    [HideInInspector]
    public MoveDistanceSetting moveDistanceSetting = new MoveDistanceSetting();
    [HideInInspector]
    public GameObject door;
    [HideInInspector]
    public float moveTime;

    public int SwitchCount { get { return switchCount; }set { switchCount = value; } }
        private int switchCount;

    bool isOpen;
    bool isAdded;

    private void Start()
    {
        Initalize();
        InitGimmickLamp();
    }

    private void Update()
    {
        OnGimmickLamp();

        if (!DoorCondition())
        {
            Close();
            return;
        }
        Open();
    }

    public override void Close()
    {
        if (!isOpen)
        {
            isAdded = false;
            return;
        }
        DoorLocalMove(false);
    }

    public override void Open()
    {
        if (isOpen)
        {
            isAdded = false;
            return;
        }
        DoorLocalMove(true);
    }

    private void DoorLocalMove(bool inFinished)
    {
        if (TryGetComponent(out OpenDoorGrops.OpenDoorSound open))
        {
            GetComponent<OpenDoorGrops.OpenDoorSound>().Set_isOpen(inFinished);
        }

        if (!isAdded)
        {
            endPos = inFinished ? transform.position + moveDistance : transform.position - moveDistance;
            isAdded = true;
        }
        door.transform.DOLocalMove(endPos, moveTime)
                   .SetLink(gameObject)
                   .OnComplete(() => isOpen = inFinished);
    }
}