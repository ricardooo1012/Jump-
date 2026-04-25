using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    public Sprite[] onFrames;

    public int sorceValue = 1;

    public float onRate = 10f;              //1秒10张图

    private bool loop = true;
    private int currentFrames;
    private float timer;

    private SpriteRenderer sr;
    private Collider2D col;
    // Start is called before the first frame update

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()       //一秒60帧
    {
        timer += Time.deltaTime;        //计时

        if(timer >= 1f / onRate)        //当计时达到0.1秒
        {
            timer = 0f;                 //归零重新计时
            currentFrames++;            //播放第一张图片
            if (currentFrames >= onFrames.Length)           //播放到最后一张图片
            {
                currentFrames = loop ? 0 : onFrames.Length - 1;
            }
            sr.sprite = onFrames[currentFrames];
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();       //collision是进入 Trigger 的那个碰撞体，也就是玩家，然后从这个玩家身上找到PC这个脚本collision.GetComponent<PlayerController>()，并存到player，这样就可以调用玩家的方法了
            if (player != null)
            {
                player.AddScore(sorceValue);
            }

            col.enabled = false;
            Destroy(gameObject,0.1f);
        }
    }
}
