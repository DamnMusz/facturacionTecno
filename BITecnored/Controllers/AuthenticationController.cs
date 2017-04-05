using AngularWebAPI.WebAPI.Models.Authentication;
using AngularWebAPI.WebAPI.Modules;
using System.Web.Http;

namespace BITecnored.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate(AuthenticateViewModel viewModel)
        {
            BasicAuthHttpModule.UserLogin result = BasicAuthHttpModule.CheckUser(viewModel.Username, viewModel.Password);

            if (result.status == BasicAuthHttpModule.status_ok)
                return Ok(new { success = true, nombre = result.nombre, apellido = result.apellido });
            else
                return Ok(new { success = false, message = result.status });
        }
    }
}
