using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MyIdIdentification.Entities.Enums;

namespace MyIdIdentification.Entities;

public class Provider
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Method { get; set; }

    public EProviderType ProviderType { get; set; }
    
    public JsonDocument Credentials { get; set; }
}