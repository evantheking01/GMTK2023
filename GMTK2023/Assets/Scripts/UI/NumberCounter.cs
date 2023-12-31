using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class NumberCounter : MonoBehaviour
{
    public bool doColorFlash;
    public Color colorFlash;
    public bool useLastValue;

    private float lastValue;

    public void QuickText(string val)
    {
       Text text = GetComponent<Text>();
       text.text = val;
    }

    public void SetText(float startVal, float endVal, bool isInt, float timeToCount = 1f, string format = "", string suffix="", string prefix="")
    {
        if (useLastValue)
            startVal = lastValue;
        lastValue = endVal;
        StartCoroutine(CountAnimation(startVal, endVal, isInt, timeToCount, format, suffix, prefix));
    }

    private IEnumerator CountAnimation(float startVal, float endVal, bool isInt, float timeToCount, string format, string suffix, string prefix)
    {
        Text text = GetComponent<Text>();
        Color startColor = text.color;
        float t = 0;

        float val = startVal;
        while (t < timeToCount)
        {
            if (doColorFlash)
            {
                float colorVal = 2f * t / timeToCount;
                colorVal = Mathf.Clamp(colorVal, 0, 1);
                Color.Lerp(startColor, colorFlash, colorVal);
            }
            t += Time.deltaTime;
            val = Mathf.Lerp(startVal, endVal, t / timeToCount);
            text.text = prefix + val.ToString(format) + suffix;
            yield return new WaitForEndOfFrame();
        }

        if (doColorFlash)
            text.color = startColor;
        val = endVal;
        if (isInt)
            text.text = prefix + ((int)val).ToString(format) + suffix;
        else
            text.text = prefix + val.ToString(format) + suffix;
    }
}