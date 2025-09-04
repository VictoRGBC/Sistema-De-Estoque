using GerenciadorEstoque.Helpers;
using GerenciadorEstoque.Models;
using GerenciadorEstoque.Repositories;

namespace GerenciadorEstoque.Services
{
    public class EstoqueService
    {
        private readonly IRepository<Produto> _produtoRepository;
        private readonly Logger _logger;

        public EstoqueService(IRepository<Produto> produtoRepository, Logger logger)
        {
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        public bool  RegistarEntrada(int produtoId, int quantidade)
        {
            if (quantidade <= 0)
            {
                return false;
            }

            var produto = _produtoRepository.GetById(produtoId);
            if (produto == null)
            {
                return false;
            }

            produto.QuantidadeEmEstoque += quantidade;
            _produtoRepository.Update(produto);
            _logger.Log($"Entrada: {quantidade} unidade(s) do produto '{produto.Nome}' (ID: {produto.Id}). Estoque atual: {produto.QuantidadeEmEstoque}.");
            return true;
        }

        public bool RegistrarSaida(int produtoId, int quantidade)
        {
            if (quantidade <= 0)
            {
                return false;
            }

            var produto = _produtoRepository.GetById(produtoId);
            if (produto == null || produto.QuantidadeEmEstoque < quantidade)
            {
                return false; //Produto não enconmtrado ou estoque insuficiente
            }

            produto.QuantidadeEmEstoque -= quantidade;
            _produtoRepository.Update(produto);
            _logger.Log($"SAÍDA: {quantidade} unidade(s) do produto '{produto.Nome}' (ID: {produtoId}). Estoque atual: {produto.QuantidadeEmEstoque}.");
            return true;
        }
    }
}
