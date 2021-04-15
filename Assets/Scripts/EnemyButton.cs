using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Кнопка врага
/// </summary>
public class EnemyButton : MonoBehaviour
{
    /// <summary>
    /// Прикрепленный враг
    /// </summary>
    public Enemy enemy;

    /// <summary>
    /// Обработчик нажатия
    /// </summary>
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
