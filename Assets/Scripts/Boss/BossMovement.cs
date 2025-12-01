using System;
using UnityEngine;
using System.Threading.Tasks;

public class BossMovement : MonoBehaviour
{
    public GameObject targetA;
    public GameObject targetB;

    public float moveSpeed;
    public float delay;
    public float attackRange = 5f;

    private Transform curTarget;
    private bool delayed;

    private Transform player;
    private MiniBossRangedAttack1 rangedAttack;   // your attack script
    private bool isFacingRight = true;

    void Start()
    {
        curTarget = targetA.transform;
        delayed = false;

        targetA.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        targetB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        player = GameObject.FindGameObjectWithTag("Player").transform;   // or assign via Inspector [web:78]
        rangedAttack = GetComponent<MiniBossRangedAttack1>();

        if (rangedAttack == null)
        {
            Debug.LogError("MiniBossRangedAttack1 missing on Boss!");
        }
    }

    async void Update()
    {
        if (player != null)
        {
            HandleFacing();     // face the player if needed [web:82][web:85]
            HandleRangedAttack();
        }

        if (!delayed)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                curTarget.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, curTarget.position) < 0.01f)
            {
                delayed = true;
                curTarget = (curTarget == targetA.transform ? targetB.transform : targetA.transform);
                await Task.Delay(TimeSpan.FromSeconds(delay));   // your existing wait
                delayed = false;
            }
        }
    }

    private void HandleFacing()
    {
        // 2D left/right flip using localScale.x sign
        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x && !isFacingRight)
        {
            isFacingRight = true;
            scale.x = Mathf.Abs(scale.x);        // face right
            transform.localScale = scale;
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            isFacingRight = false;
            scale.x = -Mathf.Abs(scale.x);       // face left
            transform.localScale = scale;
        }
    }

    private void HandleRangedAttack()
    {
        if (rangedAttack == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // dir = true if shooting right, false if shooting left (adapt to your script)
            bool dir = isFacingRight;
            rangedAttack.PerformAttack(dir);     // call the alternating shoot/double-shoot method
        }
    }
}
