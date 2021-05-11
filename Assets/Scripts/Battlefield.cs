using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, описывающий поле боя
/// </summary>
public class Battlefield : MonoBehaviour
{
    public static readonly Vector2[] DIRS = new[]
        {
            new Vector2(1, 0),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(1, -1),
            new Vector2(-1, 1)
        };
    /// <summary>
    /// Список всех клеток поля
    /// </summary>
    [SerializeField]
    private List<Cell> cells = new List<Cell>();
    /// <summary>
    /// Список всех врагов на поле
    /// </summary>
    public List<Enemy> enemies = new List<Enemy>();
    public Ally hero;
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

    public Cell FindCell(Vector2 pos)
    {
        return cells.Find(cell => cell.position == pos);
    }

    public List<Cell> Neighbors(Cell cell)
    {
        List<Cell> result = new List<Cell>();
        foreach (var dir in DIRS)
        {
            Cell next = FindCell(cell.position.x + dir.x, cell.position.y + dir.y);
            if (next != null && /*next.unit == null &&*/ next.ostacle == null)
            {
                result.Add(next);
            }
        }
        return result;
    }
}
