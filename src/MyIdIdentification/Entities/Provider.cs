using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MyIdIdentification.Entities.Enums;

namespace MyIdIdentification.Entities;

public class Provider
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long OrganizationId { get; set; }

    public EIdentificationType IdentificationType { get; set; }
    
    public JsonDocument Credentials { get; set; }
}