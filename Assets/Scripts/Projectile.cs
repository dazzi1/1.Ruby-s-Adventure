using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    //子弹越界销毁
    private void Update()
    {
        if (transform.position.magnitude > 30)
        {
            Destroy(gameObject);
        }
    }

    // 子弹施加作用力
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    // 子弹与机器人碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController enemyController = collision.gameObject.
            GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Fix();
        }
        Destroy(gameObject);
    }
}
