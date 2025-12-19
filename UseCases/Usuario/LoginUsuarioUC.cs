using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Response;
using BibliotecaApi.UseCases.Usuario.DTO;
using Microsoft.IdentityModel.Tokens;

namespace BibliotecaApi.UseCases.Usuario;

public class LoginUsuarioUC
{
    private readonly UsuarioRepository _usuarioRepository = new UsuarioRepository();
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    public async Task<UseCaseResponse<TokenDTO>> Execute(LoginUsuarioInput input)
    {
        var usuario = await _usuarioRepository.ObterPorEmail(input.Email);

        if (usuario == null)
            return UseCaseResponse<TokenDTO>.Falha("Credenciais inválidas.");

        if (!usuario.VerificarSenha(input.Senha))
            return UseCaseResponse<TokenDTO>.Falha("Credenciais inválidas.");

        // Geração de token JWT ou similar para autenticação
        var tokenDto = GerarTokenParaUsuario(usuario);

        if (tokenDto == null)
            return UseCaseResponse<TokenDTO>.Falha("Erro ao gerar token de autenticação.");

        return UseCaseResponse<TokenDTO>.Ok(tokenDto);
    }

    private TokenDTO GerarTokenParaUsuario(UsuarioEntity usuario)
    {
        // Implementar a lógica de geração de token JWT aqui
        var jwt = _configuration.GetSection("Jwt");
        if (jwt == null)
            throw new Exception("Chave JWT não configurada.");

        var secret = Encoding.UTF8.GetBytes(jwt["Secret"]!);
        if (secret.Length == 0)
            throw new Exception("Chave secreta JWT não configurada.");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
        };

        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256
        );

        var configExpiracao = jwt["ExpiresMinutes"] ?? "120";
        var expiracao = DateTime.UtcNow.AddMinutes(double.Parse(configExpiracao));

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expiracao,
            signingCredentials: credenciais
        );

        return new TokenDTO
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiracao
        };
    }
}
