using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image FadeImage;
    public AnimationCurve FadeCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    float percentDone = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(FadeImage != null);
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = FadeCurve.Evaluate(percentDone);
        FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, alpha);
    }

    public IEnumerator FadeToBlack(float timeToTake = 1.0f)
    {
        return DoFade(timeToTake, true);
    }

    public IEnumerator FadeToClear(float timeToTake = 1.0f)
    {
        return DoFade(timeToTake, false);
    }

    public IEnumerator DoFade(float time, bool fadeToBlack)
    {
        float endT = time;
        float timeElapsed = 0.0f;
        while(timeElapsed <= endT)
        {
            timeElapsed += Time.deltaTime;
            if (fadeToBlack)
            {
                percentDone = timeElapsed / time;
            }
            else
            {
                percentDone = 1.0f - (timeElapsed / time);
            }
            yield return null;
        }
        if (fadeToBlack)
        {
            percentDone = 1.0f;
        }
        else
        {
            percentDone = 0.0f;
        }
    }
}
