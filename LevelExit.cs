using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public Sprite[] flagOut;
    private SpriteRenderer sr;

    private bool finished = false;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null) return;

        finished = true;

        StartCoroutine(flagSequence());
    }

    IEnumerator flagSequence()
    {
        for(int i = 0;i < flagOut.Length; i++)
        {
            sr.sprite = flagOut[i];
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.5f);

        LevelManager.Instance.LevelComplete();

    }
}
