﻿using System.Collections.Generic;

namespace BookManagementAPI.DTO
{
    public class PageInfoDTO
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public string PageQuery { get; set; }
    }
}
