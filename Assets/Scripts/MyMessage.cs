using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class MyMessage : Singleton<MyMessage>
{
    GameObject CountObject;
    int CountComp;
    public NetworkConnection serverConnection;

    NetworkConnectionAdapter connectionAdapter;

    public enum CustomMessageID : byte
    {
        Count = MessageID.UserMessageIDStart,
        Max
    }

    public delegate void MessageCallback(NetworkInMessage msg);

    public Dictionary<CustomMessageID, MessageCallback> MessageHandlers { get; private set; }

    public long LocalUserID { get; private set; }

    protected override void Awake()
    {
        
        base.Awake();

        // initialize message handler dictionary
        MessageHandlers = new Dictionary<CustomMessageID, MessageCallback>();
        for (byte index = (byte)MessageID.UserMessageIDStart; index < (byte)CustomMessageID.Max; index++)
        {
            if (!MessageHandlers.ContainsKey((CustomMessageID)index))
            {
                MessageHandlers.Add((CustomMessageID)index, null);
            }
        }

        CountObject = GameObject.Find("Manager");
        
    }

    void Start()
    {
        SharingStage.Instance.SharingManagerConnected += Instance_SharingManagerConnected;
    }

    private void Instance_SharingManagerConnected(object sender, System.EventArgs e)
    {
        // initialization
        InitializeMessageHandlers();
    }

    private void InitializeMessageHandlers()
    {
        SharingStage sharingStage = SharingStage.Instance;

        if (sharingStage == null)
        {
            return;
        }
        serverConnection = sharingStage.Manager.GetServerConnection();
        if (serverConnection == null)
        {
            return;
        }

        connectionAdapter = new NetworkConnectionAdapter();
        connectionAdapter.MessageReceivedCallback += ConnectionAdapter_MessageReceivedCallback;

        LocalUserID = sharingStage.Manager.GetLocalUser().GetID();

        for (byte index = (byte)MessageID.UserMessageIDStart; index < (byte)CustomMessageID.Max; index++)
        {
            serverConnection.AddListener(index, connectionAdapter);
        }
    }

    private void ConnectionAdapter_MessageReceivedCallback(NetworkConnection connection, NetworkInMessage msg)
    {
        byte messageType = msg.ReadByte();
        MessageCallback messageHandler = MessageHandlers[(CustomMessageID)messageType];
        if (messageHandler != null)
        {
            messageHandler(msg);
        }
    }

    protected override void OnDestroy()
    {
        if (serverConnection != null)
        {
            for (byte index = (byte)MessageID.UserMessageIDStart; index < (byte)CustomMessageID.Max; index++)
            {
                serverConnection.RemoveListener(index, connectionAdapter);
            }
            connectionAdapter.MessageReceivedCallback -= ConnectionAdapter_MessageReceivedCallback;
        }

        base.OnDestroy();
    }

    private NetworkOutMessage CreateMessage(byte messageType)
    {
        NetworkOutMessage msg = serverConnection.CreateMessage(messageType);
        msg.Write(messageType);
        msg.Write(LocalUserID);
        return msg;
    }

    #region SendMessage

    public void SendCount(MessageReliability? reliability = MessageReliability.ReliableOrdered)
    {
        if (serverConnection != null && serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)CustomMessageID.Count);

            CountComp = CountObject.GetComponent<GetTransform>().count;
            msg.Write(CountComp);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            serverConnection.Broadcast(msg,
                MessagePriority.Immediate,
                reliability.Value,
                MessageChannel.Default); // default channel
        }
    }

    #endregion //SendMessage

    #region ReadMessage
    public static int ReadCount(NetworkInMessage msg)
    {
        // read userID, not used
        msg.ReadInt64();

        int count = msg.ReadInt32();

        return count;
    }

    #endregion //readMessage
}