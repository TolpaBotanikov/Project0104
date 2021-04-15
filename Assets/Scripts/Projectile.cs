using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для работы со снарядами
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Цель снаряда
    /// </summary>
    public Vector3 target;
    /// <summary>
    /// Расстояние на котором снаряд уничтожается
    /// </summary>
    [SerializeField]
    private float permipermissibleDistance;
    public Rigidbody rigid;
    /// <summary>
    /// Скорость снаряда
    /// </summary>
    public float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < permipermissibleDistance)
            Destroy(this.gameObject);
    }
}
