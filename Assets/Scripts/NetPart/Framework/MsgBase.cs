using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgBase
{
    public string protoName = "null";
    //����
    public static byte[] Encode(MsgBase msgBase)
    {
        string s = JsonUtility.ToJson(msgBase);
        return System.Text.Encoding.UTF8.GetBytes(s);
    }

    //����
    public static MsgBase Decode(string protoName, byte[] bytes, int offset, int count)
    {
        string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
        //Debug.Log("Debug decode:" + s);
        MsgBase msgBase = (MsgBase)JsonUtility.FromJson(s, Type.GetType(protoName));
        return msgBase;
    }

    //����Э������2�ֽڳ���+�ַ�����
    public static byte[] EncodeName(MsgBase msgBase)
    {
        //����bytes�ͳ���
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
        Int16 len = (Int16)nameBytes.Length;
        //����bytes��ֵ
        byte[] bytes = new byte[2 + len];
        //��װ2�ֽڵĳ�����Ϣ
        bytes[0] = (byte)(len % 256);
        bytes[1] = (byte)(len / 256);
        //��װ����bytes
        Array.Copy(nameBytes, 0, bytes, 2, len);

        return bytes;
    }

    //����Э������2�ֽڳ���+�ַ�����
    public static string DecodeName(byte[] bytes, int offset, out int count)
    {
        count = 0;
        //�������2�ֽ�
        if (offset + 2 > bytes.Length)
        {
            return "";
        }
        //��ȡ����
        Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
        //���ȱ����㹻
        if (offset + 2 + len > bytes.Length)
        {
            return "";
        }
        //����
        count = 2 + len;
        string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
        return name;
    }
}
