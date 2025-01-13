using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public CharacterStats characterStats;
    public bool isDead;
    protected virtual void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void UnderAttack(string state, Direction.Dir attackDirection, bool knockback)
    {
        if (!isDead) {
            if (state == "stunned")
            {
                TurnStunned(attackDirection, knockback);
            }
            else if (state == "damaged")
            {
                Damaged(attackDirection, knockback);
            }
        }
    }


    public virtual bool TurnStunned(Direction.Dir attackDirection, bool knockback)
    {
        return false;
    }

    public virtual void Damaged(Direction.Dir attackDirection, bool knockback)
    {
    }

    public virtual IEnumerator HitKnockback(Direction.Dir attackDirection, Vector2 knockBackVelocity, float KnockBackDuration)
    {
        yield return null;
    }

    public virtual IEnumerator FreezedTime(float freezedSeconds)
    {
        yield return null;
    }

    public virtual void _FreezeTime(bool isFreezing)
    {

    }

    public virtual void Die() { 
    
    }
}
