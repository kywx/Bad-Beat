using UnityEngine;

public class BurningPlatform : MonoBehaviour {

    private PlayerHealth target;
    private BurningPlatformAnimator anim;

    public int DamageValue;
    public float TimeToStartDamage;
    public float TimeBetweenDamageTicks;
    public float TimeToReset;
    private float _timeElapsed;

    private bool _playerPresent;

    public void Awake()
    {
        anim = GetComponent<BurningPlatformAnimator>();
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_playerPresent)
        {
            if(_timeElapsed >= TimeToStartDamage)
            {
                target.Damage(DamageValue);
                _timeElapsed -= TimeBetweenDamageTicks;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        target = collider.gameObject.GetComponent<PlayerHealth>();

        if (target != null)
        {
            _playerPresent = true;
            _timeElapsed = 0;
            if(anim != null)
            {
                anim.playerArrived();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        PlayerHealth health = collider.gameObject.GetComponent<PlayerHealth>();
        if (health != null)
        {
            _playerPresent = false;
            if(anim != null)
            {
                anim.playerLeft();
            }
        }
    }

}