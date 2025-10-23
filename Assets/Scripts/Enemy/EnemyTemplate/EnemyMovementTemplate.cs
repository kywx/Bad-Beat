using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyMovement
{
    Idle, // no enemy movement
    Patrol, // enemy will walk back and forth
    Chase,  // enemy will follow the player
    Evasive, // enemy will avoid or sidestep when the player is near
    Unique // code your own enemy-exclusive method for this
}
public abstract class EnemyMovementTemplate : MonoBehaviour
{
    [SerializeField] protected EnemyDefaultStats _enemyStats;
    [SerializeField] protected EnemyMovement _movementType; // lets you tweak what movement logic the enemy will follow

    [SerializeField] protected List<Transform> _patrolPoints;

    #region Movement Stats
    protected float _groundSpeed;
    protected float _airSpeed;
    protected float _knockbackResistance;
    protected float _stoppingDistance;
    protected float _startChaseDistance;

    #endregion

    protected virtual void Awake()
    {
        // initialize the movement stats
        _groundSpeed = _enemyStats.groundSpeed;
        _airSpeed = _enemyStats.airSpeed;
        _knockbackResistance = _enemyStats.knockbackResistance;
        _stoppingDistance = _enemyStats.stoppingDistance;
        _startChaseDistance = _enemyStats.startChaseDistance;



    }

    protected virtual void Update()
    {
        DetermineMovement(); // abstract method
    }

    protected virtual void FixedUpdate()
    {
        RunMovement();
    }


    // you will implement how the enemy switches states. If they don't switch states, make this method empty and modify the _movementState enum in the editor
    protected abstract void DetermineMovement();

    protected virtual void RunMovement()
    {
        if (_movementType == EnemyMovement.Idle)
        {
            RunIdle(); // can be used to play animations or set timers
        }
        if (_movementType == EnemyMovement.Patrol)
        {
            RunPatrol();
        }
        if (_movementType == EnemyMovement.Chase)
        {
            RunChase();
        }
        if (_movementType == EnemyMovement.Evasive)
        {
            RunEvasion();
        }
        if (_movementType == EnemyMovement.Unique)
        {
            RunUnique();
        }
    }

    #region Run Movement
    protected virtual void RunIdle()
    {
        //
    }

    protected virtual void RunPatrol()
    {
        //
    }


    protected virtual void RunChase()
    {
        //
    }

    protected virtual void RunEvasion()
    {
        //
    }

    protected virtual void RunUnique()
    {
        //
    }
    

    #endregion

}
