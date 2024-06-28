using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickLampEmission
{
   public void SetColorEmission(MeshRenderer meshRenderer,Color color)
    {
        int intensity = 1;
        meshRenderer.material.EnableKeyword("_EMISSION");
        float factor = Mathf.Pow(2, intensity);

        meshRenderer.material.SetColor("_EmissionColor", new Color(color.r * factor, color.g * factor, color.b * factor));
    }

    public void ExitColorEmission(MeshRenderer meshRenderer, Color color)
    {
        // �G�~�b�V����������
        meshRenderer.material.DisableKeyword("_EMISSION");

        // �F������������
        meshRenderer.material.SetColor("_EmissionColor", color);

        meshRenderer.material.color = color * 0.5f;
    }
}
