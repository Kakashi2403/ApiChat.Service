using ApiChat.Business.TokenService;
using ApiChat.Domain.ModelsDTO.TokenDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Session.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public SessionController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<TokenResponse>> GetToken(Credentials credentials) => Ok(await _tokenService.GetToken(credentials));
    }
}
