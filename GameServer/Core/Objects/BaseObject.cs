using Externals.Models.GameDataModels;

namespace GameServer.Core.Objects
{
    public class BaseObject
    {
        public int ObjectType { get; set; }
        public int Defense { get; set; }
        public ObjectProperties Props { get; set; }
        public BaseObject(int type)
        {
            ObjectType = type;
            Props = ObjectLibrary.GetProperties(ObjectType);
            if (Props == null) return;
        }
    }
}