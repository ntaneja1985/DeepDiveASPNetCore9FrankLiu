# Securing Web API Endpoints with JWT (JSON Web Token) - Summary

## Overview

This lesson introduces JWT (JSON Web Token) as an authentication mechanism for securing Web API endpoints. Unlike cookie-based authentication used for browsers, JWT provides a stateless, self-verifying token format suitable for various client types including mobile apps, IoT devices, and other non-browser applications.

## Authentication Context Comparison

### Traditional Web Application (Browser-based)

```
┌─────────────┐    ┌──────────────────┐    ┌─────────────┐
│   Browser   │───▶│   HTTP Cookie    │───▶│ Web Server  │
│             │    │ (Authentication  │    │             │
│             │    │     Ticket)      │    │             │
└─────────────┘    └──────────────────┘    └─────────────┘

Storage: Browser Cookie Storage
Format: Encrypted Cookie
Transmission: Automatic with HTTP requests
```


### Web API (Multi-client)

```
┌─────────────┐    ┌──────────────────┐    ┌─────────────┐
│Mobile App   │───▶│  Bearer Token    │───▶│  Web API    │
├─────────────┤    │   (JWT)          │    │  Server     │
│Smart TV     │───▶│                  │───▶│             │
├─────────────┤    │                  │    │             │
│IoT Device   │───▶│                  │───▶│             │
├─────────────┤    │                  │    │             │
│Desktop App  │───▶│                  │───▶│             │
└─────────────┘    └──────────────────┘    └─────────────┘

Storage: Application Memory/Storage
Format: JSON Web Token (JWT)
Transmission: Authorization Header
```


## JWT Structure and Components

### JWT Format Diagram

```
JSON Web Token Structure:

Header.Payload.Signature

┌─────────────────────────┐ . ┌─────────────────────────┐ . ┌─────────────────────────┐
│        Header           │   │        Payload          │   │       Signature         │
│                         │   │                         │   │                         │
│  {                      │   │  {                      │   │   HMACSHA256(           │
│    "alg": "HS256",      │   │    "sub": "user123",    │   │     base64UrlEncode(    │
│    "typ": "JWT"         │   │    "name": "John Doe",  │   │       header) + "." +   │
│  }                      │   │    "role": "admin",     │   │       base64UrlEncode(  │
│                         │   │    "exp": 1234567890    │   │       payload),         │
│  (Base64Url Encoded)    │   │  }                      │   │     secret)             │
│                         │   │                         │   │                         │
│                         │   │  (Base64Url Encoded)    │   │  (Base64Url Encoded)    │
└─────────────────────────┘   └─────────────────────────┘   └─────────────────────────┘
```


### Bearer Token Format

```
Authorization Header Format:

Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyMTIzIiwibmFtZSI6IkpvaG4gRG9lIiwicm9sZSI6ImFkbWluIiwiZXhwIjoxMjM0NTY3ODkwfQ.signature_hash

├─────────┘ └──────────────────────────────────────────────────────────────────────────────────────┘
   Bearer                                    JWT Token
   Prefix                            (Header.Payload.Signature)
```


## JWT Self-Verification Process

### Token Generation Flow

```
JWT Generation Process:

┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│ User Claims │───▶│Hash Algorithm│───▶│Secret Key   │───▶│ Signature   │
│             │    │  (HS256)    │    │             │    │  (Hash)     │
│ {           │    │             │    │ "MySecret"  │    │             │
│  "sub": "1" │    │   HMAC      │    │             │    │ abc123def   │
│  "role":"A" │    │   SHA256    │    │             │    │             │
│ }           │    │             │    │             │    │             │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
                                                                │
                                                                ▼
                                                    ┌─────────────────────┐
                                                    │  Complete JWT       │
                                                    │  Header.Payload.    │
                                                    │  Signature          │
                                                    └─────────────────────┘
```


### Token Verification Flow

```
JWT Verification Process:

┌─────────────────────┐    ┌─────────────────────┐    ┌─────────────────────┐
│ Received JWT        │───▶│ Extract Payload     │───▶│ Re-compute Hash     │
│ Header.Payload.     │    │                     │    │                     │
│ Signature           │    │ User Claims         │    │ Use Same Algorithm  │
└─────────────────────┘    └─────────────────────┘    │ + Secret Key        │
                                    │                  └─────────────────────┘
                                    ▼                             │
                           ┌─────────────────────┐              ▼
                           │ Extract Signature   │    ┌─────────────────────┐
                           │                     │    │ New Hash Result     │
                           │ Original Hash       │    │                     │
                           └─────────────────────┘    │ def456ghi           │
                                    │                  └─────────────────────┘
                                    ▼                             │
                           ┌─────────────────────────────────────▼─┐
                           │         Compare Hashes               │
                           │                                      │
                           │ Original: abc123def                  │
                           │ New:      def456ghi                  │
                           │                                      │
                           │ Match? ✓ Valid   ✗ Invalid/Tampered │
                           └──────────────────────────────────────┘
```


## Client Types and JWT Usage

### Multi-Platform Client Support

```csharp
// Mobile App (Xamarin/MAUI)
public class MobileApiClient
{
    private readonly HttpClient _httpClient;
    private string _jwtToken;

    public async Task AuthenticateAsync(string username, string password)
    {
        var loginData = new { Username = username, Password = password };
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginData);
        var result = await response.Content.ReadFromJsonAsync<AuthResult>();
        
        _jwtToken = result.Token; // Store JWT in app memory/secure storage
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);
        
        return await _httpClient.GetFromJsonAsync<T>(endpoint);
    }
}

// Desktop App (WPF/WinUI)
public class DesktopApiClient
{
    private string _bearerToken;

    public void StoreToken(string jwt)
    {
        _bearerToken = $"Bearer {jwt}"; // Complete bearer token
        
        // Store in secure application settings
        Settings.Default.AuthToken = _bearerToken;
        Settings.Default.Save();
    }

    public async Task<HttpResponseMessage> CallApiAsync(string endpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Add("Authorization", _bearerToken);
        
        return await _httpClient.SendAsync(request);
    }
}

// IoT Device (Minimal implementation)
public class IoTApiClient
{
    private readonly string _baseUrl;
    private string _token;

    public async Task<string> GetSensorDataAsync()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        
        var response = await client.GetAsync($"{_baseUrl}/api/sensors/data");
        return await response.Content.ReadAsStringAsync();
    }
}
```


## JWT vs Cookie Authentication Comparison

| Aspect | JWT (Bearer Token) | Cookie Authentication |
| :-- | :-- | :-- |
| **Client Support** | Universal (Mobile, Desktop, IoT) | Browser-specific |
| **Storage** | Application memory/storage | Browser cookie storage |
| **Transmission** | Manual (Authorization header) | Automatic with requests |
| **Stateless** | ✅ Server doesn't store state | ❌ Server may store sessions |
| **Self-Verification** | ✅ Contains all needed info | ❌ Requires server lookup |
| **Cross-Domain** | ✅ Easy to implement | ❌ CORS complications |
| **Size** | Larger (contains claims) | Smaller (just reference) |
| **Expiration** | Built-in (exp claim) | Server-side configuration |
| **Revocation** | Difficult (stateless) | Easy (clear server session) |

## Security Considerations

### Secret Key Management

```csharp
// WRONG: Hardcoded secret (never do this)
var secret = "MyHardcodedSecret123";

// CORRECT: Environment variables or Key Vault
var secret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
var secret = configuration["JwtSettings:SecretKey"];

// BEST: Azure Key Vault or similar
var secret = await keyVaultClient.GetSecretAsync("jwt-secret");
```


### JWT Vulnerabilities and Mitigations

```
Common JWT Security Issues:

1. Weak Secret Keys
   ❌ Problem: Short, predictable secrets
   ✅ Solution: Long, random secrets (256+ bits)

2. Algorithm Confusion
   ❌ Problem: Accepting "none" algorithm
   ✅ Solution: Explicitly validate algorithm

3. Token Storage
   ❌ Problem: localStorage (XSS vulnerable)
   ✅ Solution: Secure storage, memory only

4. Token Expiration
   ❌ Problem: Long-lived tokens
   ✅ Solution: Short expiration + refresh tokens

5. Token Revocation
   ❌ Problem: Cannot revoke stateless tokens
   ✅ Solution: Blacklist or short expiration
```


## Interview Tips Table

