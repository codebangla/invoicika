using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class ItemGroupItem
    {
        [Key]
        public Guid ItemGroupItemId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ItemGroup_id { get; set; }

        [ForeignKey(nameof(ItemGroup_id))]
        [JsonIgnore]
        public virtual ItemGroup ItemGroup { get; set; }

        [Required]
        public Guid Item_id { get; set; }

        [ForeignKey(nameof(Item_id))]
        [JsonIgnore]
        public virtual Item Item { get; set; }
    }
}
