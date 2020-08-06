﻿using MediatR;
using System.Text.Json.Serialization;

namespace SqzTo.Application.CQRS.V1.SqzLink.Commands.UpdateSqzLink
{
    public class EditDestinationUrlCommand : IRequest
    {
        [JsonPropertyName("destination_url")]
        public string DestinationUrl { get; set; }
    }
}
