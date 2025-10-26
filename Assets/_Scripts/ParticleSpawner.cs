using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public ParticleSystem slashParticle;
    public ParticleSystem jump_particle;

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
}
