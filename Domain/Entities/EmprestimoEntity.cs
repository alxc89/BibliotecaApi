namespace BibliotecaApi.Domain.Entities;

public class EmprestimoEntity
{
    public int Id { get; set; }
    public int IdUsuario { get; private set; }
    public int IdLivro { get; private set; }
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataPrevistaDevolucao { get; private set; }
    public DateTime? DataDevolucao { get; private set; }
    public Decimal Valor { get; private set; }
    public Decimal Multa { get; private set; }
    public Decimal Total { get; private set; }

    private const int PRIMEIRA_FAIXA_DIAS_ATRASO = 3;

    private const decimal VALOR_MULTA_POR_DIA_ATRASO_ATE_TRES_DIAS = 2.00m;
    private const decimal VALOR_MULTA_POR_DIA_ATRASO_ACIMA_DE_TRES_DIAS = 3.50m;
    private const decimal VALOR_MAXIMO_MULTA = 50.00m;

    public void Cadastrar(int idUsuario, int idLivro, DateTime dataPrevista)
    {
        if (idUsuario <= 0)
            throw new Exception("Usuário inválido.");

        if (idLivro <= 0)
            throw new Exception("Livro inválido.");

        if (dataPrevista <= DateTime.Now)
            throw new Exception("A data prevista de devolução deve ser futura.");

        IdUsuario = idUsuario;
        IdLivro = idLivro;
        DataEmprestimo = DateTime.Now;
        DataPrevistaDevolucao = dataPrevista;
        DataDevolucao = null;
        Valor = 5.00m;
    }

    public void RegistrarDevolucao(DateTime dataDevolucao)
    {
        DataDevolucao = dataDevolucao;
        Multa = CalcularMulta();
        Total = Valor + Multa;
    }

    private decimal CalcularMulta()
    {
        decimal valorMulta = 0.00m;
        TimeSpan atraso = CalcularDiasDeAtraso();
        
        if (atraso.TotalDays <= 0)
            return valorMulta;
        
        for (int i = 0; i < (int)atraso.TotalDays; i++)
        {
            if (i < PRIMEIRA_FAIXA_DIAS_ATRASO)
                valorMulta += VALOR_MULTA_POR_DIA_ATRASO_ATE_TRES_DIAS;
            else
                valorMulta += VALOR_MULTA_POR_DIA_ATRASO_ACIMA_DE_TRES_DIAS;
        }

        return valorMulta > VALOR_MAXIMO_MULTA ? VALOR_MAXIMO_MULTA : valorMulta;
    }

    private TimeSpan CalcularDiasDeAtraso()
    {
        if (DataDevolucao == null)
            throw new Exception("Empréstimo ainda não devolvido.");

        return DataDevolucao.Value - DataPrevistaDevolucao;
    }
}
