using UnityEngine;

public class MossEnemyAttack : EnemyAttackTemplate
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private GameObject projectileSpawnPoint;

    private float _cooldown_timer = 0;

    private void Update()
    {
        _cooldown_timer -= Time.deltaTime;
    }


    protected void RangedAttack(bool dir)
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, this.transform.rotation);
        MossProjectile script = projectile.GetComponent<MossProjectile>();
        script.setDirection(dir);
    }
    public void Shoot(bool dir)
    {
        if(_cooldown_timer <= 0){
            RangedAttack(dir);
            _cooldown_timer = _attackCooldown;
        }
    }
}