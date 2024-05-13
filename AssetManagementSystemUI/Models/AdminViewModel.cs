namespace AssetManagementSystemUI
{
    public class AdminViewModel : Login
    {
        public AdminViewModel() 
        {
            Email = "staff@intimetec.com";
            Password = "password";
        }

        public bool Login(string email, string password)
        {
            if(String.Equals(email, "staff@intimetec.com") && string.Equals(password, "password"))
            {
                return true;
            }

            return false;
        }
    }
}
