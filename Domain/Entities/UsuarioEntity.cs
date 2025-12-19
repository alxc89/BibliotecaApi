using System.Text.Json.Serialization;
using BCrypt.Net;

namespace BibliotecaApi.Domain.Entities;

public class UsuarioEntity
{
    public int Id { get; set; }
    public string Nome { get; private set; } = string.Empty;
    public string CPF { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;

    public void Cadastrar(string nome, string cpf, string email, string senha)
    {
        if(string.IsNullOrWhiteSpace(email))
            throw new Exception("O email do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(senha))
            throw new Exception("A senha do usuário é obrigatória.");

        if (senha.Trim().Length < 6)
            throw new Exception("A senha do usuário deve ter pelo menos 6 caracteres.");

        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(cpf))
            throw new Exception("O CPF é obrigatório.");

        if (cpf.Length != 11)
            throw new Exception("CPF deve conter 11 dígitos.");

        Nome = nome.Trim();
        CPF = cpf.Trim();
        Email = email.Trim().ToLowerInvariant();
        SenhaHash = HashSenha(senha.Trim());
    }

    private static string HashSenha(string senha)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(senha);
        return hash;
    }

    public bool VerificarSenha(string senha)
    {
        return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
    }
}