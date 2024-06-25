using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_collision : MonoBehaviour
{
    public GameObject hitPrefab; // Rename 'hit' to 'hitPrefab' for clarity

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name); // Debug info

        // Instantiate the hit effect at the bullet's position
        GameObject effect = Instantiate(hitPrefab, transform.position, Quaternion.identity);

        // Make the hit effect a child of the bullet
        effect.transform.parent = transform;

        // Destroy the bullet
        Destroy(gameObject);
    }
}
