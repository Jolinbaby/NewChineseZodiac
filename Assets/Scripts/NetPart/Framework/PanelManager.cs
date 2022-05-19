using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager
{
    //Layer
    public enum Layer
    {
        Panel,
        Tip,
    }
    //层级列表
    private static Dictionary<Layer, Transform> layers = new Dictionary<Layer, Transform>();
    //面板列表
    public static Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();
    //结构
    public static Transform root;
    public static Transform canvas;
    //初始化
    public static void Init()
    {
        root = GameObject.Find("Root").transform;  //找到场景当中的对应物体
        canvas = root.Find("Canvas");
        Transform panel = canvas.Find("Panel");
        Transform tip = canvas.Find("Tip");
        layers.Add(Layer.Panel, panel);
        layers.Add(Layer.Tip, tip);
    }

    //打开面板
    public static void Open<T>(params object[] para) where T : BasePanel
    {
        //已经打开
        string name = typeof(T).ToString();
        if (panels.ContainsKey(name))
        {
            return;
        }
        //组件
        BasePanel panel = root.gameObject.AddComponent<T>();
        panel.OnInit(); //调用新生成的面板类的方法,OnInit由面板来实现,Init在面板基类当中实现,会根据面板的skinPath将面板资源实例化到场景中.
        panel.Init(); //注意这里调用的是BasePanel的Init方法,不是这里的Init方法
                      //父容器
        Transform layer = layers[panel.layer];
        panel.skin.transform.SetParent(layer, false); //关于SetParent的介绍如下:https://docs.unity.cn/cn/current/ScriptReference/Transform.SetParent.html
                                                      //这里是false表示放弃原来的世界坐标位置,以layer为父节点                                              

        //列表
        panels.Add(name, panel);
        //OnShow
        panel.OnShow(para);
    }

    //关闭面板
    public static void Close(string name)
    {
        //没有打开
        if (!panels.ContainsKey(name))
        {
            return;
        }
        BasePanel panel = panels[name];

        //OnClose
        panel.OnClose();
        //列表
        panels.Remove(name);
        //销毁
        GameObject.Destroy(panel.skin);
        Component.Destroy(panel);
    }
}
