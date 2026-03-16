namespace WebAPI.Dtos
{
    public class ItemGroupDto
    {
        public Guid? ItemGroupId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid User_id { get; set; }
        public List<Guid> ItemIds { get; set; } = new List<Guid>();
    }
}
