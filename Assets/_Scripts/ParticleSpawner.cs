using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public ParticleSystem slashParticle;

    public void SpawnSlash(Vector3 spawnPos, Vector3 direction)
    {
        slashParticle.transform.position = spawnPos;
        var main = slashParticle.main;
        main.startRotationX = direction.x;
        main.startRotationY = direction.y;
        main.startRotationZ = direction.z;
        slashParticle.Play();
    }
}
