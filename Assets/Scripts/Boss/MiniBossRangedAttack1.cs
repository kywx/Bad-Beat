using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossRangedAttack1 : EnemyAttackTemplate
{
    [SerializeField] private GameObject projectilePrefab_1;
    [SerializeField] private GameObject projectilePrefab_2;
    [SerializeField] private GameObject projectileSpawnPoint;

    // true = single, false = double
    private bool _useSingle = true;

    private float _cooldown_timer = 2f;

    public bool _canShootSingle = true;

    void Update()
    {
        _cooldown_timer -= Time.deltaTime;
    }

    public void PerformAttack(bool dir)
    {
        if (_cooldown_timer > 0f) return;

        if (_useSingle && _canShootSingle)
        {
            Shoot(dir);                 // single shot
        }
        else
        {
            StartCoroutine(DoubleShootRoutine(dir));  // double shot
        }

        _useSingle = !_useSingle; 
        _cooldown_timer = _attackCooldown;      // toggle for next time [web:103][web:114]
    }

    protected void RangedAttack(bool dir)
    {
        GameObject projectile = Instantiate(projectilePrefab_1,
                                            projectileSpawnPoint.transform.position,
                                            transform.rotation);
        MiniBossProjectile1 script = projectile.GetComponent<MiniBossProjectile1>();
        script.setDirection(dir);
    }

    protected void RangedAttackCurve(bool dir)
    {
        GameObject projectile = Instantiate(projectilePrefab_2,
                                            projectileSpawnPoint.transform.position,
                                            transform.rotation);
        MiniBossProjectile2 script = projectile.GetComponent<MiniBossProjectile2>();
        script.setDirection(dir);
    }

    public void Shoot(bool dir)
    {
        if (_cooldown_timer <= 0f)
        {
            RangedAttack(dir);
        }
    }

    private IEnumerator DoubleShootRoutine(bool dir)
    {
        RangedAttackCurve(dir);
        yield return new WaitForSeconds(0.05f);
        RangedAttackCurve(dir);
    }
}
