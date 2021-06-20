using CustomerLib.Business.Entities;

namespace CustomerLib.Data.Repositories
{
	public interface ICustomerRepository
	{
		void Create(Customer customer);
		Customer Read(int customerId);
		void Update(Customer customer);
		void Delete(int customerId);
	}
}
