﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Google
{
    public class GoogleRefreshTokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }


}
}
