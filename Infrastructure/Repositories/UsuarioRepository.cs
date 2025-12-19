using Dapper;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Data;

namespace BibliotecaApi.Infrastructure.Repositories;

public class UsuarioRepository
{
    private readonly DbSession _session;

    public UsuarioRepository()
    {
        _session = new DbSession(ConfigurationHelper.GetConfiguration());
    }

    public async Task<int> Cadastrar(UsuarioEntity usuario)
    {
        const string sql = "INSERT INTO Usuarios (nome, cpf, email, senha_hash) VALUES (@nome, @cpf, @email, @senha_hash) RETURNING id";

        var parameters = new
        {
            nome = usuario.Nome,
            cpf = usuario.CPF,
            email = usuario.Email,
            senha_hash = usuario.SenhaHash
        };

        using (var connection = _session.Connection)
        {
            var result = await _session.Connection.QueryFirstAsync<int>(sql, parameters);
            return result;
        }
    }

    public async Task<bool> UsuarioExisteComCpf(string cpf)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM Usuarios WHERE cpf = @cpf) AS UsuarioExiste";

        using var connection = _session.Connection;
        return await _session.Connection.QueryFirstAsync<bool>(sql, new { cpf });
    }

    public async Task<bool> UsuarioExisteComEmail(string email)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM Usuarios WHERE email = @email) AS UsuarioExiste";

        using var connection = _session.Connection;
        return await _session.Connection.QueryFirstAsync<bool>(sql, new { email });
    }

    public async Task<UsuarioEntity?> ObterPorEmail(string email)
    {
        const string sql = "SELECT id, nome, cpf, email, senha_hash as SenhaHash FROM Usuarios WHERE email = @email";

        using var connection = _session.Connection;
        var usuario = await _session.Connection.QueryFirstOrDefaultAsync<UsuarioEntity>(sql, new { email });

        return usuario;
    }
}
