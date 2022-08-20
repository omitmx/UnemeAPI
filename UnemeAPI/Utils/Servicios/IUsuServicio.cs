using UnemeAPI.Models;

namespace UnemeAPI.Utils.Servicios
{
    public interface IUsuServicio
    {
        cUsuRespuesta Autentificar(vmAccess model);
    }
}
