using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDispenser : MonoBehaviour
{
    public enum COLOR
    {
        Red = 1,
        Blue,
        Green,
        Purple,
        Yellow,
        Cyan,
        white,
    }


    [SerializeField, Header("���[�U�[�̍ő���܉�")]
    int maxConvertCount;

    [SerializeField, Header("���[�U�[�̐F")]
    COLOR color;

    [SerializeField, Header("�`�悷�郌�[�U�[�̃I�u�W�F�N�g")]
    GameObject laserObject;

    [SerializeField, Header("���[�U�[�����������Ƃ��̃G�t�F�N�g")]
    GameObject shotEffect;

    [SerializeField, Header("���[�U�[�����e�������̃G�t�F�N�g")]
    GameObject hitEffect;

    [SerializeField, Header("���[�U�[��`�悷��X�N���v�g")]
    LaserRendering laserRendering;

    [SerializeField, Header("���[�U�[�̃G�t�F�N�g��`�悷��X�N���v�g")]
    LaserEffectRendering laserEffectRendering;

    // ���[�U�[�̓����蔻����s���X�N���v�g
    RayShoter rayShot;

    // ���[�U�[�̃f�[�^
    Laser laserData;

    // ���[�U�[�̐F�iint�^�ɂ������́j
    int value;

    private void Start()
    {
        rayShot = new RayShoter();

    }


  
    private void LateUpdate()
    {
        value = (int)color;
        Vector3 direction = transform.up;
        Vector3 startPos = transform.position;

        laserData = new Laser(value, startPos, direction, gameObject);
        Vector3 endPos = rayShot.ShotRay(laserData, 0, maxConvertCount, out GameObject hitObject);

        if (endPos == Vector3.zero)
        {
            EffectInit();
        }
        EffectActive();
       
        LaserRendering(endPos);
        EffectRendering(endPos);
        laserEffectRendering.LaserHitEffect_SetColor(value, endPos,hitEffect);

        if (hitObject == null)
        { return; }    
        if (hitObject.CompareTag("Gimmick"))
        {
            laserEffectRendering.LaserHitEffect_SetActive(false,hitEffect);
        }
    }

    private void EffectInit()
    {
        laserRendering.LaserSetActive(laserObject,false);
        laserEffectRendering.LaserShotEffect_SetActive(false,shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(false,hitEffect);
    }

    private void EffectActive()
    {
        laserRendering.LaserSetActive(laserObject,true);
        laserEffectRendering.LaserShotEffect_SetActive(true,shotEffect);
        laserEffectRendering.LaserHitEffect_SetActive(true,hitEffect);
    }

    private void LaserRendering(Vector3 endPos)
    {
        laserRendering.LaserTransform(laserData, endPos, laserObject);
        laserRendering.RenderingLaserColor(laserObject,value);
    }

    private void EffectRendering(Vector3 endPos)
    {
        laserEffectRendering.LaserShotEffect_SetColor(value,shotEffect);
        laserEffectRendering.LaserHitEffect_SetColor(value, endPos,hitEffect);
    }
}