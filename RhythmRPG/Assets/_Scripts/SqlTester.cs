using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqlTester : MonoBehaviour
{
    SqlAccess sql;

    // Start is called before the first frame update
    void Start()
    {
        sql = SqlAccess.GetAccess(Application.streamingAssetsPath + "/" + "test.db");
        sql.Open();
        sql.SqlRead("SELECT someindex, value FROM test;");
    }

    // Update is called once per frame
    void Update()
    {
        if(sql.read && sql.dataReader.Read())
        {
            Debug.Log($"read someindex({sql.dataReader.GetDecimal(0)}), value({sql.dataReader.GetDecimal(1)})");
            sql.SqlExecute($"INSERT INTO test(someindex, value) VALUES (10, 40)");
            Debug.Log($"add someindex({2}), value({21})");
            sql.ShutDown();
            enabled = false;
        }
    }
}
