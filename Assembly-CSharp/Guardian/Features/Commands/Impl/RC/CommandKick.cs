﻿using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandKick : Command
    {
        public CommandKick() : base("kick", new string[0], "<id> [reason]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null || player.isLocal || player.isMasterClient) return;

            if (!PhotonNetwork.isMasterClient && !FengGameManagerMKII.OnPrivateServer)
            {
                string name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity();
                if (name.Length == 0)
                {
                    name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
                }
                FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, "/kick #" + id, name);
            }
            else
            {
                string reason = args.Length > 1 ? string.Join(" ", args.CopyOfRange(1, args.Length)) : "Kicked.";
                FengGameManagerMKII.Instance.KickPlayer(player, false, reason);

                if (FengGameManagerMKII.OnPrivateServer) return;

                GameHelper.Broadcast(GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity() + " has been kicked!");
                GameHelper.Broadcast($"Reason: \"{reason}\"");
            }
        }
    }
}
