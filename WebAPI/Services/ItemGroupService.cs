using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IItemGroupService
    {
        Task<ItemGroupDto> GetItemGroupByIdAsync(Guid id);
        Task<IEnumerable<ItemGroupDto>> GetAllItemGroupsAsync();
        Task CreateItemGroupAsync(ItemGroupDto dto);
        Task<bool> UpdateItemGroupAsync(Guid id, ItemGroupDto dto);
        Task DeleteItemGroupAsync(Guid id);
    }

    public class ItemGroupService : IItemGroupService
    {
        private readonly InvoicikaDbContext _context;

        public ItemGroupService(InvoicikaDbContext context)
        {
            _context = context;
        }

        public async Task<ItemGroupDto> GetItemGroupByIdAsync(Guid id)
        {
            var group = await _context.ItemGroups
                .Include(g => g.ItemGroupItems)
                .FirstOrDefaultAsync(g => g.ItemGroupId == id);

            if (group == null) return null;

            return new ItemGroupDto
            {
                ItemGroupId = group.ItemGroupId,
                Title = group.Title,
                Description = group.Description,
                User_id = group.User_id,
                ItemIds = group.ItemGroupItems.Select(i => i.Item_id).ToList()
            };
        }

        public async Task<IEnumerable<ItemGroupDto>> GetAllItemGroupsAsync()
        {
            var groups = await _context.ItemGroups
                .Include(g => g.ItemGroupItems)
                .ToListAsync();

            return groups.Select(group => new ItemGroupDto
            {
                ItemGroupId = group.ItemGroupId,
                Title = group.Title,
                Description = group.Description,
                User_id = group.User_id,
                ItemIds = group.ItemGroupItems.Select(i => i.Item_id).ToList()
            });
        }

        public async Task CreateItemGroupAsync(ItemGroupDto dto)
        {
            var group = new ItemGroup
            {
                ItemGroupId = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                User_id = dto.User_id,
                ItemGroupItems = dto.ItemIds.Select(itemId => new ItemGroupItem
                {
                    Item_id = itemId
                }).ToList()
            };

            _context.ItemGroups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateItemGroupAsync(Guid id, ItemGroupDto dto)
        {
            var existingGroup = await _context.ItemGroups
                .Include(g => g.ItemGroupItems)
                .FirstOrDefaultAsync(g => g.ItemGroupId == id);

            if (existingGroup == null) return false;

            existingGroup.Title = dto.Title;
            existingGroup.Description = dto.Description;
            existingGroup.User_id = dto.User_id;

            _context.ItemGroupItems.RemoveRange(existingGroup.ItemGroupItems);
            existingGroup.ItemGroupItems = dto.ItemIds.Select(itemId => new ItemGroupItem
            {
                ItemGroup_id = id,
                Item_id = itemId
            }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteItemGroupAsync(Guid id)
        {
            var group = await _context.ItemGroups.FindAsync(id);
            if (group != null)
            {
                _context.ItemGroups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }
    }
}