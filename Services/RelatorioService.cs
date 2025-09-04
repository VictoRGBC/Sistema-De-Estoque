using GerenciadorEstoque.Models;
using GerenciadorEstoque.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GerenciadorEstoque.Services
{
    public class RelatorioService
    {
        private readonly IRepository<Produto> _produtoRepository;

        public RelatorioService(IRepository<Produto> produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public string GerarRelatorioEstoqueTotalCSV(string reportDirectory = "relatorios")
        {
            var todosOsProdutos = _produtoRepository.GetAll();

            if (!todosOsProdutos.Any())
            {
                return null; // Nenhum produto cadastrado
            }

            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory); // Cria o diretório se não existir
            }

            var filePath = Path.Combine(reportDirectory, $"Estoque_Total_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
            var csv = new StringBuilder();

            csv.AppendLine("ID;Nome;Categoria;QuantidadeEmEstoque;Preco");

            foreach (var produto in todosOsProdutos)
            {
                csv.AppendLine($"{produto.Id};{produto.Nome};{produto.Categoria};{produto.QuantidadeEmEstoque};{produto.Preco}");
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
            return filePath;
        }

        public IEnumerable<Produto> ListarProdutosComEstoqueBaixo(int limite)
        {
            return _produtoRepository.Find(p => p.QuantidadeEmEstoque < limite);
        }

        public string GerarRelatorioEstoqueBaixoCSV(int limite, string reportDirectory = "relatorios")
        {
            var produtos = ListarProdutosComEstoqueBaixo(limite);

            if (!produtos.Any())
            {
                return null; // Nenhum produto com estoque baixo
            }

            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory); // Cria o diretório se não existir
            }

            var filePath = Path.Combine(reportDirectory, $"relatorio_estoque_baixo_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
            var csv = new StringBuilder();

            csv.AppendLine("ID,Nome,Categoria,QuantidadeEmEstoque,Preco"); // Cabeçalho do CSV

            foreach (var produto in produtos)
            {
                csv.AppendLine($"{produto.Id};{produto.Nome};{produto.Categoria};{produto.QuantidadeEmEstoque};{produto.Preco}");
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
            return filePath; // Retorna o caminho do arquivo gerado
        }
    }
}
