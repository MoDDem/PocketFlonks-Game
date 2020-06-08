using System;
using System.Linq;
using UnityEngine;

class ClientRecieve {
	public static void SpawnPlayer(PacketReader packet) {
		int id = packet.ReadInt32();
		string username = packet.ReadString();
		Vector3 position = packet.ReadVector3();

		PlayersManager.instance.SpawnPlayer(id, username, position);
	}

	public static void LocationData(PacketReader packet) {
		GameLocation location = (GameLocation) packet.ReadInt32();
		Vector3 position = packet.ReadVector3();

		//make transition to loading status panel
		ClientSceneManager.instance.ChangeGameLocation(location);
		ClientSceneManager.instance.OnLocationChanged += (a) => {
			var player = PlayersManager.instance.SpawnLocalPlayer(position, a);

			ClientSend.GetPlayersInLocation(player.GetComponent<Player>());
		};
	}

	public static void PlayerLogout(PacketReader packet) {
		string reason = packet.ReadString();

		SessionScript.DeleteSession();
		PlayersManager.instance.DespawnLocalPlayer();
		/*
		if(UIScript.instance != null) {
			UIScript.instance.CloseSplashScreen();
			UIScript.instance.OpenLoginPanel();
		}
		*/
		Debug.Log(reason);
	}

	public static void PlayerMovement(PacketReader packet) {
		int id = packet.ReadInt32();
		Vector3 position = packet.ReadVector3();

		PlayersManager.instance.players.Single(x => x.id == id).transform.position = position;
	}

	public static void PlayerDisconnect(PacketReader packet) {
		int id = packet.ReadInt32();

		PlayersManager.instance.DespawnPlayer(id);
	}

	public static void LoginPlayer(PacketReader packet) {
		string sessionID = packet.ReadString();
		int id = packet.ReadInt32();
		string email = packet.ReadString();
		string name = packet.ReadString();

		if(!PlayersManager.instance.gameObject.TryGetComponent(out Player player))
			player = PlayersManager.instance.gameObject.AddComponent<Player>();

		player.SetValues(
			("isLocalPlayer", true),
			("id", id),
			("sessionID", sessionID),
			("email", email),
			("username", name)
		);

		SessionScript.SaveSession(sessionID);
		/*
		UIScript.instance.OpenSplashScreen();
		UIScript.instance.CloseLoginPanel();*/
	}
}
