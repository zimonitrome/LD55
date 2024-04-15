using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteAssigner : MonoBehaviour
{
    public List<Sprite> spritesToChooseFrom; // Assign this list in the Inspector

    void Start()
    {
        // Check if there are sprites in the list
        if (spritesToChooseFrom.Count > 0)
        {
            // Get the SpriteRenderer component on this GameObject
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Randomly assign a sprite from the list
                spriteRenderer.sprite = spritesToChooseFrom[Random.Range(0, spritesToChooseFrom.Count)];

                // Randomly decide whether to flip the sprite on the x-axis
                spriteRenderer.flipX = Random.Range(0, 2) == 0; // 50% chance to flip
            }
            else
            {
                Debug.LogError("No SpriteRenderer found on the GameObject.");
            }
        }
        else
        {
            Debug.LogError("No sprites provided in the list.");
        }
    }
}
