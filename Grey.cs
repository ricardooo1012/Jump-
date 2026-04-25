using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grey : MonoBehaviour
{
    [Header("動畫")]
    public Sprite[] rotateSprites;           // 你的 8 張 brownOn（鐵軸轉動）

    [Header("蹺蹺板參數")]
    public float maxAngle = 50f;             // 最大傾斜角度（度）
    public float tiltSpeedMultiplier = 40f;  // 傾斜速度係數（越大越敏感）
    public float returnSpeed = 40f;          // 無人時回正速度
    public float animationFrameTime = 0.08f; // 鐵軸轉動速度

    private SpriteRenderer spriteRenderer;
    private bool hasPlayer = false;
    private float currentAngle = 0f;
    private float elapsedTime = 0f;
    private Transform playerTransform;       // 暫存玩家參考

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("缺少 SpriteRenderer");
            enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasPlayer = true;
            playerTransform = collision.transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasPlayer = false;
            playerTransform = null;
        }
    }

    void FixedUpdate()   // 用 FixedUpdate 更穩定
    {
        if (hasPlayer && playerTransform != null)
        {
            // 計算玩家相對於平台的本地 x 偏移（-1 ~ 1 範圍）
            Vector3 localPos = transform.InverseTransformPoint(playerTransform.position);
            float offsetX = localPos.x / (GetPlatformHalfWidth() * 0.8f);  // 歸一化

            // 傾斜方向：右邊正，左邊負
            float targetAngle = offsetX * maxAngle;

            // 慢慢趨近目標角度
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, tiltSpeedMultiplier * Mathf.Abs(offsetX) * Time.fixedDeltaTime);
        }
        else
        {
            // 無人時慢慢回 0
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, 0f, returnSpeed * Time.fixedDeltaTime);
        }

        // 套用旋轉
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

        // 鐵軸轉動動畫（只要有傾斜就轉）
        if (Mathf.Abs(currentAngle) > 0.1f)
        {
            elapsedTime += Time.fixedDeltaTime;
            int frame = (int)(elapsedTime / animationFrameTime) % rotateSprites.Length;
            spriteRenderer.sprite = rotateSprites[frame];
        }
        else
        {
            // 可選：回正時切回靜止圖（如果你想保留 brownoff）
            // spriteRenderer.sprite = ... (如果有 offSprite 可加回來)
        }
    }

    // 幫忙估計平台一半寬度（可手動調整或用 collider bounds）
    private float GetPlatformHalfWidth()
    {
        // 假設 collider 蓋住平台，可用這個
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null) return box.size.x * 0.5f * transform.lossyScale.x;

        return 2f; // 預設值，根據你的 sprite 調整（單位：world unit）
    }
}