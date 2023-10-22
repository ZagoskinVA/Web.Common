using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebUtilities.Model;

[Index(nameof(Id), IsUnique = true)]
public abstract class BaseObject
{
    [Key] public string? Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime UpdatedDateTime { get; set; }
}