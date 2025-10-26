using System.Collections.Generic;
using UnityEngine;

public class P2AttackCollider : MonoBehaviour
{
    private List<GameObject> hitObjects;
    private float damage;

    public void Init(float damage)
    {
        this.damage = damage;
        hitObjects = new List<GameObject>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        if (hitObjects.Contains(collision.gameObject))
            return;

        hitObjects.Add(collision.gameObject);
        collision.GetComponent<EnemyController>().GotHit(damage);
    }
}
