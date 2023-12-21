using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private float maxLength;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        maxLength = rectTransform.rect.width;
    }

    public void ChangeHealth(float currentHealth, float maxHealth)
    {
        rectTransform.sizeDelta = new Vector2(
            currentHealth / maxHealth * maxLength,
            rectTransform.rect.height
        );
    }
}
