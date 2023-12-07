using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionManager : MonoBehaviour
{
    private readonly static string[] loadSQL = { 
        "SELECT Character.GraphicIndex, Character.ColorR, Character.ColorG, Character.ColorB FROM Stage, Character WHERE Stage.StageIndex = ", 
        " AND Character.Name = Stage.Enemy" };

    [SerializeField]
    private Transform[] clockwiseFromUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeStage(int index)
    {
        SqlAccess gameDB = GameDataManager.GetGameDB();
        for (int i = 0; i < 4; ++i)
        {
            gameDB.SqlRead(loadSQL[0] + index + loadSQL[1] + GameDataManager.enemyClockwiseFromUp[i]);
            if (gameDB.dataReader.Read())
            {
                CharacterDoll doll = ObjectPoolingManager.Pooling(GameStatics.instance.characterIcons[(int)gameDB.dataReader.GetDecimal(0)]).GetComponent<CharacterDoll>();
                doll.transform.SetParent(clockwiseFromUp[i]);
                doll.lookLeft = true;
                doll.color = new Color(gameDB.dataReader.GetFloat(1), gameDB.dataReader.GetFloat(2), gameDB.dataReader.GetFloat(3));
            }
        }
    }
}
