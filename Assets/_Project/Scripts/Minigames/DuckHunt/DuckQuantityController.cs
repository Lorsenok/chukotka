using TMPro;
using UnityEngine;

public class DuckQuantityController : MonoBehaviour
{
    [SerializeField] private int quantity = 10;
    [SerializeField] private TextMeshProUGUI text;
    private int curQuantity = 0;

    [SerializeField] private Minigame game;
    [SerializeField] private Fade fade;

    private void OnEnable()
    {
        Duck.OnDie += OnDuckDie;
        fade.OnFadeEnd += game.EndGame;
    }

    private void OnDisable()
    {
        Duck.OnDie -= OnDuckDie;
        fade.OnFadeEnd -= game.EndGame;
    }

    private void OnDuckDie()
    {
        curQuantity++;

        text.text = curQuantity.ToString() + "/" + quantity.ToString();

        if (curQuantity >= quantity)
        {
            fade.StartFade();
        }
    }
}
