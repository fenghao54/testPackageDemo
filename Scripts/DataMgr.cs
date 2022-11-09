using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour
{
    
    public  PlayerData _playerdata;
     UIMgr ui;
    int testDataNum = 10000000;

    float offSetTime = 0.01f;
    void Start()
    {
        ui = this.transform.GetComponent<UIMgr>();
        LoadPlayerData();
    }
    public void LoadPlayerData() {

#if UNITY_EDITOR
        _playerdata = LocalDataUtils.LoadFromFile<PlayerData>();
#else
        _playerdata = LocalDataUtils.LoadBinFromFile<PlayerData>();
#endif


        if( _playerdata == null ) {

            ui.DebugState( "数据初始化" );
            _playerdata = new PlayerData();
            _playerdata.Initdata();//重置
           
        }
        else {

            ui.DebugState( "数据大小=" + _playerdata.dicTest.Count );
        }
    }

}
