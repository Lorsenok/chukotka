using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisapearingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Image[] imgs;
    
    [SerializeField] private float speed;
    [SerializeField] private string save;
    
    private void Start()
    {
        if (save == string.Empty || save == "") return;
        if (PlayerPrefs.HasKey(save))
        {
            foreach (var text in texts) text.color = new Color(0, 0, 0, 0);
            foreach (var img in imgs) img.color = new Color(0, 0, 0, 0);
            enabled = false;
        }
        GameSaver.Save(save, "1");
    }

    private void Update()
    {
        if (texts.Length > 0) foreach (var text in texts) 
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, text.color.a - 1f), Time.deltaTime * speed);
        
        if (imgs.Length > 0) foreach (var img in imgs) 
            img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, img.color.a - 1f), Time.deltaTime * speed);
    }
}
