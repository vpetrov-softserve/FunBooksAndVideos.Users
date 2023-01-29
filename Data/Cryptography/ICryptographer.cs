using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunBooksAndVideos.Users.Data.Cryptography
{
    public interface ICryptographer
    {
        public PasswordStore  CreatePasswordHash(string password);

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
       
    }
}