namespace MessagingEvents.Shared.Services;

public class CustomerCreated
{
    public CustomerCreated()
    {
    }

    public CustomerCreated(int id, string fullName, string email, string phoneNumber, DateTime birthDate)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }

    public override string ToString()
    {
        return
            $"{{ \"Id\": {Id}, \"FullName\": \"{FullName}\", \"Email\": \"{Email}\", \"PhoneNumber\": \"{PhoneNumber}\", \"BirthDate\": \"{BirthDate:yyyy-MM-ddTHH:mm:ss}\" }}";
    }
    
}