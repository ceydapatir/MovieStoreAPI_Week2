using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieStoreAPI.Base.Encryption;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Base.Token;
using MovieStoreAPI.Data.Context;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.ViewModel;
using MovieStoreAPI.Operations.Cqrs;
namespace Vk.Operation.Query;
public class TokenCommandHandler:
    IRequestHandler<CreateTokenCommand, ApiResponse<TokenViewModel>>
{
    private readonly MovieStoreDBContext dbContext;
    private readonly JwtConfig jwtConfig;

    public TokenCommandHandler(MovieStoreDBContext dbContext, Microsoft.Extensions.Options.IOptionsMonitor<JwtConfig> jwtConfig)
    {
        this.dbContext = dbContext;
        this.jwtConfig = jwtConfig.CurrentValue;
    }
    

    public async Task<ApiResponse<TokenViewModel>> Handle(CreateTokenCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Customer>().AsQueryable<Customer>().FirstOrDefaultAsync(x => x.Username == request.Model.UserName, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse<TokenViewModel>("Invalid user informations");
        }

        var md5 = Md5.Create(request.Model.Password.ToLower());
        if (entity.Password != md5)
        {
            return new ApiResponse<TokenViewModel>("Invalid user informations");
        }

        if (!entity.IsActive)
        {
            return new ApiResponse<TokenViewModel>("Invalid user!");
        }

        string token = Token(entity);
        TokenViewModel tokenResponse = new()
        {
            Token = token,
            UserName = entity.Username
        };
        
        return new ApiResponse<TokenViewModel>(tokenResponse);
    }
    
    private string Token(Customer user)
    {
        Claim[] claims = GetClaims(user);
        var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

        var jwtToken = new JwtSecurityToken(
            jwtConfig.Issuer,
            jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        );

        string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return accessToken;
    }


    private Claim[] GetClaims(Customer customer)
    {
        var claims = new[]
        {
            new Claim("Id", customer.Id.ToString()),
            new Claim("Username", customer.Username),
            new Claim("Role", customer.Role),
            new Claim(ClaimTypes.Role, customer.Role),
            new Claim("FullName", $"{customer.Name} {customer.Surname}")
        };

        return claims;
    }
}