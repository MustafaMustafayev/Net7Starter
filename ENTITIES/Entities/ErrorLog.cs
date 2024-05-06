using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities
{
    public class ErrorLog : IEntity
    {
        public Guid Id { get; set; }
        public required DateTime DateTime { get; set; } = DateTime.Now;
        public required string AccessToken { get; set; }
        public required Guid? UserId { get; set; }
        public required string Path { get; set; }
        public required string Ip { get; set; }
        public required string ErrorMessage { get; set;}
        public required string StackTrace { get; set;}
        public bool IsDeleted { get; set; }
    }
}
