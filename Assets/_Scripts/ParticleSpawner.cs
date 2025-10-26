using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public ParticleSystem slashParticle;
    public ParticleSystem jump_particle;
    public ParticleSystem muzzle_flash;
    public ParticleSystem bullet_hit;

    public void SpawnSlash(Vector3 spawnPos, Vector3 direction)
    {
        slashParticle.transform.position = spawnPos;
        var main = slashParticle.main;
        main.startRotationX = direction.x;
        main.startRotationY = direction.y;
        main.startRotationZ = direction.z;
        slashParticle.Play();
    }


    public void SpawnJumpCloud(Vector3 spawnPos)
    {
        jump_particle.transform.position = spawnPos;
        jump_particle.Play();
    }

    public void SpawnMuzzelFlash(Vector3 spawnPos/*, Vector3 direction*/)
    {
        muzzle_flash.transform.position = spawnPos;
        /*
        var main2 = muzzle_flash.main;
        main2.startRotationX = direction.x;
        main2.startRotationY = direction.y;
        main2.startRotationZ = direction.z;
        */
        slashParticle.Play();
    }

    public void SpawnBulletHit(Vector3 spawnPos)
    {
        bullet_hit.transform.position = spawnPos;
        bullet_hit.Play();
    }
}
