using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class TurnChecker : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject turnTickUI;
    [SerializeField]
    private RectTransform turnWrapper;
    [SerializeField]
    private RectTransform pointer;
    [SerializeField]
    private List<BaseGrounds> baseGroundsClockwiseFromUp;
    [SerializeField]
    private AudioClip turnStartSound;

    private List<TurnOneTick> turnTicks;
    private List<float> turnTimes;
    private float timeStart;
    private float timeAll;
    private bool isPlaying;
    private int playIndex;
    private int checkIndex;
    private float nextCeckingTime;
    private float offsetCeckingTime;
    private List<TurnOneTickUI> ui;
    private Dictionary<int, CommandStackPiece> commands;

    private static readonly float inputOffset = 0.07f;
    private static readonly List<string> clockwiseFromUpString = new List<string>{ "Up", "Right", "Down", "Left" };

    public bool turnEnd => !isPlaying && offsetCeckingTime < 0;

    private void Awake()
    {
        offsetCeckingTime = inputOffset;
        isPlaying = false;
        commands = new Dictionary<int, CommandStackPiece>();
        turnTimes = new List<float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetEvent();
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timeStart += Time.deltaTime;
            SetIconAnchor();

            if (timeStart > nextCeckingTime)
            {
                audioSource.PlayOneShot(turnTicks[playIndex].metronomeClip);
                if (turnTicks.Count <= ++playIndex)
                {
                    isPlaying = false;
                    offsetCeckingTime = inputOffset;
                    return;
                }

                nextCeckingTime = turnTimes[playIndex];

                if (commands.ContainsKey(checkIndex) &&
                    playIndex < ui.Count)
                {
                    ui[playIndex].barColor = baseGroundsClockwiseFromUp[commands[playIndex - 1].groundIndex].color;
                }
            }

            if (timeStart > (turnTimes[checkIndex] + inputOffset))
            {
                if (commands.ContainsKey(checkIndex) &&
                            commands[checkIndex].push)
                {
                    commands.Add(checkIndex + 1, new CommandStackPiece(checkIndex + 1, commands[checkIndex].groundIndex, true));
                }

                ++checkIndex;
            }

            if (ui.Count > playIndex &&
            commands.ContainsKey(playIndex - 1) &&
            commands[playIndex - 1].push)
            {
                ui[playIndex].ratio = (turnTicks[playIndex].waitTime + timeStart - nextCeckingTime) / turnTicks[playIndex].waitTime;
            }
        }
        else if (offsetCeckingTime > 0)
        {
            timeStart += Time.deltaTime;
            offsetCeckingTime -= Time.deltaTime;
        }
        else
        {
            //foreach (KeyValuePair<int, CommandStackPiece> command in commands)
            //{
            //    Debug.Log(command.Value.timeIndex + ", " + baseGroundsClockwiseFromUp[command.Value.groundIndex] + " " + command.Value.push);
            //}

            RemoveUI();
            enabled = false;

            ReadCommand(commands, turnTicks.Count);
        }
    }

    public void TurnStart(List<TurnOneTick> ticks)
    {
        enabled = true;
        MakeTurnTime(ticks);
        MakeTurnUI();
        timeStart = 0;
        playIndex = 0;
        checkIndex = 0;
        nextCeckingTime = turnTimes[0];
        commands.Clear();
        audioSource.PlayOneShot(turnStartSound);

        isPlaying = true;
    }

    private void MakeTurnTime(List<TurnOneTick> ticks)
    {
        turnTicks = ticks;
        turnTimes.Clear();

        float time = 0f;
        foreach(TurnOneTick tick in turnTicks)
        {
            time += tick.waitTime;
            turnTimes.Add(time);
        }
    }

    private void MakeTurnUI()
    {
        ui = new List<TurnOneTickUI>();
        timeAll = 0;
        timeStart = 0;

        foreach (TurnOneTick turn in turnTicks)
            timeAll += turn.waitTime;

        foreach (TurnOneTick turn in turnTicks)
        {
            ui.Add(Instantiate(turnTickUI).GetComponent<TurnOneTickUI>());
            RectTransform trans = ui[ui.Count - 1].GetComponent<RectTransform>();
            trans.SetParent(turnWrapper);
            trans.anchorMin = new Vector2(timeStart / timeAll, 0);
            trans.anchorMax = new Vector2((timeStart + turn.waitTime) / timeAll, 1);
            trans.offsetMax = Vector2.zero;
            trans.offsetMin = Vector2.zero;

            timeStart += turn.waitTime;
        }
    }

    private void SetIconAnchor()
    {
        float posX = Mathf.Clamp(timeStart / timeAll, 0, 1);
        pointer.anchorMin = new Vector2(posX, -0.4f);
        pointer.anchorMax = new Vector2(posX, 0);
        pointer.offsetMax = Vector2.zero;
        pointer.offsetMin = Vector2.zero;
    }

    private void RemoveUI()
    {
        if (ui != null && ui.Count > 0)
        {
            foreach (TurnOneTickUI oneUI in ui)
                Destroy(oneUI.gameObject);

            ui.Clear();
        }

        timeStart = 0f;
        SetIconAnchor();
    }

    private void SetEvent()
    {
        foreach (string button in clockwiseFromUpString)
        {
            if (InputHandler.holdAbleOnEvent.ContainsKey(button))
                InputHandler.holdAbleOnEvent[button].Add(() => ButtonPress(button));
            else
                InputHandler.holdAbleOnEvent.Add(button, new List<System.Action>() { () => ButtonPress(button) });

            if (InputHandler.holdAbleOffEvent.ContainsKey(button))
                InputHandler.holdAbleOffEvent[button].Add(() => ButtonUp(button));
            else
                InputHandler.holdAbleOffEvent.Add(button, new List<System.Action>() { () => ButtonUp(button) });
        }
    }

    private void ButtonPress(string button)
    {
        if (!enabled)
            return;

        if (Math.Abs(timeStart - turnTimes[checkIndex]) <= inputOffset)
        {
            int index = clockwiseFromUpString.FindIndex(x => x == button);
            if (!commands.ContainsKey(checkIndex))
            {
                commands.Add(checkIndex, new CommandStackPiece(checkIndex, index, true));
            }
            else
            {
                commands[checkIndex].push = false;
                ui[checkIndex].push = false;
                return;
            }

            if ((checkIndex + 1) < ui.Count)
            {
                ui[checkIndex + 1].barColor = baseGroundsClockwiseFromUp[index].color;
            }
            if (checkIndex > -1 && checkIndex < ui.Count)
            {
                ui[checkIndex].arrowColor = baseGroundsClockwiseFromUp[index].color;
                ui[checkIndex].push = true;
            }
        }
    }
    private void ButtonUp(string button)
    {
        if (!enabled)
            return;

        int index = clockwiseFromUpString.FindIndex(x => x == button);
        int tatgetIndex = checkIndex;
        if (timeStart < (turnTimes[checkIndex] - inputOffset))
        {
            if (commands.ContainsKey(tatgetIndex))
                commands.Remove(tatgetIndex);

            --tatgetIndex;
        }

        if (commands.ContainsKey(tatgetIndex) &&
        commands[tatgetIndex].groundIndex == index)
        {
            commands[tatgetIndex].groundIndex = index;
            commands[tatgetIndex].push = false;
            
            if (tatgetIndex < ui.Count)
            {
                ui[tatgetIndex].push = false;
                ui[tatgetIndex].arrowColor = baseGroundsClockwiseFromUp[commands[tatgetIndex].groundIndex].color;
            }
        }
    }

    public List<OneCommand> ReadCommand(Dictionary<int, CommandStackPiece> commands, int turnTicksCount)
    {
        List<OneCommand> commandRead = new List<OneCommand>();
        bool pushed = false;
        for (int i = 0; i < turnTicksCount; i++)
        {
            if (commands.ContainsKey(i))
            {
                if (commandRead.Count > 0 &&
                    commands[i].groundIndex == commandRead[commandRead.Count - 1].groundIndex &&
                    pushed)
                    ++commandRead[commandRead.Count - 1].chargeCount;
                else
                    commandRead.Add(new OneCommand(commands[i].groundIndex));

                pushed = commands[i].push;
            }
            else if ((i - 1) > -1 && pushed)
                ++commandRead[commandRead.Count - 1].chargeCount;
            else
                commandRead.Add(new OneCommand(-1));
        }

        return commandRead;
        //foreach (OneCommand command in commandRead)
        //{
        //    if(command.groundIndex > -1)
        //        Debug.Log(clockwiseFromUpString[command.groundIndex] + ", " + command.chargeCount);
        //}
    }
}

public class CommandStackPiece
{
    public int timeIndex;
    public int groundIndex;
    public bool push;

    public CommandStackPiece(int _timeIndex, int _groundIndex, bool _push)
    {
        timeIndex = _timeIndex;
        groundIndex = _groundIndex;
        push = _push;
    }
}