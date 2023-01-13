using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase pokeBase;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    public void Setup()
    {
        Pokemon =  new Pokemon(pokeBase, level);
        if (isPlayerUnit)
        {
            Debug.Log(GetComponent<Image>());
            Debug.Log(Pokemon.Base.BackSprite);
            GetComponent<Image>().sprite = Pokemon.Base.BackSprite;
        }
        else
            GetComponent<Image>().sprite = Pokemon.Base.FrontSprite;
    }
}
