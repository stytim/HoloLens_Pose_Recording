using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ClickInput : MonoBehaviour
{

    //private string TAG = "ClickInput";
    public int count = 0;
   // public Text status;
    [System.Serializable]
    public class EventClickerClick : UnityEvent { };
    public EventClickerClick eventClickerClick;
    public static ClickInput Instance { get; private set; }
    UnityEngine.XR.WSA.Input.GestureRecognizer recognizer;

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) => {
            // Debug.Log(TAG + ": clicker event triggered");
         //   status.text = count.ToString();
            count += 1;
            GetComponent<GetTransform>().count = count;
            eventClickerClick.Invoke();
        };
        recognizer.StartCapturingGestures();
    }

    // Use this for initialization
    void Start()
    {
        ;
    }

    // Update is called once per frame
    void Update()
    {
        ;
    }
}
