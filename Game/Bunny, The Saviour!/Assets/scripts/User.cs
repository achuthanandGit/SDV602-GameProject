using System;

namespace Assets.scripts
{
    [Serializable]
    public class User
    {
        // The username
        private string Username;

        // The Password
        private string Password;

        // The Email
        private string Email;

        // The status
        private string Status;

        /// <summary>Gets or sets the Username.</summary>
        /// <value>The user.</value>
        public string Usser { get => Username; set => Username = value; }

        /// <summary>Gets or sets the Password.</summary>
        /// <value>The password.</value>
        public string Pwd { get => Password; set => Password = value; }

        /// <summary>Gets or sets the Email.</summary>
        /// <value>The mail.</value>
        public string Mail { get => Email; set => Email = value; }

        /// <summary>Gets or sets the user status.</summary>
        /// <value>The user status.</value>
        public string UserStatus { get => Status; set => Status = value; }
    }
}
