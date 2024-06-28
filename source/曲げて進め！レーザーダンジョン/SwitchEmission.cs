using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEmission : MonoBehaviour
{
    // �G�~�b�V������������
    public void ObjectEmission(GameObject obj, Color color,int intensity)
    {
        // ���b�V�������_���[���擾
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        
        // �G�~�b�V������������
        meshRenderer.material.EnableKeyword("_EMISSION");

        // HDR������
        float factor = Mathf.Pow(2, intensity);

        // �F��ύX
        meshRenderer.material.SetColor("_EmissionColor", new Color(color.r * factor, color.g * factor, color.b * factor));
    }

    // �F������������
    public void ObjectEmissionExit(GameObject obj,Color initColor)
    {
        // ���b�V�������_���[���擾
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();

        // �G�~�b�V����������
        meshRenderer.material.DisableKeyword("_EMISSION");

        // �F������������
        meshRenderer.material.SetColor("_EmissionColor", initColor * 0.5f);
    }
}
