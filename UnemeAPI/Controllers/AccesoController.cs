using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnemeAPI.Models;
using UnemeAPI.Utils.Servicios;

namespace UnemeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private IUsuServicio _usuServicio;
        public AccesoController(IUsuServicio usuServicio)
        {
            _usuServicio = usuServicio;
        }

        [HttpPost("login")]
        public IActionResult login(vmAccess model)
        {
            vmRespuesta oRespuesta = new vmRespuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    var oRes = _usuServicio.Autentificar(model);
                    if (oRes == null)
                    {
                        oRespuesta.Resultado = 0;
                        oRespuesta.Msg = "Usuario o contraseña incorrecta";
                        return BadRequest(oRespuesta);
                    }
                    else
                    {
                        if (oRes.token != null && oRes.token !="")
                        {

                            oRespuesta.Resultado = 1;
                            oRespuesta.Data = oRes.token;
                           

                        }
                        //if (oRes.info_usuario != null && oRes.info_usuario.id > 0)
                        //{

                        //    oRespuesta.Resultado = 1;
                        //    oRespuesta.Data = cFuncionesPublicas.SerializeJson(oRes);
                        //    //insertar log de sesion en Mongo
                        //    if (model.info_dispositivo != null)
                        //    {
                        //        model.info_dispositivo.login = model.login;
                        //        _sesionlog.Add(model.info_dispositivo);
                        //    }

                        //}
                        //else
                        //{
                        //    oRespuesta.Resultado = 0;
                        //    oRespuesta.Data = null;
                        //    oRespuesta.Msg = "Favor de verificar sus credenciales....!";
                        //}
                    }
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                                           .SelectMany(x => x.Errors)
                                                           .Select(x => x.ErrorMessage));
                    oRespuesta.Data = null;
                    oRespuesta.Msg = messages;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Msg = $"err:{ex.Message}, {Environment.NewLine}Favor de intentar mas tarde...!";
                #region LOG_ERROR
                ////guardar la excepcion
                //string UserId = User.Identity.Name.ToString();
                //var oError = cFuncionesPublicas.CrearLogCitesoft(UserId, ex);
                //var info_app = User.Claims.Where(d => d.Type == ClaimTypes.UserData).FirstOrDefault();
                //if (info_app != null)
                //{
                //    oError.info_dispositivo = JsonConvert.DeserializeObject<vmInfoDispositivoAdd>(info_app.Value);
                //}
                //_logError.Add(oError);
                //oRespuesta.Msg = $"Código:{oError.error_key}, {Environment.NewLine}Favor de intentar mas tarde...!";
                #endregion
            }

            return Ok(oRespuesta);
        }

        [HttpPost("AddFoto")]
        public async Task<vmRespuesta> AddFoto([FromForm] vmInfoUsuTestB model)
        {
           vmRespuesta oRespuesta = new vmRespuesta();
            if (model.foto!=null)
            {
                oRespuesta.Resultado = 1;

            }
            return oRespuesta;
        }


    }
}
