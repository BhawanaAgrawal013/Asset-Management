namespace DataAccessLayer
{
    public interface IUserRepository
    {
        public List<User> Display();
        public string Add(User user);
        public User Get(int id);
        public User Update(int id, User user);
        public string Remove(int id);
        public bool Login(string email, string password);
        public bool AdminLogin(string email, string password);
    }
}
