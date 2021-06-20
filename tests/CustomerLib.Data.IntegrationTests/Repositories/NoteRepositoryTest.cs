using CustomerLib.Business.Entities;
using CustomerLib.Data.Repositories.Implementations;
using Xunit;

namespace CustomerLib.Data.IntegrationTests.Repositories
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class NoteRepositoryTest
	{
		[Fact]
		public void ShouldCreateNoteRepository()
		{
			var repo = new NoteRepository();

			Assert.NotNull(repo);
		}

		[Fact]
		public void ShouldCreateNote()
		{
			var noteRepository = new NoteRepository();
			var customer = CustomerRepositoryFixture.CreateMockCustomer();
			NoteRepository.DeleteAll();

			var note = NoteRepositoryFixture.MockNote();
			note.CustomerId = customer.CustomerId;

			noteRepository.Create(note);
		}

		[Fact]
		public void ShouldReadNoteNotFound()
		{
			var noteRepository = new NoteRepository();
			NoteRepository.DeleteAll();

			var readNote = noteRepository.Read(1);

			Assert.Null(readNote);
		}

		[Fact]
		public void ShouldReadNote()
		{
			var noteRepository = new NoteRepository();
			var note = NoteRepositoryFixture.CreateMockNote();

			var createdNote = noteRepository.Read(1);

			Assert.NotNull(createdNote);
			Assert.Equal(note.CustomerId, createdNote.CustomerId);
			Assert.Equal(note.Content, createdNote.Content);
		}

		[Fact]
		public void ShouldReadAllNotesByCustomer()
		{
			var noteRepository = new NoteRepository();
			var note = NoteRepositoryFixture.CreateMockNote(2);

			var readNotes = noteRepository.ReadAllByCustomer(note.CustomerId);

			Assert.NotNull(readNotes);
			Assert.Equal(2, readNotes.Count);

			foreach (var readNote in readNotes)
			{
				Assert.Equal(note.CustomerId, readNote.CustomerId);
				Assert.Equal(note.Content, readNote.Content);
			}
		}

		[Fact]
		public void ShouldReadAllNotesByCustomerNotFound()
		{
			var noteRepository = new NoteRepository();
			NoteRepository.DeleteAll();

			var readNotes = noteRepository.ReadAllByCustomer(1);

			Assert.Null(readNotes);
		}

		[Fact]
		public void ShouldUpdateNote()
		{
			var noteRepository = new NoteRepository();
			var note = NoteRepositoryFixture.CreateMockNote();

			var createdNote = noteRepository.Read(1);
			createdNote.Content = "New content!";

			// Update.
			noteRepository.Update(createdNote);

			var updatedNote = noteRepository.Read(1);

			Assert.NotNull(updatedNote);
			Assert.Equal(note.CustomerId, updatedNote.CustomerId);
			Assert.Equal("New content!", updatedNote.Content);
		}

		[Fact]
		public void ShouldDeleteNote()
		{
			var noteRepository = new NoteRepository();
			NoteRepositoryFixture.CreateMockNote();

			var createdNote = noteRepository.Read(1);
			Assert.NotNull(createdNote);

			// Delete.
			noteRepository.Delete(1);

			var deletedNote = noteRepository.Read(1);
			Assert.Null(deletedNote);
		}
	}

	public class NoteRepositoryFixture
	{
		public static Note CreateMockNote(int amount = 1)
		{
			var noteRepository = new NoteRepository();
			var customer = CustomerRepositoryFixture.CreateMockCustomer();
			NoteRepository.DeleteAll();

			var note = MockNote();
			note.CustomerId = customer.CustomerId;

			for (int i = 0; i < amount; i++)
			{
				noteRepository.Create(note);
			}

			return note;
		}

		public static Note MockNote() => new()
		{
			Content = "text"
		};
	}
}
