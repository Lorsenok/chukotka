using TMPro;
using UnityEngine;

public class LetterAnimatorsCombiner : MonoBehaviour
{
    [SerializeField] private LetterAnimator[] letterAnimators;

    public void Update()
    {
        if (letterAnimators == null || letterAnimators.Length == 0)
            return;

        TextMeshProUGUI textComponent = letterAnimators[0].text;

        if (string.IsNullOrEmpty(textComponent.text))
        {
            textComponent.ForceMeshUpdate();
            return;
        }

        foreach (var animator in letterAnimators)
        {
            if (animator.NeedsRefresh)
            {
                animator.InitializeVertices();
                animator.NeedsRefresh = false;
            }
        }

        if (letterAnimators[0].OriginalVertices == null) return;
        Vector3[][] modifiedVertices = new Vector3[letterAnimators[0].OriginalVertices.Length][];
        for (int i = 0; i < letterAnimators[0].OriginalVertices.Length; i++)
        {
            modifiedVertices[i] = letterAnimators[0].OriginalVertices[i].Clone() as Vector3[];
        }

        foreach (var animator in letterAnimators)
        {
            animator.ApplyAnimation(modifiedVertices);
        }

        for (int i = 0; i < letterAnimators[0].text.textInfo.meshInfo.Length; i++)
        {
            letterAnimators[0].text.textInfo.meshInfo[i].mesh.vertices = modifiedVertices[i];
            letterAnimators[0].text.UpdateGeometry(letterAnimators[0].text.textInfo.meshInfo[i].mesh, i);
        }
    }
}
