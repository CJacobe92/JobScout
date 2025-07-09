using System;

namespace Application.Commands;

public record CreateTenantCommand(
    string Name,
    string License,
    string Phone,
    string RegisteredTo,
    string TIN,
    string Address
);
