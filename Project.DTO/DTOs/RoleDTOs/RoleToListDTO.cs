using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.RoleDTOs
{
	public record RoleToListDTO
	{
		[Required] public int RoleId { get; set; }

		[Required] public string Name { get; set; }

		public string Key { get; set; }
	}
}

