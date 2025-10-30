using UnityEngine;

public class SyncedAnimation : MonoBehaviour
{
    // Animator attached to the GameObject
    public Animator animator;

    // current animation state
    public AnimatorStateInfo animatorStateInfo;

    // addresses the current state in the Animator using the Play() function
    public int currentState;

    void Start()
    {
        animator = GetComponent<Animator>();

        // get info abt current state
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // convert the state name to an integer hash
        currentState = animatorStateInfo.fullPathHash;
    }

    
    void Update()
    {
        // start current animation from wherever the current Conductor loop is
        // NOTE: The shorter the song, the faster the animation will be
        animator.Play(currentState, -1, Conductor.instance.loopPositionInAnalog);

        

        // makes sure that frames are only changed in the next update
        animator.speed = 0;
    }
}
