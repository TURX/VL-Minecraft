using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using CmlLib.Core;

namespace VL_Launcher
{
    public class Utility
    {
        public static void DeleteFile(string url)
        {
            if (!File.Exists(url)) return;
            File.Delete(url);
        }

        public static void MoveFile(string from, string to)
        {
            if (!File.Exists(from)) return;
            new FileInfo(from).MoveTo(to);
        }

        public static void CopyFile(string from, string to)
        {
            if (!File.Exists(from)) return;
            new FileInfo(from).CopyTo(to);
        }

        public static string GetLastLine(string file)
        {
            string st = string.Empty;
            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            st = sr.ReadLine();
                        }
                    }
                }
            }
            return st;
        }

        public static bool CheckProcessSuccess(string filename, string argument)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = filename,
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = argument,
                RedirectStandardOutput = true
            };
            try
            {
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetIsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.WinCE || Environment.OSVersion.Platform == PlatformID.Win32S || Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows;
        }

        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }

        public static string GetWorkingDir()
        {
            var dir = Environment.CurrentDirectory;
            if (dir.Contains(".app") && RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                dir = new Regex("^.*(?=/.*.app)").Match(dir).ToString();
            return dir;
        }

        public static string GetJava(CMLauncher launcher)
        {
            string path;
            if (MRule.OSName != "windows")
            {
                path = "/usr/bin/java";
            } else
            {
                var p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c where javaw.exe 2>&1";
                p.Start();
                string o = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (o.Contains("javaw.exe"))
                {
                    path = o;
                } else
                {
                    path = launcher.CheckJRE();
                }
            }
            Console.WriteLine("[Java] " + path);
            return path;
        }
    }
}
