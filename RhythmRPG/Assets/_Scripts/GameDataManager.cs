using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class GameDataManager
{
    public static string[] playerClockwiseFromUp = { "", "", "", "" };
    public static string[] enemyClockwiseFromUp = { "", "", "", "" };
    public static Dictionary<string, List<ActionEffects>> effects;
    public static Dictionary<string, CharacterAction> actions;
    public static List<string> saveDatas;
    public static string stageNow;

    public static PositionChangeMethod GetPositionChangeMethod(string index)
    {
        switch (index)
        {
            case "Clockwise":
                return PositionChangeMethod.Clockwise;
            case "CounterClockwise":
                return PositionChangeMethod.CounterClockwise;
            case "FrontBack":
                return PositionChangeMethod.FrontBack;
            case "UpDown":
                return PositionChangeMethod.UpDown;
            case "None":
                return PositionChangeMethod.None;
        }
        return PositionChangeMethod.None;
    }
    public static Effects GetEffects(string index)
    {
        switch (index)
        {
            case "Heal":
                return Effects.Heal;
            case "ManaUp":
                return Effects.ManaUp;
            case "AttackUp":
                return Effects.AttackUp;
            case "DefenceUp":
                return Effects.DefenceUp;
            case "HpUp":
                return Effects.HpUp;
            case "ManaCharge":
                return Effects.ManaCharge;
            case "Dispel":
                return Effects.Dispel;
            case "BuffUp":
                return Effects.BuffUp;
            case "Taunt":
                return Effects.Taunt;
            case "Pierce":
                return Effects.Pierce;
            case "DefenceDown":
                return Effects.DefenceDown;
            case "AttackDown":
                return Effects.AttackDown;
            case "Doom":
                return Effects.Doom;
            case "ManaBurn":
                return Effects.ManaBurn;
            case "HpDown":
                return Effects.HpDown;
            case "ManaDown":
                return Effects.ManaDown;
            case "Deal":
                return Effects.Deal;
            case "Faint":
                return Effects.Faint;
            case "Silence":
                return Effects.Silence;
            case "LockOn":
                return Effects.LockOn;
            case "Exorcist":
                return Effects.Exorcist;
            case "Kill":
                return Effects.Kill;
            case "DebuffUp":
                return Effects.DebuffUp;
            case "BuffBreak":
                return Effects.BuffBreak;
        }
        return Effects.Deal;
    }
    public static Targeting GetPTargeting(string index)
    {
        switch (index)
        {
            case "Front":
                return Targeting.Front;
            case "Back":
                return Targeting.Back;
            case "Marking":
                return Targeting.Marking;
            case "FrontMany":
                return Targeting.FrontMany;
            case "BackMany":
                return Targeting.BackMany;
            case "All":
                return Targeting.All;
            case "Up":
                return Targeting.Up;
            case "Down":
                return Targeting.Down;
            case "None":
                return Targeting.None;
        }
        return Targeting.None;
    }

    private static readonly string saveDataPath = Path.Combine(Application.streamingAssetsPath, "PlayerData.db");
    private static readonly string gameDataPath = Path.Combine(Application.streamingAssetsPath, "GameDB.db");
    private static string idNow;
    private static bool gameDataCheck = false;

    public static bool savedataExist
    {
        get
        {
            if (!File.Exists(saveDataPath))
                MakeNewSave();

            if (saveDatas != null)
                saveDatas.Clear();
            else
                saveDatas = new List<string>();

            SqlAccess sql = SqlAccess.GetAccess(Application.streamingAssetsPath + "/" + "PlayerData.db");
            sql.Open();
            sql.SqlRead("SELECT COUNT (*) FROM Player;");
            if (sql.read && sql.dataReader.Read() && sql.dataReader.GetDecimal(0) > 0)
            {
                sql.SqlRead("SELECT Name FROM Player;");
                while (sql.read && sql.dataReader.Read())
                    saveDatas.Add(sql.dataReader.GetString(0));

                sql.ShutDown();
                return true;
            }

            sql.ShutDown();
            return false;
        }
    }

    private static void MakeNewSave()
    {
        File.Copy(AssetDatabase.GetAssetPath(Resources.Load("PlayerDataOrigin")), saveDataPath);
    }

    public static void LoadPlayer(string id)
    {
        idNow = id;
        SqlAccess sql = SqlAccess.GetAccess(saveDataPath);
        sql.Open();
        sql.SqlRead($"SELECT COUNT (Name) FROM Player WHERE Name = '{id}';");

        if(!(sql.read && sql.dataReader.Read() && sql.dataReader.GetDecimal(0) > 0))
        {
            sql.SqlExecute($"INSERT INTO Player(Name) VALUES ('{id}')");
            AddCharacter(id, "TestPlayer", sql);
        }

        sql.ShutDown();
    }

    public static void LoadDB()
    {

    }

    //public static void AddCharacter(string id, string character)
    //{
    //    SqlAccess sql = SqlAccess.GetAccess(Application.streamingAssetsPath + "/" + "PlayerData.db");
    //    sql.Open();
    //    AddCharacter(id, character, sql);
    //    sql.ShutDown();
    //}
    public static void AddCharacter(string id, string character, SqlAccess sql)
    {
        int index = 0;
        sql.SqlRead($"SELECT MAX (BarrackIndex) FROM Barracks WHERE PlayerName = '{id}';");
        if(sql.read && sql.dataReader.Read() && !sql.dataReader.IsDBNull(0))
            index = (int)sql.dataReader.GetDecimal(0) + 1;

        sql.SqlExecute($"INSERT INTO Barracks(PlayerName, BarrackIndex, CharacterName, Exp, level) VALUES ('{id}', {index}, '{character}', 0, 1)");
    }

    public static SqlAccess GetGameDB()
    {
        if(File.Exists(gameDataPath) && !gameDataCheck)
            File.Delete(gameDataPath);

        File.Copy(AssetDatabase.GetAssetPath(Resources.Load("GameDB")), gameDataPath);
        SqlAccess gameAccess = SqlAccess.GetAccess(gameDataPath);
        gameAccess.Open();

        return gameAccess;
    }
}

public enum PositionChangeMethod
{
    Clockwise = 0,
    CounterClockwise = 1,
    FrontBack = 2,
    UpDown = 3,
    None = 4
}
public enum Effects
{
    Heal = 0,
    ManaUp = 1,
    AttackUp = 2,
    DefenceUp = 3,
    HpUp = 4,
    ManaCharge = 5,
    Dispel = 6,
    BuffUp = 7,
    Taunt = 8,
    Pierce = 9,
    DefenceDown = 10,
    AttackDown = 11,
    Doom = 12,
    ManaBurn = 13,
    HpDown = 14,
    ManaDown = 15,
    Deal = 16,
    Faint = 17,
    Silence = 18,
    LockOn = 19,
    Exorcist = 20,
    Kill = 21,
    DebuffUp = 22,
    BuffBreak = 23
}
public enum Targeting
{
    Front = 0,
    Back = 1,
    Marking = 2,
    FrontMany = 3,
    BackMany = 4,
    All = 5,
    Up = 6,
    Down = 7,
    None = 8
}
