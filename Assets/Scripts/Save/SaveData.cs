using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Scripts.Save
{
    public class SaveData
    {
        private Dictionary<string, object> _data = new ();

        public Dictionary<string, object> Data { get { return _data; } set { _data = value; } }

        public string ToJson()
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(this, settings);
        }

        public static SaveData FromJson(string json)
        {
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
    }
}