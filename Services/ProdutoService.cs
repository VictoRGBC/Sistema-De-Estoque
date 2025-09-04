using GerenciadorEstoque.Models;
using GerenciadorEstoque.Repositories;
using System.Collections.Generic;

namespace GerenciadorEstoque.Services
{
    public class ProdutoService
    {
        private readonly IRepository<Produto> _produtoRepository;

        public ProdutoService(IRepository<Produto> produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public void CadastrarProduto(string nome, string categoria, decimal preco, int quantidade)
        {
            var produto = new Produto
            {
                Nome = nome,
                Categoria = categoria,
                QuantidadeEmEstoque = quantidade,
                Preco = preco
            };

            _produtoRepository.Add(produto);
        }

        public Produto ObterPorId(int id)
        {
            return _produtoRepository.GetById(id);
        }

        public IEnumerable<Produto> ListarTodosProdutos()
        {
            return _produtoRepository.GetAll();
        }

        public void AtualizarProduto(Produto produto)
        {
            _produtoRepository.Update(produto);
        }

        public void RemoverProduto(int id)
        {
            var produto = _produtoRepository.GetById(id);
            if (produto != null)
            {
                _produtoRepository.Remove(produto);
            }
        }

        public IEnumerable<Produto> ListarPorCategoria(string categoria)
        {
            return _produtoRepository.GetAll()
                                     .Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
        }
    }
}
