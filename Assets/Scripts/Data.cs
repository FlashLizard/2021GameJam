using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public enum TagId//Tag��ǩ��û��enemy����Ϊ������team�Ƿ���ͬ���жϵжԹ�ϵ
    {
        player,
        ice,
        end
    }
    public enum SceneId
    {
        main,//������
        ready,//׼�����棨ʱ�䣬���������
        game,//��Ϸ����
        victory,//ʤ������
        end
    }
    public enum UIId
    {
        settings,//����UI
        pause,//��ͣUI
        controlEnd,//��֮ǰΪ�ɲ��ݵ�ui������ĳЩ������Crontroller�����ȡ�Ķ���
        status,//���״̬ui
        blood,//����Ѫ��ui
        hardBullet,//Ӳѩ��ui
        softBullet,
        scoreOption,//�����������ui
        healthMove,//Ѫ���ƶ�ui
        giftMove,//�ƶ�����ui
        makeMove,//������ui
        end
    }
    public static Dictionary<TagId, string> tagIdToString = new Dictionary<TagId, string>//string����Ϸ��tag��ͬ
    {
        {TagId.player,"Player" },
        {TagId.ice,"Ice" }
    };//����ͨ��id��ȡ��Ϸ�ַ���
    public static Dictionary<UIId,string> uIIdToString = new Dictionary<UIId, string>//string����Ϸ��ui��ͬ
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

    };//����ͨ��id��ȡ��Ϸ�ַ���
    public static Dictionary<string, UIId> stringToUIId = new Dictionary<string, UIId>//string����Ϸ��ui������ͬ
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
    };//����ͨ����Ϸ�ַ�����ȡid
    public static Dictionary<string, SceneId> stringToSceneId=new Dictionary<string, SceneId>//string����Ϸ��Scene������ͬ
    {
         {"main",SceneId.main },
         {"ready",SceneId.ready },
         {"game",SceneId.game },
         {"victory",SceneId.victory },
    };//����ͨ����Ϸ�ַ�����ȡid
    public static Dictionary<SceneId, Scene> scenes = new Dictionary<SceneId, Scene>
    {
         {SceneId.main,new MainScene()},
         {SceneId.ready,new ReadyScene() },
         {SceneId.game,new GameScene() },
         {SceneId.victory,new VictoryScene() },
    };//ͨ��id��ȡ��������
    public static GameObject Generate(string prefab)//����һ��Ԥ����
    {
        return Object.Instantiate(Resources.Load<GameObject>("Prefabs/" + prefab));
    }
    public static GameObject Generate(string prefab, GameObject parent)//ͬ�ϣ����趨���ף���λ������Ϊ����λ��
    {
        GameObject child = Generate(prefab);
        child.transform.SetParent(parent.transform);
        child.transform.position = parent.transform.position;
        return child;
    }
}
