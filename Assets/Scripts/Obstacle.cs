using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для работы с препятствиями
/// </summary>
public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// Координаты препятствия
    /// </summary>
    public Vector2 position;
    /// <summary>
    /// Шанс пролета снаряда сквозь препятствие
    /// </summary>
    public float hitChance;
}
