using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health targetHealth;
    private Image healthBarImage;

    void Start()
    {
        healthBarImage = GetComponent<Image>();
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float healthPercentage)
    {
        healthBarImage.fillAmount = healthPercentage;
    }

    void OnDestroy()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged -= HandleHealthChanged;
        }
    }
}