| **Topic** | **Key Points** | **Sample Questions** | **Best Answers** |
| :-- | :-- | :-- | :-- |
| **JWT vs Cookies** | Different client support and use cases | "When would you use JWT instead of cookies?" | "JWT for APIs with diverse clients (mobile, IoT, desktop). Cookies for traditional web applications with browsers. JWT is stateless and cross-platform." |
| **JWT Structure** | Three parts: Header.Payload.Signature | "What are the parts of a JWT?" | "Header (algorithm and type), Payload (claims/user data), Signature (hash for verification). All base64url encoded and separated by dots." |
| **Self-Verification** | Token contains everything needed for validation | "How does JWT self-verification work?" | "Server re-computes signature using payload + secret key, compares with token's signature. Matching signatures prove token integrity and authenticity." |
| **Bearer Token Format** | Authorization header with Bearer prefix | "How do you send JWT in HTTP requests?" | "Authorization header: 'Bearer <jwt_token>'. Client must manually add to each request, unlike cookies which are automatic." |
| **Stateless Nature** | Server doesn't need to store session state | "What makes JWT stateless?" | "All user information is in the token itself. Server doesn't need to store or look up session data, just validates the signature with its secret key." |
| **Client Storage** | Various storage options for different platforms | "Where do clients store JWT tokens?" | "Mobile apps: secure storage/keychain. Desktop: encrypted app settings. Web: memory (not localStorage due to XSS). IoT: device memory." |
| **Security Considerations** | Secret key protection and token management | "What are JWT security best practices?" | "Strong secret keys, short expiration times, secure storage, algorithm validation, HTTPS only, consider refresh tokens for long sessions." |
| **Cross-Platform Support** | Works with any HTTP client | "Why is JWT good for microservices?" | "Platform-agnostic, works with any HTTP client, no server-side session storage needed, scales horizontally, contains all auth info." |
| **Token Expiration** | Built-in expiration mechanism | "How does JWT handle expiration?" | "exp claim in payload sets expiration timestamp. Server validates current time against exp claim. Expired tokens are automatically rejected." |
| **Revocation Challenges** | Difficulty in revoking stateless tokens | "How do you revoke a JWT token?" | "JWT is stateless, so revocation is challenging. Solutions: short expiration + refresh tokens, token blacklist, or move to stateful tokens when needed." |

## Key Takeaways

1. **Multi-Client Support**: JWT enables authentication for diverse client types beyond browsers
2. **Bearer Token Format**: Uses "Bearer <token>" in Authorization header for transmission
3. **Self-Verification**: JWT structure allows server to validate tokens without external lookups
4. **Stateless Design**: Server doesn't need to store session state, improving scalability
5. **Three-Part Structure**: Header.Payload.Signature provides security through cryptographic verification
6. **Secret Key Security**: Proper secret key management is crucial for JWT security
7. **Platform Agnostic**: Works with any HTTP client on any platform
8. **Manual Transmission**: Unlike cookies, JWT must be manually included in each request
9. **Built-in Expiration**: Tokens include expiration claims for automatic invalidation
10. **Trade-offs**: More flexible than cookies but larger size and revocation challenges

JWT authentication provides a robust, scalable solution for securing Web APIs across diverse client platforms, making it the preferred choice for modern distributed applications and microservices architectures.



# Generating JWT Authentication Tickets for Web API - Summary

## Overview

This tutorial demonstrates how to create a JWT authentication endpoint in ASP.NET Core Web API. Unlike browser-based cookie authentication, JWT tokens are designed for diverse client types (mobile apps, desktop applications, IoT devices) and require manual transmission via the Authorization header.

## Authentication Flow Comparison

### Web Application (Cookie-based)

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Browser   │───▶│ Web Server  │───▶│   Cookie    │
│  (UI Form)  │    │ Controller  │    │(Auto-stored)│
└─────────────┘    └─────────────┘    └─────────────┘
```


### Web API (JWT-based)

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│Various      │───▶│  Web API    │───▶│JWT Token    │
│Clients      │    │ Endpoint    │    │(Manual      │
│(Mobile/IoT) │    │(Minimal API)│    │ Storage)    │
└─────────────┘    └─────────────┘    └─────────────┘
```


## Implementation Code

### 1. Credential Model for API Clients

```csharp
// Models/CredentialViewModel.cs
public class CredentialViewModel
{
    [Required]
    public string ClientId { get; set; } = string.Empty;
    
    [Required]
    public string ClientSecret { get; set; } = string.Empty;
}
```


### 2. JWT Configuration in appsettings.json

```json
{
  "ClientAuthentication": {
    "SecurityKey": "KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

- Install this nuget package
- ![alt text](image.png)

### 3. Account Endpoint Implementation

```csharp
// Endpoints/AccountEndpoint.cs
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public static class AccountEndpoint
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        app.MapPost("/account/login", async (
            CredentialViewModel credential, 
            IConfiguration configuration) =>
        {
            // Hardcoded client validation (replace with database lookup in production)
            if ((credential.ClientId == "web-app-001" || credential.ClientId == "web-app-002") && 
                credential.ClientSecret == "your-generated-secret-key-here")
            {
                // Create claims based on client type
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, credential.ClientId)
                };

                // Add role-specific claims
                if (credential.ClientId == "web-app-001")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                // Set token expiration (typically longer than cookies)
                var expiresAt = DateTime.UtcNow.AddMinutes(20);

                // Generate JWT token
                var accessToken = CreateToken(claims, expiresAt, configuration);

                // Return token response
                return TypedResults.Ok(new
                {
                    access_token = accessToken,
                    expires_at = expiresAt
                });
            }

            // Return validation errors for invalid credentials
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["ClientId"] = ["Errors in Client ID or Client Secret"],
                ["ClientSecret"] = ["Errors in Client ID or Client Secret"]
            }, statusCode: 401);
        })
        .WithParameterValidation()
        .WithTags("Account");
    }

    private static string CreateToken(List<Claim> claims, DateTime expiresAt, IConfiguration configuration)
    {
        // Create JWT token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // Get security key from configuration
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                configuration["ClientAuthentication:SecurityKey"] ?? string.Empty
            )
        );

        // Create signing credentials
        var signingCredentials = new SigningCredentials(
            securityKey, 
            SecurityAlgorithms.HmacSha256Signature
        );

        // Convert claims to dictionary for JWT payload
        var claimsDictionary = new Dictionary<string, object>();
        foreach (var claim in claims)
        {
            claimsDictionary[claim.Type] = claim.Value;
        }

        // Create token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Claims = claimsDictionary,
            Expires = expiresAt,
            SigningCredentials = signingCredentials
            // Issuer and Audience can be added for public APIs
            // Issuer = "your-api-issuer",
            // Audience = "your-api-audience"
        };

        // Generate and return JWT token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
```


### 4. Program.cs Registration

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Register endpoints
app.MapAccountEndpoints(); // Add this line
// ... other endpoint mappings

app.Run();
```


## JWT Token Structure Breakdown

### Generated Token Example

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ3ZWItYXBwLTAwMSIsIm5hbWUiOiJ3ZWItYXBwLTAwMSIsInJvbGUiOiJBZG1pbiIsImV4cCI6MTY5MzQ2NzYwMH0.signature_hash_here

├─────────────────────────────────────┘ └─────────────────────────────────────────────────────────────────────────────────┘ └─────────────────┘
         Header (Base64)                                    Payload (Base64)                                    Signature (Base64)
```


### Decoded Components

#### Header

```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```


#### Payload (Claims)

```json
{
  "sub": "web-app-001",
  "name": "web-app-001", 
  "role": "Admin",
  "exp": 1693467600
}
```


#### Signature

```
HMACSHA256(
  base64UrlEncode(header) + "." + 
  base64UrlEncode(payload),
  secret_key
)
```


## Client Authentication Types

### Admin Client

```json
{
  "clientId": "web-app-001",
  "clientSecret": "your-generated-secret-key-here"
}

Response:
{
  "access_token": "eyJhbGci...",
  "expires_at": "2024-08-31T10:30:00Z"
}

Claims in Token:
- Name: "web-app-001" 
- Role: "Admin"
```


### Regular User Client

```json
{
  "clientId": "web-app-002", 
  "clientSecret": "your-generated-secret-key-here"
}

Response:
{
  "access_token": "eyJhbGci...",
  "expires_at": "2024-08-31T10:30:00Z"
}

Claims in Token:
- Name: "web-app-002"
- Role: "User"
```


## Security Considerations

### 1. Secret Key Management

```csharp
// ❌ WRONG: Hardcoded secrets
var key = "hardcoded-secret-123";

// ✅ CORRECT: Configuration-based
var key = configuration["ClientAuthentication:SecurityKey"];

// ✅ BEST: Environment variables (overrides appsettings.json)
// Environment variable: ClientAuthentication__SecurityKey=your-secure-key
```


### 2. Key Requirements

```csharp
// Security key must be at least 256 bits (32 characters) for HMAC-SHA256
var securityKey = "KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK"; // 32 characters minimum

// Generate secure random key (production approach)
using (var rng = RandomNumberGenerator.Create())
{
    var keyBytes = new byte[32];
    rng.GetBytes(keyBytes);
    var secureKey = Convert.ToBase64String(keyBytes);
}
```


### 3. Token Expiration Strategy

```csharp
// Different expiration strategies
var shortLived = DateTime.UtcNow.AddMinutes(5);   // High security
var standard = DateTime.UtcNow.AddMinutes(20);    // Balanced
var longLived = DateTime.UtcNow.AddHours(8);      // Convenience

