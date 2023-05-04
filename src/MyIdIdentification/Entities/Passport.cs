using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MyIdIdentification.Entities.Enums;

namespace MyIdIdentification.Entities;

public class Passport
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public long UserId { get; set; }

    public Guid ProviderId { get; set; }

    [ForeignKey(nameof(ProviderId))]
    public Provider Provider { get; set; }

    [MaxLength(20)]
    public string FirstName { get; set; }

    [MaxLength(20)]
    public string LastName { get; set; }

    [MaxLength(20)]
    public string MiddleName { get; set; }

    public DateTime BirthDate { get; set; }

    [MaxLength(9)]
    public string Tin { get; set; }

    [MaxLength(14)]
    public string Pinfl { get; set; }

    [MaxLength(7)]
    public string PassportNumber { get; set; }

    [MaxLength(2)]
    public string PassportSerial { get; set; }

    public DateTime PassportGivenDate { get; set; }

    public DateTime PassportExpireDate { get; set; }

    [MaxLength(100)]
    public string PassportGivenBy { get; set; }

    [MaxLength(100)]
    public string Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public EProviderType ProviderType { get; set; }

    public JsonDocument AllData { get; set; }
}