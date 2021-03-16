using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 position;
    public Unit unit;
    public Obstacle ostacle;

    private static Vector3 HexToCube(Vector2 hex)
    {
        Vector3 res = new Vector3();
        res.x = hex.x;
        res.z = hex.y;
        res.y = -res.x - res.z;
        return res;
    }

    private static Vector2 CubeToHex(Vector3 cube)
    {
        return new Vector2(cube.x, cube.z);
    }

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

    private static Vector3 CubeLerp(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, t),
                           Mathf.Lerp(a.y, b.y, t),
                           Mathf.Lerp(a.z, b.z, t));
    }

    private static float HexDistance(Vector3 a, Vector3 b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
    }

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
}
