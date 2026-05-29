using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource sound;
    [SerializeField] private float dmg= 35;
    [SerializeField] private float hitPunishAmount=100;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent(out IHitable hit))
        {
            if(gameObject.GetComponentInChildren<Renderer>().enabled == true)
            {
                if (hit.shouldDamage)
                {
                    ProceduralObjectSpawn.OnHit?.Invoke(hitPunishAmount);
                    hit.OnHitObstacle(dmg);
                }
                
                gameObject.GetComponentInChildren<Renderer>().enabled = false;
                
                sound.volume = AudioManager.i.Volume;

                particles.Play();
                sound.Play();
            }
        } 
    }
}
