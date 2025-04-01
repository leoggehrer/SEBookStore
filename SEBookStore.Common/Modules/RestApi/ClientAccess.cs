//@CodeCopy
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SEBookStore.Common.Modules.RestApi
{
    /// <summary>
    /// This class provides async methods for accessing Rest service endpoints.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the ClientAccess class.
    /// </remarks>
    /// <param name="baseAddress">The base address for the client.</param>
    /// <param name="sessionToken">The session token for the client.</param>
    public partial class ClientAccess(string baseAddress, string sessionToken)
    {
        #region static properties
        /// <summary>
        /// Returns the media type for JSON formatting.
        /// </summary>
        /// <returns>The media type string.</returns>
        protected static string MediaType => "application/json";
        /// <summary>
        /// Gets the options for deserializing JSON data into objects.
        /// </summary>
        /// <value>
        /// The options for deserializing JSON data into objects.
        /// </value>
        protected static JsonSerializerOptions DeserializerOptions => new() { PropertyNameCaseInsensitive = true };
        #endregion static properties
        
        #region static methods
        /// <summary>
        /// Creates a new instance of HttpClient with the specified base address.
        /// </summary>
        /// <param name="baseAddress">The base address of the server.</param>
        /// <returns>A new instance of HttpClient.</returns>
        protected static HttpClient CreateClient(string baseAddress)
        {
            HttpClient client = new();
            
            if (baseAddress.HasContent())
            {
                if (baseAddress.EndsWith('/') == false
                && baseAddress.EndsWith('\\') == false)
                {
                    baseAddress += "/";
                }
                
                client.BaseAddress = new Uri(baseAddress);
            }
            client.DefaultRequestHeaders.Accept.Clear();
            
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            return client;
        }
        /// <summary>
        /// Creates a new instance of HttpClient with the specified base address and session token.
        /// </summary>
        /// <param name="baseAddress">The base address of the HTTP client.</param>
        /// <param name="sessionToken">The session token to be used for authorization.</param>
        /// <returns>A new instance of HttpClient.</returns>
        protected static HttpClient CreateClient(string baseAddress, string sessionToken)
        {
            HttpClient client = CreateClient(baseAddress);
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{sessionToken}")));

            return client;
        }
        #endregion static methods

        #region properties
        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        public string BaseAddress { get; init; } = baseAddress;
        /// <summary>
        /// Gets or sets the session token for the current session.
        /// </summary>
        public string SessionToken { get; init; } = sessionToken;
        #endregion properties

        #region constructors
        /// <summary>
        /// Initializes a new instance of the ClientAccess class with the specified base address and an empty token.
        /// </summary>
        /// <param name="baseAddress">The base address of the client.</param>
        public ClientAccess(string baseAddress)
        : this(baseAddress, string.Empty)
        {
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Creates an instance of <see cref="HttpClient"/> with the specified <see cref="BaseAddress"/> and <see cref="SessionToken"/> if available.
        /// </summary>
        /// <returns>An instance of <see cref="HttpClient"/>.</returns>
        protected HttpClient CreateClient()
        {
            HttpClient result;
            
            if (SessionToken.HasContent())
            {
                result = CreateClient(BaseAddress, SessionToken);
            }
            else
            {
                result = CreateClient(BaseAddress);
            }
            return result;
        }
        #endregion methods
    }
}
