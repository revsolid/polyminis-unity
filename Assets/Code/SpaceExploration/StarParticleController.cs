using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticleController : MonoBehaviour {


    float MovementSpeed;
    ParticleSystem ParticleSys;
    public float Multiplier;
	// Use this for initialization
	void Start () {
        MovementSpeed = FindObjectOfType<SpaceMovementTracker>().TranslationSpeedMult;
        ParticleSys = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        
        if (ParticleSys)
        {
            
            ParticleSystem.Particle[] Particles = new ParticleSystem.Particle[ParticleSys.particleCount];
            ParticleSys.GetParticles(Particles);
            for( int i = 0; i < Particles.Length; i++)
            {
                
                Particles[i].velocity = -MovementSpeed * Multiplier * new Vector3(ControlHelper.GetHorizontalAxis(), 0, ControlHelper.GetVerticalAxis());
            }

            ParticleSys.SetParticles(Particles, Particles.Length);
        }

	}
}
