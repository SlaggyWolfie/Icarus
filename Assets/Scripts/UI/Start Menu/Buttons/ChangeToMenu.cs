using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class ChangeToMenu : MenuButtonBase
{
    private GameObject _menu;
    [SerializeField]
    private GameObject _accessMenu;
    private GameObject _tempAccessMenu;

    public override void Action()
    {
        ChangeMenu();
    }

    public void ChangeMenu ()
    {
        _menu.SetActive(false);

        _accessMenu.SetActive(true);
    }

    private void Start()
    {
        _menu = transform.root.gameObject;
    }
}
