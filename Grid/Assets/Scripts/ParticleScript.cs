using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private ParticleSystem particleSys2;
    public bool particleOn2;
    public GameObject GridManager;

    // Start is called before the first frame update
    void Start()
    {
        particleSys2 = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Particles()
    {
        var emission = particleSys2.emission;
        emission.enabled = particleOn2;
        Debug.Log("particles");

    }
}
