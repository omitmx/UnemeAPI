using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace UnemeAPI.Utils
{
    public class cFuncionesPublicas
    {
        // usuarios Seguridad Encriptar
        public static string GetSha256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[]? stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
      
    }
}
