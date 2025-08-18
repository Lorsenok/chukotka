using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WavyLetter : LetterAnimator
{
    [SerializeField] private float shakeIntensity = 1f;
    [SerializeField] private float shakeSpeed = 10f;

    public override void ApplyAnimation(Vector3[][] vertices)
    {
        if (currentCharacters.Count == 0) return;

        foreach (int charIndex in currentCharacters)
        {
            if (charIndex >= text.textInfo.characterInfo.Length)
                continue;

            TMP_CharacterInfo charInfo = text.textInfo.characterInfo[charIndex];
            if (!charInfo.isVisible)
                continue;

            Vector3 offset = new Vector3(
                Mathf.Sin(Time.time * shakeSpeed + charIndex) * shakeIntensity,
                Mathf.Cos(Time.time * shakeSpeed + charIndex) * shakeIntensity,
                0);

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            if (materialIndex < vertices.Length &&
                vertexIndex + 3 < vertices[materialIndex].Length)
            {
                for (int i = 0; i < 4; i++)
                {
                    vertices[materialIndex][vertexIndex + i] += offset;
                }
            }
        }
    }
}