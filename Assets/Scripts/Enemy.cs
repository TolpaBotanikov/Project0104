using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Класс для работы с врагами
/// </summary>
public class Enemy : Unit
{
    private bool _selected;

    public bool Selected 
    {
        get { return _selected; }
        set
        {
            _selected = value;
           if (_selected)
                bf.FindCell(Position).gameObject.GetComponent<Renderer>().
                        material.color = Color.blue;
           else
                bf.FindCell(Position).gameObject.GetComponent<Renderer>().
                        material.color = Color.white;
        }
    }

    public void MakeMove()
     {
        if(Cell.Distance(bf.hero.Position, Position) <= weapon.range && 
            BattleManager.S.CalculateHitChance(weapon.hitChance, Position, bf.hero.Position) > 0)
        {
            BattleManager.S.AttackUnit(bf.hero);
        }
        else
        {
            AStarSearch path = new AStarSearch(bf, bf.FindCell(Position), bf.FindCell(bf.hero.Position));
            Cell target = Cell.ReconstructPath(bf.FindCell(Position), bf.FindCell(bf.hero.Position), bf, path.cameFrom)[1];
            if (target.unit == null)
                GoToCell(target);
            else
                BattleManager.S.SwitchUnit();
        }

    }
}
