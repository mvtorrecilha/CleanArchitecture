﻿namespace API.Models.Customer;

public class CustomerInput
{
    public string Name { get; set; }

    public string Email { get; set; }

    public DateOnly BirthDate { get; set; }
}