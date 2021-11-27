using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public enum TagId//Tag标签（没有enemy是因为可以用team是否相同来判断敌对关系
    {
        player,
        ice,
        end
    }
    public enum SceneId
    {
        main,//主界面
        ready,//准备界面（时间，玩家数量）
        game,//游戏界面
        victory,//胜利界面
        end
    }
    public enum UIId
    {
        settings,//设置UI
        pause,//暂停UI
        controlEnd,//此之前为可操纵的ui，是在某些场景中Crontroller必须获取的对象
        status,//玩家状态ui
        blood,//单格血量ui
        hardBullet,//硬雪球ui
        softBullet,
        scoreOption,//结束后分数栏ui
        healthMove,//血量移动ui
        giftMove,//移动礼物ui
        makeMove,//制作条ui
        end
    }
    public static Dictionary<TagId, string> tagIdToString = new Dictionary<TagId, string>//string与游戏中tag相同
    {
        {TagId.player,"Player" },
        {TagId.ice,"Ice" }
    };//方便通过id获取游戏字符串
    public static Dictionary<UIId,string> uIIdToString = new Dictionary<UIId, string>//string与游戏中ui相同
    {
        {UIId.pause,"Pause" },
        {UIId.settings,"Settings" },
        {UIId.status,"Status" },
        {UIId.blood,"Blood" },
        {UIId.hardBullet,"HardBullet" },
        {UIId.softBullet,"SoftBullet" },
        {UIId.scoreOption,"ScoreOption" },
        {UIId.healthMove,"HealthMove" },
        {UIId.giftMove,"GiftMove" },
        {UIId.makeMove,"MakeMove" }

    };//方便通过id获取游戏字符串
    public static Dictionary<string, UIId> stringToUIId = new Dictionary<string, UIId>//string与游戏中ui名字相同
    {
        {"Pause",UIId.pause },
        {"Settings",UIId.settings },
        {"Status",UIId.status },
        {"Blood",UIId.blood },
        {"HardBullet",UIId.hardBullet },
        {"SoftBullet",UIId.softBullet },
        {"ScoreOption",UIId.scoreOption },
        {"HealthMove",UIId.healthMove },
        {"GiftMove",UIId.giftMove },
        {"MakeMove",UIId.makeMove }
    };//方便通过游戏字符串获取id
    public static Dictionary<string, SceneId> stringToSceneId=new Dictionary<string, SceneId>//string与游戏中Scene名字相同
    {
         {"main",SceneId.main },
         {"ready",SceneId.ready },
         {"game",SceneId.game },
         {"victory",SceneId.victory },
    };//方便通过游戏字符串获取id
    public static Dictionary<SceneId, Scene> scenes = new Dictionary<SceneId, Scene>
    {
         {SceneId.main,new MainScene()},
         {SceneId.ready,new ReadyScene() },
         {SceneId.game,new GameScene() },
         {SceneId.victory,new VictoryScene() },
    };//通过id获取场景对象
    public static GameObject Generate(string prefab)//生成一个预制体
    {
        return Object.Instantiate(Resources.Load<GameObject>("Prefabs/" + prefab));
    }
    public static GameObject Generate(string prefab, GameObject parent)//同上，可设定父亲，且位置设置为父亲位置
    {
        GameObject child = Generate(prefab);
        child.transform.SetParent(parent.transform);
        child.transform.position = parent.transform.position;
        return child;
    }
}
