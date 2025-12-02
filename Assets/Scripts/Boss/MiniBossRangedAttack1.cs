using System.Collections;
using UnityEngine;

public class MiniBossRangedAttack1 : EnemyAttackTemplate
{
    [SerializeField] private GameObject projectilePrefab_1;
    [SerializeField] private GameObject projectilePrefab_2;
    [SerializeField] private GameObject projectileSpawnPoint;

    private bool _useSingle = true;
    private float _cooldown_timer = 2f;
    public bool _canShootSingle = true;

    public GameObject boss;

    void Update()
    {
        _cooldown_timer -= Time.deltaTime;
    }

    public void PerformAttack(Transform target)
    {
        int randomNumber = Random.Range(1, 4 - boss.GetComponent<BossHealth>().phase);
        
        if (randomNumber == 1) {
            StartCoroutine(DoubleShootRoutine(target));  // double shot
        } else if (randomNumber == 2) {
            Shoot(target);                 // single shot
        } else {
            boss.GetComponent<BossMinionSpawner>().spawn();
        }

        /*
        if (_cooldown_timer > 0f) return;

        if (_useSingle && _canShootSingle)
        {
            Shoot(target);
        }
        else
        {
            StartCoroutine(DoubleShootRoutine(target));
        }

        _useSingle = !_useSingle; 
        _cooldown_timer = _attackCooldown;      // toggle for next time [web:103][web:114]
        */
    }

    protected void RangedAttack(Transform target)
    {
        Vector2 dir = (target.position - projectileSpawnPoint.transform.position).normalized;
        Debug.Log("Direction to target: " + dir);
        GameObject projectile = Instantiate(projectilePrefab_1,
                                            projectileSpawnPoint.transform.position,
                                            Quaternion.identity);
        MiniBossProjectile1 script = projectile.GetComponent<MiniBossProjectile1>();
        script.SetDirection(dir);
    }

    protected void RangedAttackCurve(Transform target)
    {
        Vector2 dir = target.position - projectileSpawnPoint.transform.position;
        dir.y = 0;                      // Zero out the vertical component
        dir = dir.normalized;           // Normalize to get direction with length 1

        GameObject projectile = Instantiate(projectilePrefab_2,
                                            projectileSpawnPoint.transform.position,
                                            Quaternion.identity);
        MiniBossProjectile2 script = projectile.GetComponent<MiniBossProjectile2>();
        script.SetDirection(dir);
    }

    public void Shoot(Transform target)
    {
        if (_cooldown_timer <= 0f)
        {
            RangedAttack(target);
        }
    }

    private IEnumerator DoubleShootRoutine(Transform target)
    {
        RangedAttackCurve(target);
        yield return new WaitForSeconds(0.05f);
        RangedAttackCurve(target);
    }
}
