using UnityEngine;
using UnityEngine.InputSystem;

public class MaterialOffsetSetter : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private string offsetKey;
    [SerializeField] private Transform matOffset;

    private void SetOffset(Vector2 offset)
    {
        string key = string.IsNullOrEmpty(offsetKey) ? "_MainTex_ST" : offsetKey;
        Vector4 matst = mat.GetVector(key);
        mat.SetVector(key, new Vector4(matst.x, matst.y, offset.x, offset.y));
    }

    private void Update()
    {
        if (matOffset == null) matOffset = transform;
        SetOffset(new Vector2(matOffset.position.x, matOffset.position.y));
    }

    private void OnDestroy()
    {
        SetOffset(new Vector2(0f, 0f));
    }
}
