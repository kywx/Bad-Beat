using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    //public static PlayerInput PlayerInput;

    [SerializeField] private GameObject frontAttackPoint;
    [SerializeField] private GameObject upAttackPoint;
    [SerializeField] private GameObject pogoAttackPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemies;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] float pogoForce;
    
    private int damage;
    public PlayerCombatStatsSO stats;

    [SerializeField] private Slider specialMeleeBar;

    private bool specialMeleeCharging;
    private int specialMeleeCharge;
    private bool specialMeleeReleased;
    [SerializeField] int specialMeleeChargeFrames;

    [SerializeField] private Animator anim;

    private void Start(){
        specialMeleeCharging = false;
        specialMeleeCharge = 0;
        specialMeleeReleased = false;
        specialMeleeBar.maxValue = specialMeleeChargeFrames;
        specialMeleeBar.value = 0;
        specialMeleeBar.gameObject.SetActive(false);
    }

    private void Update(){
        if(InputManager.SpecialMeleePressed){
            specialMeleeCharging = true;
            specialMeleeBar.gameObject.SetActive(true);
        }
        if(InputManager.SpecialMeleeReleased){
            specialMeleeCharging = false;
            specialMeleeBar.gameObject.SetActive(false);
            if (specialMeleeCharge >= specialMeleeChargeFrames){
                specialMeleeReleased = true;
            }
        }
    }

    private void FixedUpdate(){
        if(specialMeleeCharging){
            if(specialMeleeCharge < specialMeleeChargeFrames){
                specialMeleeCharge += 1;
                specialMeleeBar.value += 1;
            }
        }
        if(specialMeleeReleased == true){
            Collider2D[] enemy = Physics2D.OverlapCircleAll(frontAttackPoint.transform.position, radius, enemies);
            foreach (Collider2D enemyGameObject in enemy){

                //
                //PLACEHOLDER BELOW
                Destroy(enemyGameObject.gameObject);  //REPLACE WITH DAMAGING ENEMY FUNCTION/CODE HERE
                //
                //

            }
            specialMeleeCharge = 0;
            specialMeleeBar.value = 0;
            specialMeleeReleased = false;
        }
    }

    // Update is called once per frame
    private void OnSimpleAttack(){
        Collider2D[] enemy;
        bool bounce = false;
        //Pogo
        if (Input.GetKey(KeyCode.DownArrow)){
            enemy = Physics2D.OverlapCircleAll(pogoAttackPoint.transform.position, radius, enemies);
            bounce = true;
        }
        //Up Attack
        else if(Input.GetKey(KeyCode.UpArrow)){
            enemy = Physics2D.OverlapCircleAll(upAttackPoint.transform.position, radius, enemies);
        }
        //Front Attack
        else{
            enemy = Physics2D.OverlapCircleAll(frontAttackPoint.transform.position, radius, enemies);
            anim.SetTrigger("attackPrimary");
        }

        foreach (Collider2D enemyGameObject in enemy){
            //Destroy(enemyGameObject.gameObject);
            //Debug.Log("hit!");
            //Destroy(enemyGameObject.gameObject);
            EnemyHealthTemplate attackEnemy = enemyGameObject.gameObject.GetComponentInParent<EnemyHealthTemplate>();
            EnemyMovementTemplate knockbackEnemy = enemyGameObject.gameObject.GetComponentInParent<EnemyMovementTemplate>();

            if (attackEnemy != null)
            {
                attackEnemy.TakeDamageSimple(stats.AttackDamage);
            } else
            {
                print("EnemyHealthTemplate not found so can't damage");
            }

            if (knockbackEnemy != null)
            {
                knockbackEnemy.Knockback(this.transform.position, stats.KnockbackForce);
            }
            else
            {
                print("EnemyMovementTemplate not found so no knockback");
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
