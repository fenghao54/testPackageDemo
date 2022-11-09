using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


    public class LocalDataUtils : MonoBehaviour
    {
        #region save

        private static bool USE_ENCRYPT = false;

        public static void UseEncrypt(bool flag)
        {
            USE_ENCRYPT = flag;
        }

        public static void SaveToFile<T>(T data,UIMgr ui, string name )
        {
            string dataName = typeof( T ).Name;
            if( name != null ) {
            dataName = name;
            }
             string path = ResourcePath.localPath + dataName + ".json";
             string   pathClone = ResourcePath.localPath + dataName + "(clone)" + ".json";
             FileStream fs = new FileStream( pathClone, FileMode.OpenOrCreate, FileAccess.Write);
             if( fs != null ) {

                fs.Seek( 0, SeekOrigin.Begin );
                fs.SetLength( 0 );

                string jsonStr = JsonConvert.SerializeObject( data );
                byte[] bytes = Encoding.Default.GetBytes( jsonStr );
                if( USE_ENCRYPT ) {
                    byte[] newBuff = EncryptBytesSimple( bytes );
                    fs.Write( newBuff, 0, newBuff.Length );
                }
                else {
                    fs.Write( bytes, 0, bytes.Length );
                }

                ui.DebugState( "存档完成" );
                fs.Close();
                if( pathClone != path ) {
                    Debug.Log( "替换原有存档" );
                    File.Copy( pathClone, path,true );
                }
          }   
          }

    public static void SaveToFileSec<T>( T data, UIMgr ui, string name ) {
        string dataName = typeof( T ).Name;
        if( name != null ) {
            dataName = name;
        }
        string path = ResourcePath.localPath + dataName + ".json";
       
        FileStream fs = new FileStream( path, FileMode.OpenOrCreate, FileAccess.Write );
        if( fs != null ) {

            fs.Seek( 0, SeekOrigin.Begin );
            fs.SetLength( 0 );

            string jsonStr = JsonConvert.SerializeObject( data );
            byte[] bytes = Encoding.Default.GetBytes( jsonStr );
            if( USE_ENCRYPT ) {
                byte[] newBuff = EncryptBytesSimple( bytes );
                fs.Write( newBuff, 0, newBuff.Length );
            }
            else {
                fs.Write( bytes, 0, bytes.Length );
            }

            ui.DebugState( "存档完成" );
            fs.Close();
          
        }


    }



    #endregion

    #region Load

    public static T LoadFromFile<T>() where T : class, new()
        {
            T data = null;
            string path = ResourcePath.localPath + typeof(T).Name + ".json";
            if (!File.Exists(path))
            {
                return null;
            }

            FileStream fs = new FileStream(path, FileMode.Open);
            if (fs != null)
            {
                string jsonStr = null;
                if (USE_ENCRYPT)
                {
                    byte[] buff = new byte[fs.Length];
                    fs.Read(buff, 0, (int)fs.Length);
                    byte[] newBuff = DecryptBytesSimple(buff);
                    jsonStr = Encoding.Default.GetString(newBuff);
                }
                else
                {

                    byte[] bytes = new byte[fs.Length];
                    int nResult = fs.Read(bytes, 0, bytes.Length);
                    if (nResult > 0)
                        jsonStr = Encoding.Default.GetString(bytes);
                }
            if( !string.IsNullOrEmpty( jsonStr ) ) 
               data =  JsonConvert.DeserializeObject<T>(jsonStr);
            }

            fs.Close();
            return data;
        }

        #endregion

        #region SaveToBin
        public static void SaveBinToFile<T>(T data, UIMgr ui )
        {

            string path = ResourcePath.localPath + typeof(T).Name + ".bin";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            if (fs != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, data);
                fs.Close();
                ui.DebugState( "存档完成" );
             }
        }

        public static void CopeBinToFile<T>() 
        {
            string clone = ResourcePath.localPath + typeof(T).Name + "(clone)" + ".bin";
            string path = ResourcePath.localPath + typeof( T ).Name + ".bin";
            File.Copy( path, clone, true );
       }
        #endregion

        #region LoadFromBin
        public static T LoadBinFromFile<T>() where T : class, new()
        {
            T data = null;
            string path = ResourcePath.localPath + typeof(T).Name + ".bin";
            if (!File.Exists(path))
            {
                return null;
            }

            FileStream fs = new FileStream(path, FileMode.Open);
            if (fs != null)
            {
                BinaryFormatter bf = new BinaryFormatter();

                try {
                    data = bf.Deserialize( fs ) as T;
                    fs.Close();

                    CopeBinToFile<PlayerData>();//如果正常读取 那么把正常的存档 clone 给 备份文档 下次读取失败的时候用
                }
                catch//如果出现异常 就读取备份的文件
                {
                        string clonePath = ResourcePath.localPath + typeof( T ).Name+"(clone)" + ".bin";
                        if( !File.Exists( path ) ) {
                            return null;
                        }
                        FileStream fsClone = new FileStream( clonePath, FileMode.Open );
                        data = bf.Deserialize( fsClone ) as T;
                        fs.Close();
                }
                
            }
            return data;
        }

        #endregion

        public static byte[] EncryptBytesSimple(byte[] array)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(GetKey());
            int num = bytes.Length;
            int num2 = 0;
            for (int i = 0; i < array.Length; i++)
            {
                int expr_27_cp_1 = i;
                array[expr_27_cp_1] ^= bytes[num2];
                num2 = (num2 + 1) % num;
            }
            return array;
        }

        public static byte[] DecryptBytesSimple(byte[] array)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(GetKey());
            int num = bytes.Length;
            int num2 = 0;
            for (int i = 0; i < array.Length; i++)
            {
                int expr_27_cp_1 = i;
                array[expr_27_cp_1] ^= bytes[num2];
                num2 = (num2 + 1) % num;
            }
            return array;
        }

        public static string GetKey()
        {
            return "qcDY6X+aPLw=";
        }

        /// <summary>
        /// 计算用户的唯一ID
        /// </summary>
        /// <param name="srcKey"> 原始唯一id，长度限制128，比如是facebook的id</param>
        /// <returns></returns>
        public static string CalcUserDataKey(string srcKey)
        {
            if (string.IsNullOrEmpty(srcKey))
            {
                return null;
            }

            srcKey = srcKey.Trim();
            if(string.IsNullOrEmpty(srcKey))
            {
                return null;
            }

            if(srcKey.Length > 128)
            {
                return null;
            }

            byte[] buf = System.Text.Encoding.Default.GetBytes(srcKey);
            string bufStr = "";

            for (int i = 0; i < buf.Length; i++)
            {
                bufStr += string.Format("{0:X2}", (~buf[i]) & 0xff);
            }
            return EncryptMD5_32_Lower(bufStr);
        }


        ///base64编码
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="_encryptContent">需要加密的内容</param>
        /// <returns></returns>
        public static string EncryptMD5_32_Upper(string _encryptContent)
        {
            string content_Normal = _encryptContent;
            string content_Encrypt = "";
            MD5 md5 = MD5.Create();
            
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(content_Normal));

            for (int i = 0; i < s.Length; i++)
            {
                content_Encrypt = content_Encrypt + s[i].ToString("X2");
            }
            return content_Encrypt;
        }

        public static string EncryptMD5_32_Lower(string _encryptContent)
        {
            return EncryptMD5_32_Upper(_encryptContent).ToLower();
        }



        /// <summary>
        /// MD5 64位加密
        /// </summary>
        /// <param name="_encryptContent">需要加密的内容</param>
        /// <returns></returns>
        public static string EncryptMD5_64_Upper(string _encryptContent)
        {
            string content = _encryptContent;
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
            return Convert.ToBase64String(s);
        }

        public static string EncryptMD5_64_Lower(string _encryptContent)
        {
            return EncryptMD5_64_Upper(_encryptContent).ToLower();
        }


    }

