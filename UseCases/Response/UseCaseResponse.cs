using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.UseCases.Response;

public class UseCaseResponse<T>
{
    public bool Sucesso { get; private set; }
    public T? Dados { get; private set; }
    public string? MensagemErro { get; private set; }
    public static UseCaseResponse<T> Ok(T dados) => new() { Sucesso = true, Dados = dados };
    public static UseCaseResponse<T> Falha(string erro) => new() { Sucesso = false, MensagemErro = erro };
}