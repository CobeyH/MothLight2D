using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggers : MonoBehaviour
{
    public bool IsDecoy = false;

    ParticleSystem partSys;

    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    int mothsInGoal;

    // Start is called before the first frame update
    void Start()
    {
        partSys = GetComponent<ParticleSystem>();
        AddCollidersToTriggers();
    }

    void AddCollidersToTriggers()
    {
        AddCollidersWithTag("TrapLight");
        AddCollidersWithTag("Goal");
    }

    void AddCollidersWithTag(string tag)
    {
        GameObject[] gameObjs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in gameObjs)
        {
            partSys.trigger.AddCollider(obj.GetComponent<Collider2D>());
        }
    }

    void OnParticleTrigger()
    {
        // get the particles which matched the trigger conditions this frame
        int numEnter =
            partSys
                .GetTriggerParticles(ParticleSystemTriggerEventType.Enter,
                enter,
                out var enteredData);

        // If no new moths are in the goal, then no work needs to be done.
        if (numEnter <= 0)
        {
            return;
        }

        GoalsAndTraps(numEnter, enteredData);
    }

    void GoalsAndTraps(int numEntered, ParticleSystem.ColliderData colliderData)
    {
        // Delete particles that enter the goal.
        for (int i = 0; i < numEntered; i++)
        {
            ParticleSystem.Particle p = enter[i];

            if (
                colliderData.GetCollider(i, 0).gameObject.tag != "TrapLight" &&
                !IsDecoy
            )
            {
                mothsInGoal++;
            }
            p.remainingLifetime = 0;
            enter[i] = p;
        }

        partSys
            .SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }

    public int getMothsInGoal()
    {
        return mothsInGoal;
    }
}
