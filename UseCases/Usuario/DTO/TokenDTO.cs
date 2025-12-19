namespace BibliotecaApi.UseCases.Usuario.DTO;

public class TokenDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string TokenType { get; set; } = "Bearer";
}