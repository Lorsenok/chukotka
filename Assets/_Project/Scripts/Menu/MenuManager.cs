using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] Menu[] menus;

    private Controller playerController;

    public void Awake()
    {
        Instance = this;

        // Найти контроллер игрока один раз при старте
        playerController = FindAnyObjectByType<Controller>();
    }

    public void MenuOpen(string menuName)
    {
        bool anyMenuOpened = false;

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].MenuName == menuName)
            {
                menus[i].ChangeState(true);
                anyMenuOpened = true;   
            }
            else if (menus[i].Open)
            {
                menus[i].ChangeState(false);
            }
        }

        if (playerController != null)
        {
            playerController.CanMove = !anyMenuOpened;
        }
    }
}
