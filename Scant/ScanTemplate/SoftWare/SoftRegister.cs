using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace EncyptTools
{
    public static class SoftRegisterTools
    {      
        public static void killMyself()
        {
            string bat = string.Format("@echo off \n ping -n 3 127.0.0.1 \n del {0}", System.Windows.Forms.Application.ExecutablePath);
            File.WriteAllText("killme.bat", bat);
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
            System.Environment.Exit(0);
            //Application.Exit();
        }
        public static bool meIsMe()
        {
            if (getSoftMD5().Substring(0, 1) == SoftMD5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string getMachineNum()
        {
            if (SoftMD5 == string.Empty)
            {
                SoftMD5 = getSoftMD5();
            }
            if (DiskVolume == string.Empty)
            {
                DiskVolume = getDiskVolume();
            }
            if (string.IsNullOrEmpty(CPUSerialNum))
            {
                CPUSerialNum = getCPUserialNum();
            }
            string strNum = SoftVersion + SoftMD5.Substring(0, 5) + CPUSerialNum.Substring(0, 5) + DiskVolume.Substring(0, 5);
            
            //ForTest
            //strNum = "V02F6774BFEBF6A166";
            return strNum;//18个注册字符
        }        
        private static string SoftVersion = "V02";//v0.1
        private static string SoftMD5 = string.Empty;
        private static string DiskVolume = string.Empty;
        private static string CPUSerialNum = string.Empty;
        private static string PublicKey = @"<RSAKeyValue><Modulus>voMi5YtpM7on3EPTRlA2kbxrWDNsy/Ua1kZTLB7QeB1R7Ytp645l2kCMp49UG+naAxKAC6hgg4/SiSetigEfYU6jowy6nFCpJJn12cZHo+brEJvgrlS+n8dRUcK9A2vGPZKYhws+QXvR3TGjYiMriAvQ8sZmzMmlhPybKKA9IKc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
      
        private static string getDiskVolume()
        {
            //ManagementClass mc = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }
        private static string getCPUserialNum()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuCollection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
            }
            return strCpu;
        }
        private static string getSoftMD5()
        {
            string fileName = System.Windows.Forms.Application.ExecutablePath;
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }               
    }
    public class CryptionHelper
    {
        public static bool VerifySigned(string machinecode, string sign, string PublicKey)
        {
            RSAHelper rsah = new RSAHelper(PublicKey);
            return rsah.SignCheck(machinecode, sign);
        }
    }
    public class LogHelper
    {
        public static void WriteLog(Exception e)
        {
            File.AppendAllText("loginerror.log", System.DateTime.Now + " 登录失败，原因是：" + e.Message + "\r\n");
        }
    }
    public class RegHelper
    {
        public bool IsReged { get; set; }
        public string ExceptMsg { get { return _msg; } }
        public DateTime BeginDate { get { return new DateTime(2018, 1, 1).AddDays(_beginDateIndex); }  }
        public DateTime ExpirDate { get { return new DateTime(2018, 1, 1).AddDays(_expiryDateIndex); } }
        public RegHelper()
        {
            rsah.publicKey = publickey1;
            rsah.privateKey = "";
            _localmachinecode = SoftRegisterTools.getMachineNum();
            Clear();
        }
        public void Clear()
        {
            _msg = "";
            IsReged = false;
            _machinecode = "";
            _sign = "";
            _hash = "";
            _aes = "";
            _beginDateIndex = -1000;
            _expiryDateIndex = -1000;
        }
        public bool TryRegHash(string hashpart)
        {
            try
            {
                Clear();
                if (hashpart.Length < 8 || hashpart.Length > 32 ||
                    !ToolsWeb.DgvTools.ValidBase64(hashpart))
                {
                    _msg = "不正确的注册代码";
                    return false;
                }
                string htmltxt = ToolsWeb.CWeb.web.GetOKUrl(url);
                _aes = Refinestring(hashpart, ">", htmltxt) + ">";
                if (_aes == "")
                    throw new Exception("Not Correct Hash");
                _aes = Refinestring(":", ">", _aes);
                _hash = Refinestring("<", ":", htmltxt);
                if(!_hash.Contains(hashpart))
                    throw new Exception("Not Correct Hash");
                string signinfo = DESHeper.DecryptDES(_aes, "love2018");
                //Refine Sign and MachineCode
                _sign = Refinestring("<SIGN>", "</SIGN>", signinfo);
                _machinecode = Refinestring("<MACHINECODE>", "</MACHINECODE>", signinfo);
                if (_machinecode == "" || _sign == "")
                    throw new Exception("Not Correct Aes");
                _beginDateIndex = Convert.ToInt32(Refinestring("#", "-", _machinecode + ">"));
                _expiryDateIndex = Convert.ToInt32(Refinestring("-", ">", _machinecode + ">")) 
                   +_beginDateIndex;

                string signmc =_machinecode.Substring(0,3)+ _machinecode .Substring(8,10 );
                string locmachinecode = SoftRegisterTools.getMachineNum();
                //版本升级用
                string localmc = locmachinecode.Substring(0,3) + locmachinecode.Substring(8,10);
                if (signmc != localmc)
                    throw new Exception("Not Local MachineCode");
                if (rsah.SignCheck(_machinecode, _sign))
                {
                    IsReged = true;
                    SaveSettings();
                }               
            }
            catch (Exception e)
            {
                //判断是否联网
                Clear();
                _msg = e.Message;
            }
            return IsReged;
        }
        public bool TrySignInfo(string signinfo)
        {
            try
            {
                Clear();
                _sign = Refinestring("<SIGN>", "</SIGN>", signinfo);
                _machinecode = Refinestring("<MACHINECODE>", "</MACHINECODE>", signinfo);
                _aes = DESHeper.EncryptDES(signinfo, "love2018");
                _hash = ShaHelper.GetSha1Hash(_sign); ;
                if (_machinecode == "" || _sign == "")
                    throw new Exception("Not Correct Aes");
                _beginDateIndex = Convert.ToInt32(Refinestring("#", "-", _machinecode + ">"));
                _expiryDateIndex = Convert.ToInt32(Refinestring("-", ">", _machinecode + ">"))
                   + _beginDateIndex;

                string signmc = _machinecode.Substring(0, 3) + _machinecode.Substring(8,10);
                string locmachinecode = SoftRegisterTools.getMachineNum();
                //版本升级用
                string localmc = locmachinecode.Substring(0, 3) + locmachinecode.Substring(8,10);
                if (signmc != localmc)
                    throw new Exception("Not Local MachineCode");
                if (rsah.SignCheck(_machinecode, _sign))
                {
                    IsReged = true;

                    SaveSettings();
                }
            }
            catch (Exception e)
            {
                //判断是否联网
                _msg = e.Message;
                Clear();
            }
            return IsReged;
        }
        public bool CheckReged( )
        {
            IsReged = false;
            string SericalNumber = RegSetting.ReadSetting("ScanPaperV3.0", "SerialNumber", "-1");
            if (SericalNumber == "-1")
            {
                _msg  = "软件尚未注册，请注册软件！";
                return IsReged;
            }
            string regmc = SericalNumber.Split('-')[0];           
            //if (regmc != _localmachinecode)
            //{
            //    _msg = "注册机器与本机不一致,请联系管理员！";
            //    return IsReged;
            //}
            string signmc = regmc.Substring(0, 3) + regmc.Substring(8, 10);
            string locmachinecode = SoftRegisterTools.getMachineNum();
            //版本升级用
            string localmc = locmachinecode.Substring(0, 3) + locmachinecode.Substring(8, 10);
            if (signmc != localmc)
            {
                _msg = "注册机器与本机不一致,请联系管理员！";
                return IsReged;
            }



            // 比较时间 /
            string NowDate = DateTime.Now.ToString("yyyyMMdd");
            string EndDate = SericalNumber.Split('-')[1];
            
            if (Convert.ToInt32(EndDate) - Convert.ToInt32(NowDate) < 0 )
            {
                _msg = "软件使用已到期！";
                return IsReged;
            }

            ReadSettings();
            if (DateTime.Now < BeginDate || DateTime.Now > ExpirDate)
            {
                _msg = "软件使用已到期！";
                return IsReged;
            }

            IsReged = true;
            return IsReged;
        }
        public bool CheckSign()
        {
            try
            {
                ReadSettings();
                string signinfo = DESHeper.DecryptDES(_aes, "love2018");
                //Refine Sign and MachineCode
                string  sign = Refinestring("<SIGN>", "</SIGN>", signinfo);
                string  machinecode = Refinestring("<MACHINECODE>", "</MACHINECODE>", signinfo);
                if (_sign != sign || _machinecode != machinecode)
                {
                    _sign = sign;
                    _machinecode = machinecode;
                    SaveSettings();
                }
                if (!rsah.SignCheck(_machinecode, _sign))
                {
                    return false;
                }
            }
            catch
            {
                Clear();
                return false;
            }
            return true;
        }
        // 判断软件是否可用，拥有十次的试用期，也可以换成天数,再写入注册表信息      
        public static bool GetUseInfo(ref int usedcount)
        {
            usedcount = 0;
            try
            {
                string struse = RegSetting.ReadSetting(SoftName, "UseTimes", "0");
                usedcount = Convert.ToInt32(struse);
                RegSetting.WriteSetting(SoftName, "UseTimes", (usedcount + 1) + "");
            }
            catch (Exception)
            {
                RegSetting.WriteSetting(SoftName, "UseTimes", "1");
            }
            return usedcount < 10;
        }

        public static void SaveSetting(string Key, string Value)
        {
            RegSetting.WriteSetting(SoftName, Key, Value);
        }
        public static void SaveSetting(string Key, int Value)
        {
            SaveSetting(Key, Value.ToString());
        }
        public static string ReadSetting(string Key, string Default = "")
        {
            return RegSetting.ReadSetting(SoftName, Key, Default);
        }
        private void ReadSettings()
        {
            try
            {
                _machinecode = ReadSetting("MachineCode");
                _sign = ReadSetting("Sign");
                _hash = ReadSetting("Hash");
                _aes =   ReadSetting("Aes");
                _beginDateIndex  =Convert.ToInt32( ReadSetting("BeginDateIndex","-1") );
                _expiryDateIndex = Convert.ToInt32(ReadSetting("ExpiryDateIndex", "-1"));
            }
            catch
            {
                Clear();
            }
        }
        private void SaveSettings()
        {
            SaveSetting("MachineCode", _machinecode);
            SaveSetting("Sign", _sign);
            SaveSetting("Aes", _aes);
            SaveSetting("Hash", _hash);
            SaveSetting("BeginDateIndex", _beginDateIndex);
            SaveSetting("ExpiryDateIndex", _expiryDateIndex);
            //
            SaveSetting("SerialNumber",_localmachinecode + "-"+ExpirDate.ToString("yyyyMMdd"));
        }
        private string Refinestring(string begin, string end, string src)
        {
            if (src.Contains(begin))
                src = src.Substring(src.IndexOf(begin) + begin.Length);
            else
                return "";
            if (src.Contains(end))
                return src.Substring(0, src.IndexOf(end));
            return "";
        }        
        private static string SoftName = "ScanPaperV3.0";

        private string _localmachinecode;
        private string _machinecode;
        private string _sign;
        private string _aes;
        private string _hash;
        private int _beginDateIndex;
        private int _expiryDateIndex;
        private int _useTimes;

        private static RSAHelper rsah = new RSAHelper(); //用于签名和验证
        private string publickey1 = @"<RSAKeyValue><Modulus>voMi5YtpM7on3EPTRlA2kbxrWDNsy/Ua1kZTLB7QeB1R7Ytp645l2kCMp49UG+naAxKAC6hgg4/SiSetigEfYU6jowy6nFCpJJn12cZHo+brEJvgrlS+n8dRUcK9A2vGPZKYhws+QXvR3TGjYiMriAvQ8sZmzMmlhPybKKA9IKc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private string url = @"https://gitee.com/ljhbeer/ScreenData/raw/master/RegKeys/xk-ScanPapeV3.0.keys";
        private string _msg;




        internal void CopyDataToReg(LocalRegHelper loc)
        {
            TrySignInfo(loc.GetSignInfo());
        }

        public void SaveRegToLocal()
        {
            sbsettings = new StringBuilder();
            sbsettings.Clear();
            SaveSetting1("MachineCode", _machinecode);
            SaveSetting1("Sign", _sign);
            SaveSetting1("Aes", _aes);
            SaveSetting1("Hash", _hash);
            SaveSetting1("BeginDateIndex", _beginDateIndex);
            SaveSetting1("ExpiryDateIndex", _expiryDateIndex);
            SaveSetting1("BeginDate", BeginDate.ToString("yyyyMMdd"));
            SaveSetting1("ExpiryDate", ExpirDate.ToString("yyyyMMdd"));
            //
            SaveSetting1("SerialNumber", _localmachinecode + "-" + ExpirDate.ToString("yyyyMMdd"));

            File.WriteAllText("configreg.ini", sbsettings.ToString());
        }
        public void SaveSetting1(string Key, string Value)
        {
            sbsettings.AppendLine("<" + Key + ">" + Value + "</" + Key + ">");
        }
        public void SaveSetting1(string Key, int Value)
        {
            SaveSetting1(Key, Value.ToString());
        }
        private StringBuilder sbsettings;
    }
}