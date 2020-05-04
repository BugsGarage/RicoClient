using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards.Converters
{
    public class CardTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            CardType outVal = 0;
            if (reader.TokenType == JsonToken.String)
            {
                string value = reader.Value.ToString();
                var values = value.Split('|');
                foreach (var val in values)
                {
                    outVal |= (CardType) Enum.Parse(objectType, val);
                }
            }

            return outVal;
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();  // ToDo
        }
    }
}
