using UnityEngine;
using Zenject;

[RequireComponent (typeof(Collider2D))]
public class MinigameEnterance : MonoBehaviour
{
    [SerializeField] private Minigame minigame;
    [SerializeField] private Fade fade;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Controler>())
        {
            fade.StartFade();
        }
    }

    private void OnEnable()
    {
        fade.OnFadeEnd += StartGame;
    }

    private void OnDisable()
    {
        fade.OnFadeEnd -= StartGame;
    }

    private void StartGame()
    {
        minigame.StartGame();
        Destroy(gameObject);
    }
}
