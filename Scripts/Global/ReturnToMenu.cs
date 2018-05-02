using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : BaseAction
{
    [SerializeField]
    private float _afterHowManySeconds = 4;

    public static IEnumerator GoToMenu(float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds);
        SceneManager.LoadScene("Start Menu");
    }

    public override void Action()
    {
        StartCoroutine(GoToMenu(_afterHowManySeconds));
    }
}
