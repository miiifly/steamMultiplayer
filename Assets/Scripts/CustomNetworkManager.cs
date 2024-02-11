using Game.Multiplayer.Lobby;
using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Multiplayer.Network
{
    public class CustomNetworkManager : NetworkManager
    {

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);

            CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, numPlayers - 1);

            var playerInfoDisplay = conn.identity.GetComponent<PlayerInfoDisplay>();
            playerInfoDisplay.SetSteamId(steamId.m_SteamID);
        }
    }
}

