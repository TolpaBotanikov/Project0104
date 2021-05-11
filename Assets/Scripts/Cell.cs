using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для работы с клетками
/// </summary>
public class Cell : MonoBehaviour
{
    /// <summary>
    /// Координаты клетки
    /// </summary>
    public Vector2 position;
    /// <summary>
    /// Юнит, находящийся на клетке
    /// </summary>
    public Unit unit;
    /// <summary>
    /// Препятствие, находящиеся на клетке
    /// </summary>
    public Obstacle ostacle;

    #region Работа с координатами

    /// <summary>
    /// Перевод координат клетки в кубические
    /// </summary>
    /// <param name="hex">Координаты клетки</param>
    /// <returns>Координаты в кубической системе</returns>
    private static Vector3 HexToCube(Vector2 hex)
    {
        Vector3 res = new Vector3();
        res.x = hex.x;
        res.z = hex.y;
        res.y = -res.x - res.z;
        return res;
    }
    /// <summary>
    /// Перевод из координат в кубической системе
    /// </summary>
    /// <param name="cube">Координаты в кубической системе</param>
    /// <returns>Координаты клетки</returns>
    private static Vector2 CubeToHex(Vector3 cube)
    {
        return new Vector2(cube.x, cube.z);
    }

    /// <summary>
    /// Округляет координаты до целых
    /// </summary>
    /// <param name="h">Координаты в кубической системе</param>
    /// <returns>Целочисленые координаты в кубической системе</returns>
    private static Vector2 Round(Vector3 h)
    {
        float rx = Mathf.Round(h.x);
        float ry = Mathf.Round(h.y);
        float rz = Mathf.Round(h.z);

        var x_diff = Mathf.Abs(rx - h.x);
        var y_diff = Mathf.Abs(ry - h.y);
        var z_diff = Mathf.Abs(rz - h.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return CubeToHex(new Vector3(rx, ry, rz));
    }
    /// <summary>
    /// Линейная интерполяция
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns>Координаты в кубической системе</returns>
    private static Vector3 CubeLerp(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, t),
                           Mathf.Lerp(a.y, b.y, t),
                           Mathf.Lerp(a.z, b.z, t));
    }
    /// <summary>
    /// Вычисляет дистанцию между двумя клетками
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Расстояние между a и b</returns>
    private static float HexDistance(Vector3 a, Vector3 b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
    }

    public static float Distance(Vector2 a, Vector2 b)
    {
        return HexDistance(HexToCube(a), HexToCube(b));
    }

    # endregion

    /// <summary>
    ///  Рассчитывает прямую траекторию между двумя клетками
    /// </summary>
    /// <param name="start">Начало траектории</param>
    /// <param name="finish">Коонец траектории</param>
    /// <param name="bf">Поле боя</param>
    /// <returns>Список клеток траектории</returns>
    public static List<Cell> CreateTraectory(Cell start, Cell finish, Battlefield bf)
    {
        Vector3 startPos = HexToCube(start.position);
        Vector3 finishPos = HexToCube(finish.position);
        float n = HexDistance(startPos, finishPos);
        List<Cell> traectory = new List<Cell>();
        for (int i = 0; i <= n; i++)
        {
            Vector2 pos = Round(CubeLerp(startPos, finishPos, 1.0f / n * i));
            traectory.Add(bf.FindCell(pos.x, pos.y));
        }
        return traectory;
    }

    //# region Операторы
    //public static bool operator ==(Cell a, Cell b)
    //{
    //    if (null = && null == b)
    //        return true;
    //    else if (null == a || null == b)
    //        return false;
    //    else
    //        return a.position.x == b.position.x && a.position.y == b.position.y;
    //}

    //public static bool operator !=(Cell a, Cell b)
    //{
    //    return !(a == b);
    //}
    //#endregion

    public static List<Cell> ReconstructPath(Cell start, Cell finish, Battlefield bf, Dictionary<Cell, Cell> cameFrom)
    {
        List<Cell> path = new List<Cell>();
        Cell current = finish;
        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }
}
