using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private uint maxHealth;

    private uint health;

    // Start is called before the first frame update
    void Start()
    {
        // Sets max Health Value both for the code and the slider
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        
        // Sets current health to max health
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Sets slider value to health value
        slider.value = health;
    }
}
