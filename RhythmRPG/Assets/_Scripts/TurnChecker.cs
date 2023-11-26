using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private OneTurn turn;

    private List<TurnOneTick> turnTicks;
    private float timeStart;
    private float timeAll;
    private bool isPlaying;
    private int playIndex;
    private float nextCeckingTime;
    private float offsetCeckingTime;
    private List<TurnOneTickUI> ui;
    private Dictionary<int, CommandStackPiece> commands;

    private static readonly float inputOffset = 0.07f;
    private static readonly List<string> clockwiseFromUpString = new List<string>{ "Up", "Right", "Down", "Left" };

    public bool turnEnd => !isPlaying && offsetCeckingTime < 0;

    private void Awake()
    {
        offsetCeckingTime = -1f;
        isPlaying = false;
        commands = new Dictionary<int, CommandStackPiece>();
        SetEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnStart(turn);
    }

    // Update is called once per frame
    void Update()
    {
        offsetCeckingTime -= Time.deltaTime;
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
                    return;
                }

                nextCeckingTime += turnTicks[playIndex].waitTime;
                offsetCeckingTime = inputOffset;

                if (commands.ContainsKey(playIndex - 1) &&
                    commands[playIndex - 1].push)
                {
                    commands.Add(playIndex, new CommandStackPiece(playIndex, commands[playIndex - 1].groundIndex, true));
                }
            }

            if (ui.Count > playIndex &&
                commands.ContainsKey(playIndex - 1))
            {
                if (commands[playIndex - 1].push)
                    ui[playIndex].ratio = (turnTicks[playIndex].waitTime + timeStart - nextCeckingTime) / turnTicks[playIndex].waitTime;
                else
                    ui[playIndex].ratio = 0;
            }
        }
        else if(offsetCeckingTime < 0)
        {
            RemoveUI();
        }
    }

    public void TurnStart(OneTurn turn)
    {
        turnTicks = turn.ticks;
        MakeTurnUI();
        timeStart = 0;
        playIndex = 0;
        nextCeckingTime = turnTicks[0].waitTime;
        commands.Clear();
        audioSource.PlayOneShot(turnStartSound);

        isPlaying = true;
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
            InputHandler.holdAbleOnEvent[button] = new List<System.Action>() { () => ButtonPress(button) };
            InputHandler.holdAbleOffEvent[button] = new List<System.Action>() { () => ButtonUp(button) };
        }
    }

    private void ButtonPress(string button)
    {
        int turnTickIndex = playIndex;
        if (offsetCeckingTime > 0)
            --turnTickIndex;

        if (timeStart > (nextCeckingTime - inputOffset) ||
            offsetCeckingTime > 0)
        {
            int index = clockwiseFromUpString.FindIndex(x => x == button);
            if (commands.ContainsKey(turnTickIndex))
            {
                commands[turnTickIndex].groundIndex = index;
                commands[turnTickIndex].push = true;
            }
            else
            {
                commands.Add(turnTickIndex, new CommandStackPiece(turnTickIndex, index, true));
            }

            if((turnTickIndex + 1) < ui.Count)
            {
                ui[turnTickIndex + 1].buttonColor = baseGroundsClockwiseFromUp[index].color;
            }
        }
    }
    private void ButtonUp(string button)
    {
        int turnTickIndex = playIndex;
        if (offsetCeckingTime > 0)
            --turnTickIndex;


        if (timeStart > (nextCeckingTime - inputOffset) ||
            offsetCeckingTime > 0)
        {
            int index = clockwiseFromUpString.FindIndex(x => x == button); 
            if (commands.ContainsKey(turnTickIndex) &&
                commands[turnTickIndex].groundIndex == index)
            {
                commands[turnTickIndex].push = false;
            }
            else if (commands.ContainsKey(turnTickIndex - 1) &&
                commands[turnTickIndex - 1].groundIndex == index)
            {
                commands.Add(turnTickIndex - 1, new CommandStackPiece(turnTickIndex - 1, index, false));
            }

            if ((turnTickIndex + 1) < ui.Count)
            {
                ui[turnTickIndex + 1].ratio = 0;
            }
        }
    }
}

public class CommandStackPiece
{
    public int timeIndex;
    // ClockwiseFromUp >> "Up", "Right", "Down", "Left"
    public int groundIndex;
    public bool push;

    public CommandStackPiece(int _timeIndex, int _groundIndex, bool _push)
    {
        timeIndex = _timeIndex;
        groundIndex = _groundIndex;
        push = _push;
    }
}