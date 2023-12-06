using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField]
    private MenuHandler menuHandler;
    [SerializeField]
    private GameObject stageButtonPrefab;
    [SerializeField]
    private Transform selects;

    private static readonly string buttonMakeSql = "SELECT Stage.StageIndex, Character.GraphicIndex, Character.ColorR, Character.ColorG, Character.ColorB FROM Stage, Character WHERE Character.Name = Stage.Enemy";

    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        SqlAccess gameDB = GameDataManager.GetGameDB();
        Dictionary<int, StageButton> buttons = new Dictionary<int, StageButton>();
        menuHandler.enabled = true;

        gameDB.SqlRead("SELECT StageIndex, Name FROM Stage");
        if (gameDB.read)
        {
            while (gameDB.dataReader.Read())
            {
                StageButton button = ObjectPoolingManager.Pooling(stageButtonPrefab).GetComponent<StageButton>();
                button.transform.SetParent(selects);
                button.ResetUI();
                button.text = gameDB.dataReader.GetString(1);
                buttons.Add((int)gameDB.dataReader.GetDecimal(0), button);
                MenuButton buttonUI = button.gameObject.GetComponent<MenuButton>();
                buttonUI.events.AddListener(() => {
                    gameObject.SetActive(false);
                });
                menuHandler.buttonAdd = buttonUI;
            }
        }

        string[] clockwise = { "Up", "Back", "Down", "Front" };
        for(int i = 0; i < 4; ++i)
        {
            gameDB.SqlRead(buttonMakeSql + clockwise[i]);
            while (gameDB.dataReader.Read())
            {
                buttons[(int)gameDB.dataReader.GetDecimal(0)].SetGraphic(i,
                    new Color(gameDB.dataReader.GetFloat(2), gameDB.dataReader.GetFloat(3), gameDB.dataReader.GetFloat(4)),
                    ObjectPoolingManager.Pooling(GameStatics.instance.characterIcons[(int)gameDB.dataReader.GetDecimal(1)]));
            }
        }

    }
}
