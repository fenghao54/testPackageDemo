using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    // Start is called before the first frame update

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    public  Text txt1;
    DataMgr datamgr;
    int testDataNum = 10000;

    int testDataNum2 = 5000;
    void Start()
    {

        datamgr = this.transform.GetComponent<DataMgr>();
       
        btn1.onClick.AddListener( SetData1 );
        btn2.onClick.AddListener( SetData2 );


        btn3.onClick.AddListener( SetNoBackUps );
        btn4.onClick.AddListener( SetBackUps );
    }

    public  void  DebugState(string des ) {
        txt1.text = des;
    }

    public void SetData1( ) {

        PlayerData _playerdata = datamgr._playerdata;
        if( _playerdata != null ) {
           
            Dictionary<string, string> dicTemp =  new Dictionary<string, string>();
            for( int i = 0; i < testDataNum; i++ ) {
                dicTemp[i.ToString()] = "数据" + i.ToString();
            }
            _playerdata.dicTest = dicTemp;
            Debug.Log( "开始存档1" );

#if UNITY_EDITOR
        
           LocalDataUtils.SaveToFile<PlayerData>( _playerdata, this ,null );
#else
            LocalDataUtils.SaveBinToFile<PlayerData>( _playerdata, this );
#endif
        }
    }

    public void SetData2() {

        PlayerData _playerdata = datamgr._playerdata;
        if( _playerdata != null ) {
           
            Dictionary<string, string> dicTemp = new Dictionary<string, string>();
            for( int i = 0; i < testDataNum2; i++ ) {
                dicTemp[i.ToString()] = "存档" + i.ToString();
            }
            _playerdata.dicTest = dicTemp;
            Debug.Log( "开始存档2" );
#if UNITY_EDITOR
            LocalDataUtils.SaveToFile<PlayerData>( _playerdata, this, null);
#else
            LocalDataUtils.SaveBinToFile<PlayerData>( _playerdata , this);
#endif
        }
    }

    public void SetNoBackUps() {
        PlayerData _playerdata = datamgr._playerdata;
        if( _playerdata != null ) {

            Dictionary<string, string> dicTemp = new Dictionary<string, string>();
            for( int i = 0; i < 10000; i++ ) {
                dicTemp[i.ToString()] = "存档" + i.ToString();
            }
            _playerdata.dicTest = dicTemp;
            Debug.Log( "开始存档2" );
            for( int i = 0; i < 10; i++ ) {
#if UNITY_EDITOR
            LocalDataUtils.SaveToFileSec<PlayerData>( _playerdata, this, "noBackUps"+i.ToString() );
#else
            LocalDataUtils.SaveBinToFile<PlayerData>( _playerdata , this);
#endif
            }

        }
    }

    public void SetBackUps() {
        PlayerData _playerdata = datamgr._playerdata;
        if( _playerdata != null ) {

            Dictionary<string, string> dicTemp = new Dictionary<string, string>();
            for( int i = 0; i < 10000; i++ ) {
                dicTemp[i.ToString()] = "存档" + i.ToString();
            }
            _playerdata.dicTest = dicTemp;
            Debug.Log( "开始存档2" );
            for( int i = 0; i < 10; i++ ) {
#if UNITY_EDITOR
                LocalDataUtils.SaveToFile<PlayerData>( _playerdata, this, "BackUps" + i.ToString() );
#else
                LocalDataUtils.SaveBinToFile<PlayerData>( _playerdata , this);
#endif
            }

        }
    }


}
