using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace BLL.App.DTO;

public class Ticket : DomainEntityMetaId
{
    [Required] [MaxLength(4096)] public string Description { get; set; } = "";

    [Required] public DateTime Deadline { get; set; }

    public bool Solved { get; set; } = false;
}