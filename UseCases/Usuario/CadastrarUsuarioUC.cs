using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Response;
using BibliotecaApi.UseCases.Usuario.DTO;

namespace BibliotecaApi.UseCases.Usuario;

public class CadastrarUsuarioUC
{
    private UsuarioEntity _usuario = new UsuarioEntity();
    private UsuarioRepository _repository = new UsuarioRepository();

    public async Task<UseCaseResponse<int>> Execute(CadastrarUsuarioInputDTO input)
    {
        try
        {
            if (await _repository.UsuarioExisteComCpf(input.CPF))
                return UseCaseResponse<int>.Falha("Usuário com este CPF já está cadastrado.");
            
            _usuario.Cadastrar(input.Nome, input.CPF, input.Email);
            
            int idNovoUsuario = await _repository.Cadastrar(_usuario);
            return UseCaseResponse<int>.Ok(idNovoUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao cadastrar usuário: " + ex.Message);
        }
    }
}