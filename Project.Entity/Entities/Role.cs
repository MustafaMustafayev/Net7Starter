using System.ComponentModel.DataAnnotations;
using Project.Entity.IEntities;

namespace Project.Entity.Entities
{
	public class Role : AuditableEntity, IEntity
	{
		[Key] public int RoleId { get; set; }

		[Required] public string Name { get; set; }

		public string Key { get; set; }
	}
}