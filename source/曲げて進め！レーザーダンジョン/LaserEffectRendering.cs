using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffectRendering : MonoBehaviour
{
    [SerializeField, Header("���[�U�[�̐F�f�[�^")]
    LaserColorData laserColorData;

    [Header("�|�C���g���C�grange�̉������l")]
    public float fluctuationNumber;

    [Header("�|�C���g���C�g��range�̍ő�l")]
    public float maxPointRange;
    [Header("�|�C���g���C�g��range�̍ŏ��l")]
    public float minPointRange;

    public void LaserHitEffect_SetActive(bool isActive,GameObject hitEffect)
    {
        hitEffect.SetActive(isActive);
    }
    public void LaserShotEffect_SetActive(bool isActive,GameObject shotEffect)
    {
        shotEffect.SetActive(isActive);
    }

    public void LaserShotEffect_Angle(GameObject shotEffect,float angle)
    {
        shotEffect.transform.localRotation = Quaternion.Euler(0,0,angle + 90);
    }

    public void LaserShotEffect_SetColor(int intColorCode,GameObject shotEffect)
    {
        GameObject effectParticleObject = GetChildObject(shotEffect, 0);

        // �G�t�F�N�g�̃x�[�X�J���[��ύX
        SetColor_BaseColor(effectParticleObject, intColorCode);

        // �G�t�F�N�g�̃G�~�b�V������ύX
        SetColor_Emission(effectParticleObject, intColorCode);

        // ���C�g�̃J���[��ύX
        SetLightColor(shotEffect, intColorCode);
    }

    public void LaserHitEffect_SetColor(int intColorCode, Vector3 hitPos,GameObject hitEffect)
    {
        // �G�t�F�N�g���Đ������|�W�V������ύX
        SetPosition(hitEffect, hitPos);

        // �G�t�F�N�g�̃p�[�e�B�N�����擾
        GameObject effectParticleObject = GetChildObject(hitEffect, 0);

        // �G�t�F�N�g�̃x�[�X�J���[��ύX
        SetColor_BaseColor(effectParticleObject, intColorCode);

        // �G�t�F�N�g�̃G�~�b�V������ύX
        SetColor_Emission(effectParticleObject, intColorCode);

        // �p�[�e�B�N���̃X�^�[�g�J���[��ύX
        SetStartColor(effectParticleObject, intColorCode);

        SetMeshColor(hitEffect, intColorCode);

        GameObject pointLightObject = GetChildObject(hitEffect, 2);
        Light poingtLight = pointLightObject.GetComponent<Light>();


        PointLight_SetColor(poingtLight, intColorCode);

        PointLight_Expand_Shrink(poingtLight);
    }


    private void SetColor_Emission(GameObject effect, int intColorCode)
    {
        // �G�t�F�N�g�̃����_���[���擾
        Renderer renderer = effect.GetComponent<Renderer>();

        // �}�e���A���ɃG�~�b�V������������
        renderer.material.EnableKeyword("_EMISSION");

        // �}�e���A���ɓn���J���[���擾
        Color color = laserColorData.effectColors[intColorCode];

        // Emission�̃J���[�ύX
        renderer.material.SetColor("_EmissionColor", color);
    }
    private void SetColor_BaseColor(GameObject effect, int intColorCode)
    {
        // �G�t�F�N�g�̃����_���[���擾
        Renderer renderer = effect.GetComponent<Renderer>();

        // �}�e���A���ɓn���J���[���擾
        Color color = laserColorData.colors[intColorCode];

        // �x�[�X�J���[��ύX
        renderer.material.color = color;
    }

    private void SetStartColor(GameObject effect, int intColorCode)
    {
        // �X�^�[�g�J���[�ɓn���J���[���擾
        Color color = laserColorData.colors[intColorCode];

        // ParticleSystem.MainModule���擾
        ParticleSystem.MainModule particleSystem = effect.GetComponent<ParticleSystem>().main;

        // �X�^�[�g�J���[��ύX
        particleSystem.startColor = color;
    }
    private void SetPosition(GameObject effect, Vector3 pos)
    {
        // �|�W�V������ύX
        effect.transform.position = pos;
    }


    private void SetLightColor(GameObject effect, int intColorCode)
    {
        // �G�t�F�N�g�̃��C�g���擾
        GameObject light = effect.transform.GetChild(1).gameObject;

        // ���C�g�̃J���[��ύX
        light.GetComponent<Light>().color = laserColorData.colors[intColorCode];
    }

    private void SetMeshColor(GameObject effect,int intColorCode)
    {
        GameObject effectChild = effect.transform.GetChild(1).gameObject;

        MeshRenderer meshRenderer = effectChild.GetComponent<MeshRenderer>();
        meshRenderer.material.color = laserColorData.colors[intColorCode]
;    }


    private void PointLight_SetColor(Light pointLight, int intColorCode)
    {
        pointLight.intensity = 0.05f;
        pointLight.color = laserColorData.colors[intColorCode];
    }

    // �|�C���g���C�g��Range�̊g��E�k��
    private void PointLight_Expand_Shrink(Light pointLight)
    {
        float sin = Mathf.Abs(Mathf.Sin(2 * Mathf.PI * fluctuationNumber * Time.time)) * maxPointRange;

        if(sin <= minPointRange)
        {
            sin = minPointRange;
        }

        pointLight.range = sin;
    }

    private GameObject GetChildObject(GameObject gameObject, int getChildNumber)
    {
        return gameObject.transform.GetChild(getChildNumber).gameObject;
    }

}
