using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterAnimator : LetterController
{
    private TMP_MeshInfo[] cachedMeshInfo;
    public Vector3[][] OriginalVertices { get; set; }
    private bool isInitialized = false;
    public bool NeedsRefresh { get; set; } = true;

    public override void TextUpdate(TMP_TextInfo info)
    {
        base.TextUpdate(info);
        NeedsRefresh = true;
    }

    public override void Start()
    {
        base.Start();
        InitializeVertices();
    }

    public void InitializeVertices()
    {
        text.ForceMeshUpdate();

        if (text.textInfo.characterCount == 0)
        {
            isInitialized = false;
            return;
        }

        cachedMeshInfo = text.textInfo.CopyMeshInfoVertexData();
        OriginalVertices = new Vector3[cachedMeshInfo.Length][];

        for (int i = 0; i < cachedMeshInfo.Length; i++)
        {
            OriginalVertices[i] = cachedMeshInfo[i].vertices.Clone() as Vector3[];
        }

        isInitialized = true;
    }

    public virtual void ApplyAnimation(Vector3[][] vertices)
    {

    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (!isInitialized) return;

        for (int i = 0; i < OriginalVertices.Length; i++)
        {
            if (i < text.textInfo.meshInfo.Length)
            {
                text.textInfo.meshInfo[i].mesh.vertices = OriginalVertices[i];
                text.UpdateGeometry(text.textInfo.meshInfo[i].mesh, i);
            }
        }
    }
}