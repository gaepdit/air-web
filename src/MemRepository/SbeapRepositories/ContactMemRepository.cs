using AirWeb.Domain.Sbeap.Entities.Contacts;
using AirWeb.TestData.Sbeap;

namespace AirWeb.MemRepository.SbeapRepositories;

public sealed class ContactMemRepository()
    : BaseRepository<Contact, Guid>(ContactData.GetContacts), IContactRepository
{
    // Local repository requires ID to be manually set.
    public int GetNextPhoneNumberId()
    {
        var phoneNumbers = Items.SelectMany(e => e.PhoneNumbers).ToList();
        return phoneNumbers.Count == 0 ? 1 : phoneNumbers.Max(number => number.Id) + 1;
    }
}
