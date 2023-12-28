using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectiable : MonoBehaviour
{
    public AudioClip audioClip;

    public GameObject effectParticle;
    // 吃草莓回血
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();

        if (rubyController != null)
        {
            // 判断Ruby是否满血
            if (rubyController.Health < rubyController.maxHealth)
            {
                // 回血
                rubyController.ChangeHealth(1);
                rubyController.PlaySound(audioClip);
                Instantiate(effectParticle, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }

        }
    }
}
