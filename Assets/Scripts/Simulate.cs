using UnityEngine;

public class Simulate : MonoBehaviour
{
    public ParticleSystem galaxyParticleSystem;
    public float simulationTime = 180f;

    void Start()
    {
        galaxyParticleSystem.Simulate(simulationTime, true, true);
        galaxyParticleSystem.Play();
    }
}
