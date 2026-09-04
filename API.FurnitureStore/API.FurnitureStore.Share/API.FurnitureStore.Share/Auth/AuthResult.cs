using System;
using System.Collections.Generic;
using System.Text;

namespace API.FurnitureStore.Share.Auth
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<String> Errors { get; set; }
    }
}
