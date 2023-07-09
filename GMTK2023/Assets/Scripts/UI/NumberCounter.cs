using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class NumberCounter : MonoBehaviour
{
    public void QuickText(string val)
    {
       Text text = GetComponent<Text>();
       text.text = val;
    }

    public void SetText(float startVal, float endVal, bool isInt, float timeToCount = 1f, string format = "", string suffix="")
    {
        StartCoroutine(CountAnimation(startVal, endVal, isInt, timeToCount, format, suffix));
    }

    private IEnumerator CountAnimation(float startVal, float endVal, bool isInt, float timeToCount, string format, string suffix)
    {
        Text text = GetComponent<Text>();

        float t = 0;

        float val = startVal;
        while (t < timeToCount)
        {
            t += Time.deltaTime;
            val = Mathf.Lerp(startVal, endVal, t / timeToCount);
            text.text = val.ToString(format) + suffix;
            yield return new WaitForEndOfFrame();
        }

        val = endVal;
        if (isInt)
            text.text = ((int)val).ToString(format);
        else
            text.text = val.ToString(format);
    }
}