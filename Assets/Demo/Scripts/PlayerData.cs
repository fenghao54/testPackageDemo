using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerData 
{
    public Dictionary<string, string> dicTest;
    public int dicNum;   


    public void Initdata() {
        dicTest  =   new Dictionary<string, string>();
        dicNum = 0;
    }
}
