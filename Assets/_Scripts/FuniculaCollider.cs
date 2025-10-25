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
}
