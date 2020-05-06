using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Externals.Utilities;

namespace GameServer.Networking.Messaging
{
    public class MessagePooler
    {
        private static Dictionary<byte, Type> Messages { get; set; }

        private static void InitIfNeeded()
        {
            Messages = new Dictionary<byte, Type>();

            foreach (var t in Assembly.GetAssembly(typeof(Message)).GetTypes().Where(_ => _.IsSubclassOf(typeof(Message)) && _.IsClass && !_.IsAbstract && _.GetConstructor(Type.EmptyTypes) != null))
            {
                byte id = ((Message)Activator.CreateInstance(t)).MessageId;
                Messages.Add(id, t);
            }
        }

        public static Message GetMessage(byte id)
        {
            if (Messages == null) InitIfNeeded();

            if (!Messages.TryGetValue(id, out Type t))
            {
                LoggingUtils.LogIfDebug($"Message not found: {id}");
                return null;
            }

            return (Message)Activator.CreateInstance(t);
        }

        public static void Unload()
        {
            Messages.Clear();
            LoggingUtils.LogIfDebug("Message pool has been unloaded.");
        }
    }
}