using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunicularController : MonoBehaviour
{
    public Image hpBar;
    public List<Transform> funicularCars = new List<Transform>();
    public float maxHp;


    private float hp;

    private void Start()
    {
        hp = maxHp;
    }

    // HP
    public void GotHit(float damage)
    {
        hp -= damage;
        SetHP();
        GameManager.Instance.AudioManager.PlayClip("funiculi_auts");
        if (hp < 0)
        {
            GameManager.Instance.GameOver();
        }
    }
    void SetHP()
    {
        hpBar.fillAmount = Mathf.Max(0, hp) / maxHp;
    }
}
