using System.Collections;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZXing;
using ZXing.QrCode;

public class ConnectSceneManager : MonoBehaviour
{
    //Server
    public Server server;
    private ushort port = 8007;

    // UI
    [SerializeField] RawImage QRimage;
    private Texture2D storeEncodedTexture;

    private void Start()
    {
        // Generate QRcode
        storeEncodedTexture = new Texture2D(256, 256);
        EncodeTextToQRcode(getNetworkIp());
        server.Init(port);

        RegisterEvents();
    }
    public void OnclickRefresh()
    {
        EncodeTextToQRcode(getNetworkIp());
    }

    private string getLocalIp()
    {
        IPHostEntry host;
        string localIP = "?";
        host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily.ToString() == "InterNetwork")
            {
                localIP = ip.ToString();
            }
        }

        return localIP;
    }

    private string getNetworkIp()
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        NetworkInterface wirelessInterface = interfaces.FirstOrDefault(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && i.OperationalStatus == OperationalStatus.Up);

        if (wirelessInterface != null)
        {
            // Get the IPv4 address of the wireless interface
            IPInterfaceProperties properties = wirelessInterface.GetIPProperties();
            IPAddress ipAddress = properties.UnicastAddresses.FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address;

            if (ipAddress != null)
            {
                // Do something with the IPv4 address
                Debug.Log("Wireless IPv4 address: " + ipAddress.ToString());
                return ipAddress.ToString();
            }
            else
            {
                Debug.Log("Could not find wireless IPv4 address.");
            }
        }
        else
        {
            Debug.Log("Wireless interface not found.");
        }
        return null;
    }

    private void EncodeTextToQRcode(string text)
    {
        Color32[] _convertPixelTotexture = Encode(text, storeEncodedTexture.width, storeEncodedTexture.height);
        storeEncodedTexture.SetPixels32(_convertPixelTotexture);
        storeEncodedTexture.Apply();

        QRimage.texture = storeEncodedTexture;
    }
    private Color32[] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    private void storeUser(string access_token)
    {
        StartCoroutine(ApiCall.me(access_token, () =>
        {
            Debug.Log("login successfull");
        },
        () =>
        {
            Debug.Log("login fail");
        }));
    }

    #region Server
    private void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.S_USER += OnUser;
        NetUtility.S_PING += OnPing;
    }
    private void UnRegisterEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.S_USER -= OnUser;
        NetUtility.S_PING -= OnPing;
    }

    // Server
    private void OnPing(NetworkMessage msg, NetworkConnection cnn)
    {
        Debug.Log("hello ping");
        // NetPing netWelcome = msg as NetPing;
    }
    private void OnWelcomeServer(NetworkMessage msg, NetworkConnection cnn)
    {
        Debug.Log("new connector : " + cnn.InternalId);
        NetWelcome netWelcome = msg as NetWelcome;
        Server.Instance.SendToClient(cnn, netWelcome);

        SceneManager.LoadScene("StartMenu");
    }

    private void OnUser(NetworkMessage msg, NetworkConnection cnn)
    {
        NetUser currentUser = msg as NetUser;
        Debug.Log("currentUser:" + currentUser.access_token);
        storeUser(currentUser.access_token + "");
    }
    #endregion
}
