﻿namespace DTO.Logging;

public record RequestLogDto
(
    string? TraceIdentifier,
    string? ClientIp,
    string? Uri,
    DateTimeOffset RequestDate,
    string? Payload,
    string? Method,
    string? Token,
    Guid? UserId,
    ResponseLogDto? ResponseLog);

public record ResponseLogDto
(
    string? TraceIdentifier,
    DateTimeOffset ResponseDate,
    string? StatusCode,
    string? Token,
    Guid? UserId
);