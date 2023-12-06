using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionEndChecker : MonoBehaviour
{
    [SerializeField]
    private UnityEvent animationEndEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndAnimation()
    {
        animationEndEvent.Invoke();
    }
}
