using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkModeGUI : MonoBehaviour
{
    public void StartServer() => NetworkManager.Singleton.StartServer();
    public void StartHost() => NetworkManager.Singleton.StartHost();
    public void StartClient() => NetworkManager.Singleton.StartClient();
}
