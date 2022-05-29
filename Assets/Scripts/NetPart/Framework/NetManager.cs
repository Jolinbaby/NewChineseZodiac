using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class NetManager
{
    //�����׽���
    static Socket socket;
    //���ջ�����
    static ByteArray readBuff;
    //д�����
    static Queue<ByteArray> writeQueue;
    //�Ƿ���������
    static bool isConnecting = false;
    //�Ƿ����ڹر�
    static bool isClosing = false;
    //��Ϣ�б�
    static List<MsgBase> msgList = new List<MsgBase>();
    //��Ϣ�б���
    static int msgCount = 0;
    //ÿһ��Update�������Ϣ��
    readonly static int MAX_MESSAGE_FIRE = 10;
    //�Ƿ���������
    public static bool isUsePing = true;
    //�������ʱ��
    public static int pingInterval = 30;
    //��һ�η���PING��ʱ��
    static float lastPingTime = 0;
    //��һ���յ�PONG��ʱ��
    static float lastPongTime = 0;

    //�¼�
    public enum NetEvent
    {
        ConnectSucc = 1,
        ConnectFail = 2,
        Close = 3,
    }
    //�¼�ί������
    public delegate void EventListener(String err);
    //�¼������б�
    private static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();
    //����¼�����
    public static void AddEventListener(NetEvent netEvent, EventListener listener)
    {
        //����¼�
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] += listener;
        }
        //�����¼�
        else
        {
            eventListeners[netEvent] = listener;
        }
    }
    //ɾ���¼�����
    public static void RemoveEventListener(NetEvent netEvent, EventListener listener)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] -= listener;
        }
    }
    //�ַ��¼�
    private static void FireEvent(NetEvent netEvent, String err)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent](err);
        }
    }


    //��Ϣί������
    public delegate void MsgListener(MsgBase msgBase);
    //��Ϣ�����б�
    private static Dictionary<string, MsgListener> msgListeners = new Dictionary<string, MsgListener>();
    //�����Ϣ����
    public static void AddMsgListener(string msgName, MsgListener listener)
    {
        //���
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName] += listener;
        }
        //����
        else
        {
            msgListeners[msgName] = listener;
        }
    }
    //ɾ����Ϣ����
    public static void RemoveMsgListener(string msgName, MsgListener listener)
    {
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName] -= listener;
        }
    }

    //�ַ���Ϣ
    private static void FireMsg(string msgName, MsgBase msgBase)
    {
        //Debug.Log("msgName=" + msgName);
        if (msgListeners.ContainsKey(msgName))
        {
            msgListeners[msgName](msgBase);
        }
    }


    //����
    public static void Connect(string ip, int port)
    {
        //״̬�ж�
        if (socket != null && socket.Connected)
        {
            Debug.Log("Connect fail, already connected!");
            return;
        }
        if (isConnecting)
        {
            Debug.Log("Connect fail, isConnecting");
            return;
        }
        //��ʼ����Ա
        InitState();
        //��������
        socket.NoDelay = true;
        //Connect
        isConnecting = true;
        socket.BeginConnect(ip, port, ConnectCallback, socket);
    }

    //��ʼ��״̬
    private static void InitState()
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        //���ջ�����
        readBuff = new ByteArray();
        //д�����
        writeQueue = new Queue<ByteArray>();
        //�Ƿ���������
        isConnecting = false;
        //�Ƿ����ڹر�
        isClosing = false;
        //��Ϣ�б�
        msgList = new List<MsgBase>();
        //��Ϣ�б���
        msgCount = 0;
        //��һ�η���PING��ʱ��
        lastPingTime = Time.time;
        //��һ���յ�PONG��ʱ��
        lastPongTime = Time.time;
        //����PONGЭ��
        if (!msgListeners.ContainsKey("MsgPong"))
        {
            AddMsgListener("MsgPong", OnMsgPong);
        }
    }

    //Connect�ص�
    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ ");
            FireEvent(NetEvent.ConnectSucc, "");
            isConnecting = false;
            //��ʼ����
            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx,
                                            readBuff.remain, 0, ReceiveCallback, socket);

        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail " + ex.ToString());
            FireEvent(NetEvent.ConnectFail, ex.ToString());
            isConnecting = false;
        }
    }


    //�ر�����
    public static void Close()
    {
        //״̬�ж�
        if (socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting)
        {
            return;
        }
        //���������ڷ���
        if (writeQueue.Count > 0)
        {
            isClosing = true;
        }
        //û�������ڷ���
        else
        {
            socket.Close();
            FireEvent(NetEvent.Close, "");
        }
    }

    //��������
    public static void Send(MsgBase msg)
    {
        //״̬�ж�
        if (socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting)
        {
            return;
        }
        if (isClosing)
        {
            return;
        }
        //���ݱ���
        byte[] nameBytes = MsgBase.EncodeName(msg);
        byte[] bodyBytes = MsgBase.Encode(msg);
        int len = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + len];
        //��װ����
        sendBytes[0] = (byte)(len % 256);
        sendBytes[1] = (byte)(len / 256);
        //��װ����
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        //��װ��Ϣ��
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
        //д�����
        ByteArray ba = new ByteArray(sendBytes);
        int count = 0;  //writeQueue�ĳ���
        lock (writeQueue)
        {
            writeQueue.Enqueue(ba);
            count = writeQueue.Count;
        }
        //send
        if (count == 1)
        {
            socket.BeginSend(sendBytes, 0, sendBytes.Length,
                0, SendCallback, socket);
        }
    }

    //Send�ص�
    public static void SendCallback(IAsyncResult ar)
    {

        //��ȡstate��EndSend�Ĵ���
        Socket socket = (Socket)ar.AsyncState;
        //״̬�ж�
        if (socket == null || !socket.Connected)
        {
            return;
        }
        //EndSend
        int count = socket.EndSend(ar);
        //��ȡд����е�һ������            
        ByteArray ba;
        lock (writeQueue)
        {
            ba = writeQueue.First();
        }
        //��������
        ba.readIdx += count;
        if (ba.length == 0)
        {
            lock (writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.First();
            }
        }
        //��������
        if (ba != null)
        {
            socket.BeginSend(ba.bytes, ba.readIdx, ba.length,
                0, SendCallback, socket);
        }
        //���ڹر�
        else if (isClosing)
        {
            socket.Close();
        }
    }



    //Receive�ص�
    public static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            //��ȡ�������ݳ���
            int count = socket.EndReceive(ar);
            readBuff.writeIdx += count;
            //�����������Ϣ
            OnReceiveData();
            //������������
            if (readBuff.remain < 8)
            {
                readBuff.MoveBytes();
                readBuff.ReSize(readBuff.length * 2);
            }
            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx,
                    readBuff.remain, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Receive fail" + ex.ToString());
        }
    }

    //���ݴ���
    public static void OnReceiveData()
    {
        //��Ϣ����
        if (readBuff.length <= 2)
        {
            return;
        }
        //��ȡ��Ϣ�峤��
        int readIdx = readBuff.readIdx;
        byte[] bytes = readBuff.bytes;
        Int16 bodyLength = (Int16)((bytes[readIdx + 1] << 8) | bytes[readIdx]);
        if (readBuff.length < bodyLength)
            return;
        readBuff.readIdx += 2;
        //����Э����
        int nameCount = 0;
        string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);
        if (protoName == "")
        {
            Debug.Log("OnReceiveData MsgBase.DecodeName fail");
            return;
        }
        readBuff.readIdx += nameCount;
        //����Э����
        int bodyCount = bodyLength - nameCount;
        MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
        readBuff.readIdx += bodyCount;
        readBuff.CheckAndMoveBytes();
        //��ӵ���Ϣ����
        lock (msgList)
        {
            msgList.Add(msgBase);
            msgCount++;
        }
        //������ȡ��Ϣ
        if (readBuff.length > 2)
        {
            OnReceiveData();
        }
    }

    //Update
    public static void Update()
    {
        MsgUpdate();
        PingUpdate();
    }

    //������Ϣ
    public static void MsgUpdate()
    {
        //�����жϣ�����Ч��
        if (msgCount == 0)
        {
            return;
        }
        //�ظ�������Ϣ
        for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
        {
            //��ȡ��һ����Ϣ
            MsgBase msgBase = null;
            lock (msgList)
            {
                if (msgList.Count > 0)
                {
                    msgBase = msgList[0];
                    msgList.RemoveAt(0);
                    msgCount--;
                }
            }
            //�ַ���Ϣ
            if (msgBase != null)
            {
                FireMsg(msgBase.protoName, msgBase);
            }
            //û����Ϣ��
            else
            {
                break;
            }
        }
    }

    //����PINGЭ��
    private static void PingUpdate()
    {
        //�Ƿ�����
        if (!isUsePing)
        {
            return;
        }
        //����PING
        if (Time.time - lastPingTime > pingInterval)
        {
            MsgPing msgPing = new MsgPing();
            Send(msgPing);
            lastPingTime = Time.time;
        }
        //���PONGʱ��
        if (Time.time - lastPongTime > pingInterval * 40)
        {
            Debug.Log("I want to close----(Time.time - lastPongTime > pingInterval * 40");
            Close();
        }
    }

    //����PONGЭ��
    private static void OnMsgPong(MsgBase msgBase)
    {
        lastPongTime = Time.time;
    }
}
