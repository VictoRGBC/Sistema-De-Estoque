using GerenciadorEstoque.Helpers;
using GerenciadorEstoque.Models;
using GerenciadorEstoque.Repositories;
using System.Linq;

namespace GerenciadorEstoque.Services
{
    public class UsuarioService
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public UsuarioService(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public bool CadastrarUsario(string nomeUsuario, string senha)
        {
            //Verifica se o usuário existe
            var usuarioExistente = _usuarioRepository.Find(u => u.NomeUsuario == nomeUsuario).FirstOrDefault();
            if(usuarioExistente != null)
            {
                return false; // Usuário já existe
            }

            var novoUsuario = new Usuario
            {
                NomeUsuario = nomeUsuario,
                SenhaHash = CriptografiaHelper.GerarHash(senha)
            };

            _usuarioRepository.Add(novoUsuario);
            return true; // Usuário cadastrado com sucesso
        }

        public Usuario Autenticar(string nomeUusuario, string senha)
        {
            var usuario = _usuarioRepository.Find(u => u.NomeUsuario == nomeUusuario).FirstOrDefault();
            if (usuario != null)
            {
                // Compara o hash da senha fornecida com o hash armazenado
                if(usuario.SenhaHash == CriptografiaHelper.GerarHash(senha))
                {
                    return usuario; // Autenticação bem-sucedida
                }
            }
            return null; // Falha na autenticação
        }
    }
}
