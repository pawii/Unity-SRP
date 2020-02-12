using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancedColor : MonoBehaviour
{
    [SerializeField]
    private Color color = Color.white;

    private static MaterialPropertyBlock materialPropertyBlock;
    private static int ColorPropertyId = Shader.PropertyToID("_Color");

    private void OnValidate()
    {
        if (materialPropertyBlock == null)
        {
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        materialPropertyBlock.SetColor(ColorPropertyId, color);
        GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
    }
}
