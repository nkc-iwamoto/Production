using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : GoalBase
{
    [SerializeField, Header("ÉSÅ[ÉãÇÃÉÅÉbÉVÉÖ")]
    MeshRenderer goalMeshRenderer;

    BoxCollider boxCollider;

    bool isGoal;

    private void Start()
    {
        Initalize();
        InitGimmickLamp();

        isGoal = false;
        goalMeshRenderer.enabled = false;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
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

    public override void Open()
    {
        if (isGoal)
        { return; }

        boxCollider.enabled = true;
        isGoal = true;
        goalMeshRenderer.enabled = true;

        GetComponent<Sound.GoalOpenSound>().Set_isGoal(isGoal);
    }

    public override void Close()
    {
        if (!isGoal)
        { return; }

        boxCollider.enabled = false;
        isGoal = false;
        goalMeshRenderer.enabled = false;
        GetComponent<Sound.GoalOpenSound>().Set_isGoal(isGoal);
    }
}
