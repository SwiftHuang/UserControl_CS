using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace hwj.UserControls.Suggest
{
    [Serializable]
    public class SuggestValue
    {
        public string Key { get; set; }
        public string PrimaryValue { get; set; }
        public string SecondValue { get; set; }
        public object Item { get; set; }
    }
    [Serializable]
    public class SuggestList : List<SuggestValue>
    {
        public SuggestList DeepClone()
        {
            if (null == this) return null;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream) as SuggestList;
            }
        }
    }
}
