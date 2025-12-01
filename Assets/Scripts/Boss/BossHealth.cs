using UnityEngine;

public class BossHealth : EnemyHealthTemplate
{   public GameObject keyPrefab; // Assign the key prefab in Inspector
    public override void Die()
    {
        // Spawn the key at boss position
        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
    
}