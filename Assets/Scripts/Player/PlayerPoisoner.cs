using UnityEngine;

public class PlayerPoisoner : MonoBehaviour
{
    //a script that applies and removes poisons from the player.
    private PlayerHealth HealthManager;
    private SpriteRenderer PlayerSprite;
    private float _duration;
    private float _timeBetweenTicks;
    private float _timeActive;
    private float _timeSinceLastTick;
    private int _damagePerTick;
    private bool _poisoned;

    private void Awake()
    {
        HealthManager = this.GetComponent<PlayerHealth>();
        PlayerSprite = this.GetComponent<SpriteRenderer>();
        originalColor = PlayerSprite.color;
    }
    private void Update()
    {
        if (_poisoned)
        {
            _timeActive += Time.deltaTime;
            _timeSinceLastTick += Time.deltaTime;
            if (_timeSinceLastTick >= _timeBetweenTicks)
            {
                HealthManager.Damage(_damagePerTick);
                _timeSinceLastTick = 0;
            }
            if (_timeActive >= _duration)
            {
                _poisoned = false;
                UpdateSprite();
            }
        }
    }

    /*
    * If the player is already poisoned, the previous one will be overwritten or have its duration refreshed.
    */
    public void Poison(float duration, int damagePerTick, float timeBetweenTicks)
    {
        _poisoned = true;
        _timeActive = 0;
        _timeSinceLastTick = 0;
        _duration = duration;
        _damagePerTick = damagePerTick;
        _timeBetweenTicks = timeBetweenTicks;
        UpdateSprite();
    }
    public void CurePoison()
    {
        _poisoned = false;
        UpdateSprite();
    }

    //This is just a temporary visual for the poison effect that works with the current placeholder sprite
    //I'm sure we'll replace this with something else, like a particle or ui fluorish.

    private Color poisonedColor = new Color(0f, 0.4f, 0);
    private Color originalColor;
    
    private void UpdateSprite()
    {
        if (_poisoned)
        {
            PlayerSprite.color = poisonedColor;
        }
        else
        {
            PlayerSprite.color = originalColor;
        }
    }

}