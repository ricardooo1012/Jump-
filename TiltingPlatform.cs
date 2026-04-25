using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltingPlatform : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 10f;       //1秒播放10张图
    public bool loop = true;            //循环播放开启

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float timer;
    private int currentFrame;
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 2f;


    private bool movingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        leftPoint.parent = null;
        rightPoint.parent = null;
    }

    void Update()
    {
        if (frames.Length == 0) return;           //如果不是在运行或者动图数量为0

        timer += Time.deltaTime;                                //上一帧到这一帧经过的时间

        if (timer >= 1f / frameRate)                            //1/10=0.1，这个其实是1张图播放多少秒，由于framerate=10f，也就是1秒10张图，一张图0.1秒
        {
            timer = 0f;                                         //数数器超过0.1，就重置数数器，然后播放下一张图，达到每0.1秒播放一张图的目的
            currentFrame++;

            if (currentFrame >= frames.Length)                  //如果播放到最后一张图，看是否开了循环，是就从零开始继续，否则从倒数第二张开始
            {
                if (loop)
                    currentFrame = 0;
                else
                    currentFrame = frames.Length - 1;
            }

            sr.sprite = frames[currentFrame];                   //播放图片，
        }
    }

    private void FixedUpdate()
    {
        MovePlatform();

    }


    void MovePlatform()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        Vector2 target = rb.position + direction * speed * Time.fixedDeltaTime; //transform.position是直接改坐标，rigbody2D.position是通过物理系统移动。

        rb.MovePosition(target);

        if (movingRight && rb.position.x >= rightPoint.position.x)
        {
            movingRight = false;
        }

        if (!movingRight && rb.position.x <= leftPoint.position.x)
        {
            movingRight = true;
        }
    }
}