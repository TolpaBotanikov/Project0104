using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyButton : MonoBehaviour
{
    public Enemy enemy;

    public void EnemyBtnClick()
    {
        if(!enemy.Selected)
        {
            enemy.Selected = true;
            if (BattleManager.S.selectedEnemy != null)
                BattleManager.S.selectedEnemy.Selected = false;
            BattleManager.S.selectedEnemy = enemy;
        }
        else
        {
            BattleManager.S.AttackUnit(enemy);
            BattleManager.S.selectedEnemy = null;
        }
    }
}
