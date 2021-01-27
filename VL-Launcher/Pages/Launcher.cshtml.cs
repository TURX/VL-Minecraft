using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmlLib.Core.Downloader;
using CmlLib.Core;
using System.IO;
using CmlLib.Core.Version;
using CmlLib.Core.Auth;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace VL_Launcher.Pages
{
    public class LauncherModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public int Memory { get; set; }

        [BindProperty]
        public string Version { get; set; }

        [BindProperty]
        public bool Fullscreen { get; set; }

        public void ChangeOption(string rootPath, string lang)
        {
            string[] properties;
            if (System.IO.File.Exists(rootPath + "/options.txt"))
            {
                properties = System.IO.File.ReadAllLines(rootPath + "/options.txt");
                for (int t = 0; t < properties.Length; t++)
                {
                    if (properties[t].Split(':')[0] == "gamma")
                    {
                        properties[t] = properties[t].Replace("1", "0").Replace("2", "0").Replace("3", "0").Replace("4", "0").Replace("5", "0").Replace("6", "0").Replace("7", "0").Replace("8", "0").Replace("9", "0");
                        continue;
                    }
                    if (properties[t].Split(':')[0] == "lang")
                    {
                        properties[t] = "lang:" + lang;
                        continue;
                    }
                }
            }
            else
            {
                properties = new string[]
                {
                    "lang:" + lang,
                    "lastServer:124.71.131.172",
                    "skipMultiplayerWarning:true",
                    "joinedFirstServer:true",
                    "gamma:0.0"
                };
            }
            System.IO.File.WriteAllLines(rootPath + "/options.txt", properties);
        }

        private static string LFileKind = string.Empty;
        private static string LFileName = string.Empty;
        private static string LProgressedFileCnt = "0";
        private static string LTotFileCnt = "?";
        private static string LProgressPercentage = "0";
        private static string LSuccess = "false";
        private static bool LStarted = false;

        public string Launch(string username, string password, int memory, string version, bool fullScreen)
        {
            var account = new MLogin().Authenticate(username, password);
            if (!account.IsSuccess)
                return account.Result.ToString() + '\n' + account.ErrorMessage;
            var session = account.Session;
            var mcp = new MinecraftPath(Path.Combine(Utility.GetWorkingDir(), ".minecraft"));
            ChangeOption(mcp.BasePath, Thread.CurrentThread.CurrentUICulture.Name.Replace('-', '_').ToLower());
            var launcher = new CMLauncher(mcp);
            launcher.FileChanged += (e) => {
                LFileKind = e.FileKind.ToString();
                LFileName = e.FileName;
                LProgressedFileCnt = e.ProgressedFileCount.ToString();
                LTotFileCnt = e.TotalFileCount.ToString();
            };
            launcher.ProgressChanged += (sender, e) => {
                LProgressPercentage = e.ProgressPercentage.ToString();
            };
            var launchOption = new MLaunchOption
            {
                MaximumRamMb = memory,
                Session = session,
                FullScreen = fullScreen,
                ServerIp = "124.71.131.172",
                ServerPort = 25565,
                JavaPath = Utility.GetJava(launcher),
                JVMArguments = new string[] {
                    "-javaagent:../nide8auth.jar=28f8f58a8a7f11e88feb525400b59b6a",
                    "-Dnide8auth.client=true"
                }
            };
            var process = launcher.CreateProcess(version, launchOption);
            Console.WriteLine("[Launch] FileName: " + process.StartInfo.FileName);
            Console.WriteLine("[Launch] Arguments: " + process.StartInfo.Arguments);
            Console.WriteLine("[Launch] WorkingDirectory: " + process.StartInfo.WorkingDirectory);
            process.Start();
            return string.Empty;
        }

        public async Task<IActionResult> OnPostLogin()
        {
            if (DLStarted == true)
            {
                HttpContext.Session.Set("DLQueryProgress", true);
                return RedirectToPage("/Launcher");
            }
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Append("Username", Username ?? string.Empty, cookieOptions);
            Response.Cookies.Append("Password", Password ?? string.Empty, cookieOptions);
            Response.Cookies.Append("Version", Version ?? string.Empty, cookieOptions);
            Response.Cookies.Append("Fullscreen", Fullscreen.ToString(), cookieOptions);
            HttpContext.Session.Set("LQueryProgress", true);
            if (LStarted == false) {
                LStarted = true;
                LFileKind = string.Empty;
                LFileName = string.Empty;
                LProgressedFileCnt = "0";
                LTotFileCnt = "?";
                LProgressPercentage = "0";
                LSuccess = "false";
                await Task.Run(() => {
                    try
                    {
                        string err = Launch(Username, Password, Memory, Version, Fullscreen);
                        if (err != string.Empty)
                        {
                            HttpContext.Session.Set("ErrorMsg", err);
                        }
                        else
                        {
                            HttpContext.Session.Set("SuccessMsg", Translation.Translate("launcher-success-login"));
                        }
                    }
                    catch (Exception e)
                    {
                        HttpContext.Session.Set("ErrorMsg", e.Message);
                    }
                    HttpContext.Session.Set("LQueryProgress", false);
                    LSuccess = "true";
                    LStarted = false;
                });
            }
            return RedirectToPage("/Launcher");
        }

        public IActionResult OnGetLoginStatus()
        {
            Dictionary<string, string> ans = new Dictionary<string, string>()
            {
                { "kind", LFileKind },
                { "name", LFileName },
                { "pgfc", LProgressedFileCnt },
                { "ttfc", LTotFileCnt },
                { "prog", LProgressPercentage },
                { "success", LSuccess }
            };
            return new JsonResult(ans);
        }

        public static MVersionCollection LoadVersions()
        {
            var mcp = new MinecraftPath(Path.Combine(Utility.GetWorkingDir(), ".minecraft"));
            var launcher = new CMLauncher(mcp);
            return launcher.GetAllVersions();
        }

        public IActionResult OnPostRegister()
        {
            Utility.OpenBrowser("https://login2.nide8.com:233/28f8f58a8a7f11e88feb525400b59b6a/register");
            return RedirectToPage("/Launcher");
        }

        private static string DLFileKind = string.Empty;
        private static string DLFileName = string.Empty;
        private static string DLProgressedFileCnt = "0";
        private static string DLTotFileCnt = "?";
        private static string DLProgressPercentage = "0";
        private static string DLSuccess = "false";
        private static bool DLStarted = false;

        public async Task<IActionResult> OnPostDownload()
        {
            if (LStarted == true)
            {
                HttpContext.Session.Set("LQueryProgress", true);
                return RedirectToPage("/Launcher");
            }
            HttpContext.Session.Set("DLQueryProgress", true);
            if (DLStarted == false) {
                DLStarted = true;
                DLFileKind = string.Empty;
                DLFileName = string.Empty;
                DLProgressedFileCnt = "0";
                DLTotFileCnt = "?";
                DLProgressPercentage = "0";
                DLSuccess = "false";
                var minecraft = new MinecraftPath(Path.Combine(Utility.GetWorkingDir(), ".minecraft"));
                var versionMetadatas = new MVersionLoader().GetVersionMetadatas(minecraft);
                var version = versionMetadatas.GetVersion("1.16.5");
                MDownloader downloader = new MDownloader(minecraft, version);
                downloader.ChangeFile += (e) => {
                    DLFileKind = e.FileKind.ToString();
                    DLFileName = e.FileName;
                    DLProgressedFileCnt = e.ProgressedFileCount.ToString();
                    DLTotFileCnt = e.TotalFileCount.ToString();
                };
                downloader.ChangeProgress += (sender, e) => {
                    DLProgressPercentage = e.ProgressPercentage.ToString();
                };
                await Task.Run(() => {
                    try
                    {
                        downloader.DownloadAll();
                        HttpContext.Session.Set("SuccessMsg", Translation.Translate("launcher-success-dl"));
                    } catch (Exception e)
                    {
                        HttpContext.Session.Set("ErrorMsg", e.Message);
                    }
                    HttpContext.Session.Set("DLQueryProgress", false);
                    DLSuccess = "true";
                    DLStarted = false;
                });
            }
            return RedirectToPage("/Launcher");
        }

        public IActionResult OnGetDownloadStatus()
        {
            Dictionary<string, string> ans = new Dictionary<string, string>()
            {
                { "kind", DLFileKind },
                { "name", DLFileName },
                { "pgfc", DLProgressedFileCnt },
                { "ttfc", DLTotFileCnt },
                { "prog", DLProgressPercentage },
                { "success", DLSuccess }
            };
            return new JsonResult(ans);
        }
    }
}
