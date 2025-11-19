using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private int _health;

    public PlayerCombatStatsSO stats;
    private PlayerRespawn RespawnManager;

    private float iframeTimer;

    // Property to allow UI to read health without reflection
    public int CurrentHealth => _health;
    public int MaxHealth => stats != null ? stats.MaxHealth : 0;

    private void Awake()
    {
        RespawnManager = this.GetComponent<PlayerRespawn>();
        _health = stats.MaxHealth;
        iframeTimer = 0;
    }

    private void Update()
    {
        if(iframeTimer > 0)
        {
            iframeTimer -= Time.deltaTime;
        }
    }

    public void Damage(int dmg)
    {
        if(iframeTimer > 0){
            _health -= dmg;

        // Clamp to prevent negative health
            _health = Mathf.Max(0, _health);
            iframeTimer = stats.iframes;

            if (_health <= 0 && RespawnManager != null)
            {
                RespawnManager.Respawn();
            }
        }
    }

    public void Heal(int amt)
    {
        _health = Mathf.Min(stats.MaxHealth, _health + amt); // Fixed: was Mathf.Max
    }

    public void HealToMax()
    {
        _health = stats.MaxHealth;
    }
}