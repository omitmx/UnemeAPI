using System.Text.Json;

namespace UnemeAPI.Utils
{
    public class cFuncionesJson<Tmodel>
    {
        Tmodel data;
        public cFuncionesJson(Tmodel data)
        {
            this.data = data;
        }

        public static string SerializeJson(Tmodel obj)
        {
            return JsonSerializer.Serialize(obj);
            //return JsonSerializer.Serialize(obj, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //JsonSerializer.Deserialize<model>(obj)
        }
        public static Tmodel DeserializeJson(string StrJson)
        {
            return JsonSerializer.Deserialize<Tmodel>(StrJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
