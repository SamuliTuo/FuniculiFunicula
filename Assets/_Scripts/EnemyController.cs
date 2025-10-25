using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float attackInterval = 4;
    public GameObject bullet;
    public float bulletSpawnOffset = 1;
    public float bulleSpeed = 1;
    public float bulletLifeTime = 3;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackInterval);

        Vector3 shootDir = (GameManager.Instance.FunicularController.funicularCars[0].transform.position - transform.position).normalized;
        var clone = Instantiate(bullet, transform.position + shootDir * bulletSpawnOffset, Quaternion.identity).GetComponent<BulletController>();
        clone.Init(shootDir, true);

        StartCoroutine(Attack());
    }
}
