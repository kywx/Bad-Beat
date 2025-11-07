using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

public class BurningPlatformAnimator : MonoBehaviour
{
    private Animator anim;
    private float _enter_speed;
    private float _exit_speed;

    private void Start()
    {
        BurningPlatform source = this.GetComponent<BurningPlatform>();
        anim = GetComponent<Animator>();

        _enter_speed = 1 / source.TimeToStartDamage;
        _exit_speed = 1 / source.TimeToReset;
    }

    public void playerArrived()
    {
        if (anim != null)
        {
            anim.speed = _enter_speed;
            anim.ResetTrigger("PlayerLeft");
            anim.SetTrigger("PlayerArrived");
        }    
    }
    public void playerLeft()
    {
        if (anim != null)
        {
            anim.speed = _exit_speed;
            anim.ResetTrigger("PlayerArrived");
            anim.SetTrigger("PlayerLeft");
        }
    }
}