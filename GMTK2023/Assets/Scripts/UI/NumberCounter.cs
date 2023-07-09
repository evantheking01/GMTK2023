using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class NumberCounter : MonoBehaviour
{
    public bool doColorFlash;
    public Color colorFlash;

    public void QuickText(string val)
    {
       Text text = GetComponent<Text>();
       text.text = val;
    }

    public void SetText(float startVal, float endVal, bool isInt, float timeToCount = 1f, string format = "", string suffix="", string prefix="")
    {
        StartCoroutine(CountAnimation(startVal, endVal, isInt, timeToCount, format, suffix, prefix));
    }

    public float GetText(bool isCurrency=true)
    {
        Text text = GetComponent<Text>();
        //float textVal = 0;
        if (isCurrency)
            return float.Parse(text.text, System.Globalization.NumberStyles.Currency);
        
        return float.Parse(text.text);
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
                float colorVal = t / timeToCount / 4f;
                //if (t > timeToCount * 0.75f)
                 //   colorVal = 1f - (t / )
                Color.Lerp(startColor, colorFlash, colorVal);
            }
            t += Time.deltaTime;
            val = Mathf.Lerp(startVal, endVal, t / timeToCount);
            text.text = prefix + val.ToString(format) + suffix;
            yield return new WaitForEndOfFrame();
        }

        val = endVal;
        if (isInt)
            text.text = prefix + ((int)val).ToString(format) + suffix;
        else
            text.text = prefix + val.ToString(format) + suffix;
    }
}