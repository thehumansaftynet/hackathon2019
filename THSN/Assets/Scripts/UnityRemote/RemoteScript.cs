using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class RemoteScript : MonoBehaviour
{
    private bool running = true;

    private TcpClient tcpClient;
    private NetworkStream serverStream;
    private bool ConnectedToServer = false;

    public UnityEvent<int> ButtonPressed;
    public UnityEvent<string> QRCodeEntered;
    public bool StartServerInUnity = true;

    [Header("Server settings")]
    [Range(1f, 30f)]
    public float connectionAttemptInterval = 10f;
    public UnityEngine.Object ServerFile;
    [Header("Show Console false + logging true = Server Ouput in Unity")]
    public bool ShowServerConsole = false;
    public bool logging = false;

    private Queue<string> SendQueue;
    private System.Diagnostics.Process ServerProcess;
    private const int messageSize = 6;

    void Awake()
    {
        gameObject.AddComponent<UnityMainThreadDispatcher>();
        SendQueue = new Queue<string>();

        ButtonPressed = ButtonPressed ?? new RemoteButtonPressedEvent();
        QRCodeEntered = QRCodeEntered ?? new RemoteQRCodeScannedEvent();

        //StartClient();

        if (StartServerInUnity)
        {
            StartServer();
        }

        StartCoroutine(ConnectToServer());
    }

    private IEnumerator ConnectToServer()
    {
        while (running)
        {
            if (!ConnectedToServer)
            {
                yield return new WaitForSeconds(1f);
                if (logging)
                    Debug.Log("Try connecting to Remote server.");
                SafeAsync(StartClientAsync(), (e) => Debug.LogError(e));
            }
            else
            {
                SendMessageToServer("ping");
            }

            yield return new WaitForSeconds(connectionAttemptInterval);
        }
    }

    public static async void SafeAsync(Task task, Action<Exception> handler = null)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            handler?.Invoke(ex);
        }
    }
    public void StartServer()
    {
        var pathToFile = $@"{Application.streamingAssetsPath}/Server/server.js";
        var pathToFolder = pathToFile.Substring(0, pathToFile.LastIndexOf("/")).Replace("/", "\\");
        var filename = pathToFile.Substring(pathToFile.LastIndexOf("/") + 1);

        if (logging)
            Debug.Log("Path is " + pathToFolder + "  " + Directory.GetCurrentDirectory() + " - " + pathToFolder + " - " + filename);
        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
        {
            WorkingDirectory = $@"{pathToFolder}",
            UseShellExecute = ShowServerConsole,   // This is important
            CreateNoWindow = !ShowServerConsole,     // This is what hides the command window.
            FileName = @"node.exe",
            RedirectStandardOutput = !ShowServerConsole && logging,
            Arguments = $@"""{pathToFolder}\{filename}"""
        };

        ServerProcess = System.Diagnostics.Process.Start(psi);
        if (logging)
        {
            var serverTask = Task.Factory.StartNew(() =>
            {
                while (!ServerProcess?.HasExited ?? false)
                {
                    string output = ServerProcess.StandardOutput.ReadLine();
                    Debug.Log($"SERVER OUTPUT: {output}");
                }
            });
        }
    }

    public async Task StartClientAsync()
    {
        ConnectedToServer = true;
        tcpClient = new TcpClient();
        serverStream = default;

        try
        {
            var task = tcpClient.ConnectAsync("server.srings.eu", 4050);    
            await task;

            SendMessageToServer("ping");

            Debug.Log("Connected to RemoteControl Server");

            if (StartServerInUnity)
            {
                SendMessageToServer("THIS_IS_UNITY_SERVER");
            }

            Task sendThread = Task.Factory.StartNew(() =>
            {
                while (ConnectedToServer)
                {
                    // Write bytes
                    serverStream = tcpClient.GetStream();

                    lock (SendQueue)
                    {
                        try
                        {
                            while (SendQueue.Count > 0)
                            {
                                byte[] outStream2 = Encoding.UTF8.GetBytes(EncodeMessage(SendQueue.Dequeue()));
                                serverStream.Write(outStream2, 0, outStream2.Length);
                                serverStream.Flush();
                            }
                        }
                        catch (SocketException e)
                        {
                            Debug.LogWarning($"Error Sending Messages. {e}");
                        }
                        catch (IOException e)
                        {
                            Debug.LogWarning($"IO Error Sending Messages. {e}");
                            ConnectedToServer = false;
                        }

                    }

                    Thread.Sleep(30);
                }
            });

            Task ReceiveThread = Task.Factory.StartNew(() =>
            {
                while (ConnectedToServer)
                {
                    // Read bytes
                    serverStream = tcpClient.GetStream();
                    serverStream.ReadTimeout = 10;


                    UTF8Encoding encoder = new UTF8Encoding();

                    byte[] messageLength = new byte[messageSize];
                    byte[] message = new byte[4096];
                    int bytesRead;
                    bytesRead = 0;
                    if (serverStream.DataAvailable)
                    {
                        try
                        {
                            // Read message length
                            bytesRead = serverStream.Read(message, 0, messageSize);

                            if (int.TryParse(encoder.GetString(message, 0, bytesRead), out int n))
                            {
                                message = new byte[n];

                                // Read message
                                bytesRead = serverStream.Read(message, 0, n);
                                ReceivedMessage(encoder.GetString(message, 0, bytesRead));
                            }

                        }
                        catch (SocketException e)
                        {
                            // a socket error has occured 
                            Debug.LogWarning($"Error Receiving Messages. {e}");
                        }


                    }

                    Thread.Sleep(30);
                }
            });
        }
        catch (SocketException e)
        {
            Debug.LogWarning($"Connection couldn't be established! {e}");
            ConnectedToServer = false;
        }

    }

    private string EncodeMessage(string message)
    {
        var l = $"{UTF8Encoding.UTF8.GetByteCount(message)}";
        l = $"{message.Length}";

        for (int i = l.Length; i < messageSize; i++)
        {
            l = "0" + l;
        }

        message = l + message;

        return message;
    }

    private void ReceivedMessage(string v)
    {
        if (logging)
            Debug.Log($"Received Message: { v}");
        if (v.StartsWith("Pressed"))
        {
            v = v.Split('_')[1];
            int buttonIndex = 0;
            int.TryParse(v, out buttonIndex);

            if (buttonIndex >= 0)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => ButtonPressed.Invoke(buttonIndex));
            }
            // TEST
            SendMessageToServer($"Sie haben Knopf {buttonIndex} geklickt!");
        }
        else if (v.StartsWith("pong"))
        {

        }
        else if (v.StartsWith("QR"))
        {
            var rgx = new System.Text.RegularExpressions.Regex(@"^[\w]{2}_[\w]{2}_\d{2}_\d{4}$");
            var code = v.Substring(3);

            if (rgx.IsMatch(code))
            {
                SendMessageToServer($"Teilnehmer ID {code} gescannt!");
                UnityMainThreadDispatcher.Instance().Enqueue(() => QRCodeEntered.Invoke(code));
            }
            else
            {
                SendMessageToServer($"QR Code {v} gescannt! Keine Teilnehmerkennung erkannt!");
            }
        }
        else
        {
            Debug.LogWarning($"RemoteControl received unhandled Message: { v}");
        }
    }

    public void SendMessageToServer(string message)
    {
        lock (SendQueue)
        {
            SendQueue.Enqueue(message);
        }
    }

    private void OnApplicationQuit()
    {
        running = false;
        if (!ServerProcess?.HasExited ?? false)
        {
            ServerProcess?.Kill();
        }
    }

    public void SetServerButtonText(string text, int buttonID)
    {
        string msg = "CT_" + buttonID + "_" + text;
        SendMessageToServer(msg);
    }
}

[System.Serializable]
public class RemoteButtonPressedEvent : UnityEvent<int>
{
}

[System.Serializable]
public class RemoteQRCodeScannedEvent : UnityEvent<string>
{
}

