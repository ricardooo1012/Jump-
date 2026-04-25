using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrap : MonoBehaviour
{
    public SpriteRenderer topPart;
    public SpriteRenderer bottomPart;

    public Sprite idle;

    public Sprite[] topBreak;
    public Sprite[] bottomBreak;

    public bool triggered = false;

    void Start()
    {
        topPart.sprite = idle;
        bottomPart.sprite = idle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;

        if (!collision.gameObject.CompareTag("Player")) return;

        StartCoroutine(BreakBlock());

        //ContactPoint2D contact = collision.contacts[0];

        //if (contact.normal.y > 0.5f)
        //{
        //    StartCoroutine(BreakBlock());
        //}
    }

    IEnumerator BreakBlock()
    {
        triggered = true;

        for (int i = 0; i < topBreak.Length; i++)
        {
            topPart.sprite = topBreak[i];
            bottomPart.sprite = bottomBreak[i];

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}