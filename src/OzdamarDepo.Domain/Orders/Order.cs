﻿using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.Users;

public class Order : Entity
{
    public string OrderNumber { get; set; } = default!;
    public DateTimeOffset Date { get; set; }

    public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;

    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string City { get; set; } = default!;
    public string District { get; set; } = default!;
    public string FullAdress { get; set; } = default!;

    public CargoStatusEnum CargoStatus { get; set; } = CargoStatusEnum.Bekliyor;

    public ICollection<Basket> Baskets { get; set; } = new List<Basket>();
}
