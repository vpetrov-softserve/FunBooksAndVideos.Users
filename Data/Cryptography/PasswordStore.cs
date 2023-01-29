using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunBooksAndVideos.Users.Data.Cryptography
{
    public class PasswordStore
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt {get; set; }
    }
}