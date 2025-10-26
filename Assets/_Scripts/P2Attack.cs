using System.Collections;
using UnityEngine;

public class P2Attack : MonoBehaviour
{
    public float damage = 3;
    public GameObject colObj;
    public float attackColliderLifeTime = 0.2f;

    private P2AttackCollider col;

    private void Awake()
    {
        col = GetComponentInChildren<P2AttackCollider>(true);
    }


    public void StartAttack(Vector3 axis, bool facingRight)
    {
        if (axis.x != 0)
        {
            if (axis.x > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            if (facingRight)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        

        //transform.localRotation = Quaternion.LookRotation(direction);
        colObj.SetActive(true);
        col.Init(damage);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackColliderLifeTime);
        colObj.SetActive(false);
    }
}
