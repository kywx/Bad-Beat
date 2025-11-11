using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //public static PlayerInput PlayerInput;
    private Collider2D rightAttackRange;
    [SerializeField] private GameObject frontAttackPoint;
    [SerializeField] private GameObject upAttackPoint;
    [SerializeField] private GameObject pogoAttackPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemies;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] float pogoForce;
    
    private int damage;
    public PlayerCombatStatsSO stats;

    // Update is called once per frame
    private void OnSimpleAttack(){
        Collider2D[] enemy;
        bool bounce = false;
        //Debug.Log("Tried hit!");
        if (Input.GetKey(KeyCode.DownArrow)){
            //Debug.Log("Down!");
            enemy = Physics2D.OverlapCircleAll(pogoAttackPoint.transform.position, radius, enemies);
            bounce = true;
        }
        else if(Input.GetKey(KeyCode.UpArrow)){
            //Debug.Log("Up!");
            enemy = Physics2D.OverlapCircleAll(upAttackPoint.transform.position, radius, enemies);
        }
        else{
            //Debug.Log("Forward!");
            enemy = Physics2D.OverlapCircleAll(frontAttackPoint.transform.position, radius, enemies);
        }

        foreach (Collider2D enemyGameObject in enemy){
            Debug.Log("hit!");
            //Destroy(enemyGameObject.gameObject);
            EnemyHealthTemplate attackedEnemy = enemyGameObject.gameObject.GetComponentInParent<EnemyHealthTemplate>();
            if (attackedEnemy != null)
            {
                attackedEnemy.TakeDamageSimple(1);
            } else
            {
                print("EnemyHealthTemplate not found");
            }
            if (bounce){
                playerRB.AddForce(transform.up * pogoForce);
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(pogoAttackPoint.transform.position, radius);
        Gizmos.DrawWireSphere(upAttackPoint.transform.position, radius);
        Gizmos.DrawWireSphere(frontAttackPoint.transform.position, radius);
    }




}
