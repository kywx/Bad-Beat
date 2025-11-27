using UnityEngine;

public class MiniBossRangedAttack1 : EnemyAttackTemplate
{

     // ============ Can be copy to other place for ranged attack, 
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private GameObject projectileSpawnPoint;

    private float _cooldown_timer = 0;

    void Update()
    {
        _cooldown_timer -= Time.deltaTime;
    }
   
    protected void RangedAttack(bool dir)
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, transform.rotation);
        // Assuming you're using a similar projectile script
        MiniBossProjectile script = projectile.GetComponent<MiniBossProjectile>();
        script.setDirection(dir); // Set direction, left/right
    }

    public void Shoot(bool dir)
    {
        if (_cooldown_timer <= 0)
        {
            RangedAttack(dir);
            _cooldown_timer = _attackCooldown; // Ensure attack cooldown is respected
        }
    }
     //  Can be copy to other place for ranged attack ============
}
