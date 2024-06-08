using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Proto2CS
{

    [MenuItem("Tools/Proto2CS")]
    public static void doProto2CS()
    {
        //Initailize Path
        var rootPath = Environment.CurrentDirectory;
        var protoPath = Path.Combine(rootPath, "Proto/");

        //Get Proto Compiler 
        string protoc;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            protoc = Path.Combine(protoPath, "protoc.exe");
        }
        else
        {
            protoc = Path.Combine(protoPath, "protoc");
        }

        //Get Output Path
        string hotfixMessagePath = Path.Combine(rootPath, "Assets", "Scripts", "ProtoMessage");

        //Prepare Commnad Argument
        string argument = $"--csharp_out=\"{hotfixMessagePath}\" --proto_path=\"{protoPath}\" test_message.proto";

        //Excute
        Run(protoc, argument, isWaitExit: true);

        //Update
        AssetDatabase.Refresh();

    }

    public static Process Run(string exe, string argument, string rootPath = ".", bool isWaitExit = false)
    {
        try
        {
            bool isRedirectStandardOutput = true;
            bool isRedirectStandardError = true;
            bool isUseShaellExecute = false;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                isRedirectStandardOutput = false;
                isRedirectStandardError = false;
                isUseShaellExecute = true;
            }

            if (isWaitExit)
            {
                isRedirectStandardOutput = true;
                isRedirectStandardError = true;
                isUseShaellExecute = false;
            }

            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = exe,
                Arguments = argument,
                CreateNoWindow = true,
                UseShellExecute = isUseShaellExecute,
                WorkingDirectory = rootPath,
                RedirectStandardOutput = isRedirectStandardOutput,
                RedirectStandardError = isRedirectStandardError,
            };

            Process process = Process.Start(info);

            if (isWaitExit)
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
                }
            }

            return process;
        }
        catch (Exception e)
        {
            throw new Exception($"dir: {Path.GetFullPath(rootPath)}, command: {exe} {argument}", e);
        }
    }
}


