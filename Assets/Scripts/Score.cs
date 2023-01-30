using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class Score : NetworkBehaviour
{
    private Dictionary<ulong, int> playerscore = new Dictionary<ulong, int>();
    private TextMeshProUGUI scoreTMP;

    private void Start()
    {
        scoreTMP = GetComponent<TextMeshProUGUI>();
        scoreTMP.text = "score: " + 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddNewPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        playerscore.Add(clientId, 0);
    }
    [ClientRpc]
    private void ChangeScoreOnPlayerClientRpc(int score, ClientRpcParams clientRpcParams = default) => scoreTMP.text = "score: " + score;
    public void ChangePoint(ulong id, int change)
    {
        playerscore[id] += change;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { id }
            }
        };
        ChangeScoreOnPlayerClientRpc(playerscore[id], clientRpcParams);
    }
}