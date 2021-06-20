using System;
using CustomerLib.Business.Entities;
using CustomerLib.Business.Enums;
using CustomerLib.Data.IntegrationTests.Repositories.TestHelpers;
using CustomerLib.Data.Repositories.Implementations;
using Xunit;

namespace CustomerLib.Data.IntegrationTests.Repositories
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class AddressRepositoryTest
	{
		[Fact]
		public void ShouldCreateAddressRepository()
		{
			var repo = new AddressRepository();

			Assert.NotNull(repo);
		}

		[Fact]
		public void ShouldCreateAddress()
		{
			var addressRepository = new AddressRepository();
			var customer = CustomerRepositoryFixture.CreateMockCustomer();
			AddressRepository.DeleteAll();
			AddressTypeHelperRepository.UnsafeRepopulateAddressTypes();

			var address = AddressRepositoryFixture.MockAddress();
			address.CustomerId = customer.CustomerId;

			addressRepository.Create(address);
		}

		[Fact]
		public void ShouldReadAddressNotFound()
		{
			var addressRepository = new AddressRepository();
			AddressRepository.DeleteAll();

			var readAddress = addressRepository.Read(1);

			Assert.Null(readAddress);
		}

		public class CreateMockAddressData : TheoryData<Func<Address>>
		{
			public CreateMockAddressData()
			{
				Add(() => AddressRepositoryFixture.CreateMockAddress());
				Add(() => AddressRepositoryFixture.CreateMockOptionalAddress());
			}
		}

		[Theory]
		[ClassData(typeof(CreateMockAddressData))]
		public void ShouldReadAddressIncludingNullOptionalFields(Func<Address> createMockAddress)
		{
			var addressRepository = new AddressRepository();
			var address = createMockAddress.Invoke();

			var readAddress = addressRepository.Read(1);

			Assert.NotNull(readAddress);
			Assert.Equal(address.CustomerId, readAddress.CustomerId);
			Assert.Equal(address.AddressLine, readAddress.AddressLine);
			Assert.Equal(address.AddressLine2, readAddress.AddressLine2);
			Assert.Equal(address.Type, readAddress.Type);
			Assert.Equal(address.City, readAddress.City);
			Assert.Equal(address.PostalCode, readAddress.PostalCode);
			Assert.Equal(address.State, readAddress.State);
			Assert.Equal(address.Country, readAddress.Country);
		}

		[Fact]
		public void ShouldReadAllAddressesByCustomer()
		{
			var addressRepository = new AddressRepository();
			var address = AddressRepositoryFixture.CreateMockAddress(2);

			var readAddresses = addressRepository.ReadAllByCustomer(address.CustomerId);

			Assert.NotNull(readAddresses);
			Assert.Equal(2, readAddresses.Count);

			foreach (var readAddress in readAddresses)
			{
				Assert.Equal(address.CustomerId, readAddress.CustomerId);
				Assert.Equal(address.AddressLine, readAddress.AddressLine);
				Assert.Equal(address.AddressLine2, readAddress.AddressLine2);
				Assert.Equal(address.Type, readAddress.Type);
				Assert.Equal(address.City, readAddress.City);
				Assert.Equal(address.PostalCode, readAddress.PostalCode);
				Assert.Equal(address.State, readAddress.State);
				Assert.Equal(address.Country, readAddress.Country);
			}
		}

		[Fact]
		public void ShouldReadAllAddressesByCustomerNotFound()
		{
			var addressRepository = new AddressRepository();
			AddressRepository.DeleteAll();

			var readAddresses = addressRepository.ReadAllByCustomer(1);

			Assert.Null(readAddresses);
		}

		[Fact]
		public void ShouldUpdateAddress()
		{
			var addressRepository = new AddressRepository();
			var address = AddressRepositoryFixture.CreateMockAddress();

			var createdAddress = addressRepository.Read(1);
			createdAddress.AddressLine = "New line!";

			// Update.
			addressRepository.Update(createdAddress);

			var updatedAddress = addressRepository.Read(1);

			Assert.NotNull(updatedAddress);
			Assert.Equal(address.CustomerId, updatedAddress.CustomerId);
			Assert.Equal("New line!", updatedAddress.AddressLine);
			Assert.Equal(address.AddressLine2, updatedAddress.AddressLine2);
			Assert.Equal(address.Type, updatedAddress.Type);
			Assert.Equal(address.City, updatedAddress.City);
			Assert.Equal(address.PostalCode, updatedAddress.PostalCode);
			Assert.Equal(address.State, updatedAddress.State);
			Assert.Equal(address.Country, updatedAddress.Country);
		}

		[Fact]
		public void ShouldDeleteAddress()
		{
			var addressRepository = new AddressRepository();
			AddressRepositoryFixture.CreateMockAddress();

			var createdAddress = addressRepository.Read(1);
			Assert.NotNull(createdAddress);

			// Delete.
			addressRepository.Delete(1);

			var deletedAddress = addressRepository.Read(1);
			Assert.Null(deletedAddress);
		}
	}

	public class AddressRepositoryFixture
	{
		public static Address CreateMockAddress(int amount = 1)
		{
			var addressRepository = new AddressRepository();
			var customer = CustomerRepositoryFixture.CreateMockCustomer();
			AddressRepository.DeleteAll();
			AddressTypeHelperRepository.UnsafeRepopulateAddressTypes();

			var address = MockAddress();
			address.CustomerId = customer.CustomerId;

			for (int i = 0; i < amount; i++)
			{
				addressRepository.Create(address);
			}

			return address;
		}

		public static Address CreateMockOptionalAddress()
		{
			var addressRepository = new AddressRepository();
			var customer = CustomerRepositoryFixture.CreateMockCustomer();
			AddressRepository.DeleteAll();
			AddressTypeHelperRepository.UnsafeRepopulateAddressTypes();

			var address = MockOptionalAddress();
			address.CustomerId = customer.CustomerId;

			addressRepository.Create(address);

			return address;
		}

		public static Address MockAddress() => new()
		{
			AddressLine = "one",
			AddressLine2 = "two",
			Type = AddressType.Shipping,
			City = "Seattle",
			PostalCode = "123456",
			State = "WA",
			Country = "United States"
		};

		public static Address MockOptionalAddress()
		{
			var mockAddress = MockAddress();
			mockAddress.AddressLine2 = null;
			return mockAddress;
		}
	}
}
