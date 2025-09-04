using GerenciadorEstoque.Data;
using GerenciadorEstoque.Helpers;
using GerenciadorEstoque.Models;
using GerenciadorEstoque.Repositories;
using GerenciadorEstoque.Services;
using System;
using System.Linq;

public class Porgram
{
    private static UsuarioService _usuarioService;
    private static ProdutoService _produtoService;
    private static EstoqueService _estoqueService;
    private static RelatorioService _relatorioService;
    private static Usuario _usuarioLogado;

    public static void Main(string[] args)
    {
        // --- Injeção de Dependência Manual ---
        var dbContext = new AppDbContext();
        var logger = new Logger("logs/operacoes_estoque.log");

        // Repositórios
        var usuarioRepository = new Repository<Usuario>(dbContext);
        var produtoRepository = new Repository<Produto>(dbContext);

        // Serviços
        _usuarioService = new UsuarioService(usuarioRepository);
        _produtoService = new ProdutoService(produtoRepository);
        _estoqueService = new EstoqueService(produtoRepository, logger);
        _relatorioService = new RelatorioService(produtoRepository);

        // --- Fim da Injeção de Dependência ---

        Console.WriteLine("=== Bem-vindo ao Gerenciador de Estoque ===");

        while (_usuarioLogado == null)
        {
            AutenticarUsuario();
        }

        bool sair = false;
        while (!sair)
        {
            ExibirMenuPrincipal();
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    MenuGerenciarProdutos();
                    break;
                case "2":
                    MenuGerenciarEstoque();
                    break;
                case "3":
                    MenuRelatorios();
                    break;
                case "4":
                    sair = true;
                    Console.WriteLine("Saindo do sistema...");
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    private static void AutenticarUsuario()
    {
        Console.WriteLine("\n--- Autenticação ---");
        Console.WriteLine("1. Logar");
        Console.WriteLine("2. Cadastrar Novo Usuário");
        Console.Write("Opção: ");
        string opcao = Console.ReadLine();

        if (opcao == "1")
        {
            Console.Write("Usuario: ");
            string usuario = Console.ReadLine();
            Console.Write("Password: ");
            string senha = Console.ReadLine();
            _usuarioLogado = _usuarioService.Autenticar(usuario, senha);

            if (_usuarioLogado != null)
            {
                Console.Clear();
                Console.WriteLine($"Login bem-sucedido! Bem-vindo, {usuario}!");
            }
            else
            {
                Console.WriteLine("Username ou senha inválidos.");
            }
        }
        else if (opcao == "2")
        {
            Console.Write("Digite o novo username: ");
            string novoUsuario = Console.ReadLine();
            Console.Write("Digite a nova password: ");
            string novaSenha = Console.ReadLine();

            if (_usuarioService.CadastrarUsario(novoUsuario, novaSenha))
            {
                Console.WriteLine("Usuário cadastrado com sucesso! Você já pode fazer login.");
            }
            else
            {
                Console.WriteLine("Erro: Username já existe.");
            }
        }
        else
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }
    }

    private static void ExibirMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine($"--- Gerenciador de Estoque (Usuário: {_usuarioLogado.NomeUsuario}) ---");
        Console.WriteLine("1. Gerenciar Produtos");
        Console.WriteLine("2. Gerenciar Estoque (Entrada/Saída)");
        Console.WriteLine("3. Gerar Relatórios");
        Console.WriteLine("4. Sair");
        Console.WriteLine("------------------------------------------");
    }

