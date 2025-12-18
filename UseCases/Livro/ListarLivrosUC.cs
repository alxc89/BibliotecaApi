using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Response;

namespace BibliotecaApi.UseCases.Livro;

public class ListarLivrosUC
{
    private LivroRepository _repository = new();

    public async Task<UseCaseResponse<IEnumerable<DTO.LivroDTO>>> Execute()
    {
        try
        {
            var livros = await _repository.ListarTodos();
            var livroDTOs = new List<DTO.LivroDTO>();
            livroDTOs = livros
                 .Select(livro => Mappers.LivroMapper.ToDTO(livro))
                 .ToList();

            return UseCaseResponse<IEnumerable<DTO.LivroDTO>>.Ok(livroDTOs);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao listar livros: " + ex.Message);
        }
    }
}