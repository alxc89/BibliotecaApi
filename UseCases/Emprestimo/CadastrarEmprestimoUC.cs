using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Emprestimo.DTO;
using BibliotecaApi.UseCases.Response;

namespace BibliotecaApi.UseCases.Emprestimo;

public class CadastrarEmprestimoUC
{
    private readonly EmprestimoRepository _emprestimoRepository = new EmprestimoRepository();
    private readonly LivroRepository _livroRepository = new LivroRepository();

    public async Task<UseCaseResponse<int>> Execute(CadastrarEmprestimoInputDTO input)
    {
        try
        {
            if (await _livroRepository.LivroEmprestado(input.IdLivro))
                return UseCaseResponse<int>.Falha("Este livro já está emprestado e ainda não foi devolvido.");

            if (await _emprestimoRepository.VerificarUsuarioComEmprestimosAtrasados(input.IdUsuario))
                return UseCaseResponse<int>.Falha("Usuário com empréstimo em atraso não pode realizar novo empréstimo.");
                
            var emprestimo = new EmprestimoEntity();
            emprestimo.Cadastrar(input.IdUsuario, input.IdLivro, input.DataPrevistaDevolucao);

            int idEmprestimo = await _emprestimoRepository.Cadastrar(emprestimo);

            // Marca o livro como indisponível
            await _livroRepository.MarcarComoIndisponivel(input.IdLivro);

            return UseCaseResponse<int>.Ok(idEmprestimo);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao cadastrar empréstimo: " + ex.Message);
        }
    }
}
