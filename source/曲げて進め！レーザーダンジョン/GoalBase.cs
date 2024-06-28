using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoalBase : MonoBehaviour
{
    [Header("�Ή�����X�C�b�`�̃X�N���v�g")]
    public Switch[] switches;

    private int goalCount;

    [Header("��x������������")]
    public bool isOnlyOnce;

    [HideInInspector,Header("�����v�̃��b�V�������_���[")]
    public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();


    [Header("�M�~�b�N�����������Ƃ��Ƀ����v������X�N���v�g")]
    public GimmickLampEmission gimmickOnLamp;

    [Header("���[�U�[�̐F�f�[�^")]
    public LaserColorData laserColorData;

    // �S�[��������ۑ�����ϐ�
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
