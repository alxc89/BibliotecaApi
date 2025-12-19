## BibliotecaApi

API em ASP.NET Core (.NET 8) usando SQLite (arquivo) e JWT.

### Configuração do JWT (Secret)

Por segurança, não versione o `Jwt:Secret` no `appsettings.json`. Gere uma chave e configure via variável de ambiente (recomendado) ou via `appsettings.Development.json` (apenas local).

**Gerar chave (32 bytes base64)**
- `openssl rand -base64 32`

### Executando a aplicação

**Pré-requisitos**
- .NET SDK 8

**Executar**
- `dotnet restore`
- `dotnet run`

Por padrão (via `Properties/launchSettings.json`) a aplicação sobe em:
- HTTP: `http://localhost:5023`
- Swagger: `http://localhost:5023/swagger`

**Banco (SQLite)**
- A string de conexão padrão está em `appsettings.json` (`ConnectionStrings:Database`) e aponta para `Biblioteca.db` no diretório de execução.

### Executando a aplicação utilizando o Docker

**Build da imagem**
- `docker build -t biblioteca-api .`

**Subir o container (persistindo o SQLite em uma pasta local `./data`)**
- `mkdir -p data`
- `docker run --rm -p 8080:8080 -v "$(pwd)/data:/data" --name biblioteca-api biblioteca-api`

Endpoints:
- API/Swagger: `http://localhost:8080/swagger`

### Tarefas implementadas

**FEATURE: Validar CPF do usuário**
- No cadastro de usuário, validar se o CPF já existe antes de cadastrar.
- Caso o CPF já esteja cadastrado, retornar erro: `"Usuário com este CPF já está cadastrado."`

**FEATURE: Validar ISBN do livro**
- O campo ISBN deve conter exatamente 13 dígitos numéricos.
- Caso o formato esteja incorreto, retornar erro apropriado (`"ISBN inválido."` / `"ISBN deve conter apenas números."`).

**BUG: Gerando multa mesmo quando o empréstimo é devolvido no prazo**
- Corrigido o cálculo: a multa só é gerada se a devolução ocorrer após `data_prevista_devolucao`.

**BUG: Permite emprestar um livro que já está emprestado**
- Antes de registrar um novo empréstimo, verificar se o livro já está emprestado e ainda não foi devolvido.
- Caso esteja, retornar: `"Este livro já está emprestado e ainda não foi devolvido."`

**FEATURE: Listar todos os livros**
- Criado endpoint `GET /Livro/Listar` para retornar a lista completa de livros cadastrados.

**FEATURE: Impedir novo empréstimo se o usuário tiver atraso**
- Validar se o usuário possui empréstimo(s) em atraso e impedir um novo empréstimo.
- Caso possua, retornar: `"Usuário com empréstimo em atraso não pode realizar novo empréstimo."`
- Observação: eu não entendi o que era para ser feito sobre “criar uma marcação no banco de dados para indicar se o usuário possui atraso ativo”.

**FEATURE: Ajustar regra de multa**
- Implementada lógica de multa por dia de atraso (com faixas) e teto máximo.

**FEATURE: Implementar autenticação (Bearer / API Token)**
- Endpoints de livro e empréstimo exigem autenticação via Bearer token; login/cadastro de usuário ficam públicos.
