using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap1 : MonoBehaviour
{
    public SpriteRenderer hit;
    public SpriteRenderer fire;
    public Sprite off;

    public Sprite[] onHit;
    public Sprite[] onFire;

    private bool isActive = false;//防止重复触发,false=机关现在可以触发


    private void Start()
    {
        hit.sprite = off;
        fire.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null) return;

        isActive = true;

        StartCoroutine(TrapSequence());

    }

    IEnumerator TrapSequence()
    {
        for(int i = 0; i < onHit.Length; i++)
        {
            hit.sprite = onHit[i];
            yield return new WaitForSeconds(0.1f);
        }

        fire.gameObject.SetActive(true);//显示火焰

        for (int i = 0; i < onFire.Length; i++)
        {
            fire.sprite = onFire[i];
            yield return new WaitForSeconds(0.5f);
        }

        fire.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        isActive = false;
    }

}