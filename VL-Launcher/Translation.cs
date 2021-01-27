using System.Collections.Generic;
using System.Threading;

namespace VL_Launcher
{
    public class Translation
    {
        public static Dictionary<string, string> japanese = new Dictionary<string, string>()
        {
            { "head", "Minecraft Vast-Light" },
            { "welcome", "ようこそ" },
            { "welcome-intro-head", "はじめに" },
            { "welcome-intro-txt", "ログインの前に、指定された Minecraft バージョンをダウンロードしてください。" },
            { "welcome-auth-head", "ログイン" },
            { "welcome-auth-txt", "このフォルダに nide8auth.jar があるかどうかをご確認してください。https://login2.nide8.com:233/index/jar でダウンロードできます。" },
            { "welcome-unix-head", "macOS と Linux の方" },
            { "welcome-unix-txt", "ダウンロードは Java の実行可能ファイルが含まなくて、代わりに /usr/bin/java のまま使っています。" },
            { "launcher", "ランチャー" },
            { "launcher-memory", "メモリ制限 (MB)" },
            { "launcher-password", "パスワード" },
            { "launcher-username", "ID" },
            { "launcher-fullscreen", "全画面" },
            { "launcher-version", "バージョン" },
            { "launcher-register", "初めての方（中国語）" },
            { "launcher-login", "ログイン" },
            { "register", "登録" },
            { "error", "エラー" },
            { "success", "成功" },
            { "launcher-success-dl", "ダウンロードが完了しました" },
            { "launcher-success-login", "ロードが完了しました" },
            { "launcher-download", "ダウンロード" },
            { "current-dir", "パス" },
            { "file", "ファイル" },
            { "about", "について" },
            { "back", "戻る" },
            { "copyright", "著作" },
            { "oss", "このプロジェクトは GNU GPL v3 ライセンスでのオープンソースソフトウェアです。" },
            { "credits", "ありがとう" }
        };

        public static Dictionary<string, string> chinese = new Dictionary<string, string>()
        {
            { "head", "Minecraft 浩光域" },
            { "welcome", "欢迎" },
            { "welcome-intro-head", "首先" },
            { "welcome-intro-txt", "在登录之前，请下载指定的 Minecraft 版本。" },
            { "welcome-auth-head", "登录" },
            { "welcome-auth-txt", "请确认当前目录下是否有 nide8auth.jar 。您可以在 https://login2.nide8.com:233/index/jar 处下载。" },
            { "welcome-unix-head", "macOS 和 Linux 的用户" },
            { "welcome-unix-txt", "下载中不包含 Java 的可执行文件，会直接使用 /usr/bin/java。" },
            { "launcher", "启动器" },
            { "launcher-memory", "内存限制 (MB)" },
            { "launcher-password", "密码" },
            { "launcher-username", "ID" },
            { "launcher-fullscreen", "全屏" },
            { "launcher-version", "版本" },
            { "launcher-register", "注册" },
            { "launcher-login", "登录" },
            { "register", "注册" },
            { "error", "错误" },
            { "success", "成功" },
            { "launcher-success-dl", "下载完成" },
            { "launcher-success-login", "启动完成" },
            { "launcher-download", "下载" },
            { "current-dir", "当前目录" },
            { "file", "文件" },
            { "about", "关于" },
            { "back", "返回" },
            { "copyright", "版权" },
            { "oss", "本项目为使用 GNU GPL v3 协议的开源软件。" },
            { "credits", "致谢" }
        };

        public static Dictionary<string, string> english = new Dictionary<string, string>()
        {
            { "head", "Minecraft Vast-Light" },
            { "welcome", "Welcome" },
            { "welcome-intro-head", "Get Started" },
            { "welcome-intro-txt", "Before login, please download the specified Minecraft version required by the server." },
            { "welcome-auth-head", "Login" },
            { "welcome-auth-txt", "Please check if nide8auth.jar is in current directory. You can also download it from https://login2.nide8.com:233/index/jar." },
            { "welcome-unix-head", "macOS and Linux Users" },
            { "welcome-unix-txt", "Java executables are not included in download, and /usr/bin/java will be used." },
            { "launcher", "Launcher" },
            { "launcher-memory", "Memory (MB)" },
            { "launcher-password", "Password" },
            { "launcher-username", "ID" },
            { "launcher-fullscreen", "Fullscreen" },
            { "launcher-version", "Version" },
            { "launcher-register", "Register (Chinese)" },
            { "launcher-login", "Login" },
            { "register", "Register" },
            { "error", "Error" },
            { "success", "Success" },
            { "launcher-success-dl", "Download completed" },
            { "launcher-success-login", "Launch completed" },
            { "launcher-download", "Download" },
            { "file", "File" },
            { "current-dir", "Current directory" },
            { "about", "About" },
            { "back", "Back" },
            { "copyright", "Copyright" },
            { "oss", "This project is an open source software under GNU GPL v3 license." },
            { "credits", "Credits" }
        };

        public static string Translate(string key)
        {
            string lang = Thread.CurrentThread.CurrentUICulture.Name;
            string ans;
            if (lang.Contains("ja"))
            {
                if (!japanese.TryGetValue(key, out ans)) english.TryGetValue(key, out ans);
            } else if (lang.Contains("zh"))
            {
                if (!chinese.TryGetValue(key, out ans)) english.TryGetValue(key, out ans);
            } else
            {
                english.TryGetValue(key, out ans);
            }
            return ans;
        }
    }
}