    private static void MenuGerenciarProdutos()
    {
        Console.Clear();
        Console.WriteLine("--- Gerenciar Produtos ---");
        Console.WriteLine("1. Cadastrar Produto");
        Console.WriteLine("2. Listar todos os Produtos");
        Console.WriteLine("3. Buscar Produto por ID");
        Console.WriteLine("4. Atualizar Produto");
        Console.WriteLine("5. Deletar Produto");
        Console.WriteLine("6. Listar Produtos por Categoria");
        Console.WriteLine("7. Voltar");
        Console.Write("Opção: ");
        string opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                CadastrarProduto();
                break;
            case "2":
                ListarTodosProdutos();
                break;
            case "3":
                BuscarProdutoPorId();
                break;
            case "4":
                AtualizarProduto();
                break;
            case "5":
                DeletarProduto();
                break;
            case "6":
                ListarProdutosPorCategoria();
                break;
            case "7":
                ExibirMenuPrincipal();
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    private static void MenuGerenciarEstoque()
    {
        Console.Clear();
        ListarTodosProdutos();
        Console.WriteLine("\n--- Gerenciar Estoque ---");
        Console.WriteLine("1. Registrar Entrada");
        Console.WriteLine("2. Registrar Saída");
        Console.WriteLine("3. Voltar");
        Console.Write("Opção: ");
        string opcao = Console.ReadLine();

        if (opcao != "1" && opcao != "2")
        {
            Console.WriteLine("Opção inválida.");
        }

        Console.WriteLine("Digite o ID do produto: ");
        if (!int.TryParse(Console.ReadLine(), out int produtoId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        int quantidade = 0;
        Console.WriteLine("Digite a quantidade: ");
        if (!int.TryParse(Console.ReadLine(), out quantidade) || quantidade <= 0)
        {
            Console.WriteLine("Quantidade inválida.");
        }

        bool sucesso = false;
        if (opcao == "1")
        {
            sucesso = _estoqueService.RegistarEntrada(produtoId, quantidade);
            Console.WriteLine(sucesso ? "Entrada registrada com sucesso!" : "Falha ao registrar entrada");
        }
        else if (opcao == "2")
        {
            sucesso = _estoqueService.RegistrarSaida(produtoId, quantidade);
            Console.WriteLine(sucesso ? "Saída registrada com sucesso!" : "Falha ao registrar saída. Verifique o estoque.");
        }
        else if (opcao == "3")
        {
            ExibirMenuPrincipal();
        }
        else
        {
            Console.WriteLine("Opção Inválida.");
            MenuGerenciarEstoque();
        }
                
    }

    private static void MenuRelatorios()
    {
        Console.Clear();
        Console.WriteLine("--- Relatórios ---");
        Console.WriteLine("1. Relatório de Estoque Baixo");
        Console.WriteLine("2. Relatório de Estoque Total");
        Console.WriteLine("3. Voltar");
        Console.Write("Opção: ");
        string opcao = Console.ReadLine();

        if (opcao == "1")
        {
            Console.Write("Qual o nível mínimo de estoque para o alerta? ");
            if (int.TryParse(Console.ReadLine(), out int limite))
            {
                var caminho = _relatorioService.GerarRelatorioEstoqueBaixoCSV(limite);
                if (caminho != null)
                {
                    Console.WriteLine($"Relatório de estoque baixo gerado em: {Path.GetFullPath(caminho)}");
                }
                else
                {
                    Console.WriteLine("Nenhum produto com estoque baixo encontrado.");
                }
            }
            else
            {
                Console.WriteLine("Limite inválido.");
            }
        }
        else if (opcao == "2")
        {
            var caminho = _relatorioService.GerarRelatorioEstoqueTotalCSV();
            if (caminho != null)
            {
                Console.WriteLine($"Relatório de estoque total gerado em: {Path.GetFullPath(caminho)}");
            }
            else
            {
                Console.WriteLine("Nenhum produto cadastrado para gerar o relatório.");
            }
        }
        else if (opcao == "3") 
        {
            ExibirMenuPrincipal();
        }
        else
        {
            Console.WriteLine("Opção inválida.");
        }
    }

    // --- Métodos para CRUD de Produtos ---

    private static void CadastrarProduto()
    {
        try
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Categoria: ");
            string categoria = Console.ReadLine();
            Console.Write("Preço: ");
            decimal preco = decimal.Parse(Console.ReadLine());
            Console.Write("Quantidade em Estoque: ");
            int quantidade = int.Parse(Console.ReadLine());

            _produtoService.CadastrarProduto(nome, categoria, preco, quantidade);
            Console.WriteLine("Produto cadastrado com sucesso!");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
        catch (FormatException)
        {
            Console.WriteLine("Erro de formato. Preço e quantidade devem ser números.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
    }

    private static void ListarTodosProdutos()
    {
        Console.WriteLine("\n--- Lista de Produtos ---");
        var produtos = _produtoService.ListarTodosProdutos();
        if (!produtos.Any())
        {
            Console.WriteLine("Nenhum poroduto cadastrado.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
        foreach (var p in produtos)
        {
            Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Categoria: {p.Categoria} | Preço: {p.Preco:C} | Estoque: {p.QuantidadeEmEstoque}");
        }

        Console.ReadKey();
        MenuGerenciarProdutos();
    }

    private static void BuscarProdutoPorId()
    {
        Console.Write("Digite o ID do produto: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var p = _produtoService.ObterPorId(id);
            if (p != null)
            {
                Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Categoria: {p.Categoria} | Preço: {p.Preco:C} | Estoque: {p.QuantidadeEmEstoque}");

                Console.ReadKey();
                MenuGerenciarProdutos();
            }
            else
            {
                Console.WriteLine("Produto não encontrado.");

                Console.ReadKey();
                MenuGerenciarProdutos();
            }
        }
        else
        {
            Console.WriteLine("ID inválido.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
    }

    private static void ListarProdutosPorCategoria()
    {
        Console.WriteLine("Digite a categoria para filtrar");
        string categoria = Console.ReadLine();

        var produtos = _produtoService.ListarPorCategoria(categoria);

        if (!produtos.Any())
        {
            Console.WriteLine($"Nenhum produto encontrado na categoria `{categoria}`.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }

        Console.WriteLine($"\n--- Produtos na Categoria: {categoria} ---");
        foreach(var p in produtos)
        {
            Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Preço: {p.Preco:C} | Estoque: {p.QuantidadeEmEstoque}");
        }

        Console.ReadKey();
        MenuGerenciarProdutos();
    }

    private static void AtualizarProduto()
    {
        Console.Write("Digite o ID do produto a ser atualizado: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }

        var produto = _produtoService.ObterPorId(id);
        if (produto == null)
        {
            Console.WriteLine("Produto não encontrado.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }

        try
        {
            Console.Write($"Novo nome (atual: {produto.Nome}): ");
            produto.Nome = Console.ReadLine();
            Console.Write($"Nova categoria (atual: {produto.Categoria}): ");
            produto.Categoria = Console.ReadLine();
            Console.Write($"Novo preço (atual: {produto.Preco}): ");
            produto.Preco = decimal.Parse(Console.ReadLine());

            _produtoService.AtualizarProduto(produto);
            Console.WriteLine("Produto atualizado com sucesso!");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
        catch (FormatException)
        {
            Console.WriteLine("Erro de formato. Preço deve ser um número.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
    }

    private static void DeletarProduto()
    {
        Console.Write("Digite o ID do produto a ser deletado: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _produtoService.RemoverProduto(id);
            Console.WriteLine("Produto deletado com sucesso!");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
        else
        {
            Console.WriteLine("ID inválido.");

            Console.ReadKey();
            MenuGerenciarProdutos();
        }
    }

}