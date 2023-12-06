using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> clockWiseStartUp;
    [SerializeField]
    private TextMeshProUGUI stageName;
    [SerializeField]
    private RectTransform rectTransform;

    public string text { set { stageName.text = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetUI()
    {
        foreach(RectTransform rectTransform in clockWiseStartUp)
        {
            for(int i = 0; i < rectTransform.childCount; ++i)
                rectTransform.GetChild(i).gameObject.SetActive(false);
        }

        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
        rectTransform.offsetMin = new Vector2(0, 0);
    }

    public void SetGraphic(int rectTransformIndex, Color color, GameObject icon)
    {
        icon.transform.SetParent(clockWiseStartUp[rectTransformIndex]);
        icon.GetComponent<Image>().color = color;
    }

    public void StageSelect()
    {
        GameDataManager.stageNow = stageName.text;
    }
}
