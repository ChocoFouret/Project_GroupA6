﻿using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseUpdateAccount : IUseCaseWriter<Boolean, DtoInputUpdateAccount>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseUpdateAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Call the method into EfAccountRepesitory
    public Boolean Execute(DtoInputUpdateAccount input)
    {
        var account = _accountRepository.FetchById(input.IdAccount);

        account.FirstName = input.FirstName;
        account.LastName = input.LastName;

        account.Email = input.Email;
        // account.Password = EncryptPassword.HashPassword(input.Password);
        
        account.IsAdmin = input.IsAdmin;

        return _accountRepository.Update(account);
    }
}