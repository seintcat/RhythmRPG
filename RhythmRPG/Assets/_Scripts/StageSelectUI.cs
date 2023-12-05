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
        gameDB.SqlRead("SELECT Stage.Name, Character.ColorR, Character.ColorG, Character.ColorB, Character.GraphicIndex FROM Stage, Character WHERE Character.Name = Stage.EnemyFront");
    }
}
