using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float attackInterval = 4;
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
        yield return new WaitForSeconds(attackInterval);
        Vector3 shootDir = GameManager.Instance.FunicularController.funicularCars[0].transform.position - transform.position;
        if (shootDir.magnitude < maxShootRange)
        {
            var clone = Instantiate(bullet, transform.position + shootDir * bulletSpawnOffset, Quaternion.identity).GetComponent<BulletController>();
            clone.Init(shootDir, true);
        }
        StartCoroutine(Attack());
    }

    public void GotHit(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
