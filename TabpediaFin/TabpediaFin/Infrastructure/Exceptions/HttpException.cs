﻿using System.Net;

namespace TabpediaFin.Infrastructure.Exceptions;

public class HttpException : Exception
{
    public HttpException(HttpStatusCode code, object? errors = null)
    {
        Code = code;
        Errors = errors;
    }

    public object? Errors { get; set; }

    public HttpStatusCode Code { get; }
}
