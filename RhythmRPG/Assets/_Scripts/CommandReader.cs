using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadCommand(Dictionary<int, CommandStackPiece> commands, int turnTicksCount)
    {
        List<OneCommand> commandRead = new List<OneCommand>();
        for (int i = 0; i < turnTicksCount; i++)
        {
            if (commands.ContainsKey(i))
            {
                commandRead.Add(new OneCommand(commands[i].groundIndex));
            }
            else if ((i - 1) > -1)
            {
                commandRead.Add(new OneCommand(-1));
            }
            else
                commandRead.Add(new OneCommand(-1));
        }
    }
}

public class OneCommand
{
    public int groundIndex;
    public int chargeCount;

    public OneCommand(int _groundIndex)
    {
        groundIndex = _groundIndex;
        chargeCount = 0;
    }
}