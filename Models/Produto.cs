
namespace GerenciadorEstoque.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public int QuantidadeEmEstoque { get; set; }
        public decimal Preco { get; set; }

        public override string ToString()
        {
            return $"ÌD: {Id}, Nome: {Nome}, Categoria: {Categoria}, Qtd: {QuantidadeEmEstoque}, Preço: R$ {Preco:F2}";
        }
    }
}
