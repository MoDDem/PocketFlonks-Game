using System;
using UnityEngine;

public class ClientSend {
	public static void StringMessage(string msg) {
		PacketWriter packet;
		using(packet = new PacketWriter()) {
			packet.Write((ushort) ServerHeaders.Text);
			packet.Write(msg);
		}

		ClientNetwork.client.Send(packet.GetBytes());
	}
	
	//rewrite the way how we joined game
	public static void RequestJoinGame(Player player) {
		PacketWriter packet;
		using(packet = new PacketWriter()) {
			packet.Write((ushort) ClientHeaders.RequestJoinGameData);
		}

		ClientNetwork.client.Send(packet.GetBytes());
	}

	public static void GetPlayersInLocation(Player player) {
		PacketWriter packet;
		using(packet = new PacketWriter()) {
			packet.Write((ushort) ClientHeaders.GetPlayersInLocation);
		}

		ClientNetwork.client.Send(packet.GetBytes());
	}

	public static void PlayerMovement(int playerID, Vector2 movementInput) {
		PacketWriter packet;
		using(packet = new PacketWriter()) {
			packet.Write((ushort) ClientHeaders.PlayerMovement);
			packet.Write(playerID);
			packet.Write(movementInput);
		}

		ClientNetwork.client.Send(packet.GetBytes());
	}

	public static void RegisterUser(string name, string email, string password) {
		PacketWriter packet;
		using(packet = new PacketWriter()) {
			packet.Write((ushort) ClientHeaders.RegisterPlayer);
			packet.Write(name);
			packet.Write(email);
			packet.Write(password);
			packet.Write((ushort) DeviceType.Desktop);
		}

		ClientNetwork.client.Send(packet.GetBytes());
	}

	public static void LoginPlayer(string email, string pass) {
		PacketWriter packet;
		using(packet = new PacketWriter()) {
			packet.Write((ushort) ClientHeaders.LoginPlayerByLocal);
			packet.Write(email);
			packet.Write(pass);
			packet.Write((ushort) DeviceType.Desktop);
		}

		ClientNetwork.client.Send(packet.GetBytes());
	}

	public static void LoginUsingSession(string sessionID) {
		PacketWriter packet;
		using(packet = new PacketWriter()) {
			packet.Write((ushort) ClientHeaders.LoginPlayerBySession);
			packet.Write(sessionID);
			packet.Write((ushort) DeviceType.Desktop);
		}

		ClientNetwork.client.Send(packet.GetBytes());
	}
}
