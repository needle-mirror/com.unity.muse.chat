using System;

namespace Unity.Muse.Chat
{
    class WebAPIException : Exception
    {
        public object APIData { get; set; }

        public WebAPIException(string message, object data) : base(message)
        {
            APIData = data;
        }
    }
}
