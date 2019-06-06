using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 写入TXT文本
/// </summary>
public class WriteTxt
{
    //存放路径
    private static string path = Application.streamingAssetsPath;
    //划分线
    private static string line = "--------------------------------------------------------------------------------";
    //大类的字符串数组
    private static string[] typeArr = new string[] { "其他", "地面/墙面", "门窗/吊顶", "柜类/床具", "沙发", "桌/几/台", "椅/凳", "照明", "电器", "家饰", "厨卫" };

    public static string CreateList(string fileName)
    {
        StreamWriter sw;
        string filePath = string.Format("{0}/GoodsList/{1}", path, fileName);
        FileInfo file = new FileInfo(filePath);
        sw = file.CreateText();

        System.Text.StringBuilder str = new System.Text.StringBuilder();

        //编辑抬头
        sw.WriteLine(line);
        str.Append(line);
        str.Append("\r\n");

        string name = "";
        string store = "";
        if (DataMgr.user.current != null)
        {
            name = DataMgr.user.current.username;
            store = DataMgr.user.current.storeInfo;
        }

        string account = string.Format("账号：{0}", name);
        sw.WriteLine(account);
        str.Append(account);
        str.Append("\r\n");

        string storeStr = string.Format("门店：{0}", store);
        sw.WriteLine(storeStr);
        str.Append(storeStr);
        str.Append("\r\n");

        string time = string.Format("时间：{0}", kMethod.GetCurrTime());
        sw.WriteLine(time);
        str.Append(time);
        str.Append("\r\n");

        sw.WriteLine(line);
        str.Append(line);
        str.Append("\r\n");

        //编辑内容
        Dictionary<GoodsType, Dictionary<string, GoodsBuyInfo>> dic = DataMgr.buy.buyList;

        foreach (GoodsType type in dic.Keys)
        {
            WriteOneType(sw, str,type);
            foreach (string unitID in dic[type].Keys)
            {
                WriteOneGoods(sw, str, dic[type][unitID]);
            }
        }

        //编辑结尾
        sw.WriteLine(line);
        str.Append(line);
        str.Append("\r\n");

        string strTotal = "合计:";
        float total = DataMgr.buy.GetTotalPrice();
        sw.WriteLine(string.Format("{0,-63}￥{1:N}", strTotal, total));
        str.Append(string.Format("{0,-63}￥{1:N}", strTotal, total));
        str.Append("\r\n");

        sw.WriteLine(line);
        str.Append(line);
        str.Append("\r\n");

        sw.Close();
        sw.Dispose();

        return str.ToString();
    }

    /// <summary>
    /// 写入一个商品的信息
    /// </summary>
    private static void WriteOneGoods(StreamWriter sw, System.Text.StringBuilder str, GoodsBuyInfo goods)
    {
        sw.WriteLine(string.Format("{0}", goods.name));
        str.Append(string.Format("{0}", goods.name));
        str.Append("\r\n");

        sw.Write(string.Format("{0,-28}", goods.ArtNo));
        str.Append(string.Format("{0,-28}", goods.ArtNo));

        //数量或面积
        string numOrArea = string.Format("{0}{1}", goods.NumOrArea, goods.unit);
        sw.Write(string.Format("{0,-18}", numOrArea));
        str.Append(string.Format("{0,-18}", numOrArea));

        //单价
        string price = string.Format("￥{0}/{1}", goods.price, goods.unit);
        sw.Write(string.Format("{0,-16}", price));
        str.Append(string.Format("{0,-16}", price));

        sw.WriteLine(string.Format("￥{0:N}", goods.totalPrice));
        str.Append(string.Format("￥{0:N}", goods.totalPrice));
        str.Append("\r\n");
    }

    /// <summary>
    /// 写入一个大类的抬头
    /// </summary>
    /// <param name="sw"></param>
    /// <param name="type"></param>
    private static void WriteOneType(StreamWriter sw, System.Text.StringBuilder str, GoodsType type)
    {   
        //空格
        sw.WriteLine("");
        str.Append("\r\n");

        //大类 名称
        sw.WriteLine(typeArr[(int)type]);
        str.Append(typeArr[(int)type]);
        str.Append("\r\n");

        //划分线
        sw.WriteLine(line);
        str.Append(line);
        str.Append("\r\n");

        string goodsName = "  商品名";
        sw.Write("{0,-25}", goodsName);
        str.Append(string.Format("{0,-25}", goodsName));

        string unitName = "";
        if (type == GoodsType.FLoorOrWall)  unitName = "面积";
        else unitName = "数量";
        sw.Write("{0,-18}", unitName);
        str.Append(string.Format("{0,-18}", unitName));

        sw.Write("{0,-18}", "单价");
        str.Append(string.Format("{0,-18}", "单价"));

        sw.WriteLine("{0,-18}", "金额");
        str.Append(string.Format("{0,-18}", "金额"));
        str.Append("\r\n");
    }
}
