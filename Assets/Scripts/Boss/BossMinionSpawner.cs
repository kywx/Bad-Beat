using UnityEngine;

public class BossMinionSpawner : MonoBehaviour
{
    [SerializeField]
    private bool timed = false; //can be enabled to spawn on regular interval decided by cooldown.
    
    [SerializeField]
    private float cooldown;

    [SerializeField]
    GameObject summonTarget;

    [SerializeField]
    Collider2D colliderOfSummoner; //collider used for rigidbody collision by the parent. Used to disable collision between target and parent.

    new private bool enabled = true;

    private float timer = 0;

    /*
    private void Update()
    {
        
        if (timed && enabled)
        {
            if(timer <= 0)
            {
                spawn();
                timer = cooldown;
                Debug.Log(timer);
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
}
*/

    public void spawn()
    {
        if(!enabled) return;
        
        GameObject target = Instantiate(summonTarget, this.transform);
        Debug.Log("spawn");
        
        if(colliderOfSummoner != null){
            Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), colliderOfSummoner);
            Collider2D[] colliders = target.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                Physics2D.IgnoreCollision(col, colliderOfSummoner);            
            }  
        }
    }
    public void disable() //public method to stop the spawning when the boss loses access to it.
    {
        enabled = false;
    }
}