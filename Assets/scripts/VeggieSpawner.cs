using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieSpawner : MonoBehaviour
{
    public List<GameObject> prefabsToSpawn;
    public ParticleSystem spawnEffect;
    public Animator farmerAnimator; // Reference to the farmer's Animator component

    private int[] priceList = { 1, 5, 2 };
    void Start()
    {
        farmerAnimator = GameObject.Find("farmer").GetComponent<Animator>();
    }

    public void SpawnPrefab(int index)
    {
        if (index < 0 || index >= prefabsToSpawn.Count) return;

        if (!CoinManager.Instance.SpendCoins(priceList[index])) return;

        GameObject spawnedPrefab = Instantiate(prefabsToSpawn[index], transform.position, Quaternion.identity);
        spawnedPrefab.GetComponent<Potato>().isSpawning = true;
        TriggerSpawnEffect();
        ConfigurePrefabPhysics(spawnedPrefab);
        if (farmerAnimator != null)
        {
            farmerAnimator.SetBool("is_spawning", true);
        }
    }

    private void TriggerSpawnEffect()
    {
        if (spawnEffect != null)
        {
            Instantiate(spawnEffect, transform.position, Quaternion.identity).Play();
        }
    }

    private void ConfigurePrefabPhysics(GameObject prefab)
    {
        Rigidbody2D rb = prefab.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, 1);

        BoxCollider2D collider = prefab.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        collider.isTrigger = true;

        StartCoroutine(EnablePhysics(prefab, collider));
    }

    private IEnumerator EnablePhysics(GameObject prefab, BoxCollider2D collider)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Collider2D[] hits = Physics2D.OverlapCircleAll(prefab.transform.position + Vector3.down*(collider.size.y / 2), 0.1f);
            bool isGrounded = false;
            foreach (var hit in hits)
            {
                if (hit.gameObject.CompareTag("Ground"))
                {
                    isGrounded = true;
                    break;
                }
            }

            if (!isGrounded)
            {
                collider.enabled = true;
                collider.isTrigger = false;
                Rigidbody2D rb = prefab.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 1;
                }

                Animator animator = prefab.GetComponent<Animator>();
                if (animator != null)
                {
                    Debug.Log("Setting walking to true");
                    animator.SetBool("walking", true);
                }
                
                if (farmerAnimator != null)
                {
                    farmerAnimator.SetBool("is_spawning", false);
                }
                break;
            }
        }
    }
}
