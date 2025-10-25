using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime = 3.0f;
    public float flySpeed = 5.0f;
    public float knockbackForce = 1.0f;
    public float stunDuration = 1.0f;
    public float invulnerableDuration = 1.0f;
    public bool airStagger = true;

    private Vector3 dir;
    private bool enemyBullet;

    public void Init(Vector3 dir, bool enemyBullet)
    {
        this.dir = dir;
        this.enemyBullet = enemyBullet;
        StartCoroutine(BulletFly());
    }

    IEnumerator BulletFly()
    {
        float t = 0;
        while (t < lifeTime)
        {
            transform.position += dir * flySpeed * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }

    void EnemyBulletCollisions(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CharacterController2D character = collision.GetComponent<CharacterController2D>();
            if (character && !character.Invulnerable)
            {
                if (knockbackForce > 0)
                {
                    Vector2 force = Vector3.up * knockbackForce;
                    character.Knockback(force, stunDuration);
                }
                if (invulnerableDuration > 0)
                {
                    character.setInvunerable(invulnerableDuration);
                }
                if (airStagger)
                {
                    character.SetAirStagger(stunDuration);
                }
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player)
                {
                    player.SoftRespawn();
                }
                Player2Controller player2 = collision.GetComponent<Player2Controller>();
                if (player2)
                {
                    player2.SoftRespawn();
                }
            }
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Funicula"))
        {
            collision.GetComponent<FuniculaCollider>().GotHit(1);
            Destroy(this.gameObject);
        }
    }

    void PlayerBulletCollisions(Collider2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyBullet)
        {
            EnemyBulletCollisions(collision);
        }
        else
        {
            PlayerBulletCollisions(collision);
        }
    }
}
