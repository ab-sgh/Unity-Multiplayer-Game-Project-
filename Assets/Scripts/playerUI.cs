using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform HealthUI;

    private networkHealthState healthState;

    void OnEnable()
    {
        healthState = GetComponent<networkHealthState>();
        if (healthState != null)
        {
            healthState.HealthPoint.OnValueChanged += HealthChanged;
        }
    }

    void OnDisable()
    {
        if (healthState != null)
        {
            healthState.HealthPoint.OnValueChanged -= HealthChanged;
        }
    }

    private void HealthChanged(int previousValue, int newValue)
    {
        HealthUI.transform.localScale = new Vector3(newValue / 100f, 1, 1);
    }
}

