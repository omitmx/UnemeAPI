using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnemeAPI.Models;
using UnemeAPI.Models.DBs;
using UnemeAPI.Models.Sesion;
using UnemeAPI.Models.Usuarios;
using UnemeAPI.Utils.Jwt;

namespace UnemeAPI.Utils.Servicios
{
    public class cUsuServicio : IUsuServicio
    {
        private readonly AppSettings _appSettings;
        private readonly PGCnn _pgCnn;
        public cUsuServicio(IOptions<AppSettings> appSettings, IOptions<PGCnn> pgCnn)
        {
            _appSettings = appSettings.Value;
            _pgCnn = pgCnn.Value;
        }

        public cUsuRespuesta Autentificar(vmAccess model)
        {
            cUsuRespuesta oUsuRes = new cUsuRespuesta();
            oUsuRes.token = "";
            try
            {
               
                string pwdEncryp = cFuncionesPublicas.GetSha256(model.pwd);
                string qUsu = "select usuario_key id, nombres, app, apm, correo, tipo_usuario_link tipo_usuario_id,   estatus_link estatus_id, login from cat_usuarios  where login=@login  and pwd=@pwd";
                using (var conDbPG = new NpgsqlConnection(_pgCnn.ConDBContext)) {
                    var oReq = conDbPG.Query<vmUsuarioSesion>(qUsu, new { login = model.login,pwd=pwdEncryp }).FirstOrDefault();
                    if (oReq != null)
                    {
                        oUsuRes.token= GetToken(model);
                    }
                }

            }
            catch (Exception ex)
            {
                //falta log
                
            }
            return oUsuRes;
        }

        private string GetToken(vmAccess model)
        {
            var tokenHdler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto??"");
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier,model.login??""),
                    new Claim(ClaimTypes.Name,model.login??""),
                    new Claim(ClaimTypes.UserData,cFuncionesJson<vmInfoDispositivo>.SerializeJson(model.info_dispositivo))
                    }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHdler.CreateToken(tokenDesc);
            return tokenHdler.WriteToken(token);
        }
    }
}
