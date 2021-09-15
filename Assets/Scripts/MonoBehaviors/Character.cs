using UnityEngine;
using System.Collections;


// 1
public abstract class Character : MonoBehaviour
{
    // 2
    //public HitPoints hitPoints;
    public float maxHitPoints;
    public float startingHitPoints;

    public enum CharacterCategory
    {
        PLAYER,
        ENEMY
    }

    public CharacterCategory characterCategory;

    // 1
    public virtual void KillCharacter()
    {
        // 2
        Destroy(gameObject);
    }

    // 1
    public abstract void ResetCharacter();
    // 2
    public abstract IEnumerator DamageCharacter(int damage, float interval);

    public virtual IEnumerator FlickerCharacter()
    {
        // 1
        GetComponent<SpriteRenderer>().color = Color.red;
        // 2
        yield return new WaitForSeconds(0.1f); 
        
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}