using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibliotecaApi.Domain.Entities;

namespace BibliotecaApi.UseCases.Livro.Mappers;

public class LivroMapper
{
    public static DTO.LivroDTO ToDTO(LivroEntity livro)
    {
        return new DTO.LivroDTO
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            Autor = livro.Autor,
            ISBN = livro.ISBN
        };
    }
}
