using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, описывающий поле боя
/// </summary>
public class Battlefield : MonoBehaviour
{
    /// <summary>
    /// Список всех клеток поля
    /// </summary>
    [SerializeField]
    private List<Cell> cells = new List<Cell>();
    /// <summary>
    /// Список всех врагов на поле
    /// </summary>
    public List<Enemy> enemies = new List<Enemy>();
    /// <summary>
    ///Список перекрашенных клеток
    /// </summary>
    public List<Cell> recoloredCells = new List<Cell>();

    private void Awake()
    {
        Cell[] c = GetComponentsInChildren<Cell>();
        cells = new List<Cell>(c);
    }
    /// <summary>
    /// Поиск клеток по их координатам
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Cell FindCell(float x, float y)
    {
       return cells.Find(cell => cell.position.x == x && cell.position.y == y);
    }

    
}
