using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] Menu[] menus;

    public void Awake()
    {
        Instance = this;
    }

    public void MenuOpen(string menuName)
    {
        foreach (var t in menus)
        {
            if (t.MenuName == menuName)
            {
                t.ChangeState(true);
            }
            else if (t.Open)
            {
                t.ChangeState(false);
            }
        }
    }
}
