namespace EFDatabase.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public List<User?> Users { get; set; }
    }
}
