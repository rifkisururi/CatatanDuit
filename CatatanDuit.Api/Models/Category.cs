using System.Collections.Generic;

namespace CatatanDuit.Api.Models;

public class Category : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
