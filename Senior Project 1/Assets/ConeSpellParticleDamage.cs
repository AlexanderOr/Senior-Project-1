using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSpellParticleDamage : MonoBehaviour
{
    public ParticleSystem ps;
    public int damageAmount = 10;
    public Spellinstance spellinstance;

    private ParticleSystem.TriggerModule trigger;
    private List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();

    void Awake()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        trigger = ps.trigger;

        
    }

    private void Update()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 10f); // adjust radius

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                TryAddToTrigger(enemy);
            }
        }
    }

    void OnParticleTrigger()
    {
        Spells spellData = spellinstance.spellData;
        int finalDamage = spellData.Damage + (5 * (spellData.Level - 1));

        // Get particles currently inside the collider trigger
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enterParticles);

        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle p = enterParticles[i];

            // Check all enemy colliders
            Collider[] overlapped = Physics.OverlapSphere(p.position, 0.1f); // Tiny radius to detect who is hit
            foreach (var col in overlapped)
            {
                if (col.CompareTag("Enemy"))
                {
                    // Apply damage
                    EnemyBehavior enemy = col.GetComponent<EnemyBehavior>();
                    if (enemy != null)
                    {
                        enemy.Damage(finalDamage);
                    }
                }
            }
        }

        // If you want to keep particles alive:
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, enterParticles);
    }

    private void TryAddToTrigger(Collider col)
    {
        var trigger = ps.trigger;
        // Prevent duplicates
        for (int i = 0; i < trigger.colliderCount; i++)
        {
            if (trigger.GetCollider(i) == col)
                return;
        }

        trigger.SetCollider(trigger.colliderCount, col);
    }


}
