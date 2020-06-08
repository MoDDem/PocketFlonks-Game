public enum ServerHeaders : ushort {
	Text,

	PlayerLoginData = 7,
	PlayerDisconnect,
	PlayerLogout,

	PlayerPosition,
	PlayerRotation,

	JoinLocationData,
	SpawnPlayerInLocation
}

public enum ClientHeaders : ushort {
	Text,

	RegisterPlayer,
	LoginPlayerByLocal,
	LoginPlayByGoogle,
	LoginPlayerBySession,

	RequestJoinGameData,
	GetPlayersInLocation,

	PlayerMovement
}