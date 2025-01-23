using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssAggregator.Application.UseCases.Identity.CreateUser;

public interface ICreateUserUseCase
{
    Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken ct = default); 
}