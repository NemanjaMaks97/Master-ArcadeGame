using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var main = this.gameObject.GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
        this.gameObject.GetComponent<ParticleSystem>().Play();
    }

    private void OnParticleSystemStopped()
    {
        Destroy(this.gameObject);
    }
}
