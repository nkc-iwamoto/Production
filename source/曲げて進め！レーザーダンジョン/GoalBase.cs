using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoalBase : MonoBehaviour
{
    [Header("対応するスイッチのスクリプト")]
    public Switch[] switches;

    private int goalCount;

    [Header("一度だけ動かすか")]
    public bool isOnlyOnce;

    [HideInInspector,Header("ランプのメッシュレンダラー")]
    public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();


    [Header("ギミックが当たったときにランプが光るスクリプト")]
    public GimmickLampEmission gimmickOnLamp;

    [Header("レーザーの色データ")]
    public LaserColorData laserColorData;

    // ゴール条件を保存する変数
    private bool isGoal_keep = false;

    private int laserColorData_int;


    [HideInInspector]
    public Vector3 center;

    [HideInInspector]
    public GameObject toGenerateObject;


    public List<GameObject> generatedObjectList = new List<GameObject>();


    public void Initalize()
    {
        goalCount = switches.Length;
        gimmickOnLamp = new GimmickLampEmission();
    }

    public bool DoorCondition()
    {

        if (isOnlyOnce && isGoal_keep) { return isGoal_keep; }

        if (goalCount == 0) { return isGoal_keep; }
        for (int i = 0; i < goalCount; ++i)
        {
            if (switches[i] == null)
            { return false; }

            if (!switches[i].getIsGoal)
            { return false; }


            laserColorData_int = switches[i].GetGoalColor;
        }
        isGoal_keep = true;

        return isGoal_keep;
    }

    public void OnGimmickLamp()
    {
        for (int i = 0; i < goalCount; i++)
        {
            if (meshRenderers.Count != goalCount)
            {
                break;
            }

            if (meshRenderers[i] == null)
            { continue; }

            if (switches[i] == null)
            { return; }

            if (!switches[i].getIsGoal)
            {
                gimmickOnLamp.ExitColorEmission(meshRenderers[i], laserColorData.colors[switches[i].GetGoalColor]);
                continue;
            }
            gimmickOnLamp.SetColorEmission(meshRenderers[i], laserColorData.colors[switches[i].GetGoalColor]);
        }
    }

    public void InitGimmickLamp()
    {
        for (int i = 0; i < goalCount; i++)
        {
            if (meshRenderers.Count != goalCount)
            {
                break;
            }

            if (meshRenderers[i] == null)
            { continue; }

            gimmickOnLamp.SetColorEmission(meshRenderers[i], laserColorData.colors[switches[i].GetGoalColor] * 0.25f);
        }
    }



    public abstract void Open();
    public abstract void Close();

}
