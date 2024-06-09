using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    // 控制生命值
    private Slider _healthSlider;

    /// <summary>
    /// 生命值
    /// </summary>
    public float Health
    {
        get
        {
            return _healthSlider != null ? _healthSlider.value : 0;
        }

        set
        {
            if (_healthSlider != null)
            {
                _healthSlider.value = Mathf.Clamp(value, 0, _healthSlider.maxValue);
            }
        }
    }

    private void Start()
    {
        if (_healthSlider == null)
        {
            Debug.LogError("Health slider is not assigned!");
        }
    }

    /// <summary>
    /// 傷害
    /// </summary>
    /// <param name="value">傷害值</param>
    public void Damage(float value)
    {
        if (_healthSlider != null)
        {
            Health -= value;
        }
    }

    /// <summary>
    /// 治療
    /// </summary>
    /// <param name="value">治療值</param>
    public void Heal(float value)
    {
        if (_healthSlider != null)
        {
            Health += value;
        }
    }

    /// <summary>
    /// 完全恢復
    /// </summary>
    public void RecoverFully()
    {
        if (_healthSlider != null)
        {
            Health = _healthSlider.maxValue;
        }
    }
}