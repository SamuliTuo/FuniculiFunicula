using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 attackIntervalMinMax = new Vector2(3.0f, 5.0f);
    public GameObject bullet;
    public float bulletSpawnOffset = 1;
    public float bulleSpeed = 1;
    public float bulletLifeTime = 3;
    public float maxHp = 3;
    public float maxShootRange = 50f;

    private float hp;

    private void Start()
    {
        hp = maxHp;
        StartCoroutine(Attack());
    }


    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(attackIntervalMinMax.x, attackIntervalMinMax.y));
        Vector3 shootDir = GameManager.Instance.FunicularController.funicularCars[0].transform.position - transform.position;
        if (shootDir.magnitude < maxShootRange)
        {
            GameManager.Instance.AudioManager.PlayClip("enemy_shoot");
            shootDir = shootDir.normalized;
            var clone = Instantiate(bullet, transform.position + Vector3.up * 0.4f + shootDir * bulletSpawnOffset, Quaternion.identity).GetComponent<BulletController>();
            clone.Init(shootDir, true);
        }
        StartCoroutine(Attack());
    }

    public void GotHit(float damage)
    {
        hp -= damage;
        GameManager.Instance.ParticleSpawner.SpawnBulletHit(transform.position);
        if (hp <= 0)
        {
            GameManager.Instance.AudioManager.PlayClip("enemy_ded");
            Destroy(gameObject);
        }
    }
}
