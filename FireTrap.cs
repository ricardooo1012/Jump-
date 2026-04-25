using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("Sprites")]
    public SpriteRenderer firePart;     // Off/On 的主火焰
    public SpriteRenderer hitPart;      // Hit 特效（初始隱藏）
    public Sprite offSprite;            // 靜止狀態
    public Sprite[] onSprites;          // 3張噴火循環
    public Sprite[] hitSprites;         // 4張踩踏特效

    [Header("Timing")]
    public float hitDuration = 0.4f;    // Hit 播完時間（4張 x 0.1s）
    public float fireDuration = 2.5f;   // 總噴火持續時間
    public float onFrameTime = 0.15f;   // 每張 On 播多久（調速感）

    private bool isActive = false;      // 目前是否在噴火（防重觸發）

    void Start()
    {
        // 初始 Off + 藏 Hit
        firePart.sprite = offSprite;
        hitPart.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive) return;  // 噴火中不重觸發
        if (!other.CompareTag("Player")) return;

        StartCoroutine(TriggerFire());
    }

    IEnumerator TriggerFire()
    {
        isActive = true;

        // 1. 先播 Hit 特效（顯示 + 4張切換）
        hitPart.gameObject.SetActive(true);
        for (int i = 0; i < hitSprites.Length; i++)
        {
            hitPart.sprite = hitSprites[i];
            yield return new WaitForSeconds(0.1f);  // Hit 快一點
        }
        hitPart.gameObject.SetActive(false);  // 播完藏起來

        // 2. 有限循環噴火：while 直到 fireDuration 結束
        float elapsed = 0f;
        while (elapsed < fireDuration)
        {
            for (int i = 0; i < onSprites.Length; i++)
            {
                firePart.sprite = onSprites[i];
                yield return new WaitForSeconds(onFrameTime);
                elapsed += onFrameTime;
                if (elapsed >= fireDuration) break;  // 超過時間就跳出
            }
        }

        // 3. 強制回 Off
        firePart.sprite = offSprite;
        isActive = false;
    }
}