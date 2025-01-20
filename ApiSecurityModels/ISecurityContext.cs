using System.Net.Http.Headers;

namespace SecurityModels;

public interface ISecurityContext
{

    int? UserId { get; }

	AuthenticationHeaderValue? Token { get; }

}
