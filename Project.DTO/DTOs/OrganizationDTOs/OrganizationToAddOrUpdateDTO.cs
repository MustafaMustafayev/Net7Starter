using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.OrganizationDTOs
{
	public record OrganizationToAddOrUpdateDTO
	{
		public string FullName { get; set; }

		public string ShortName { get; set; }

		public string Address { get; set; }

		public int? ParentOrganizationId { get; set; }

		[Phone] public string PhoneNumber { get; set; }

		public string TIN { get; set; }

		[EmailAddress] public string Email { get; set; }

		public string Rekvizit { get; set; }
	}
}

