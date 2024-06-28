using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionConverter : MonoBehaviour, IRayReceiver
{
    Vector3 direction;

    private RayShoter rayShoter;

    // int�^�̐F����
    int intColorCode;
    // ���܉�
    int count;
    Vector3 endPos;
    Laser laserData;
    bool isConvert = false;
    GameObject hitObject;

    [SerializeField, Header("�`�悷�郌�[�U�[�̃I�u�W�F�N�g")]
    GameObject laserObject;

    [SerializeField, Header("���[�U�[�����������Ƃ��̃G�t�F�N�g")]
    GameObject shotEffect;

    [SerializeField, Header("���[�U�[�����e�������̃G�t�F�N�g")]
    GameObject hitEffect;

    [SerializeField, Header("���[�U�[��`�悷��X�N���v�g")]
    LaserRendering laserRendering;

    [SerializeField]
    LaserDataList laserDataList;

    [SerializeField, Header("���[�U�[�̃G�t�F�N�g��`�悷��X�N���v�g")]
    LaserEffectRendering laserEffectRendering;

    private void Start()
    {
        // ���܂�����������w��
        direction = transform.up;
        // ���C���΂��X�N���v�g�𐶐�
        rayShoter = new RayShoter();
    }

    private void Update()
    {
        // ���܂�����������w��
        direction = transform.up;

        if (!isConvert)
        {
            EffectInit();
            return;
        }
        EffectActive();
        LaserRendering(endPos);
        EffectRendering(endPos);
        laserEffectRendering.LaserHitEffect_SetColor(intColorCode, endPos, hitEffect);

        if (hitObject == null) { return; }

        if (hitObject.CompareTag("Gimmick"))
        {
            laserEffectRendering.LaserHitEffect_SetActive(false, hitEffect);
        }

        laserDataList.InitalizeLaserData();
    }
    public void RayExit()
    {
        laserDataList.InitalizeLaserData();
        EffectInit();

        rayShoter.ExitRay();

        intColorCode = 0;
        count = 0;
        
        laserData = null;
        isConvert = false;
    }
    /// <summary>
    /// ���[�U�[���������Ă���Ƃ��A���[�U�[�����܂�����
    /// </summary>
    /// <param name="laser">���[�U�[���</param>
    /// <param name="f_count">1�t���[�����ł̋��܉�</param>
    /// <param name="maxConvertCount">1�t���[�����ł̍ő���܉�</param>
    public void RayStay(Laser laser, int f_count, int maxConvertCount)
    {

        if (laserDataList.IsSameLaserData(laser))
        {
            laserDataList.OverWritingLaserDataList(laser);
        }

        count = laserDataList.AddLaserDataList(laser, intColorCode);

        intColorCode = laserDataList.SetIntColor(count);
        laserData = new Laser(intColorCode, transform.position, direction, gameObject);
        f_count++;
        endPos = rayShoter.ShotRay(laserData, f_count, maxConvertCount, out GameObject hitObject);

        this.hitObject = hitObject;



        isConvert = true;
    }

    /// <summary>
    /// �G�t�F�N�g�̏�����
    /// </summary>
    private void EffectInit()
    {
        laserRendering.LaserSetActive(laserObject, false);
        laserEffectRendering.LaserShotEffect_SetActive(false, shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(false, hitEffect);
    }

    /// <summary>
    /// �G�t�F�N�g��\��������
    /// </summary>
    private void EffectActive()
    {
        laserRendering.LaserSetActive(laserObject, true);
        laserEffectRendering.LaserShotEffect_SetActive(true, shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(true, hitEffect);
    }

    /// <summary>
    /// ���[�U�[��transform�ƁA�F�̕ύX
    /// </summary>
    /// <param name="endPos">���[�U�[���������Ă�����W</param>
    private void LaserRendering(Vector3 endPos)
    {
        if (laserData == null)
        { return; }
        laserRendering.LaserTransform(laserData, endPos, laserObject);
        laserRendering.RenderingLaserColor(laserObject, intColorCode);
    }

    /// <summary>
    /// �G�t�F�N�g�̕`��
    /// </summary>
    /// <param name="endPos">���[�U�[���������Ă�����W</param>
    private void EffectRendering(Vector3 endPos)
    {
        laserEffectRendering.LaserShotEffect_SetColor(intColorCode, shotEffect);
        laserEffectRendering.LaserHitEffect_SetColor(intColorCode, endPos, hitEffect);
    }


}
