using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3;

    private Rigidbody2D rd;
    // 轴向控制
    public bool vertical;
    // 方向控制
    private int direction = 1;
    // 方向改变时间间隔
    public float changeTime = 3.0f;
    // 计时器
    private float timer;

    public AudioClip fixedSound;
    public AudioClip[] hitSounds;
    public GameObject hitEffectParticle;

    private AudioSource audioSource;

    private Animator animator;
    // 是否故障
    private bool broken;

    public ParticleSystem smokeEffect;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        // 按照初始先播放动画
        /*animator.SetFloat("MoveX", direction);
        animator.SetBool("Vertical", vertical);*/
        PlayMoveAnimation();
        broken = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //已修好，不再移动
        if (!broken)
        {
            return;
        }
        // 一定时间后更改运动方向
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            // 切换动画
            //animator.SetFloat("MoveX",direction);
            PlayMoveAnimation();
            timer = changeTime;
        }
        Vector2 position = rd.position;
        if (vertical)
        {
            // 垂直轴向
            position.y += Time.deltaTime * speed * direction;
        }
        else
        {
            // 水平轴向
            position.x += Time.deltaTime * speed * direction;
        }
        // 更改位置
        rd.MovePosition(position);
    }
    // 触发Ruby扣血检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null)
        {
            //扣血
            rubyController.ChangeHealth(-1);
        }
    }

    // 播放动画
    private void PlayMoveAnimation()
    {
        if (vertical)//垂直轴向动画控制
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        { //水平轴向
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }
    // 修复机器人
    public void Fix()
    {
        // 击中特效
        Instantiate(hitEffectParticle, transform.position, Quaternion.identity);
        broken = false;
        // 关闭碰撞检测
        rd.simulated = false;
        // 修复后动画
        animator.SetTrigger("Fixed");
        // 关闭粒子效果
        smokeEffect.Stop();
        // 关闭音效
        int randomNum = Random.Range(0, 2);
        audioSource.Stop();
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(hitSounds[randomNum]);
        Invoke("PlayFixedSound", 1f);
        UIHealthBar.instance.fixedNum++;
    }
    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(fixedSound);
    }

}
