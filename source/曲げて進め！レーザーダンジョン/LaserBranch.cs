using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBranch : MonoBehaviour, IRayReceiver
{
    [SerializeField, Header("�`�悷�郌�[�U�[�̃I�u�W�F�N�g")]
    GameObject[] laserObjects;

    [SerializeField, Header("�`�悷�郌�[�U�[�̃G�t�F�N�g(hit)�̃I�u�W�F�N�g")]
    GameObject[] laserHitEffect;

    [SerializeField, Header("�`�悷�郌�[�U�[�̃G�t�F�N�g(shot)�̃I�u�W�F�N�g")]
    GameObject[] laserShotEffect;

    [SerializeField, Header("��]����p�x(Deg)")]
    private float rotationAngle;

    // ��̕`�悷�邷�����v�g
    private RayShoter[] rayShoters;

    // ���[�U�[�̃J���[
    private int intColorCode;

    // ���򂵂Ă��邩
    private bool isBranch;

    // ���򂷂鐔
    private int branchLaserCount = 2;

    // ���򂵂����[�U�[�̃f�[�^�z��
    private Laser[] branchLaserData;

    // ���򂵂����[�U�[�̍Ō�ɓ��������|�W�V�����̔z��
    private Vector3[] branchLaser_endPos;

    // ���򂵂����[�U�[�̍Ō�ɓ��������I�u�W�F�N�g�̔z��
    private GameObject[] hitObjects;

    // ���򂵂����[�U�[�̕���
    private Vector3[] branchDirection;

    // �������Ă��郌�[�U�[�̐�
    private int count;

    [SerializeField, Header("���[�U�[��`�悷��X�N���v�g")]
    LaserRendering laserRendering;

    [SerializeField, Header("���[�U�[�̃G�t�F�N�g��`�悷��X�N���v�g")]
    LaserEffectRendering laserEffectRendering;

    [SerializeField,Header("���[�U�[�̃f�[�^�Ɋւ���X�N���v�g")]
    LaserDataList laserDataList;


    private void Start()
    {
        rayShoters = new RayShoter[branchLaserCount];
        for(int i = 0;i<branchLaserCount;++i)
        {
            rayShoters[i] = new RayShoter();
        }

        hitObjects = new GameObject[branchLaserCount];
        branchLaserData = new Laser[branchLaserCount];
        branchLaser_endPos = new Vector3[branchLaserCount];
        branchDirection = new Vector3[branchLaserCount];
    }

    private void Update()
    {
        // ���򂵂Ă邩
        if (!isBranch)
        {
            // ���Ă��Ȃ��Ȃ�
            // ���[�U�[�ƃG�t�F�N�g�������Ȃ�����
            InitRender();

            laserDataList.InitalizeLaserData();

            return;
        }
        // ���Ă���Ȃ�
        for (int i = 0; i < branchLaserCount; ++i)
        {
            // ���[�U�[��`��
            laserRendering.LaserSetActive(laserObjects[i], true);

            // ���[�U�[��transform�𒲐�
            laserRendering.LaserTransform(branchLaserData[i], branchLaser_endPos[i], laserObjects[i]);
            laserRendering.LaserAngle(branchLaserData[i], branchLaser_endPos[i], laserObjects[i]);

            // ���[�U�[�̃J���[��`�ʂ���
            laserRendering.RenderingLaserColor(laserObjects[i], intColorCode);

            // ���[�U�[�̃G�t�F�N�g��`��
            laserEffectRendering.LaserHitEffect_SetActive(true, laserHitEffect[i]);
            laserEffectRendering.LaserShotEffect_SetActive(true, laserShotEffect[i]);

            // ���[�U�[�̃G�t�F�N�g�̃J���[��`��
            laserEffectRendering.LaserHitEffect_SetColor(intColorCode, branchLaser_endPos[i], laserHitEffect[i]);
            laserEffectRendering.LaserShotEffect_SetColor(intColorCode, laserShotEffect[i]);

            // �Ō�ɓ��������I�u�W�F�N�g�����邩
            if (hitObjects[i] == null)
            { continue; }

            // ����ꍇ
            // ���ꂪGimmick�^�O�������Ă��邩
            if (hitObjects[i].CompareTag("Gimmick"))
            {
                // �����Ă���Ȃ�
                // �������Ă���G�t�F�N�g�������Ȃ�����
                laserEffectRendering.LaserHitEffect_SetActive(false, laserHitEffect[i]);
            }
        }

        // ���[�U�[�����������G�t�F�N�g�̊p�x�𒲐�
        laserEffectRendering.LaserShotEffect_Angle(laserShotEffect[0], rotationAngle);
        laserEffectRendering.LaserShotEffect_Angle(laserShotEffect[1], -rotationAngle);
    }
    public void RayExit()
    {
        for (int i = 0; i < branchLaserCount; ++i)
        {
            rayShoters[i].ExitRay();
            InitRender();            
        }

        laserDataList.InitalizeLaserData();

        intColorCode = 0;
        count = 0;

        // ���򂵂Ă邩
        if (!isBranch)
            return;

        // ���򂵂ĂȂ���
        isBranch = false;
    }

    public void RayStay(Laser laser, int f_count, int maxConvertCount)
    {
        // ���򂳂���
        Branch(laser, f_count, maxConvertCount);

        // ���򂳂��Ă��邩
        if (isBranch)
        { return; }

        // ���򂵂Ă��
        isBranch = true;
    }

    private void Branch(Laser laser, int f_count, int maxConvertCount)
    {
        // ���������Ă��郌�[�U�[�̔��ˌ��������Ȃ�
        if (laserDataList.IsSameLaserData(laser))
        {
            // �����
            // �㏑�������
            laserDataList.OverWritingLaserDataList(laser);
        }

        // ���[�U�[�̃f�[�^��ǉ����āA���������Ă��郌�[�U�[�̐���Ԃ�
        count = laserDataList.AddLaserDataList(laser, intColorCode);

        // ���[�U�[�̐F����
        intColorCode = laserDataList.SetIntColor(count);

        // �������w��
        Vector3 direction = transform.right;

        // �����ϊ��������𑝂₷
        f_count++;

        // ���[�U�[�𕪊򂳂���
        branchDirection[0] = Quaternion.Euler(0, 0, rotationAngle) * direction;
        branchDirection[1] = Quaternion.Euler(0, 0, -rotationAngle) * direction;

        // ���[�U�[�̃f�[�^���擾
        branchLaserData[0] = new Laser(intColorCode, transform.position, branchDirection[0], gameObject);
        branchLaserData[1] = new Laser(intColorCode, transform.position, branchDirection[1], gameObject);

        // ���[�U�[���ˏo���āA���������|�W�V�������擾
        branchLaser_endPos[0] = rayShoters[0].ShotRay(branchLaserData[0], f_count, maxConvertCount, out GameObject hitObject1);
        branchLaser_endPos[1] = rayShoters[1].ShotRay(branchLaserData[1], f_count, maxConvertCount, out GameObject hitObject2);

        // �Ō�ɓ��������I�u�W�F�N�g���擾
        hitObjects[0] = hitObject1;
        hitObjects[1] = hitObject2;
    }

    void InitRender()
    {
        for (int i = 0; i < branchLaserCount; ++i)
        {
            laserRendering.LaserSetActive(laserObjects[i], false);
            laserEffectRendering.LaserHitEffect_SetActive(false, laserHitEffect[i]);
            laserEffectRendering.LaserShotEffect_SetActive(false, laserShotEffect[i]);
        }
    }
}
