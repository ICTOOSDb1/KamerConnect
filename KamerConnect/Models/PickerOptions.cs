namespace KamerConnect.Models;

public static class PickerOptions
{
    
    public enum DutchGender
    {
        Man,
        Vrouw,
        Anders
    }
    
    public enum DutchHouseType
    {
        Appartement,
        Huis,
        Studio
    }

    public static DutchGender TranslateGender(Gender gender)
    {
        return gender switch
        {
            Gender.Male => DutchGender.Man,
            Gender.Female => DutchGender.Vrouw,
            Gender.Other => DutchGender.Anders,
            _ => throw new ArgumentOutOfRangeException(nameof(gender), $"No mapping for {gender}")
        };
    }
    
    public static Gender TranslateGender(DutchGender gender)
    {
        return gender switch
        {
            DutchGender.Man => Gender.Male,
            DutchGender.Vrouw => Gender.Female,
            DutchGender.Anders => Gender.Other,
            _ => throw new ArgumentOutOfRangeException(nameof(gender), $"No mapping for {gender}")
        };
    }

    public static HouseType TranslateHouseType(DutchHouseType houseType)
    {
        return houseType switch
        {
            DutchHouseType.Appartement => HouseType.Apartment,
            DutchHouseType.Studio => HouseType.Studio,
            DutchHouseType.Huis => HouseType.House,
            _ => HouseType.House
        };
    }
    public static DutchHouseType TranslateHouseType(HouseType houseType)
    {
        return houseType switch
        {
            HouseType.Apartment => DutchHouseType.Appartement,
            HouseType.Studio => DutchHouseType.Studio,
            HouseType.House => DutchHouseType.Huis,
            _ => DutchHouseType.Huis
        };
    }
}