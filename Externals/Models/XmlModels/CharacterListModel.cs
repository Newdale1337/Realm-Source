using System.Collections.Generic;

namespace Externals.Models.XmlModels
{
    public class CharacterListModel
    {
        public int CharacterCount { get; set; }
        public List<ServerModel> Servers { get; set; }

        public CharacterListModel()
        {
            
        }
    }
}