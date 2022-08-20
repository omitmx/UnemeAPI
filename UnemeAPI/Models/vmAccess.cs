using System.ComponentModel.DataAnnotations;
using UnemeAPI.Models.Sesion;
using UnemeAPI.Models.Usuarios;

namespace UnemeAPI.Models
{
    public class vmAccess
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string pwd { get; set; }       
        public vmInfoDispositivo? info_dispositivo { get; set; }
        public vmAccess(vmInfoDispositivo info_dispositivo)
        {
            login = "";
            pwd = "";
            this.info_dispositivo = info_dispositivo;
        }
    }
}
