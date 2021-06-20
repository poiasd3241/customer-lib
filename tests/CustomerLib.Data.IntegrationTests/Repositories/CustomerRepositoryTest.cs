using System;
using CustomerLib.Business.Entities;
using CustomerLib.Data.Repositories.Implementations;
using Xunit;

namespace CustomerLib.Data.IntegrationTests.Repositories
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class CustomerRepositoryTest
	{
		[Fact]
		public void ShouldCreateCustomerRepository()
		{
			var repo = new CustomerRepository();

			Assert.NotNull(repo);
		}

		[Fact]
		public void ShouldCreateCustomer()
		{
			var customerRepository = new CustomerRepository();
			CustomerRepository.DeleteAll();
			var customer = CustomerRepositoryFixture.MockCustomer();

			customerRepository.Create(customer);
		}

		[Fact]
		public void ShouldReadCustomerNotFound()
		{
			var customerRepository = new CustomerRepository();
			CustomerRepository.DeleteAll();

			var readCustomer = customerRepository.Read(1);

			Assert.Null(readCustomer);
		}

		public class CreateMockCustomerData : TheoryData<Func<Customer>>
		{
			public CreateMockCustomerData()
			{
				Add(() => CustomerRepositoryFixture.CreateMockCustomer());
				Add(() => CustomerRepositoryFixture.CreateMockOptionalCustomer());
			}
		}

		[Theory]
		[ClassData(typeof(CreateMockCustomerData))]
		public void ShouldReadCustomerIncludingNullOptionalFields(Func<Customer> createMockCustomer)
		{
			var customerRepository = new CustomerRepository();
			var customer = createMockCustomer.Invoke();

			var readCustomer = customerRepository.Read(1);

			Assert.NotNull(readCustomer);
			Assert.Equal(customer.FirstName, readCustomer.FirstName);
			Assert.Equal(customer.LastName, readCustomer.LastName);
			Assert.Equal(customer.PhoneNumber, readCustomer.PhoneNumber);
			Assert.Equal(customer.Email, readCustomer.Email);
			Assert.Equal(customer.TotalPurchasesAmount, readCustomer.TotalPurchasesAmount);

			Assert.Null(readCustomer.Addresses);
			Assert.Null(readCustomer.Notes);
		}

		[Fact]
		public void ShouldUpdateCustomer()
		{
			var customerRepository = new CustomerRepository();
			var customer = CustomerRepositoryFixture.CreateMockCustomer();

			var createdCustomer = customerRepository.Read(1);
			createdCustomer.FirstName = "New FN";

			// Update.
			customerRepository.Update(createdCustomer);

			var updatedCustomer = customerRepository.Read(1);

			Assert.NotNull(updatedCustomer);
			Assert.Equal("New FN", updatedCustomer.FirstName);
			Assert.Equal(customer.LastName, updatedCustomer.LastName);
			Assert.Equal(customer.PhoneNumber, updatedCustomer.PhoneNumber);
			Assert.Equal(customer.Email, updatedCustomer.Email);
			Assert.Equal(customer.TotalPurchasesAmount, updatedCustomer.TotalPurchasesAmount);

			Assert.Null(createdCustomer.Addresses);
			Assert.Null(createdCustomer.Notes);
		}

		[Fact]
		public void ShouldDeleteCustomer()
		{
			var customerRepository = new CustomerRepository();
			CustomerRepositoryFixture.CreateMockCustomer();

			var createdCustomer = customerRepository.Read(1);
			Assert.NotNull(createdCustomer);

			// Delete.
			customerRepository.Delete(1);

			var deletedCustomer = customerRepository.Read(1);
			Assert.Null(deletedCustomer);
		}
	}

	public class CustomerRepositoryFixture
	{
		public static Customer CreateMockCustomer()
		{
			var customerRepository = new CustomerRepository();
			CustomerRepository.DeleteAll();

			var customer = MockCustomer();
			customerRepository.Create(customer);

			// Simulate identity.
			customer.CustomerId = 1;
			return customer;
		}
		public static Customer CreateMockOptionalCustomer()
		{
			var customerRepository = new CustomerRepository();
			CustomerRepository.DeleteAll();

			var customer = MockOptionalCustomer();
			customerRepository.Create(customer);

			// Simulate identity.
			customer.CustomerId = 1;
			return customer;
		}

		public static Customer MockCustomer() => new()
		{
			FirstName = "John",
			LastName = "Doe",
			PhoneNumber = "+12345",
			Email = "john@doe.com",
			TotalPurchasesAmount = 123
		};

		public static Customer MockOptionalCustomer() => new()
		{
			FirstName = null,
			LastName = "Doe",
			PhoneNumber = null,
			Email = null,
			TotalPurchasesAmount = null
		};
	}
}
