using System.Collections;
using UnityEngine;
// 1
public class Player : Character
{
    // Empty, for now.
    public HealthBar healthBarPrefab;
    // 2
    HealthBar healthBar;
    public HitPoints hitPoints;

    // 1
    public Inventory inventoryPrefab;
    // 2
    Inventory inventory;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        // 1
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        // 1
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        // 2
        hitPoints.value = startingHitPoints;
    }

    // 1
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            bool shouldDisappear = false;
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;
            // 2
            if (hitObject != null)
            {
                // 3
                print("Git: " + hitObject.objectName);

                switch (hitObject.itemType)
                {
                    // 2
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);                      
                        break;
                    // 3
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }
                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }
    

    public bool AdjustHitPoints(int amount)
    {
        // 5
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount > maxHitPoints ? maxHitPoints : hitPoints.value + amount;

            print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints);
            return true;
        }
        return false;
    }
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints.value -= damage;
            if (hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }


    public override void KillCharacter() 
    {
        // 1
        base.KillCharacter();
        // 2
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }
}