using UnityEngine;
using Telepathy;
using System;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;

public class ClientNetwork : MonoBehaviour
{
    private static ClientNetwork instance;

    private DateTime connectionLostTime = DateTime.MinValue;
    private int attempt = -1;

    public string ip = "127.0.0.1";
    public int port = 25698;
    public static Client client = new Client();

    void Awake() {
        Application.runInBackground = true;

        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        client.Connect(ip, port);
    }

    void OnApplicationQuit() {
        client.Disconnect();
    }

    void Update() {
        if(client.Connected) {
            if(UIScript.instance != null) {
                if(UIScript.instance.connectionLostPanel.activeSelf) {
                    UIScript.instance.connectionLostPanel.SetActive(false);
                    connectionLostTime = DateTime.MinValue;
                    attempt = -1;
                }
            }
            
            while(client.GetNextMessage(out Message msg)) {
                switch(msg.eventType) {
                    case Telepathy.EventType.Connected:
                        OnPlayerConnected();
                        break;
                    case Telepathy.EventType.Data:
                        OnServerMessageReceived(msg);
                        break;
                    case Telepathy.EventType.Disconnected:
                        OnPlayerDisconnected();
                        break;
                }
            }
        }else if(!client.Connected & !client.Connecting) {
            if(UIScript.instance == null)
                return;

            if(attempt == -1) {
                attempt = 0;
                OnPlayerDisconnected();
			}

            if(connectionLostTime == DateTime.MinValue) {
                UIScript.instance.connectionLostPanel.GetComponentInChildren<Text>().text = "Connection lost.\nRetry in 15s...";

                connectionLostTime = DateTime.Now;
                /*
				if(!UIScript.instance.connectionLostPanel.activeSelf) {
                    UIScript.instance.connectionLostPanel.SetActive(true);
                    UIScript.instance.CloseSplashScreen();
				}
                */
			}

            if(TimeSpan.FromSeconds(15) < DateTime.Now - connectionLostTime) {
                UIScript.instance.connectionLostPanel.GetComponentInChildren<Text>().text = "Retrying...";

                connectionLostTime = DateTime.MinValue;
                client.Connect(ip, port);

                attempt++;
			}
        }
    }

    private void OnPlayerDisconnected() {
        Debug.Log("Disconnected");

        PlayersManager.instance.DespawnLocalPlayer();
    }

    private void OnPlayerConnected() {
        Debug.Log("Connected");
        
        string sessionID = SessionScript.LoadSession();
        if(sessionID != null) {
            ClientSend.LoginUsingSession(sessionID);
		} else if(UIScript.instance != null) {
            /*
            UIScript.instance.CloseSplashScreen();
            UIScript.instance.OpenLoginPanel();
            */
		}
    }

    private void OnServerMessageReceived(Message msg) {
        PacketReader packet = new PacketReader(msg.data);
        ServerHeaders header = (ServerHeaders) packet.ReadUInt16();

		switch(header) {
			case ServerHeaders.Text:
				Debug.Log(packet.ReadString());
				break;
			case ServerHeaders.PlayerLoginData:
				ClientRecieve.LoginPlayer(packet);
				break;
			case ServerHeaders.PlayerDisconnect:
				ClientRecieve.PlayerDisconnect(packet);
				break;
			case ServerHeaders.PlayerPosition:
				ClientRecieve.PlayerMovement(packet);
				break;
			case ServerHeaders.PlayerRotation:
				Debug.LogWarning("Somehow we recieved player rotation");
				break;
			case ServerHeaders.JoinLocationData:
				ClientRecieve.LocationData(packet);
				break;
			case ServerHeaders.SpawnPlayerInLocation:
				ClientRecieve.SpawnPlayer(packet);
				break;
			case ServerHeaders.PlayerLogout:
                ClientRecieve.PlayerLogout(packet);
				break;
			default:
				throw new Exception("Looks like you forget to impliment Server Headers data.");
		}
	}
}
