
using Dapper;
using BibliotecaApi.Infrastructure.Data;
using BibliotecaApi.Domain.Entities;

namespace BibliotecaApi.Infrastructure.Repositories;


public class LivroRepository
{
    private readonly DbSession _session;
    public LivroRepository()
    {
        _session = new DbSession(ConfigurationHelper.GetConfiguration());
    }

    public async Task<int> Cadastrar(LivroEntity livro)
    {
        
        var sql = "INSERT INTO Livros(titulo, autor, isbn) VALUES(@titulo, @autor,  @isbn) returning id";

        var parameters = new { titulo = livro.Titulo, autor= livro.Autor, isbn = livro.ISBN};

        using (var connection = _session.Connection)
        {
            var result = await _session.Connection.QueryFirstAsync<int>(sql, parameters, null);
            return result;
        }
    }

    public async Task<bool> LivroEmprestado(int idLivro)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM Emprestimos WHERE id_livro = @id AND data_devolucao IS NULL)";
        return await _session.Connection.QueryFirstAsync<bool>(sql, new { id = idLivro });
    }

    public async Task<IEnumerable<LivroEntity>> ListarTodos()
    {
        const string sql = "SELECT id, titulo, autor, isbn FROM Livros";
        return await _session.Connection.QueryAsync<LivroEntity>(sql);
    }

    public async Task MarcarComoIndisponivel(int idLivro)
    {
        const string sql = "UPDATE Livros SET disponivel = FALSE WHERE id = @id";
        await _session.Connection.ExecuteAsync(sql, new { id = idLivro });
    }

    public async Task MarcarComoDisponivel(int idLivro)
    {
        const string sql = "UPDATE Livros SET disponivel = TRUE WHERE id = @id";
        await _session.Connection.ExecuteAsync(sql, new { id = idLivro });
    }
}
