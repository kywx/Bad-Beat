using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private int _health;

    public PlayerCombatStatsSO stats;
    private PlayerRespawn RespawnManager;

    public GameObject UIManager;

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
        //Debug.Log("Damage: "+dmg);
        //Debug.Log("current health: "+_health);
        if(iframeTimer <= 0){
            _health -= dmg;
            //UIManager.GetComponent<SteampunkUIManager>().standaloneCurrentHealth -= dmg;
            for (int i = 0; i < dmg; i++) {
                //UIManager.GetComponent<SteampunkUIManager>().standaloneCurrentHealth -= 1;
            }
            Debug.Log("Health: " + CurrentHealth);
            //Debug.Log("Actually decrease");
            //Debug.Log("current health: "+_health);

            // Clamp to prevent negative health
            _health = Mathf.Max(0, _health);
            iframeTimer = stats.iframes;

            if (_health <= 0 && RespawnManager != null)
            {
                //Debug.Log("should respawn");
                RespawnManager.Respawn();
                //UIManager.GetComponent<SteampunkUIManager>().standaloneCurrentHealth = MaxHealth;
            }
        }
    }

    public void Heal(int amt)
    {
        _health = Mathf.Min(stats.MaxHealth, _health + amt); // Fixed: was Mathf.Max
        //UIManager.GetComponent<SteampunkUIManager>().standaloneCurrentHealth = Mathf.Min(stats.MaxHealth, _health + amt);
    }

    public void HealToMax()
    {
        _health = stats.MaxHealth;
        //UIManager.GetComponent<SteampunkUIManager>().standaloneCurrentHealth = stats.MaxHealth;
        //Debug.Log(_health);
    }
}