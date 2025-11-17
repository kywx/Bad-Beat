using UnityEngine;

public class FadeOut : StateMachineBehaviour
{

    public float fadeTimer = 0.5f;
    private float fadeCounter;
    public float delayTimer = 2f;
    SpriteRenderer spriteRenderer;
    GameObject fadingObject;
    Color startColor;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fadeCounter = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        fadingObject = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (delayTimer <= 0)
        {
            fadeCounter += Time.deltaTime;

            float newAlpha = (1 - (fadeCounter / fadeTimer));
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            if (fadeCounter > fadeTimer)
            {
                Destroy(fadingObject);
            }
        } else
        {
            delayTimer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
