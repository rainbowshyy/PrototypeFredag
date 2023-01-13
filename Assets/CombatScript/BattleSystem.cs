using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Anim}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogueBox dialogueBox;

    BattleState state;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogueBox.SetMoveNames(playerUnit.Pokemon.Moves);

        dialogueBox.SetDialogue($"A wild {enemyUnit.Pokemon.Base.Name} appeared");
        yield return new WaitForSeconds(2.5f);

        PlayerAction();
    }

    public void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogueBox.SetDialogue($"What should {playerUnit.Pokemon.Base.Name} do?");
        dialogueBox.EnableActionSelector(true);
        dialogueBox.EnableDialogueText(true);
        dialogueBox.EnableMoveSelector(false);
    }

    public void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableMoveSelector(true);
        HoverMove(0);
    }

    public void HoverMove(int index)
    {
        if (index < playerUnit.Pokemon.Moves.Count)
            dialogueBox.UpdateMoveSelection(playerUnit.Pokemon.Moves[index]);
    }

    public void DoPlayerMove(int index)
    {
        StartCoroutine(PerformPlayerMove(index));
    }

    IEnumerator PerformPlayerMove(int index)
    {
        state = BattleState.Anim;
        dialogueBox.Busy();
        var move = playerUnit.Pokemon.Moves[index];
        dialogueBox.SetDialogue($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(2f);
        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        enemyHud.UpdateHP();
        if (isFainted)
        {
            dialogueBox.SetDialogue($"{enemyUnit.Pokemon.Base.Name} fainted");
            yield return new WaitForSeconds(2f);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        dialogueBox.SetDialogue($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(2f);
        bool isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        playerHud.UpdateHP();
        if (isFainted)
        {
            dialogueBox.SetDialogue($"{playerUnit.Pokemon.Base.Name} fainted");
            yield return new WaitForSeconds(2f);
        }
        else
        {
            PlayerAction();
        }
    }
}
