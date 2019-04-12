using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.ViewModels
{
    public class LoginResponse
    {
        bool IsSuccess;
        List<Error> Errors;
        string Role;
        string EmailAddress;
    }
}
