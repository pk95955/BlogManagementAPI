﻿namespace BlogManagementAPI.CustomException
{

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

}
