namespace Project.DTO.DTOs.UserDTOs;

public record UserToListDTO
{
    public int UserId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Fathername { get; set; }

    public string Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public string ImagePath { get; set; }

    public string PIN { get; set; }

    public int OrganizationId { get; set; }

    public string Position { get; set; }

    public int RoleId { get; set; }

    public string PhoneNumber { get; set; }

    public string OfficePhoneNumber { get; set; }

    public string Email { get; set; }
}