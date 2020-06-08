using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersManager : MonoBehaviour {
    public static PlayersManager instance;


    public List<Player> players = new List<Player>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }

    public GameObject SpawnLocalPlayer(Vector3 position, GameLocation location) {
        GameObject player = Instantiate(localPlayerPrefab, position, Quaternion.identity);
        player.GetComponent<Player>().SetValues(
            ("isLocalPlayer", true),
            ("sessionID", gameObject.GetComponent<Player>().sessionID),
            ("id", gameObject.GetComponent<Player>().id),
            ("email", gameObject.GetComponent<Player>().email),
            ("username", gameObject.GetComponent<Player>().username),
            ("location", location)
        );

        players.Add(player.GetComponent<Player>());

        return player;
    }

    public GameObject SpawnPlayer(int uID, string uName, Vector3 position) {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        player.GetComponent<Player>().SetValues(
            ("id", uID),
            ("username", uName)
        );

        players.Add(player.GetComponent<Player>());

        return player;
    }

    public void DespawnPlayer(int id) {
        Player player = players.Single(x => x.id == id);

        Destroy(player.gameObject);
        players.Remove(player);
    }

    public void DespawnLocalPlayer() {
        Player player = GetComponent<Player>();

        players.Remove(player);
        Destroy(player);

        if(players.Contains(player))
            Destroy(players[players.IndexOf(player)].gameObject);
    }
}
