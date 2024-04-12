using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DTO.App.V1;

public class Request : DomainEntityMetaId
{
    [Required] [MaxLength(4096)] public string Description { get; set; } = "";

    [Required] public DateTime? Deadline { get; set; }

    public bool Solved { get; set; }
}