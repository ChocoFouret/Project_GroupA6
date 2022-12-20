using Application.UseCases.Accounts;
using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Has.Dtos;
using Domain;
using JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Service.UseCases.Companies;
using Service.UseCases.Has.Dtos;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UseCaseLoginAccount _useCaseLoginAccount;
    private readonly UseCaseCreateAccount _useCaseCreateAccount;
    private readonly UseCaseUpdateAccount _useCaseUpdateAccount;
    private readonly UseCaseUpdatePasswordAccount _useCaseUpdatePasswordAccount;
    private readonly UseCaseDeleteAccount _useCaseDeleteAccount;
    private readonly UseCaseFetchAllAccounts _useCaseFetchAllAccounts;
    private readonly UseCaseFetchAccountById _useCaseFetchAccountById;
    private readonly UseCaseFetchAccountByEmail _useCaseFetchAccountByEmail;
    private readonly UseCaseFetchHasByAccount _useCaseFetchHasByAccount;
    private readonly UseCaseFetchFunctionById _useCaseFetchFunctionById;
    private readonly UseCaseFetchProfilById _useCaseFetchProfilById;
    
    private readonly ISessionService _sessionService;
    private readonly IConfiguration _config;
    
    public AccountController(
        UseCaseLoginAccount useCaseLoginAccount,
        UseCaseCreateAccount useCaseCreateAccount,
        UseCaseUpdateAccount useCaseUpdateAccount,
        UseCaseUpdatePasswordAccount useCaseUpdatePasswordAccount,
        UseCaseDeleteAccount useCaseDeleteAccount,
        UseCaseFetchAllAccounts useCaseFetchAllAccounts,
        UseCaseFetchAccountById useCaseFetchAccountById,
        UseCaseFetchAccountByEmail useCaseFetchAccountByEmail,
        ISessionService sessionService,
        IConfiguration configuration, 
        UseCaseFetchHasByAccount useCaseFetchHasByAccount,
        UseCaseFetchFunctionById useCaseFetchFunctionById,
        UseCaseFetchProfilById useCaseFetchProfilById
    )
    {
        _useCaseLoginAccount = useCaseLoginAccount;
        _useCaseCreateAccount = useCaseCreateAccount;
        _useCaseUpdateAccount = useCaseUpdateAccount;
        _useCaseUpdatePasswordAccount = useCaseUpdatePasswordAccount;
        _useCaseDeleteAccount = useCaseDeleteAccount;
        _useCaseFetchAllAccounts = useCaseFetchAllAccounts;
        _useCaseFetchAccountById = useCaseFetchAccountById;
        _useCaseFetchAccountByEmail = useCaseFetchAccountByEmail;
        _useCaseFetchHasByAccount = useCaseFetchHasByAccount;
        _useCaseFetchFunctionById = useCaseFetchFunctionById;
        _useCaseFetchProfilById = useCaseFetchProfilById;
        
        _sessionService = sessionService;
        _config = configuration;
    }

    /// <summary>
    /// It returns a list of DtoOutputAccount objects, which are the result of the Execute() function of the
    /// _useCaseFetchAllAccounts object
    /// </summary>
    /// <returns>
    /// A list of DtoOutputAccount objects.
    /// </returns>
    [HttpGet]
    [Route("fetch/")]
    public IEnumerable<DtoOutputAccount> FetchAll()
    {
        return _useCaseFetchAllAccounts.Execute();
    }

    /// <summary>
    /// It returns a DtoOutputAccount object if the id is found, otherwise it returns a 404 Not Found error
    /// </summary>
    /// <param name="id">int - This is the route parameter. It's a required parameter.</param>
    /// <returns>
    /// ActionResult<DtoOutputAccount>
    /// </returns>
    [HttpGet]
    [Route("fetch/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputAccount> FetchById(int id)
    {
        try
        {
            return _useCaseFetchAccountById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    [Route("fetch/profil/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputProfilAccount> FetchProfilById(int id)
    {
        try
        {
            return _useCaseFetchProfilById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// It takes an email address as a parameter, and returns a DTO object containing the account details
    /// </summary>
    /// <param name="email">The email of the account to fetch.</param>
    /// <returns>
    /// The action result is returning a DtoOutputAccount object.
    /// </returns>
    [HttpGet]
    [Route("fetch/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Account> FetchByEmail(string email)
    {
        try
        {
            return _useCaseFetchAccountByEmail.Execute(email);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// The function is called when a POST request is made to the /create route. It takes a DtoInputCreateAccount object as
    /// a parameter, and returns a DtoOutputAccount object
    /// </summary>
    /// <param name="DtoInputCreateAccount">The input data transfer object (DTO) that contains the data that the user will
    /// send to the API.</param>
    /// <returns>
    /// The action result of the create method is being returned.
    /// </returns>
    // [Authorize(Policy = "all")]
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputAccount> Create(DtoInputCreateAccount dto)
    {
        // Use for add new account easily
        //dto.account.Function = "Employee";
        dto.account.IsAdmin = false;
        var output = _useCaseCreateAccount.Execute(dto);

        if (output == null) return Conflict(new Account());

        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdAccount },
            output
        );
    }

    /// <summary>
    /// The function takes in a DTO (Data Transfer Object) and returns a boolean
    /// </summary>
    /// <param name="id">The id of the account to be deleted</param>
    /// <returns>
    /// The return type is ActionResult<Boolean>
    /// </returns>
    [HttpDelete]
    [Route("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteAccount.Execute(id);
    }

    /// <summary>
    /// The function takes a DTO as input, calls the use case, and returns the result of the use case
    /// </summary>
    /// <param name="DtoInputUpdateAccount">This is the input parameter for the use case.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    [HttpPut]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateAccount dto)
    {
        return _useCaseUpdateAccount.Execute(dto);
    }
    
    [HttpPut]
    [Route("update/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdatePassword(DtoInputUpdatePasswordAccount dto)
    {
        return Ok(new {Password = _useCaseUpdatePasswordAccount.Execute(dto)});
    }
    
    /// <summary>
    /// It takes a DTO as input, checks if the user exists, and if so, it generates a JWT token and returns it as a cookie
    /// </summary>
    /// <param name="DtoInputLoginAccount">This is the input data transfer object that will be used to pass the data to the
    /// use case.</param>
    /// <returns>
    /// A cookie with the name "session" and the value of the generated token.
    /// </returns>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login(DtoInputLoginAccount dto)
    {
        if (_useCaseLoginAccount.Execute(dto))
        {
            Account account = _useCaseFetchAccountByEmail.Execute(dto.Email);
            IEnumerable<DtoOutputHas> has = _useCaseFetchHasByAccount.Execute(account.IdAccount);
            bool isHas = has.ToList().Count != 0;

            int idCompanie = -1;
            string functionName = "";
            if (isHas)
            {
                idCompanie = has.ToList().FirstOrDefault().IdCompanies;
                
                DtoOutputFunction function = _useCaseFetchFunctionById.Execute(has.FirstOrDefault().IdFunctions);
                if (function != null)
                {
                    functionName = function.Title;   
                }
                
            }
            
            var generatedToken =
                _sessionService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), account, functionName);
            
            var cookie = new CookieOptions()
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("session", generatedToken, cookie);

            var generatedTokenPublic =
                _sessionService.BuildTokenPublic(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), account, idCompanie, functionName);
            var cookiePublic = new CookieOptions()
            {
                Secure = true,
                HttpOnly = false,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("public", generatedTokenPublic, cookiePublic);
            return Ok(new {});
        }
        return Unauthorized();
    }
    
    [HttpPost]
    [Route("login/phone")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAccountPhone> LoginByPhone(DtoInputLoginAccount dto)
    {
        if (_useCaseLoginAccount.Execute(dto))
        {
            var account = _useCaseFetchAccountByEmail.Execute(dto.Email);
            IEnumerable<DtoOutputHas> has = _useCaseFetchHasByAccount.Execute(account.IdAccount);
            bool isHas = has.ToList().Count != 0;

            int idCompanie = -1;
            string functionName = "";
            if (isHas)
            {
                idCompanie = has.ToList().FirstOrDefault().IdCompanies;
                
                DtoOutputFunction function = _useCaseFetchFunctionById.Execute(has.FirstOrDefault().IdFunctions);
                if (function != null)
                {
                    functionName = function.Title;   
                }
                
            }
            
            var generatedToken =
                _sessionService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), account, functionName);
            var generatedTokenPublic =
                _sessionService.BuildTokenPublic(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(),
                    account, idCompanie, functionName);
            var dtoOutputAccount = new DtoOutputAccountPhone
            {
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                IdAccount = account.IdAccount,
                Token = generatedTokenPublic,
                TokenPrivate = generatedToken
            };
            
            return dtoOutputAccount;
        }
        return null;
    }
}