using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Subtitles : MonoBehaviour
{
    private Text _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<Text>();
        if (_textComponent == null) Debug.LogError("WARNING: " + GetType().ToString() +
            " missing " + _textComponent.GetType().ToString() + " component.");
        if (!_textComponent.isActiveAndEnabled) _textComponent.enabled = true;
        _textComponent.CrossFadeAlpha(0, 0, false);
    }

    public void UpdateText(string text = "")
    {
        _textComponent.text = text;
    }

    public void SetUpSubs(Subtitles[] subs)
    {
        FadeIn();
        float previousTime = 0;
        for (int i = 0; i < subs.Length; i++)
        {
            subs[i].text = subs[i].text.Replace("[Name]", "Henk");
            subs[i].text = subs[i].text.Replace("Dolores", "ICARUS");

            if (i != 0) previousTime += subs[i - 1].duration;
            StartCoroutine(ShowSubs(subs[i].text, previousTime));

            if (subs[i].playSoundAction != null)
            {
                //if (subs[i].playSoundAction.audioSourceToUse == null) subs[i].playSoundAction.audioSourceToUse =
                //        GlobalObjectManager.Instance.player.GetComponent<AudioSource>();
                if (subs[i].playSoundAction.audioSourceToUse != null && subs[i].playSoundAction.audioClipToUse != null)
                    StartCoroutine(PlaySubs(subs[i].playSoundAction, previousTime));
            }
        }
        StartCoroutine(MyCoroutines.StartTimer(previousTime + subs[subs.Length - 1].duration, FadeOut));
    }

    public void SetUpSingleSub(Subtitles sub)
    {
        ResetSubs();
        FadeIn();
        StartCoroutine(ShowSubs(sub.text, 0));
        StartCoroutine(MyCoroutines.StartTimer(sub.duration, FadeOut));
    }

    public void ResetSubs()
    {
        StopAllCoroutines();
        UpdateText();
    }

    public void FadeIn()
    {
        _textComponent.CrossFadeAlpha(1, 0, false);
        //StartCoroutine(MyCoroutines.StartTimer(_fadeOutAfter, FadeOut));
    }

    public void FadeOut()
    {
        _textComponent.CrossFadeAlpha(0, 0, false);
    }

    private IEnumerator ShowSubs(string text, float previousTime)
    {
        yield return new WaitForSeconds(previousTime);
        _textComponent.text = text;
    }

    private IEnumerator PlaySubs(PlaySound sound, float previousTime)
    {
        yield return new WaitForSeconds(previousTime);
        sound.Action();
    }

    private IEnumerator ShowSubs(string text, float previousTime, float duration)
    {
        FadeIn();
        yield return new WaitForSeconds(previousTime);
        _textComponent.text = text;
        yield return new WaitForSeconds(duration);
        FadeOut();
    }
}
