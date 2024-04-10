namespace IceCreamMAUI.Shared.Dtos;

public record struct IcecreamOptionDto(string Flavor, string Topping);

public record IceCreamDto(int Id, string Name, string Image, double Price, IcecreamOptionDto[] Options);