// Refresh token pattern (recommended for production)
var accessToken = DateTime.UtcNow.AddMinutes(15);  // Short-lived
var refreshToken = DateTime.UtcNow.AddDays(30);    // Long-lived
```


## Testing with Swagger

### 1. Valid Credentials Test

```
POST /account/login
{
  "clientId": "web-app-001",
  "clientSecret": "your-generated-secret-key-here"
}

Expected Response (200 OK):
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expires_at": "2024-08-31T10:30:00Z"
}
```


### 2. Invalid Credentials Test

```
POST /account/login  
{
  "clientId": "web-app-001",
  "clientSecret": "wrong-secret"
}

Expected Response (401 Unauthorized):
{
  "errors": {
    "ClientId": ["Errors in Client ID or Client Secret"],
    "ClientSecret": ["Errors in Client ID or Client Secret"]
  }
}
```


### 3. JWT Token Verification

Use [jwt.io](https://jwt.io) to decode and verify generated tokens:

- Paste token in "Encoded" section
- Verify algorithm shows "HS256"
- Check payload contains expected claims
- Verify signature with your secret key


## Next Steps Preview

### Client-Side Token Usage

```csharp
// How clients will use the generated JWT (next lesson topic)
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", accessToken);

var response = await httpClient.GetAsync("/api/protected-endpoint");
```


## Interview Tips Table

| **Topic** | **Key Points** | **Sample Questions** | **Best Answers** |
| :-- | :-- | :-- | :-- |
| **JWT vs Cookies** | Different storage and transmission mechanisms | "Why use JWT for APIs instead of cookies?" | "JWT works with any HTTP client (mobile, desktop, IoT), while cookies are browser-specific. JWT is stateless and platform-agnostic." |
| **JWT Structure** | Header.Payload.Signature format | "What are the three parts of a JWT?" | "Header (algorithm/type), Payload (claims/user data), Signature (HMAC hash for verification). All base64url encoded, separated by dots." |
| **Token Generation** | JwtSecurityTokenHandler creates tokens | "How do you generate JWT in .NET?" | "Use JwtSecurityTokenHandler with SecurityTokenDescriptor containing claims, expiration, and signing credentials with symmetric key." |
| **Claims vs Identity** | JWT uses claims directly, not Identity/Principal | "How does JWT differ from cookie authentication claims?" | "JWT stores claims directly in payload, no need for ClaimsIdentity/ClaimsPrincipal wrappers. Claims are visible in decoded token." |
| **Security Key** | Symmetric key for HMAC-SHA256 signing | "How do you secure JWT tokens?" | "Use strong 256-bit symmetric key, store in configuration/environment variables, never hardcode. Key signs and verifies tokens." |
| **Token Expiration** | Built-in exp claim for automatic validation | "How does JWT handle expiration?" | "exp claim in payload contains Unix timestamp. Server automatically rejects expired tokens during validation." |
| **Client Authentication** | ClientId/ClientSecret instead of username/password | "What's the difference between user and client authentication?" | "Client auth identifies applications (ClientId/Secret), user auth identifies people (username/password). APIs often authenticate client applications first." |
| **Stateless Design** | No server-side session storage needed | "What makes JWT stateless?" | "All user/client information is in the token. Server doesn't store sessions, just validates signature and claims on each request." |
| **Configuration Management** | Externalize secrets from code | "How do you manage JWT secrets securely?" | "Store in appsettings.json for development, environment variables for production, Azure Key Vault for enterprise. Never commit secrets to source control." |
| **Error Handling** | 401 for authentication failures | "How do you handle JWT authentication errors?" | "Return 401 Unauthorized for invalid credentials, generic error messages to prevent information disclosure, proper HTTP status codes." |

## Key Takeaways

1. **API-Specific Authentication**: JWT designed for diverse client types beyond browsers
2. **Minimal API Approach**: Use endpoint mapping for clean, lightweight API design
3. **Token Structure**: Three-part format (Header.Payload.Signature) enables self-verification
4. **Claims-Based**: Direct claims storage in JWT payload, no Identity/Principal wrappers needed
5. **Security Key Management**: Use configuration for secrets, environment variables for production
6. **Client Authentication**: ClientId/ClientSecret pattern for application authentication
7. **Token Expiration**: Built-in exp claim for automatic expiration validation
8. **Stateless Design**: No server-side session storage required
9. **Manual Transmission**: Clients must manually include JWT in Authorization headers
10. **Testing Tools**: Use jwt.io for token decoding and verification during development

This JWT authentication endpoint provides a foundation for securing Web API endpoints across multiple client platforms, with the next step being to implement JWT validation middleware for protected endpoints.



# Storing and Sending JWT from Client to Server - Summary

## Overview

This tutorial demonstrates how a web application (client) can store and send JWT tokens when communicating with a Web API (server). Unlike browser-cookie authentication that handles token storage automatically, JWT requires manual handling of token storage, retrieval, and transmission via HTTP headers.

## Architecture Change

### Previous: Browser ↔ Web Application

```
┌─────────────┐    ┌─────────────┐
│   Browser   │───▶│Web Application│
│   (Client)  │    │   (Server)   │
│             │◀───│              │
└─────────────┘    └─────────────┘
Cookie: Automatic storage & transmission
```


### Current: Web Application ↔ Web API

```
┌─────────────┐    ┌─────────────┐
│Web Application│───▶│  Web API    │
│   (Client)   │    │  (Server)   │
│              │◀───│             │
└─────────────┘    └─────────────┘
JWT: Manual storage & transmission required
```


## JWT Token Flow Diagram

```
JWT Authentication Flow:

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Web App       │───▶│   POST Login    │───▶│   Web API       │
│   HTTP Client   │    │   Endpoint      │    │   /account/login│
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                                              │
         ▼                                              ▼
┌─────────────────┐                          ┌─────────────────┐
│  Session Store  │                          │  JWT Token      │
│  (Cookie-based) │◀─────────────────────────│  Response       │
└─────────────────┘                          └─────────────────┘
         │
         ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Subsequent      │───▶│ Authorization   │───▶│ Protected API   │
│ API Requests    │    │ Header          │    │ Endpoints       │
│                 │    │ Bearer <token>  │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```




## Implementation Code

### 1. JWT Token Model

```csharp
// Models/JsonWebToken.cs
using System.Text.Json.Serialization;

public class JsonWebToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }
}
```

- ![alt text](image-1.png)
### 2. JWT Authentication Handler (Message Handler)

```csharp
// MessageHandlers/JwtAuthenticationHandler.cs
using System.Net.Http.Headers;
using System.Text.Json;

public class JwtAuthenticationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public JwtAuthenticationHandler(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        // Get HTTP context and session
        var httpContext = _httpContextAccessor.HttpContext;
        var session = httpContext?.Session;

        // Attempt to get cached token from session
        var strJwtToken = session?.GetString("access_token");
        JsonWebToken? token = null;

        if (string.IsNullOrWhiteSpace(strJwtToken))
        {
            // No cached token - login to get new token
            token = await LoginAsync();
        }
        else
        {
            // Deserialize cached token
            token = JsonSerializer.Deserialize<JsonWebToken>(strJwtToken) ?? new JsonWebToken();

            // Check if token is expired or invalid
            if (token == null || 
                string.IsNullOrWhiteSpace(token.AccessToken) || 
                token.ExpiresAt <= DateTime.UtcNow)
            {
                // Token expired - get new token
                token = await LoginAsync();
            }
        }

        // Add JWT to Authorization header if valid token exists
        if (!string.IsNullOrWhiteSpace(token.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        // Continue with the request
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<JsonWebToken> LoginAsync()
    {
        // Create separate HTTP client (avoid recursive loop)
        using var client = new HttpClient();
        
        // Build login endpoint URL
        var loginUrl = $"{_configuration["WebApi:Url"]}account/login";
        
        // Prepare credentials
        var credentials = new
        {
            clientId = _configuration["WebApi:ClientId"],
            clientSecret = _configuration["WebApi:ClientSecret"]
        };

        // Post credentials to login endpoint
        var response = await client.PostAsJsonAsync(loginUrl, credentials);
        response.EnsureSuccessStatusCode();

        // Read response as string
        var strJwt = await response.Content.ReadAsStringAsync();

        // Store token in session for caching
        var httpContext = _httpContextAccessor.HttpContext;
        httpContext?.Session.SetString("access_token", strJwt);

        // Deserialize and return token object
        return JsonSerializer.Deserialize<JsonWebToken>(strJwt) ?? new JsonWebToken();
    }
}
```


### 3. Configuration in appsettings.json

```json
{
  "WebApi": {
    "Url": "https://localhost:7001/",
    "ClientId": "web-app-001",
    "ClientSecret": "your-generated-secret-key-here"
  },
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\mssqllocaldb;Database=EmployeeManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
  }
}
```


### 4. Program.cs Configuration

```csharp
// Program.cs (Web Application)
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add HTTP Context Accessor for message handler
builder.Services.AddHttpContextAccessor();

// Configure Session (for JWT caching)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);         // Session timeout
    options.Cookie.HttpOnly = true;                        // Security: prevent JavaScript access
    options.Cookie.IsEssential = true;                     // Essential for functionality
    options.Cookie.SameSite = SameSiteMode.Strict;         // CSRF protection
});

