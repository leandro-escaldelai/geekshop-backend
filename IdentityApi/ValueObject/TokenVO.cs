using Microsoft.AspNetCore.Authentication;
using System.Text.Json.Serialization;
using IdentityApi.Model;
using AutoMapper;

namespace IdentityApi.ValueObject
{

    public class TokenVO
    {

        [JsonPropertyName("unauthorized_reason")]
        public string? UnauthorizedReason { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public long? ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

    }


    public class TokenProfile : Profile
    {

        public TokenProfile()
        {
            CreateMap<Token, TokenVO>();
        }

    }

}
