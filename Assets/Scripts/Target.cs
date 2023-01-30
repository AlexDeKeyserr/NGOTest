using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Target : NetworkBehaviour
{
    private Animator animator;
    private Score score;

    private bool blockAnim;

    private void Start()
    {
        animator = GetComponent<Animator>();
        score = FindObjectOfType<Score>();
    }

    public void TargetHit()
    {
        animator.SetTrigger("hit");
        blockAnim = true;
        HitAnimationServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void TargetHitServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        score.ChangePoint(clientId, 1);
    }

    [ServerRpc(RequireOwnership = false)]
    private void HitAnimationServerRpc() => HitAnimationClientRpc();
    [ClientRpc]
    private void HitAnimationClientRpc()
    {
        if (!blockAnim)
            animator.SetTrigger("hit");

        blockAnim = false;
    }
}