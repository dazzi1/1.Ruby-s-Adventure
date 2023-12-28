using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    public int speed;

    // 最大生命值
    public int maxHealth = 5;
    // 当前生命值
    private int currentHealth;
    public int Health { get { return currentHealth; } }

    // 无敌时间
    public float timeInvincible = 2.0f;
    // 无敌状态
    private bool isInvincible;
    // 无敌计时器
    private float invincibleTimer;
    // 二维向量朝向
    private Vector2 lookDirection = new Vector2(1, 0);

    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;
    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    private Vector3 respawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        //改变帧率
        //Application.targetFrameRate = 10;
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();

        //记录初始的位置
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 玩家输入监听
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        // 判断当前玩家输入的某个轴向值不为0
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            lookDirection.Set(move.x, move.y);
            // 单位向量化
            lookDirection.Normalize();
            // 行走音效
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }
        // 动画选择
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = transform.position;
        /*position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;*/
        // 位置坐标移动
        position = position + speed * move * Time.deltaTime;
        // 变更坐标位置
        //transform.position = position;
        rigidbody2d.MovePosition(position);

        if (isInvincible)
        {
            // 无敌记时
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                // 关闭无敌
                isInvincible = false;
            }
        }
        // 发动攻击
        if (Input.GetKeyDown(KeyCode.J))
        {
            Launch();

        }

        // 检测是否与NPC对话
        if (Input.GetKeyDown(KeyCode.F))
        {

            // 发射射线
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position +
                Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                // 获取脚本组件 展示对话框
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if (npcDialog != null)
                {
                    npcDialog.DisplayDialog();
                }
            }
        }
    }

    //改变生命值
    public void ChangeHealth(int amount)
    {
        // 受伤
        if (amount < 0)
        {
            // 已经无敌，返回空
            if (isInvincible)
            {
                return;
            }
            // 受到伤害，开启无敌
            isInvincible = true;
            // 设定无敌时间持续时间
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
        }
        // 把生命限制为0~5，更新HP
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // 更新血条UI的显示
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        // 复活
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    private void Launch()
    {
        // 接取了任务才能发射子弹
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }
        // 生成子弹的实例化方法
        GameObject projectileObject = Instantiate(projectilePrefab,
            rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        // 发起攻击
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    // 初始位置满血重生
    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition;
    }
}
