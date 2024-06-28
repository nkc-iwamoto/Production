using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDataList : MonoBehaviour
{
    // ���[�U�[�̃f�[�^�����J
    public Laser[] laserData => laserDataList;

    // ���[�U�[�̃f�[�^
    Laser[] laserDataList = new Laser[4];

    // enum�̗v�f��
    int enumMaxNum;

    // �������Ă��郌�[�U�[�̐�
    int laserCount;

    bool isCanInit = true;

    private void Start()
    {
        // �v�f�����擾
        enumMaxNum = Common.GetEnumLength<COLOR_CODE>();
    }

    // ���[�U�[�̏�����
    public void InitalizeLaserData()
    { 
        if(!isCanInit)
        {
            return;
        }

        for (int i = 0; i < laserCount; i++)
        {
            laserDataList[i] = null;
        }
        isCanInit = false;
    }

    // �������[�U�[�̃f�[�^�����邩
    public bool IsSameLaserData(Laser laser)
    {
        // ���[�U�[�̔��ˌ��������ł͂Ȃ���
        int count = 0;
        for (int i = 0; i < laserDataList.Length; ++i)
        {
            // ���[�U�[�̃f�[�^�ɓ����Ă��Ȃ���΃R���e�j���[
            if (laserDataList[i] == null)
            { continue; }

            // ���[�U�[�̔��ˌ��������Ȃ�΃R���e�j���[
            if (laserDataList[i].StartGameObject == laser.StartGameObject)
            { continue; }

            // 1���₷
            count++;
        }
        // �����̂����邩
        if (count == laserDataList.Length)
        // �Ȃ�
        { return false; }

        // ����
        return true;
    }

    // �㏑������
    public void OverWritingLaserDataList(Laser laser)
    {
        // ���ˌ��������z��̓Y��
        int elementNumber = 0;

        for (int i = 0; i < laserDataList.Length; ++i)
        {
            // ���[�U�[�̃f�[�^�������Ă��Ȃ��ꍇ
            if (laserDataList[i] == null)
            // ���^�[��
            { return; }

            // �����
            // ���[�U�[�̔��ˌ��������Ȃ��
            if (laserDataList[i].StartGameObject == laser.StartGameObject)
            {
                // �z��̓Y�����擾
                elementNumber = i;
                break;
            }
        }
        // �㏑��
        laserDataList[elementNumber] = laser;
    }

    // ���[�U�[�̐F���Z�b�g����
    public int SetIntColor(int arrayNum)
    {
        // �Ԃ�l�̈���
        int tmp = 0;

        // �������Ă��郌�[�U�[�̐���1��
        const int SINGLE_COLOR = 1;

        // �z��̓Y�����擾
        int elementNumber = arrayNum - 1;

        // ���[�U�[�̃f�[�^�������Ă��邩
        if (laserDataList[0] == null)
        {  return tmp; }

        // ����
        // �������Ă��鐔��1��
        if (arrayNum == SINGLE_COLOR)
        {
            // 1�Ȃ�
            // �F�����͂Ȃ�
            tmp = laserDataList[elementNumber].ColorNum;
            return tmp;
        }

        for (int i = 0; i < arrayNum; ++i)
        {
            switch (laserDataList[i].ColorNum)
            {
                case (int)COLOR_CODE.Purple:
                    {
                        for (int k = 0; k < arrayNum; ++k)
                        {
                            if (!(laserDataList[k].ColorNum == (int)COLOR_CODE.Red || laserDataList[k].ColorNum == (int)COLOR_CODE.Blue))
                                continue;
                            
                            tmp = (int)COLOR_CODE.Purple;
                        }
                    }
                    break;
                case (int)COLOR_CODE.Yellow:
                    {
                        for (int k = 0; k < arrayNum; ++k)
                        {

                            if (!(laserDataList[k].ColorNum == (int)COLOR_CODE.Red || laserDataList[k].ColorNum == (int)COLOR_CODE.Green))                            
                                continue;

                            tmp = (int)COLOR_CODE.Yellow;
                        }
                    }
                    break;
                case (int)COLOR_CODE.Cyan:
                    {
                        for (int k = 0; k < arrayNum; ++k)
                        {
                            if (!(laserDataList[k].ColorNum == (int)COLOR_CODE.Blue || laserDataList[k].ColorNum == (int)COLOR_CODE.Green))
                                continue;
                            tmp = (int)COLOR_CODE.Cyan;
                        }
                    }
                    break;
                case (int)COLOR_CODE.White:
                    {
                        tmp = (int)COLOR_CODE.White;
                    }
                    break;
                default:
                    continue;
            }
            return tmp;
        }

        // �F����
        for (int i = 0; i < arrayNum; i++)
        {
            tmp += laserDataList[i].ColorNum;
        }
        tmp += 1;
        
        // enum�̗v�f����葽��������ő�l����
        int setColorNumber_int = tmp > enumMaxNum ? 0 : tmp;
        return setColorNumber_int;
    }

    // ���[�U�[�̃f�[�^��ǉ�����
    public int AddLaserDataList(Laser laser, int nowColorCode)
    {
        // ���[�U�[�̃f�[�^�������Ă��Ȃ����
        if (laserDataList[0] == null)
        {
            // �ǉ�
            laserDataList[0] = laser;
            // �������Ă��鐔���P�ɂ���
            laserCount = 1;
        }
        // ���[�U�[�̃f�[�^�������Ă��Ȃ����
        if (laserDataList[1] == null)
        {
            // 1�O�̃f�[�^�����������Ă��郌�[�U�[�̐F�Ɠ����ł͂Ȃ�
            // ����
            // ���̃��[�U�[�̐F�ƍ��������Ă��郌�[�U�[�̐F�Ɠ����ł͂Ȃ�
            if (laserDataList[0].ColorNum != laser.ColorNum
                && nowColorCode != laser.ColorNum)
            {
                // �ǉ�
                laserDataList[1] = laser;
                // �������Ă��鐔���Q�ɂ���
                laserCount = 2;
            }
        }
        // ���[�U�[�̃f�[�^�������Ă��Ȃ����
        if (laserDataList[2] == null)
        {
            // 1�O�̃f�[�^�����������Ă��郌�[�U�[�̐F�Ɠ����ł͂Ȃ�
            // ����
            // �Q�O�̃f�[�^�����������Ă��郌�[�U�[�̐F�Ɠ����ł͂Ȃ�
            // ����
            // ���̃��[�U�[�̐F�ƍ��������Ă��郌�[�U�[�̐F�Ɠ����ł͂Ȃ�
            if ((laserDataList[0].ColorNum != laser.ColorNum)
                && (laserDataList[1].ColorNum != laser.ColorNum)
                && (nowColorCode != laser.ColorNum))
            {
                // �ǉ�
                laserDataList[2] = laser;
                // �������Ă��鐔���R�ɂ���
                laserCount = 3;
            }
        }

        isCanInit = true;

        return laserCount;
    }
    private bool AAA(int intColorCode)
    {
        if (intColorCode > (int)COLOR_CODE.Purple)
            return false;


        return true;
    }
}
