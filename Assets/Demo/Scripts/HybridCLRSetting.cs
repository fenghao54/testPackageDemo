using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HybridCLRSetting : ScriptableObject {
    public const string formatAssemblys = "Assets/Public";
    [Header( "测试一" )]
    public string[] ddd = new string[] { "dd", "cc", "cb" };
    [Tooltip( "测试二" )]
    public string[] hotassemblys = new []{ "vvv" };


    public async Task LoadDll() {

    }
    public async void Start() {
        await LoadDll();
    }
}
