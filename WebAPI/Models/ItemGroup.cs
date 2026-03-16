using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class ItemGroup
    {
        [Key]
        public Guid ItemGroupId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(1024)]
        public string? Description { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [ForeignKey(nameof(User_id))]
        [JsonIgnore]
        public virtual User? User { get; set; }

        public virtual ICollection<ItemGroupItem> ItemGroupItems { get; set; } = new List<ItemGroupItem>();
    }
}
