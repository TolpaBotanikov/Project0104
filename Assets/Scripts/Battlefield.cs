using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    [SerializeField]
    private List<Cell> cells = new List<Cell>();
    public List<Enemy> enemies = new List<Enemy>();
    public List<Cell> recoloredCells = new List<Cell>();

    private void Awake()
    {

    }

    public Cell FindCell(int x, int y)
    {
       return cells.Find(cell => cell.position.x == x && cell.position.y == y);
    }
}
