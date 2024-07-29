

using Core.Models;

namespace Application.InterfaceServices
{
	public interface IPessoaService 
	{
        public Task<Pessoa> GetPessoa(int id);
        public Task<IEnumerable<Pessoa>> GetListPessoa();
        public bool DeletePessoa(Pessoa pessoa);
        public bool UpdatePessoa(Pessoa pessoa);
        public bool InsertPessoa(Pessoa pessoa);

    }
}
