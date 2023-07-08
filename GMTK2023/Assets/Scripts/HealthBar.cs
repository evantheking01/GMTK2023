using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private Image[] image;

    void Awake ()
    {
        
    }

    public void SetMaxHealth(float maxHealth)
    {
        image = GetComponentsInChildren<Image>();
        
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        // hide the health bar
        for (int i = 0; i < image.Length; i++)
        {
            Color currentColor = image[i].color;
            image[i].color = new Color(currentColor.r, currentColor.g, currentColor.b, .2f);
        }
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        // if we are not at full health, show the health bar
        float alpha = (slider.value < slider.maxValue) ? 1f : .2f;
        for (int i = 0; i < image.Length; i++)
        {
            Color currentColor = image[i].color;
            image[i].color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        }
    }
}
