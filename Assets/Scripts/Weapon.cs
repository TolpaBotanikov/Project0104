using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, описывающий оружие
/// </summary>
public class Weapon : MonoBehaviour
{
    /// <summary>
    /// Дистанция оружияы
    /// </summary>
    public int range;
    /// <summary>
    /// Урон оружия
    /// </summary>
    public int damage;
    /// <summary>
    /// Шанс попадания
    /// </summary>
    public float hitChance;
    public GameObject shootPoint;
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
