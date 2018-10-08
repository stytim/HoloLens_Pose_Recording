using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using System;

public class SyncCount : MonoBehaviour
{
    MyMessage customMessage;
    public Text usertext;
    
    void Start()
    {
        customMessage = MyMessage.Instance;

        customMessage.MessageHandlers[MyMessage.CustomMessageID.Count] = OnCountReceived;
    }

    #region On Message Received
    private void OnCountReceived(NetworkInMessage msg)
    {
        //Debug.Log("On Received");
        int count = MyMessage.ReadCount(msg);
        // display
        usertext.text = count.ToString();
        Debug.Log(count);
    }
    #endregion // On Message Received


    #region Send Message
    public void SendCurrentCount()
    {
        customMessage.SendCount(MessageReliability.UnreliableSequenced);

    }

    
    #endregion //SendMessage


    #region Input and Update
    void Update()
    {
        

    }

    #endregion
}