// Configure HTTP Client with JWT Authentication Handler
builder.Services.AddHttpClient("ApiEndpoint", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebApi:Url"] ?? "");
})
.AddHttpMessageHandler<JwtAuthenticationHandler>();

// Register JWT Authentication Handler
builder.Services.AddTransient<JwtAuthenticationHandler>();

// Configure Cookie Authentication (existing)
builder.Services.AddAuthentication("cookie-scheme")
    .AddCookie("cookie-scheme", options =>
    {
        options.Cookie.Name = "cookie-scheme";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;                     // Enhanced security
        options.Cookie.SameSite = SameSiteMode.Strict;      // Enhanced security
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

// Register API repositories
builder.Services.AddSingleton<IDepartmentsApiRepository, DepartmentsApiRepository>();
builder.Services.AddSingleton<IEmployeesApiRepository, EmployeesApiRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();                    // Add session middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```


### 5. HTTP Client Middleware Pipeline Concept

```csharp
// Conceptual representation of HTTP Client middleware pipeline
HttpClient Pipeline:
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Application     │───▶│JWT Auth Handler │───▶│HTTP Client      │
│ Code            │    │(Add Bearer      │    │Handler          │
│                 │    │ Token)          │    │(Send Request)   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                                │                       │
                                ▼                       ▼
                       ┌─────────────────┐    ┌─────────────────┐
                       │ Session Check   │    │ HTTP Request    │
                       │ Token Caching   │    │ to Web API      │
                       │ Login if needed │    │                 │
                       └─────────────────┘    └─────────────────┘
```


## Session-based Token Caching

### Session Storage Strategy

```csharp
// Token storage in session (cookie-based session)
public void StoreToken(JsonWebToken token)
{
    var json = JsonSerializer.Serialize(token);
    HttpContext.Session.SetString("access_token", json);
}

public JsonWebToken? GetStoredToken()
{
    var json = HttpContext.Session.GetString("access_token");
    return string.IsNullOrEmpty(json) ? null : 
           JsonSerializer.Deserialize<JsonWebToken>(json);
}

// Automatic expiration check
public bool IsTokenValid(JsonWebToken token)
{
    return token != null && 
           !string.IsNullOrWhiteSpace(token.AccessToken) && 
           token.ExpiresAt > DateTime.UtcNow;
}
```


### Token Lifecycle Management

```
Token Lifecycle in Session:

1. First Request:
   ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
   │ No Token    │───▶│ Login API   │───▶│ Store in    │
   │ in Session  │    │ Call        │    │ Session     │
   └─────────────┘    └─────────────┘    └─────────────┘

2. Subsequent Requests (Token Valid):
   ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
   │ Get Token   │───▶│ Check       │───▶│ Use Cached  │
   │ from Session│    │ Expiration  │    │ Token       │
   └─────────────┘    └─────────────┘    └─────────────┘

3. Token Expired:
   ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
   │ Expired     │───▶│ New Login   │───▶│ Update      │
   │ Token Found │    │ API Call    │    │ Session     │
   └─────────────┘    └─────────────┘    └─────────────┘
```


## Example API Repository Usage

```csharp
// DepartmentsApiRepository.cs
public class DepartmentsApiRepository : IDepartmentsApiRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DepartmentsApiRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Department>?> GetDepartmentsAsync()
    {
        // Create named client - JWT handler automatically applies
        var client = _httpClientFactory.CreateClient("ApiEndpoint");
        
        // JWT token automatically added to Authorization header by handler
        var departments = await client.GetFromJsonAsync<List<Department>>("api/departments");
        
        return departments;
    }

    public async Task<Department?> GetDepartmentAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiEndpoint");
        
        // Authorization: Bearer <jwt_token> header added automatically
        var department = await client.GetFromJsonAsync<Department>($"api/departments/{id}");
        
        return department;
    }

    // All methods automatically include JWT without modification
    public async Task<Department?> AddDepartmentAsync(Department department)
    {
        var client = _httpClientFactory.CreateClient("ApiEndpoint");
        var response = await client.PostAsJsonAsync("api/departments", department);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Department>();
        }
        return null;
    }
}
```


## Security Considerations

### Session Security Configuration

```csharp
builder.Services.AddSession(options =>
{
    // Session timeout - balance between security and usability
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    
    // Prevent JavaScript access to session cookie
    options.Cookie.HttpOnly = true;
    
    // CSRF protection - only send with same-site requests
    options.Cookie.SameSite = SameSiteMode.Strict;
    
    // Mark as essential for application functionality
    options.Cookie.IsEssential = true;
    
    // HTTPS only in production
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
```


### Token Validation Logic

```csharp
// Comprehensive token validation
private bool IsTokenValid(JsonWebToken? token)
{
    if (token == null) return false;
    if (string.IsNullOrWhiteSpace(token.AccessToken)) return false;
    if (token.ExpiresAt <= DateTime.UtcNow.AddMinutes(-1)) return false; // 1-minute buffer
    
    return true;
}
```


## Debugging and Testing

### Setting Breakpoints for Token Flow

```csharp
// Key debugging points:
// 1. Message handler entry point
protected override async Task<HttpResponseMessage> SendAsync(/* ... */)
{
    // Breakpoint here - inspect session state
    
    // 2. Token retrieval from session
    var strJwtToken = session?.GetString("access_token");
    // Breakpoint here - check if token exists in session
    
    // 3. Token validation
    if (token == null || string.IsNullOrWhiteSpace(token.AccessToken) || 
        token.ExpiresAt <= DateTime.UtcNow)
    {
        // Breakpoint here - see why token is invalid
    }
    
    // 4. Authorization header assignment
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
    // Breakpoint here - verify token is attached to request
}
```


## Interview Tips Table

| **Topic** | **Key Points** | **Sample Questions** | **Best Answers** |
| :-- | :-- | :-- | :-- |
| **Manual Token Handling** | JWT requires explicit storage and transmission | "How does JWT differ from cookie authentication in client handling?" | "Cookies are automatically stored and sent by browsers. JWT requires manual storage (session, memory, etc.) and manual inclusion in Authorization headers for each request." |
| **Message Handlers** | HTTP Client middleware pipeline for cross-cutting concerns | "How do you add JWT to all API requests automatically?" | "Use HTTP message handlers (DelegatingHandler) in the HTTP Client pipeline. They intercept all requests and can add headers, modify requests, or handle authentication centrally." |
| **Token Caching Strategy** | Session-based caching to avoid repeated login calls | "Where do you store JWT tokens in web applications?" | "Session storage (cookie-backed), in-memory cache, or secure browser storage. Session provides automatic expiration and server-side control." |
| **Session Configuration** | Security settings for session cookies | "How do you secure session storage for JWT tokens?" | "Configure HttpOnly, SameSite=Strict, appropriate timeout, mark as essential. Use HTTPS in production for secure transmission." |
| **Token Expiration Handling** | Automatic refresh when tokens expire | "How do you handle expired JWT tokens?" | "Check expiration before use, automatically call login endpoint for new token, cache the new token, retry the original request with fresh token." |
| **HTTP Client Factory** | Named clients with configured message handlers | "How do you configure HTTP clients for API communication?" | "Use IHttpClientFactory with named clients, register message handlers for cross-cutting concerns like authentication, configure base addresses and default headers." |
| **Separation of Concerns** | Authentication logic separate from business logic | "Where should JWT authentication logic be placed?" | "In message handlers or middleware - separate from business logic. Repository classes shouldn't handle authentication; it's handled transparently by the HTTP client pipeline." |
| **Error Handling** | Graceful handling of authentication failures | "How do you handle JWT authentication errors?" | "Ensure success status code from login endpoint, handle network failures, provide fallback mechanisms, log authentication errors for debugging." |
| **Configuration Management** | External configuration for endpoints and credentials | "How do you manage API endpoint configuration?" | "Use appsettings.json for base configuration, environment variables for secrets, consider Azure Key Vault for production secrets." |
| **Testing Strategy** | Debugging token flow and API communication | "How do you test JWT authentication in web applications?" | "Set breakpoints in message handlers, inspect session state, verify Authorization headers in network traffic, test token expiration scenarios." |

## Key Takeaways

1. **Manual Token Management**: JWT requires explicit storage and transmission, unlike automatic cookie handling
2. **Message Handler Pattern**: Use `DelegatingHandler` to centrally manage JWT authentication for all API calls
3. **Session-based Caching**: Store JWT tokens in session to avoid repeated authentication calls
4. **Token Expiration**: Implement automatic token refresh when expired tokens are detected
5. **Security Configuration**: Properly configure session cookies with security settings
6. **HTTP Client Factory**: Use named clients with message handlers for clean separation of concerns
7. **Configuration Externalization**: Store API endpoints and credentials in configuration, not code
8. **Error Handling**: Implement robust error handling for authentication failures and network issues
9. **Debugging Strategy**: Use breakpoints in message handlers to trace token flow and troubleshoot issues
10. **Transparent Integration**: Business logic classes (repositories) don't need to handle authentication directly

This implementation provides a robust, maintainable approach to JWT authentication in web applications, with automatic token management that's transparent to the business logic layers.


# Alternative JWT Storage: Static Repository Approach - Summary

## Overview

This brief lesson discusses alternative storage methods for JWT tokens beyond session storage. While session storage works well for web applications due to browser support, other client types (mobile apps, IoT devices, desktop applications) require different storage strategies.

## Storage Strategy Comparison

### Current Approach: Session Storage (Web Application)

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Browser   │───▶│Web Application│───▶│  Web API    │
│             │    │   (Client)   │    │  (Server)   │
│   Session   │◀───│  HTTP Client │    │             │
│   Cookie    │    │   Handler    │    │             │
└─────────────┘    └─────────────┘    └─────────────┘

Storage: Session (Cookie-backed)
Scope: Per browser session
Thread Safety: Automatic (per session)
```


### Alternative: Static Repository (Mobile/IoT)

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│ Mobile App  │───▶│ Static Token │───▶│  Web API    │
│             │    │ Repository   │    │  (Server)   │
│  IoT Device │◀───│  (In-Memory) │    │             │
│             │    │   Storage    │    │             │
└─────────────┘    └─────────────┘    └─────────────┘

Storage: Static variables or Singleton
Scope: Application-wide
Thread Safety: Manual (locks required)
```


## Implementation Examples

### 1. Static Repository Approach

```csharp
// TokenRepository/StaticJwtTokenRepository.cs
public static class StaticJwtTokenRepository
{
    private static JsonWebToken? _cachedToken;
    private static readonly object _lockObject = new object();

    public static JsonWebToken? GetToken()
    {
        lock (_lockObject)
        {
            // Check if token exists and is still valid
            if (_cachedToken != null && _cachedToken.ExpiresAt > DateTime.UtcNow)
            {
                return _cachedToken;
            }
            return null; // Token expired or doesn't exist
        }
    }

    public static void SetToken(JsonWebToken token)
    {
        lock (_lockObject)
        {
            _cachedToken = token;
        }
    }

    public static void ClearToken()
    {
        lock (_lockObject)
        {
            _cachedToken = null;
        }
    }

    public static bool IsTokenValid()
    {
        lock (_lockObject)
        {
            return _cachedToken != null && 
                   !string.IsNullOrWhiteSpace(_cachedToken.AccessToken) && 
                   _cachedToken.ExpiresAt > DateTime.UtcNow;
        }
    }
}
```


### 2. Singleton Service Approach

```csharp
// Services/IJwtTokenService.cs
public interface IJwtTokenService
{
    JsonWebToken? GetToken();
    void SetToken(JsonWebToken token);
    void ClearToken();
    bool IsTokenValid();
}

// Services/JwtTokenService.cs
public class JwtTokenService : IJwtTokenService
{
    private JsonWebToken? _cachedToken;
    private readonly object _lockObject = new object();

    public JsonWebToken? GetToken()
    {
        lock (_lockObject)
        {
            if (_cachedToken != null && _cachedToken.ExpiresAt > DateTime.UtcNow)
            {
                return _cachedToken;
            }
            return null;
        }
    }

    public void SetToken(JsonWebToken token)
    {
        lock (_lockObject)
        {
            _cachedToken = token;
        }
    }

    public void ClearToken()
    {
        lock (_lockObject)
        {
            _cachedToken = null;
        }
    }

    public bool IsTokenValid()
    {
        lock (_lockObject)
        {
            return _cachedToken != null && 
                   !string.IsNullOrWhiteSpace(_cachedToken.AccessToken) && 
                   _cachedToken.ExpiresAt > DateTime.UtcNow;
        }
    }
}
```


### 3. Mobile Application Usage Example

```csharp
// Mobile App - JWT Authentication Handler with Static Repository
public class MobileJwtAuthenticationHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;

    public MobileJwtAuthenticationHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        // Get token from static repository (no session available)
        var token = StaticJwtTokenRepository.GetToken();

        if (token == null || !StaticJwtTokenRepository.IsTokenValid())
        {
            // No valid token - login to get new token
            token = await LoginAsync();
            
            if (token != null)
            {
                // Store in static repository
                StaticJwtTokenRepository.SetToken(token);
            }
        }

        // Add JWT to Authorization header
        if (token != null && !string.IsNullOrWhiteSpace(token.AccessToken))
        {
            request.Headers.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<JsonWebToken?> LoginAsync()
    {
        // Similar login logic as web application, but no session storage
        using var client = new HttpClient();
        
        var credentials = new
        {
            clientId = _configuration["WebApi:ClientId"],
            clientSecret = _configuration["WebApi:ClientSecret"]
        };

        var response = await client.PostAsJsonAsync($"{_configuration["WebApi:Url"]}account/login", credentials);
        
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonWebToken>(jsonString);
        }

        return null;
    }
}
```


### 4. IoT Device Implementation

```csharp
// IoT Device - Simplified JWT handling
public class IoTApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public IoTApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        // Ensure we have a valid token
        await EnsureValidTokenAsync();

        // Get token from static repository
        var token = StaticJwtTokenRepository.GetToken();
        
        if (token != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
        
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }

        return default;
    }

    private async Task EnsureValidTokenAsync()
    {
        if (!StaticJwtTokenRepository.IsTokenValid())
        {
            // Login and cache token
            var token = await LoginAsync();
            if (token != null)
            {
                StaticJwtTokenRepository.SetToken(token);
            }
        }
    }

    private async Task<JsonWebToken?> LoginAsync()
    {
        // IoT device authentication logic
        var credentials = new
        {
            clientId = "iot-device-001",
            clientSecret = "iot-device-secret"
        };

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}account/login", credentials);
        
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonWebToken>(json);
        }

        return null;
    }
}
```


## Platform-Specific Storage Considerations

### Web Applications (Multi-User)

```csharp
// Problem: Multiple users, potential bottleneck with static storage
// Solution: Session storage (current approach)

// Why Session is Better for Web Apps:
// ✅ Per-user isolation
// ✅ Automatic cleanup on session expiry  
// ✅ No thread contention between users
// ✅ Browser handles cookie storage automatically

public class WebAppJwtHandler : DelegatingHandler
{
    // Uses session storage - one token per user session
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    // Each user gets their own token in their session
    // No shared state = no bottleneck
}
```


### Mobile/Desktop Applications (Single User)

```csharp
// Mobile apps typically serve single user
// Static storage is simpler and sufficient

public class MobileApp
{
    private void InitializeAuth()
    {
        // Single user - static storage works well
        // No multi-threading concerns for different users
        // Simple lock handles concurrent API calls from same user
    }
}
```


### IoT Devices (Single Purpose)

```csharp
// IoT devices often single-purpose, single "user"
// Static storage ideal for simplicity

public class SmartThermostat
{
    private async Task UpdateTemperatureAsync()
    {
        // Single device, single token
        // Static storage perfectly appropriate
        var client = new IoTApiClient("https://api.smarthome.com/");
        await client.PostAsync("temperature", new { temp = 72.5 });
    }
}
```


## Thread Safety Considerations

### Lock-based Thread Safety

```csharp
public static class ThreadSafeTokenRepository
{
    private static JsonWebToken? _token;
    private static readonly object _lockObject = new object();

    public static JsonWebToken? GetToken()
    {
        lock (_lockObject)  // Synchronize access
        {
            return _token?.ExpiresAt > DateTime.UtcNow ? _token : null;
        }
    }

    public static void SetToken(JsonWebToken token)
    {
        lock (_lockObject)  // Synchronize modification
        {
            _token = token;
        }
    }

    // Potential bottleneck in high-traffic web applications
    // Multiple users waiting for lock = performance issue
}
```


### Concurrent Collections (Advanced)

```csharp
// For more advanced scenarios
public static class ConcurrentTokenRepository
{
    private static readonly ConcurrentDictionary<string, JsonWebToken> _tokens = new();

    public static JsonWebToken? GetToken(string userId)
    {
        if (_tokens.TryGetValue(userId, out var token))
        {
            return token.ExpiresAt > DateTime.UtcNow ? token : null;
        }
        return null;
    }

    public static void SetToken(string userId, JsonWebToken token)
    {
        _tokens.AddOrUpdate(userId, token, (key, oldToken) => token);
    }

    // Better for multi-user scenarios, but adds complexity
}
```


## Platform Comparison Table

| Platform Type | Storage Method | Thread Safety | Complexity | Best Use Case |
| :-- | :-- | :-- | :-- | :-- |
| **Web Application** | Session Storage | Automatic (per-session) | Medium | Multi-user browser apps |
| **Mobile App** | Static Repository | Manual locks | Low | Single-user mobile apps |
| **Desktop App** | Singleton Service | Manual locks | Medium | Single-user desktop apps |
| **IoT Device** | Static Variables | Manual locks | Low | Single-purpose devices |
| **Console App** | Static Repository | Manual locks | Low | Batch processing, utilities |

## Interview Tips Table

| **Topic** | **Key Points** | **Sample Questions** | **Best Answers** |
| :-- | :-- | :-- | :-- |
| **Storage Strategy Selection** | Different platforms need different approaches | "How do you choose JWT storage method?" | "Web apps use session storage for per-user isolation. Mobile/IoT use static storage for simplicity. Consider user concurrency and platform capabilities." |
| **Thread Safety** | Static storage requires manual synchronization | "What are JWT storage thread safety concerns?" | "Static storage needs locks to prevent race conditions. Session storage provides automatic per-user isolation. Consider performance impact of locks in high-traffic scenarios." |
| **Multi-User vs Single-User** | Different concurrency patterns | "Why avoid static storage in web applications?" | "Web apps serve multiple users simultaneously. Static storage creates bottleneck with locks. Session storage provides per-user token isolation without contention." |
| **Platform Limitations** | Not all platforms support all storage types | "What storage options exist for mobile apps?" | "Mobile: static variables, singleton services, secure storage APIs. No session/cookie support. Must handle token persistence manually." |
| **Performance Considerations** | Lock contention in high-traffic scenarios | "What are performance implications of static JWT storage?" | "Locks can create bottlenecks with many concurrent users. Fine for single-user apps, problematic for multi-user web applications." |
| **Singleton Lifetime** | Dependency injection lifetime management | "How does singleton lifetime affect JWT storage?" | "Singleton services live for application lifetime, suitable for caching tokens. Provides clean interface over static variables with same performance characteristics." |
| **Token Expiration** | Consistent expiration handling across storage types | "How do you handle token expiration in static storage?" | "Check expiration in getter methods, clear expired tokens, implement automatic refresh logic. Same concepts as session storage but manual implementation." |
| **Security Considerations** | Platform-specific security features | "What are security considerations for JWT storage?" | "Web: httpOnly session cookies. Mobile: secure storage APIs. IoT: encrypted local storage. Avoid logging tokens, clear on app termination." |
| **Testing Strategy** | Different testing approaches per platform | "How do you test static JWT storage?" | "Unit tests with controlled token states, thread safety tests with concurrent access, integration tests with real authentication flows." |
| **Cleanup Strategies** | Memory management for long-running applications | "How do you prevent memory leaks in token storage?" | "Implement token cleanup on expiration, clear tokens on logout/app termination, consider weak references for advanced scenarios." |

## Key Takeaways

1. **Platform-Appropriate Storage**: Choose storage method based on platform capabilities and user concurrency patterns
2. **Session for Web Apps**: Multi-user web applications benefit from session storage's automatic per-user isolation
3. **Static for Single-User**: Mobile apps, desktop apps, and IoT devices can use simpler static storage approaches
4. **Thread Safety Required**: Static storage requires manual synchronization with locks or concurrent collections
5. **Performance Trade-offs**: Lock contention can create bottlenecks in high-traffic scenarios
6. **Singleton Alternative**: Dependency injection with singleton lifetime provides cleaner interface than static variables
7. **Consistent Token Logic**: Expiration checking, refresh logic remains the same regardless of storage method
8. **Security Considerations**: Each platform has specific security features for token storage
9. **Testing Implications**: Different storage methods require different testing strategies
10. **Cleanup Responsibility**: Manual storage approaches require explicit cleanup to prevent memory leaks

This lesson highlights that while session storage works well for web applications, other client types require different approaches, with static repositories or singleton services providing simpler alternatives for single-user scenarios.


# Interpreting JSON Web Token in Web API - Summary

## Overview

This tutorial demonstrates how to configure a Web API to interpret and validate JWT tokens sent by clients. The process involves adding JWT authentication middleware, configuring token validation parameters, and protecting endpoints with authorization attributes.

## JWT Interpretation Flow

```
JWT Validation Process in Web API:

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ HTTP Request    │───▶│ Authentication  │───▶│ Token           │
│ Authorization:  │    │ Middleware      │    │ Validation      │
│ Bearer <token>  │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                                │                        │
                                ▼                        ▼
                       ┌─────────────────┐    ┌─────────────────┐
                       │ Extract JWT     │    │ Validate        │
                       │ from Header     │    │ Signature       │
                       └─────────────────┘    └─────────────────┘
                                │                        │
                                ▼                        ▼
                       ┌─────────────────┐    ┌─────────────────┐
                       │ Parse Claims    │───▶│ Create          │
                       │ from Payload    │    │ ClaimsPrincipal │
                       └─────────────────┘    └─────────────────┘
                                                       │
                                                       ▼
                                              ┌─────────────────┐
                                              │ Authorization   │
                                              │ Decisions       │
                                              └─────────────────┘
```


## Implementation Steps

### 1. Install Required NuGet Package

```xml
<!-- Web API Project -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```
- ![alt text](image-2.png)


### 2. Configure JWT Authentication in Program.cs

```csharp
// Program.cs (Web API)
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    // Set default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Audience validation (disabled for demo)
        ValidateAudience = false,
        
        // Issuer validation (disabled for demo)
        ValidateIssuer = false,
        
        // Token lifetime validation (enabled)
        ValidateLifetime = true,
        
        // Remove default 5-minute clock skew for precise expiration
        ClockSkew = TimeSpan.Zero,
        
        // Signature validation (REQUIRED for security)
        ValidateIssuerSigningKey = true,
        
        // Signing key must match the one used to create tokens
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["ClientAuthentication:SecurityKey"] ?? string.Empty
            )
        )
    };
});

// Add Authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CRITICAL: Add authentication and authorization middleware
app.UseAuthentication();    // Must come before UseAuthorization
app.UseAuthorization();     // Must come after UseAuthentication

// Map endpoints
app.MapAccountEndpoints();
app.MapEmployeeEndpoints();
app.MapDepartmentEndpoints();

app.Run();
```


### 3. Protect API Endpoints with Authorization

```csharp
// Endpoints/EmployeeEndpoints.cs
public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        // Method 1: Using Authorize attribute (Recommended)
        app.MapGet("/api/employees", async (IEmployeesRepository repo) =>
        {
            var employees = await repo.GetEmployeesAsync();
            return TypedResults.Ok(employees);
        })
        .RequireAuthorization()  // Alternative method
        .WithTags("Employees");

        // Method 2: Using Authorize attribute (Preferred for consistency)
        app.MapGet("/api/employees/{id}", async (int id, IEmployeesRepository repo) =>
        {
            var employee = await repo.GetEmployeeAsync(id);
            return employee != null ? TypedResults.Ok(employee) : TypedResults.NotFound();
        })
        .RequireAuthorization()  // Requires valid JWT token
        .WithTags("Employees");

        app.MapPost("/api/employees", async (Employee employee, IEmployeesRepository repo) =>
        {
            var created = await repo.AddEmployeeAsync(employee);
            return TypedResults.Created($"/api/employees/{created?.Id}", created);
        })
        .RequireAuthorization()
        .WithTags("Employees");

        app.MapPut("/api/employees/{id}", async (int id, Employee employee, IEmployeesRepository repo) =>
        {
            employee.Id = id;
            var updated = await repo.UpdateEmployeeAsync(employee);
            return updated != null ? TypedResults.Ok(updated) : TypedResults.NotFound();
        })
        .RequireAuthorization()
        .WithTags("Employees");

        app.MapDelete("/api/employees/{id}", async (int id, IEmployeesRepository repo) =>
        {
            var deleted = await repo.DeleteEmployeeAsync(id);
            return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .RequireAuthorization()
        .WithTags("Employees");
    }
}
```


### 4. Alternative: Controller-based Authorization

```csharp
// Controllers/EmployeesController.cs (if using controllers)
[ApiController]
[Route("api/[controller]")]
[Authorize] // Protects entire controller
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesRepository _repository;

    public EmployeesController(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetEmployees()
    {
        var employees = await _repository.GetEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _repository.GetEmployeeAsync(id);
        return employee != null ? Ok(employee) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // More specific authorization
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
        var created = await _repository.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployee), new { id = created.Id }, created);
    }
}
```


## Token Validation Parameters Explanation

```csharp
options.TokenValidationParameters = new TokenValidationParameters
{
    // Validate token signature using secret key
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    
    // Validate token hasn't expired
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero, // No tolerance for expired tokens
    
    // Skip audience validation (for demo - enable in production)
    ValidateAudience = false,
    // ValidAudience = "your-api-audience", // Enable for production
    
    // Skip issuer validation (for demo - enable in production) 
    ValidateIssuer = false,
    // ValidIssuer = "your-api-issuer", // Enable for production
    
    // Additional security settings
    RequireExpirationTime = true,     // Require exp claim
    RequireSignedTokens = true,       // Require signature
    SaveSigninToken = false           // Don't save raw token
};
```


## Testing Authorization

### 1. Test Without JWT Token (Should Fail)

```csharp
// In JwtAuthenticationHandler - comment out token attachment
protected override async Task<HttpResponseMessage> SendAsync(
    HttpRequestMessage request, 
    CancellationToken cancellationToken)
{
    // Comment out this line to test unauthorized access
    // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
    
    return await base.SendAsync(request, cancellationToken);
}

// Expected Result: 401 Unauthorized
// Error: "Response status code does not indicate success: 401 (Unauthorized)"
```


### 2. Test With Valid JWT Token (Should Succeed)

```csharp
// Uncomment token attachment in handler
request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

// Expected Result: 200 OK with API data
// Employees page loads successfully
```


### 3. HTTP Request Examples

```http
# Without Authorization Header (401 Unauthorized)
GET /api/employees HTTP/1.1
Host: localhost:7001
Content-Type: application/json

# With Valid JWT Token (200 OK)
GET /api/employees HTTP/1.1  
Host: localhost:7001
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```


## Authorization Middleware Pipeline

```
Request Processing Pipeline:

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ HTTP Request    │───▶│ Routing         │───▶│ Authentication  │
│                 │    │ Middleware      │    │ Middleware      │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                                                       │
                                                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Endpoint        │◀───│ Authorization   │◀───│ Token           │
│ Execution       │    │ Middleware      │    │ Validation      │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │
         ▼                       ▼
┌─────────────────┐    ┌─────────────────┐
│ Return Response │    │ 401 Unauthorized│
│ (200 OK)        │    │ (if failed)     │
└─────────────────┘    └─────────────────┘
```


## Configuration Consistency

```csharp
// Token Creation (Account Endpoint)
var signingCredentials = new SigningCredentials(
    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    SecurityAlgorithms.HmacSha256Signature  // Algorithm used to sign
);

// Token Validation (Program.cs)
IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(secretKey)       // MUST be same secret key
),
```


## Error Scenarios and Solutions

### Common Issues

```csharp
// Issue 1: Secret Key Mismatch
// Token Creation: Uses "secret1"
// Token Validation: Uses "secret2"  
// Result: 401 Unauthorized - Invalid signature

// Issue 2: Missing Authentication Middleware
// Missing: app.UseAuthentication();
// Result: Authorization always fails

// Issue 3: Wrong Middleware Order
// Wrong: app.UseAuthorization(); app.UseAuthentication();
// Correct: app.UseAuthentication(); app.UseAuthorization();

// Issue 4: Clock Skew Issues  
// Problem: Default 5-minute tolerance
// Solution: ClockSkew = TimeSpan.Zero
```


## Interview Tips Table

| **Topic** | **Key Points** | **Sample Questions** | **Best Answers** |
| :-- | :-- | :-- | :-- |
| **JWT Validation Process** | Multi-step validation of signature, claims, expiration | "How does Web API validate JWT tokens?" | "Extract from Authorization header, validate signature with secret key, check expiration, parse claims, create ClaimsPrincipal for authorization decisions." |
| **Middleware Order** | Authentication must precede authorization | "What's the correct order for auth middleware?" | "UseAuthentication() before UseAuthorization(). Authentication populates user identity, authorization uses that identity for access decisions." |
| **Token Validation Parameters** | Configure security requirements | "What JWT validation settings are important?" | "ValidateIssuerSigningKey (required), ValidateLifetime (expiration), ValidateIssuer/Audience (production), ClockSkew (time tolerance)." |
| **Authorization Attributes** | Protect endpoints with [Authorize] | "How do you protect API endpoints?" | "Use RequireAuthorization() for minimal APIs or [Authorize] for controllers. Can specify policies, roles, or just require authentication." |
| **Secret Key Consistency** | Same key for creation and validation | "Why might JWT validation fail?" | "Secret key mismatch between token creation and validation, wrong middleware order, expired tokens, missing authentication middleware." |
| **Bearer Token Format** | Authorization header standard | "How are JWT tokens transmitted?" | "Authorization header with 'Bearer ' prefix followed by JWT token. Client must include with every protected API request." |
| **Claims Extraction** | Automatic claims parsing | "How do you access user claims in API?" | "JWT middleware automatically parses claims into HttpContext.User. Access via User.Claims, User.FindFirst(), or User.IsInRole()." |
| **Error Handling** | 401 vs 403 status codes | "What HTTP status codes indicate auth issues?" | "401 Unauthorized: missing/invalid token. 403 Forbidden: valid token but insufficient permissions for resource." |
| **Production Considerations** | Issuer and audience validation | "What should be configured for production?" | "Enable ValidateIssuer and ValidateAudience, use strong secret keys, implement proper key rotation, consider certificate-based signing." |
| **Testing Strategy** | Verify auth with/without tokens | "How do you test JWT authentication?" | "Test without token (expect 401), test with invalid token (expect 401), test with valid token (expect success), test expired tokens." |

## Key Takeaways

1. **JWT Bearer Package Required**: Install `Microsoft.AspNetCore.Authentication.JwtBearer` NuGet package
2. **Middleware Order Critical**: UseAuthentication() must come before UseAuthorization()
3. **Secret Key Consistency**: Same secret key must be used for token creation and validation
4. **Token Validation Parameters**: Configure security requirements like signature validation and expiration checking
5. **Authorization Protection**: Use RequireAuthorization() or [Authorize] attributes to protect endpoints
6. **Bearer Token Format**: Clients send JWT in Authorization header with "Bearer " prefix
7. **Automatic Claims Processing**: Middleware automatically parses JWT claims into HttpContext.User
8. **Error Codes**: 401 for authentication failures, 403 for authorization failures
9. **Production Security**: Enable issuer and audience validation for public APIs
10. **Testing Approach**: Verify authorization works with and without valid tokens

This JWT interpretation setup enables secure Web API endpoints that can validate tokens from various client types while maintaining consistency with ASP.NET Core's authentication and authorization patterns.


# Authorization Based on JSON Web Token - Summary

## Overview

This lesson demonstrates how authorization with JWT tokens works identically to cookie-based authorization. Both systems rely on claims within authentication tickets, making the authorization logic and policies exactly the same between Web Applications (cookie-based) and Web APIs (JWT-based).

## Authorization Equivalence

### Cookie-based vs JWT-based Authorization

```
Cookie Authentication          JWT Authentication
┌─────────────────┐           ┌─────────────────┐
│   Cookie        │           │   JWT Token     │
│   ├─Claims      │           │   ├─Header      │
│   │  ├─Name     │    ≡≡≡    │   ├─Payload     │
│   │  └─Role     │           │   │  ├─Claims   │
│   └─Encryption  │           │   │  │ ├─Name   │
└─────────────────┘           │   │  │ └─Role   │
                              │   │  └─exp      │
                              │   └─Signature   │
                              └─────────────────┘

Same Authorization Logic
Same Policy Configuration  
Same Claims Processing
```


## Implementation Code

### 1. Authorization Policies Configuration (Web API)

```csharp
// Program.cs (Web API) - Same as Web Application
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["ClientAuthentication:SecurityKey"] ?? string.Empty)
        ),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

// Configure Authorization - IDENTICAL to Web Application
builder.Services.AddAuthorization(options =>
{
    // Same policy configuration as web application
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });
    
    // Additional policies can be added
    options.AddPolicy("AdminOrManager", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Admin", "Manager");
    });
    
    options.AddPolicy("UserOnly", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "User");
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapAccountEndpoints();
app.MapEmployeeEndpoints();
app.MapDepartmentEndpoints();

app.Run();
```


### 2. Client Configuration for Different Roles

```csharp
// Endpoints/AccountEndpoint.cs - Different clients, different roles
public static void MapAccountEndpoints(this WebApplication app)
{
    app.MapPost("/account/login", async (
        CredentialViewModel credential, 
        IConfiguration configuration) =>
    {
        if ((credential.ClientId == "web-app-001" || credential.ClientId == "web-app-002") && 
            credential.ClientSecret == "your-generated-secret-key-here")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, credential.ClientId)
            };

            // Role assignment based on client ID
            if (credential.ClientId == "web-app-001")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin")); // Admin role
            }
            else // web-app-002 or others
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));  // User role
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(20);
            var accessToken = CreateToken(claims, expiresAt, configuration);

            return TypedResults.Ok(new
            {
                access_token = accessToken,
                expires_at = expiresAt
            });
        }

        return TypedResults.ValidationProblem(new Dictionary<string, string[]>
        {
            ["ClientId"] = ["Errors in Client ID or Client Secret"],
            ["ClientSecret"] = ["Errors in Client ID or Client Secret"]
        }, statusCode: 401);
    })
    .WithParameterValidation()
    .WithTags("Account");
}
```


### 3. Protected Endpoints with Policies

```csharp
// Endpoints/EmployeeEndpoints.cs
public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        // Basic authentication required (any valid JWT)
        app.MapGet("/api/employees", async (IEmployeesRepository repo) =>
        {
            var employees = await repo.GetEmployeesAsync();
            return TypedResults.Ok(employees);
        })
        .RequireAuthorization() // Basic auth - any valid token
        .WithTags("Employees");

        // Admin-only endpoint
        app.MapGet("/api/employees/{id}", async (int id, IEmployeesRepository repo) =>
        {
            var employee = await repo.GetEmployeeAsync(id);
            return employee != null ? TypedResults.Ok(employee) : TypedResults.NotFound();
        })
        .RequireAuthorization("AdminOnly") // Policy-based authorization
        .WithTags("Employees");

        // Admin-only create
        app.MapPost("/api/employees", async (Employee employee, IEmployeesRepository repo) =>
        {
            var created = await repo.AddEmployeeAsync(employee);
            return TypedResults.Created($"/api/employees/{created?.Id}", created);
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Employees");

        // Admin-only update
        app.MapPut("/api/employees/{id}", async (int id, Employee employee, IEmployeesRepository repo) =>
        {
            employee.Id = id;
            var updated = await repo.UpdateEmployeeAsync(employee);
            return updated != null ? TypedResults.Ok(updated) : TypedResults.NotFound();
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Employees");

        // Admin-only delete
        app.MapDelete("/api/employees/{id}", async (int id, IEmployeesRepository repo) =>
        {
            var deleted = await repo.DeleteEmployeeAsync(id);
            return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Employees");
    }
}
```


### 4. Web Application Client Configuration

```json
// appsettings.json (Web Application)
{
  "WebApi": {
    "Url": "https://localhost:7001/",
    "ClientId": "web-app-001",  // Admin client
    "ClientSecret": "your-generated-secret-key-here"
  }
}

// To test User role, change to:
{
  "WebApi": {
    "Url": "https://localhost:7001/",
    "ClientId": "web-app-002",  // User client  
    "ClientSecret": "your-generated-secret-key-here"
  }
}
```


## Authorization Testing Scenarios

### Scenario 1: Admin Client (web-app-001)

```http
# Login Request
POST /account/login HTTP/1.1
Content-Type: application/json

{
  "clientId": "web-app-001",
  "clientSecret": "your-generated-secret-key-here"
}

# JWT Payload Contains:
{
  "name": "web-app-001",
  "role": "Admin",
  "exp": 1693467600
}

# API Access Results:
GET /api/employees     ✅ 200 OK (basic auth)
GET /api/employees/1   ✅ 200 OK (AdminOnly policy)
POST /api/employees    ✅ 201 Created (AdminOnly policy)
PUT /api/employees/1   ✅ 200 OK (AdminOnly policy)
DELETE /api/employees/1 ✅ 204 No Content (AdminOnly policy)
```


### Scenario 2: User Client (web-app-002)

```http
# Login Request
POST /account/login HTTP/1.1
Content-Type: application/json

{
  "clientId": "web-app-002", 
  "clientSecret": "your-generated-secret-key-here"
}

# JWT Payload Contains:
{
  "name": "web-app-002",
  "role": "User",
  "exp": 1693467600
}

# API Access Results:
GET /api/employees     ✅ 200 OK (basic auth)
GET /api/employees/1   ❌ 403 Forbidden (AdminOnly policy)
POST /api/employees    ❌ 403 Forbidden (AdminOnly policy)  
PUT /api/employees/1   ❌ 403 Forbidden (AdminOnly policy)
DELETE /api/employees/1 ❌ 403 Forbidden (AdminOnly policy)
```


## HTTP Status Code Differentiation

### Authentication vs Authorization Failures

```csharp
// Authentication Failure (No token or invalid token)
// Status Code: 401 Unauthorized
// Reason: Missing or invalid JWT token
// Example: No Authorization header or expired token

// Authorization Failure (Valid token, insufficient permissions)  
// Status Code: 403 Forbidden
// Reason: Valid JWT token but role doesn't meet policy requirements
// Example: User role trying to access AdminOnly endpoint
```


## Error Response Examples

### 401 Unauthorized (Authentication Failure)

```json
// When JWT token is missing or invalid
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Authorization header is missing or invalid"
}
```


### 403 Forbidden (Authorization Failure)

```json
// When JWT token is valid but user lacks required role/claims
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.3",
  "title": "Forbidden", 
  "status": 403,
  "detail": "User does not have permission to access this resource"
}
```


## Key Differences: Web App vs Web API Authorization

| Aspect | Web Application (Cookie) | Web API (JWT) |
| :-- | :-- | :-- |
| **Authentication Ticket** | SignInAsync() creates encrypted cookie | JwtSecurityTokenHandler creates JWT |
| **Storage** | Browser cookie storage | Client application storage |
| **Transmission** | Automatic with HTTP requests | Manual Authorization header |
| **Claims Source** | Server-side session/database | JWT payload (client-side) |
| **Expiration** | Server-controlled | Token-embedded (exp claim) |
| **Policy Logic** | Identical | Identical |
| **Authorization Middleware** | Same ASP.NET Core middleware | Same ASP.NET Core middleware |
| **Status Codes** | Same (401/403) | Same (401/403) |

## Important Security Notes

### Client vs User Authentication

```csharp
// Important Distinction:
// Web Application User Login: admin@company.com (UI authentication)
// Web API Client Login: web-app-001 (application authentication)

// The JWT contains CLIENT identity, not USER identity
// JWT Role: Based on client application (web-app-001 = Admin)
// Not based on web app user (admin@company.com)

// Web app user login is separate from API client authentication
// User logs into web app UI, web app authenticates to API as client
```


## Interview Tips Table

| **Topic** | **Key Points** | **Sample Questions** | **Best Answers** |
| :-- | :-- | :-- | :-- |
| **Authorization Equivalence** | Same logic for cookies and JWT | "How does JWT authorization differ from cookie auth?" | "Authorization logic is identical - both use claims in authentication tickets. Only difference is ticket format (cookie vs JWT) and transmission method." |
| **Policy Configuration** | Identical policy setup | "How do you configure JWT authorization policies?" | "Exactly same as cookie auth - AddAuthorization() with policies using RequireClaim(), RequireRole(), etc. Same policy names and requirements." |
| **Status Code Meanings** | 401 vs 403 distinction | "What's the difference between 401 and 403 errors?" | "401 Unauthorized: missing/invalid authentication token. 403 Forbidden: valid token but insufficient permissions for resource." |
| **Client vs User Identity** | JWT contains client identity, not user | "What identity does JWT contain in this architecture?" | "JWT contains client application identity (web-app-001), not end-user identity (admin@company.com). Web app authenticates to API as a client." |
| **Role Assignment** | Based on client ID in this example | "How are roles assigned to JWT tokens?" | "In this example, roles assigned based on client ID during token creation. Production would use database-driven role assignment." |
| **Testing Strategies** | Change client configuration to test roles | "How do you test different authorization levels?" | "Change client ID in configuration (web-app-001 vs web-app-002), restart application, verify different endpoints allow/deny access based on role." |
| **Middleware Pipeline** | Same authentication/authorization flow | "Does JWT change the middleware pipeline?" | "No - same UseAuthentication() and UseAuthorization() pipeline. JWT middleware populates same HttpContext.User with same claims structure." |
| **Policy Reusability** | Policies work across auth types | "Can you reuse authorization policies?" | "Yes - policies are authentication-agnostic. Same policy works with cookies, JWT, or any auth scheme that populates claims correctly." |
| **Configuration Consistency** | Same secret key for creation/validation | "What configuration must match between services?" | "Secret key for JWT signing/validation must match. Policy names and claim requirements must match across services using same authorization rules." |
| **Error Debugging** | Status codes indicate failure type | "How do you debug authorization failures?" | "401 indicates auth problem (check token validity), 403 indicates authz problem (check user claims vs policy requirements). Inspect JWT payload for claims." |

## Key Takeaways

1. **Authorization Logic Identity**: JWT and cookie-based authorization use identical logic and policies
2. **Same Middleware Pipeline**: UseAuthentication() and UseAuthorization() work the same way regardless of token type
3. **Claims-Based Consistency**: Both authentication methods populate HttpContext.User with claims for authorization decisions
4. **Policy Reusability**: Authorization policies configured once work with any authentication scheme
5. **Status Code Distinction**: 401 for authentication failures, 403 for authorization failures
6. **Client vs User Identity**: JWT contains client application identity, not end-user identity in this architecture
7. **Configuration Consistency**: Secret keys and policy names must match between token creation and validation
8. **Testing Approach**: Change client configuration to test different authorization levels
9. **Error Debugging**: Status codes and response bodies provide clear indication of failure type
10. **Production Considerations**: Real applications would use database-driven role assignment rather than hardcoded client-role mapping

The key insight is that authorization with JWT tokens is functionally identical to cookie-based authorization - the only differences are in how the authentication ticket is created, stored, and transmitted. The authorization logic, policies, and middleware remain exactly the same.
