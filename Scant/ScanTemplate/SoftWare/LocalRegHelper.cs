using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EncyptTools;
using System.IO;
using System.Threading;

namespace EncyptTools
{
    public class LocalRegHelper
    {
        public bool IsReged { get; set; } 
        public string ExceptMsg { get { return _msg; } }
        public DateTime BeginDate { get { return new DateTime(2018, 1, 1).AddDays(_beginDateIndex); } }
        public DateTime ExpirDate { get { return new DateTime(2018, 1, 1).AddDays(_expiryDateIndex); } }

        public LocalRegHelper()
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
            sbsettings = new StringBuilder();
            strsettings = "";
        }

        public bool TryRegHash(string hashpart)
        {
            try
            {
                Clear();
                string htmltxt = ToolsWeb.CWeb.web.GetOKUrl(url);
                _aes = Refinestring(hashpart, ">", htmltxt) + ">";
                if (_aes == "")
                    throw new Exception("Not Correct Hash");
                _aes = Refinestring(":", ">", _aes);
                _hash =  Refinestring(hashpart, ":", htmltxt);
                if (_hash == "")
                    throw new Exception("Not Correct Hash");
                _hash = hashpart + _hash;
                string signinfo = DESHeper.DecryptDES(_aes, "love2018");
                //Refine Sign and MachineCode
                _sign = Refinestring("<SIGN>", "</SIGN>", signinfo);
                _machinecode = Refinestring("<MACHINECODE>", "</MACHINECODE>", signinfo);
                if (_machinecode == "" || _sign == "")
                    throw new Exception("Not Correct Aes");
                _beginDateIndex = Convert.ToInt32(Refinestring("#", "-", _machinecode + ">"));
                _expiryDateIndex = Convert.ToInt32(Refinestring("-", ">", _machinecode + ">"))
                   + _beginDateIndex;

                string signmc = _machinecode.Substring(0, 3) + _machinecode.Substring(8, 10);
                string locmachinecode = SoftRegisterTools.getMachineNum();
                //版本升级用
                string localmc = locmachinecode.Substring(0, 3) + locmachinecode.Substring(8, 10);
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
                Clear();
                //判断是否联网
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

                string signmc = _machinecode.Substring(0, 3) + _machinecode.Substring(8, 10);
                string locmachinecode = SoftRegisterTools.getMachineNum();
                //版本升级用
                string localmc = locmachinecode.Substring(0, 3) + locmachinecode.Substring(8, 10);
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
        public bool CheckReged()
        {
            IsReged = false;
            ReadSettings();

            string SericalNumber = ReadSetting("SerialNumber", "-1");
            if (SericalNumber == "-1")
            {
                _msg = "软件尚未注册，请注册软件！";
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
            string localmc = locmachinecode.Substring(0,3) + locmachinecode.Substring(8,10);
            if (signmc != localmc)
            {
                _msg = "注册机器与本机不一致,请联系管理员！";
                return IsReged;
            }

            // 比较时间 /
            string NowDate = DateTime.Now.ToString("yyyyMMdd");
            string EndDate = SericalNumber.Split('-')[1];

            if (Convert.ToInt32(EndDate) - Convert.ToInt32(NowDate) < 0)
            {
                _msg = "软件使用已到期！";
                return IsReged;
            }
            
            if (DateTime.Now < BeginDate || DateTime.Now > ExpirDate)
            {
                _msg = "软件使用已到期！";
                return IsReged;
            }

            if (rsah.SignCheck(_machinecode, _sign))
            {
                IsReged = true;
            }
            return IsReged;
        }

        private void ReadSettings()
        {
            try
            {
                Clear();
                sbsettings.Clear();
                if (File.Exists("configreg.ini"))
                {
                    strsettings = File.ReadAllText("configreg.ini");
                    _machinecode = ReadSetting("MachineCode");
                    _sign = ReadSetting("Sign");
                    _hash = ReadSetting("Hash");
                    _aes = ReadSetting("Aes");
                    _beginDateIndex = Convert.ToInt32(ReadSetting("BeginDateIndex", "-1"));
                    _expiryDateIndex = Convert.ToInt32(ReadSetting("ExpiryDateIndex", "-1"));
                    string beginDate = ReadSetting("BeginDate", "-1");
                    string endDate = ReadSetting("ExpiryDate", "-1");
                    if (BeginDate.ToString("yyyyMMdd") != beginDate || endDate != ExpirDate.ToString("yyyyMMdd"))
                    {
                        _msg = "注册日期遭到篡改";
                        throw new Exception(_msg);
                    }
                }
                else
                {
                    _msg = "找不到注册文件";
                }
            }
            catch
            {
                Clear();
            }
        }
        private string ReadSetting(string Key,string Default = "")
        {
            string res = Refinestring("<" + Key + ">", "</" + Key + ">", strsettings);
            if (res == "")
                return Default;
            return res;
        }
        private void SaveSettings()
        {
            sbsettings.Clear();
            SaveSetting("MachineCode", _machinecode);
            SaveSetting("Sign", _sign);
            SaveSetting("Aes", _aes);
            SaveSetting("Hash", _hash);
            SaveSetting("BeginDateIndex", _beginDateIndex);
            SaveSetting("ExpiryDateIndex", _expiryDateIndex);
            SaveSetting("BeginDate",BeginDate.ToString("yyyyMMdd"));
            SaveSetting("ExpiryDate",ExpirDate.ToString("yyyyMMdd"));
            //
            SaveSetting("SerialNumber", _localmachinecode + "-" + ExpirDate.ToString("yyyyMMdd"));

            File.WriteAllText("configreg.ini", sbsettings.ToString());
        }
        public void SaveSetting(string Key, string Value)
        {
            sbsettings.AppendLine("<" + Key + ">" + Value + "</" + Key + ">");
        }
        public void SaveSetting(string Key, int Value)
        {
            SaveSetting(Key, Value.ToString());
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

        private StringBuilder sbsettings;
        private string strsettings;

        public void ClearData()
        {
            if (CheckReged())
            {
                _machinecode = _machinecode.Split('-')[0];
                SaveSettings();
            }
        }

        internal string GetSignInfo()
        {
            return "<SIGN>[sign]</SIGN><MACHINECODE>[mc]</MACHINECODE>".Replace("[sign]", _sign).Replace("[mc]", _machinecode);
        }
    }
    public class ThreadCheckSign
    {
        public ThreadCheckSign()
        {
            isrunning = false;
        }
        public void StartRun()
        {
            if (!isrunning)
            {
                isrunning = true;
                ThreadCheckSign tus = new ThreadCheckSign();
                System.Threading.Thread nonParameterThread = new Thread(new ThreadStart(tus.Run));
                nonParameterThread.Start();
            }
        }
        public void Run()
        {
            DateTime now = DateTime.Now;
            Random random = new Random((int)now.Ticks);
            int begintime = (random.Next(1800) + 600) * 1000;

            Thread.Sleep(begintime);
            RegHelper reghelper = new RegHelper();
            if (!reghelper.CheckSign())
            {
                LocalRegHelper lrh = new LocalRegHelper();
                lrh.ClearData();
                ScanTemplate.global.LocReghelper.IsReged = false;
            }
            isrunning = false;
        }
        private static bool isrunning;
    }
}
