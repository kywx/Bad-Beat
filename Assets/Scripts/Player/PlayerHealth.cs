using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private int _health;

    public PlayerCombatStatsSO stats;
    private PlayerRespawn RespawnManager;

    // Property to allow UI to read health without reflection
    public int CurrentHealth => _health;
    public int MaxHealth => stats != null ? stats.MaxHealth : 0;

    private void Awake()
    {
        RespawnManager = this.GetComponent<PlayerRespawn>();
        _health = stats.MaxHealth;
    }

    public void Damage(int dmg)
    {
        _health -= dmg;

        // Clamp to prevent negative health
        _health = Mathf.Max(0, _health);

        if (_health <= 0 && RespawnManager != null)
        {
            RespawnManager.Respawn();
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