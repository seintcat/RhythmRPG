using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> buttons;
    [SerializeField]
    private TMP_InputField newID;
    [SerializeField]
    private GameObject oneSaveDataPrefab;
    [SerializeField]
    private RectTransform saveDataTransform;
    [SerializeField]
    private MenuHandler saveDataUI;

    private Dictionary<string, SaveDataUIButton> saveDatas;

    public bool interactable
    {
        set
        {
            foreach (Image button in buttons)
                button.raycastTarget = value;

            newID.interactable = value;
        }
    }

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

    private void OnEnable()
    {

    }

    private void Init()
    {
        if(saveDatas == null)
            saveDatas = new Dictionary<string, SaveDataUIButton>();
        saveDatas.Clear();

        Color color = buttons[1].color;
        if (GameDataManager.savedataExist)
        {
            color.r = 1f;
            foreach(string id in GameDataManager.saveDatas)
            {
                saveDatas.Add(id, Instantiate(oneSaveDataPrefab).GetComponent<SaveDataUIButton>());
                saveDatas[id].transform.SetParent(saveDataTransform);
                saveDatas[id].events.AddListener(() => GameDataManager.LoadPlayer(id));
                saveDatas[id].text = id;
            }
        }
        else
            color.r = 0.8f;

        buttons[1].color = color;
    }


}
