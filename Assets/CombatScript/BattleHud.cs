using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HpBar hpBar;

    Pokemon pokemon;

    public void SetData(Pokemon pokemonParam)
    {
        pokemon = pokemonParam;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;
        UpdateHP();
    }

    public void UpdateHP()
    {
        hpBar.SetHP((float)pokemon.HP / (float)pokemon.MaxHp);
    }
}
