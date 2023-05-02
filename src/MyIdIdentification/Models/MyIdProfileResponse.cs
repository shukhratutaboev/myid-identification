using System.Text.Json.Serialization;

namespace MyIdIdentification.Models;

public class MyIdProfileResponse
{
    [JsonPropertyName("common_data")]
    public MyIdCommonDataResponse CommonData { get; set; }

    [JsonPropertyName("doc_data")]
    public MyIdDocDataResponse DocData { get; set; }

    [JsonPropertyName("contacts")]
    public MyIdContactsResponse Contacts { get; set; }

    [JsonPropertyName("address")]
    public MyIdAddressResponse Address { get; set; }

    [JsonPropertyName("authentication_method")]
    public string AuthenticationMethod { get; set; }
}

public class MyIdCommonDataResponse
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("first_name_en")]
    public string FirstNameEn { get; set; }

    [JsonPropertyName("middle_name")]
    public string MiddleName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("last_name_en")]
    public string LastNameEn { get; set; }

    [JsonPropertyName("pinfl")]
    public string Pinfl { get; set; }

    [JsonPropertyName("inn")]
    public string Inn { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("birth_place")]
    public string BirthPlace { get; set; }

    [JsonPropertyName("birth_country")]
    public string BirthCountry { get; set; }

    [JsonPropertyName("birth_country_id")]
    public string BirthCountryId { get; set; }

    [JsonPropertyName("birth_date")]
    public string BirthDate { get; set; }

    [JsonPropertyName("nationality")]
    public string Nationality { get; set; }

    [JsonPropertyName("nationality_id")]
    public string NationalityId { get; set; }

    [JsonPropertyName("nationality_id_cbu")]
    public string NationalityIdCbu { get; set; }

    [JsonPropertyName("citizenship")]
    public string Citizenship { get; set; }

    [JsonPropertyName("citizenship_id")]
    public string CitizenshipId { get; set; }

    [JsonPropertyName("sdk_hash")]
    public string SdkHash { get; set; }

    [JsonPropertyName("last_update_pass_data")]
    public string LastUpdatePassData { get; set; }

    [JsonPropertyName("last_update_inn")]
    public string LastUpdateInn { get; set; }

    [JsonPropertyName("last_update_address")]
    public string LastUpdateAddress { get; set; }
}

public class MyIdContactsResponse
{
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}

public class MyIdDocDataResponse
{
    [JsonPropertyName("pass_data")]
    public string PassData { get; set; }

    [JsonPropertyName("issued_by")]
    public string IssuedBy { get; set; }

    [JsonPropertyName("issued_by_id")]
    public string IssuedById { get; set; }

    [JsonPropertyName("issued_date")]
    public string IssuedDate { get; set; }

    [JsonPropertyName("expiry_date")]
    public string ExpiryDate { get; set; }

    [JsonPropertyName("doc_type")]
    public string DocType { get; set; }

    [JsonPropertyName("doc_type_id")]
    public string DocTypeId { get; set; }
}

public class MyIdAddressResponse
{
    [JsonPropertyName("permanent_address")]
    public string PermanentAddress { get; set; }

    [JsonPropertyName("temporary_address")]
    public string TemporaryAddress { get; set; }

    [JsonPropertyName("permanent_registration")]
    public MyIdRegistrationAddressResponse PermanentRegistration { get; set; }

    [JsonPropertyName("temporary_registration")]
    public MyIdRegistrationAddressResponse TemporaryRegistration { get; set; }
}

public class MyIdRegistrationAddressResponse
{
    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("cadastre")]
    public string Cadastre { get; set; }

    [JsonPropertyName("district")]
    public string District { get; set; }

    [JsonPropertyName("region_id")]
    public string RegionId { get; set; }

    [JsonPropertyName("country_id")]
    public string CountryId { get; set; }

    [JsonPropertyName("district_id")]
    public string DistrictId { get; set; }

    [JsonPropertyName("region_id_cbu")]
    public string RegionIdCbu { get; set; }

    [JsonPropertyName("country_id_cbu")]
    public string CountryIdCbu { get; set; }

    [JsonPropertyName("district_id_cbu")]
    public string DistrictIdCbu { get; set; }

    [JsonPropertyName("registration_date")]
    public string RegistrationDate { get; set; }
}