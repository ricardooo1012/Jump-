using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackgroundLoop : MonoBehaviour
{
    public float speed = 100f;
    public float width = 1920f;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        if (rect.anchoredPosition.x <= -width)
        {
            rect.anchoredPosition += new Vector2(width * 2f, 0);
        }
    }
}