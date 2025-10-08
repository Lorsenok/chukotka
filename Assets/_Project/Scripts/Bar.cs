using System;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private int orighp = 10;
    [SerializeField] private float speed = 5f;
    
    private void Update()
    {
        image.fillMethod = Image.FillMethod.Horizontal;
        image.fillOrigin = 0;
        image.fillAmount = Mathf.Lerp(image.fillAmount, (float)destroyableObject.HP / (float)orighp, speed * Time.deltaTime);
    }
}
