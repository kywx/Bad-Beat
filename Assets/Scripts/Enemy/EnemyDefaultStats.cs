using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDefaultStats", menuName = "Scriptable Objects/Enemy/EnemyDefaultStats")]
public class EnemyDefaultStats : ScriptableObject
{
    [System.Serializable]
    public enum Element
    {
        Fire,
        Water,
        Leaf
    }

    [Header("Health")]

    [SerializeField] public float hp; // continuous health
    [SerializeField] public int armor; // very optional. Intended to be discrete shield health that depletes for each attack the player does
    // maybe it can go down faster when hitting the weakness

    [Header("Resistances & Vulnerabilities")]

    [SerializeField] public float resistanceModifier; // bigger number = more damage reduction
    [SerializeField] public float vulnerabilityModifier; // bigger number = more damage taken

    [SerializeField] public Element weakness;
    [SerializeField] public Element strength;


    [Header("Movement")]

    [SerializeField] public float groundSpeed;
    [SerializeField] public float airSpeed;
    [SerializeField] public float knockbackResistance; // higher number = less knockback
    [SerializeField] public float stoppingDistance;
    [SerializeField] public float startChaseDistance;

    [Header("Attacks")]

    [SerializeField] public float minAttackRange; // may be redundant to have two ranges. Change if needed
    [SerializeField] public float maxAttackRange;

    [SerializeField] public float attackCooldown;
    [SerializeField] public int attackDamage; // enemy deals discrete damage to the player like in Hollow Knight

    [Header("Gambling?")]
    [SerializeField][Range(0f, 1f)] public float probability;

}
