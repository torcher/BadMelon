namespace BadMelon.Data.Entities
{
    public abstract class UserPermittedEntity : Entity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}