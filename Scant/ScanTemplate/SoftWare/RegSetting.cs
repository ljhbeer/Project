using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace EncyptTools
{
    public class RegSetting
    {
        public static void WriteSetting(string Section, string Key, string Setting) // name = key value=setting Section= path
        {
            if (!Section.StartsWith( RootSection))
            {
                if (Section.StartsWith("\\"))
                    Section = Section.Substring(1);
                Section = RootSection + Section;
            }
            RegistryKey key1 = Registry.LocalMachine.CreateSubKey(Section); // .LocalMachine.CreateSubKey("Software\\mytest");
            if (key1 == null)
            {
                return;
            }
            try
            {
                key1.SetValue(Key, Setting);
            }
            catch (Exception exception1)
            {
                return;
            }
            finally
            {
                key1.Close();
            }
        }
        // 读取注册表       
        public static string ReadSetting(string Section, string Key, string Default="")
        {
            if (!Section.StartsWith(RootSection))
            {
                if (Section.StartsWith("\\"))
                    Section = Section.Substring(1);
                Section = RootSection + Section;
            }
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Section);
            if (registryKey != null)
            {
                object objValue = registryKey.GetValue(Key, Default);
                if (objValue != null)
                {
                    Default = objValue.ToString() ;
                }
                registryKey.Close();
            }
            return Default;
        }
        private static string RootSection = "SOFTWARE\\JHSOFT\\";
    }
}
