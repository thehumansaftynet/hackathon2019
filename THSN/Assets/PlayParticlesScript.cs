using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticlesScript : MonoBehaviour
{

    public ParticleSystem Particles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticles()
    {
        var par = Instantiate(Particles.gameObject, transform);

    }
}
