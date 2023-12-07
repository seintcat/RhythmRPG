using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatics : MonoBehaviour
{
    public static GameStatics instance;

    public List<AudioClip> clipIndex;
    public AudioClip defaultClip;
    public AudioClip startClip;

    public List<GameObject> characterDolls;
    public List<GameObject> characterIcons;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public GameObject MakeCharacterDoll(int graphicIndex)
    {
        CharacterDoll doll = ObjectPoolingManager.Pooling(characterDolls[graphicIndex]).GetComponent<CharacterDoll>();

        return doll.gameObject;
    }
}

public class Character_
{
    public string name {  get; private set; }
    public string description { get; private set; }
    public Color color { get; private set; }
    public int graphicIndex { get; private set; }

    public List<TurnOneTick> turn { get; private set; }

    private int expNow;
    public int expAdd 
    { 
        set 
        {  
            expNow += value; 
            if(expNow >= expMax && expMax > 0)
            {
                LevelUp();
            }
        } 
    }
    public int expMax { get; private set; }
    public int level { get; private set; }

    public int defence { get; private set; }
    public int guard { get; private set; }
    public int hpMax { get; private set; }
    public int manaMax { get; private set; }
    public int guardTime { get; private set; }
    public int exp { get; private set; }

    public string normalAction { get; private set; }
    public string skillAction { get; private set; }

    private void LevelUp()
    {
        expNow -= expMax;
        ++level;
    }
}

public class CharacterAction
{
    public string description { get; private set; }

    public int offensiveValue { get; private set; }
    public int defensiveValue { get; private set; }
    public int mana { get; private set; }
    public int stack { get; private set; }
    public PositionChangeMethod positionChangeMethod { get; private set; }
}

public class ActionEffects
{
    public int remain { get; private set; }
    public Targeting targetOption { get; private set; }
    public int amount { get; private set; }
}