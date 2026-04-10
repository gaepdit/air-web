using AirWeb.Domain.Sbeap.Entities.Contacts;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.SbeapRepositories;

public sealed class ContactRepository(AppDbContext context)
    : BaseRepository<Contact, Guid, AppDbContext>(context), IContactRepository
{
    // EF will set the ID automatically.
    public int GetNextPhoneNumberId() => 0;
}
