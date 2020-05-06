using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using Externals.Cryptography;
using Externals.Database;
using Externals.Models.FirestoreModels;
using Externals.Utilities;
using GameServer.Core.Objects;
using GameServer.Networking.Messaging;
using GameServer.Networking.Messaging.OutgoingMessages;

namespace GameServer.Networking
{
    public class NetworkClient
    {
        /* Network stuff */
        public Socket Socket { get; set; }
        private SocketAsyncEventArgs HeaderSAEA { get; set; }
        private byte[] HeaderBytes { get; set; }
        private byte MessageId { get; set; }
        private int ReceiveLength { get; set; }
        private RC4Cipher IncomingCipher { get; set; }
        private RC4Cipher OutgoingCipher { get; set; }
        private SocketAsyncEventArgs BodySAEA { get; set; }
        private byte[] MessageBytes { get; set; }

        /* End of network stuff */

        /* Client related stuff */
        public CoreManager Manager { get; set; }
        public GameServerDatabase Database { get; set; }
        public Player Player { get; set; }
        public AccountModel Account { get; set; }
        public ClientRandom ClientRandom { get; set; }

        /* End of client related stuff */

        public NetworkClient(Socket socket, GameServerDatabase db, CoreManager manager)
        {
            (Socket = socket).NoDelay = true;
            Database = db;
            Manager = manager;

            IncomingCipher = new RC4Cipher("6a39570cc9de4ec71d64821894c79332b197f92ba85ed281a023".Substring(0, 26));
            OutgoingCipher = new RC4Cipher("6a39570cc9de4ec71d64821894c79332b197f92ba85ed281a023".Substring(26));

            HeaderSAEA = new SocketAsyncEventArgs();
            HeaderSAEA.Completed += HeaderReceived;
            HeaderSAEA.SetBuffer(HeaderBytes = new byte[5], 0, 5);

            BodySAEA = new SocketAsyncEventArgs();
            BodySAEA.Completed += BodyReceived;

            if (!Socket.ReceiveAsync(HeaderSAEA))
                HeaderReceived(this, HeaderSAEA);
        }

        private void HeaderReceived(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.BytesTransferred != 5)
                {
                    Disconnect("e.BytesTransferred != 5");
                    return;
                }

                MessageId = HeaderBytes[4];
                ReceiveLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(HeaderBytes, 0)) - 5;
                BodySAEA.SetBuffer(MessageBytes = new byte[ReceiveLength], 0, MessageBytes.Length);

                Socket.ReceiveAsync(BodySAEA);
            }
            catch (Exception exception)
            {
                LoggingUtils.LogErrorIfDebug(exception.ToString());
            }
        }

        private void BodyReceived(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.BytesTransferred < ReceiveLength)
                {
                    Disconnect("if (e.BytesTransferred < ReceiveLength)");
                    return;
                }

                var msg = MessagePooler.GetMessage(MessageId);
                if (msg != null)
                {
                    IncomingCipher.ProcessBytes(MessageBytes, 0, MessageBytes.Length);
                    using (MemoryStream ms = new MemoryStream(MessageBytes))
                    using (MessageReader rdr = new MessageReader(ms))
                        msg.Read(rdr);

                    msg.Handle(this);
                }

                if (Socket?.Connected == true)
                {
                    HeaderSAEA.SetBuffer(HeaderBytes = new byte[5], 0, 5);
                    if (!Socket.ReceiveAsync(HeaderSAEA))
                        HeaderReceived(this, HeaderSAEA);
                }
            }
            catch (Exception exception)
            {
                LoggingUtils.LogErrorIfDebug(exception.ToString());
            }
        }

        public void SendFailure(string description, int errorId) => SendMessage(new FailureMessage(description, errorId));

        public void SendMessage(Message msg)
        {
            byte[] data = MessageToBytes(msg);
            if (data == null)
            {
                Disconnect("data == null");
                return;
            }

            Socket.Send(data);
        }

        private byte[] MessageToBytes(Message msg)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();
                using MessageWriter wrt = new MessageWriter(ms);
                msg.Write(wrt);

                int len = (int)ms.Position;
                byte[] lengthBytes = BitConverter.GetBytes(IPAddress.NetworkToHostOrder(len + 5));

                var data = new byte[ms.Position + 5];
                data[0] = lengthBytes[0];
                data[1] = lengthBytes[1];
                data[2] = lengthBytes[2];
                data[3] = lengthBytes[3];
                data[4] = msg.MessageId;

                var streamBuffer = ms.GetBuffer();
                Array.Resize(ref streamBuffer, len);

                OutgoingCipher.ProcessBytes(streamBuffer, 0, streamBuffer.Length);
                Buffer.BlockCopy(streamBuffer, 0, data, 5, streamBuffer.Length);

                return data;
            }
            catch (Exception e)
            {
                LoggingUtils.LogErrorIfDebug(e.ToString());
                return null;
            }
        }

        public void Disconnect(string reason = null)
        {
            if (reason != null)
                LoggingUtils.LogIfDebug(reason);
            Unload();
        }

        public void Unload()
        {
            Player?.Unload(Database);
            Socket?.Dispose();
            BodySAEA?.Dispose();
            HeaderSAEA?.Dispose();
            Database = null;
            IncomingCipher = null;
            OutgoingCipher = null;
            MessageBytes = null;
            HeaderBytes = null;
            Manager.RemoveClient(this);
        }
    }
}