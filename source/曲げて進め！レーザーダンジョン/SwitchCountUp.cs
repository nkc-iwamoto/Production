using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCountUp : MonoBehaviour
{
    [SerializeField, Header("����Ԋu����")]
    float intervalTime;
    [SerializeField, Header("�X�C�b�`�̃G�~�b�V������������X�N���v�g")]
    SwitchEmission switchEmission;

    float countDownTime = 0;
    // �����Ă��鐔
    int switchOnLightCount = 0;
    // �����Ă���X�C�b�`�̍ő�l
    int maxOnLightCount = 4;

    // �X�C�b�`�̔�������
    public bool IsOnSwitch(GameObject[] objs,Color color,int intensity)
    {
        // �X�C�b�`���S����������
        if (switchOnLightCount >= objs.Length)
        {
            // ���Ԃ��ő�l�ɂ�����
            countDownTime = intervalTime * 3;

            // �������Ă��锻����o��
            return true;
        }

        // ���Ԃ𑪒�
        countDownTime += Time.deltaTime;

        // �J�E���g�A�b�v�@�X�C�b�`�����点��
        if (countDownTime > intervalTime * 3)
        {
            switchEmission.ObjectEmission(objs[3], color,intensity);
            // �����Ă��鐔���ő�ɂ���
            switchOnLightCount = maxOnLightCount;
            
        }
        else if (countDownTime > intervalTime * 2)
        {
            switchEmission.ObjectEmission(objs[2], color,intensity);
        }
        else if (countDownTime > intervalTime)
        {
            switchEmission.ObjectEmission(objs[1], color,intensity);
        }
        else if (countDownTime > 0)
        {
            switchEmission.ObjectEmission(objs[0], color,intensity);
        }

        return false;
    }

    // �X�C�b�`�̏���������
    public void Init(GameObject[] objects,Color initColor)
    {
        // �X�C�b�`�̌�������
        foreach (var item in objects)
        {
            switchEmission.ObjectEmissionExit(item, initColor);
        }
        // ���肵�����Ԃ�������
        countDownTime = 0;
        // �����Ă��鐔��������
        switchOnLightCount = 0;


    }
}