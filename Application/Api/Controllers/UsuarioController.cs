using BibliotecaApi.UseCases.Usuario;
using BibliotecaApi.UseCases.Usuario.DTO;
using BibliotecaApi.Application.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UsuarioController : Controller
{
    private readonly CadastrarUsuarioUC _cadastrarUsuarioUC = new CadastrarUsuarioUC();
    private readonly LoginUsuarioUC _loginUsuarioUC = new LoginUsuarioUC();
    
    [HttpPost]
    [SwaggerOperation(Summary = "Cadastra um novo usuário retornando o seu respectivo Id")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<int>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarUsuarioInputDTO input)
    {
        try
        {
            var response = await _cadastrarUsuarioUC.Execute(input);
            if (!response.Sucesso)
                return BadRequest(ApiResponse<int?>.Falha(response.MensagemErro!));
            
            return Ok(ApiResponse<int>.Ok(response.Dados));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha("Erro ao cadastrar usuário: " + ex.Message));
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Efetua o login um novo token de autenticação")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<int>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioInput input)
    {
        try
        {
            var response = await _loginUsuarioUC.Execute(input);
            if (!response.Sucesso)
                return BadRequest(ApiResponse<int?>.Falha(response.MensagemErro!));
            
            return Ok(ApiResponse<TokenDTO>.Ok(response.Dados));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha("Erro ao cadastrar usuário: " + ex.Message));
        }
    }
}
