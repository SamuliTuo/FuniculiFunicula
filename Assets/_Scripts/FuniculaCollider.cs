using System.Collections;
using UnityEngine;

public class FuniculaCollider : MonoBehaviour
{
    FunicularController controller;

    private void Awake()
    {
        controller = GetComponentInParent<FunicularController>();
    }

    public void GotHit(float damage)
    {
        controller.GotHit(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            StartCoroutine(LootObject(collision));
        }
    }

    IEnumerator LootObject(Collider2D collider)
    {
        collider.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(collider.gameObject);
    }
}
