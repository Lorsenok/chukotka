using UnityEngine;

public class MaterialOffsetSetter : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private string offsetKey;
    [SerializeField] private Transform matOffset;

    private void Update()
    {
        if (matOffset == null) matOffset = transform;
        string key = string.IsNullOrEmpty(offsetKey) ? "_MainTex_ST" : offsetKey;
        Vector4 matst = mat.GetVector(key);
        mat.SetVector(key, new Vector4(matst.x, matst.y, matOffset.position.x, matOffset.position.y));
    }
}
