using System.Collections.Generic;
using System.Net.Http.Headers;

namespace GameServer.Core.Objects
{
    public class ObjectStorage
    {
        public Dictionary<int, BaseObject> ObjectMap { get; set; }

        public ObjectStorage()
        {
            ObjectMap = new Dictionary<int, BaseObject>();
        }
    }
}