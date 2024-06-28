using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RayShoter
{
    // �Ō�ɓ��������I�u�W�F�N�g��ۑ�����ϐ�
    GameObject lastHitObj;

    // ���[�U�[���������Ă��鎞
    public Vector3 ShotRay(Laser laser, int f_count, int maxConvertCount, out GameObject hitObject)
    {
        // ���[�U�[���������Ă�����W�̕ϐ���������
        Vector3 laserHitPos = Vector3.zero;
        // �������Ă���I�u�W�F�N�g�̏�����
        hitObject = null;

        // ���C���΂�
        if (!Physics.Raycast(laser.StartPos, laser.Direction, out RaycastHit hit)) { return laserHitPos; }

        // ������������IRayreceiver�������Ă��邩-------------------------------------------------------

        if (IsExit(hit ,out IRayReceiver rayReceiver))
        {
            // �����Ă��Ȃ����
            // �Ō�ɓ��������I�u�W�F�N�g�ɓ������Ă��Ȃ��������Ăяo��
            ExitRay();
        }
        else
        {
            // �����Ă����
            // ���[�U�[���Ȃ���񐔂��ő�𒴂�����
            if (f_count <= maxConvertCount)
            {
                // �������Ă��鏈�����Ăяo��
                rayReceiver.RayStay(laser, f_count, maxConvertCount);
            }
            // �Ō�ɓ��������I�u�W�F�N�g���L������
            lastHitObj = hit.collider.gameObject;
        }
        // ---------------------------------------------------------------------------------------------

        // �v���C���[�ɓ���������
        if (hit.collider.TryGetComponent(out PlayerDeath playerDeath))
        {
            // �v���C���[�����S�����郁�\�b�h���Ăяo��
            playerDeath.Death().Preserve();
        }

        // �������Ă���I�u�W�F�N�g�ɑ��
        hitObject = lastHitObj;

        // ���[�U�[�������������W����
        laserHitPos = hit.point;

        // ���[�U�[�������������W��Ԃ�
        return laserHitPos;
    }

    public void ExitRay()
    {
        // �Ō�ɓ��������I�u�W�F�N�g���Ȃ������� return
        if (lastHitObj == null) { return; }
        // �Ō�ɓ��������I�u�W�F�N�g��IRayReceiver�������Ă��邩
        // �����Ă��Ȃ���� return
        if (!lastHitObj.TryGetComponent(out IRayReceiver lastHitRecevier)) { return; }
        // �Ō�ɓ��������I�u�W�F�N�g������������
        lastHitObj = null;
        // �����Ă���Γ������Ă��Ȃ��������Ăяo��
        lastHitRecevier.RayExit();
    }

    // ���[�U�[���������Ă��Ȃ����������Ă�����
    private bool IsExit(RaycastHit hit, out IRayReceiver rayReceiver)
    {
        // IRayReceiver�������Ă��Ȃ���
        // ���^�[��
        if (!hit.collider.TryGetComponent(out rayReceiver)) { return true; }
        // �O�t���[�����[�U�[�����������I�u�W�F�N�g�����t���[���������Ă���I�u�W�F�N�g�Ɠ�����
        // ����
        // �O�t���[�����[�U�[�����������I�u�W�F�N�g��null�ł͂Ȃ���
        // ���^�[��
        if(lastHitObj != hit.collider.gameObject�@&& lastHitObj != null) { return true; }


        return false;
    }
}