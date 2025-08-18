using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpiringLetter : LetterAnimator
{
    [SerializeField] private float speed;

    public override void ApplyAnimation(Vector3[][] vertices)
    {
        if (currentCharacters.Count == 0) return;

        int j = 0;
        foreach (int charIndex in currentCharacters)
        {
            if (charIndex >= text.textInfo.characterInfo.Length)
                continue;

            TMP_CharacterInfo charInfo = text.textInfo.characterInfo[charIndex];
            if (!charInfo.isVisible)
                continue;

            Color32[] vertexColors = text.textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
            for (int i = 0; i < 4; i++)
            {
                Color origColor = vertexColors[charInfo.vertexIndex + i];
                vertexColors[charInfo.vertexIndex + i] = Color.Lerp(
                    origColor,
                    new Color(origColor.r, origColor.g, origColor.b, 0f), 
                    currentCharactersExistTime[j] * speed);
            }

            j++;
        }

        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
