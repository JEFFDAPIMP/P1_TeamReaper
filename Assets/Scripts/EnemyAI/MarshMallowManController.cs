using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
[RequireComponent(typeof(Health))]
public class MarshMallowManController : MonoBehaviour
{
    private EnemyBase enemyBase;
    private Health health;
    private Animator animator;
    private float normalSpeed;
    private float currentHealth;
    private bool isRaging = false;
    private bool iced = false;
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 7f;

    // passive while whole
    // if take any damage rage and attack player
    // if frozen turn back from rage

    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        normalSpeed = enemyBase.speed;
        currentHealth = health.health;
    }

    void Update()
    {
        // Track current health
        float healthDifference = currentHealth - health.health;
        currentHealth = health.health;

        // If I lose health, Rage!
        if (healthDifference > 0)
        {
            isRaging = true;
        }

        // IF I'M RAGING, but player is nice enough to cool me off, no more rage
        if (isRaging && enemyBase.iced)
        {
            isRaging = false;
            currentHealth = health.health; // Reset current health when calmed
        }

        // IF I'M RAGING, DO THE RAGE BEHAVIOR, otherwise do the normal behavior
        if (isRaging)
        {
            RageBehavior();
        }
        else
        {
            NormalBehavior();
        }
    }

    //IM ANGRY, TIME TO TAKE IT OUT ON THE PLAYER BECAUSE ANGRY!
    private void RageBehavior()
    {
        animator.SetBool("Burnt", true);
        animator.SetBool("Walking", true);
        animator.SetBool("Running", true);
        enemyBase.wondering = false;
        //enemyBase.speed = enemyBase.stuck ? 0f : runSpeed;
    }

    //Normal everyday marshmallow man minding his own business, let's wander around!
    private void NormalBehavior()
    {
        animator.SetBool("Burnt", false);
        animator.SetBool("Walking", true);
        animator.SetBool("Running", false);
        enemyBase.wondering = true;
        //enemyBase.speed = enemyBase.stuck ? 0f : walkSpeed;
    }
}
