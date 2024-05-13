namespace DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        private readonly AssetManagmentContext _managmentContext;
        public UserRepository(AssetManagmentContext _managmentContext)
        {
            this._managmentContext = _managmentContext;
        }
        public List<User> Display()
        {
            return _managmentContext.Users.ToList();
        }

        public User Get(int id)
        {
            return _managmentContext.Users.Where(i => i.Id == id).FirstOrDefault();
        }

        public string Add(User user)
        {
            if (user == null)
            {
                return "User cannot be added, the object seems to be of null type";
            }
            else
            {
                _managmentContext.Users.Add(user);
                _managmentContext.SaveChanges();
                return "Sucessfully Added User";
            }
        }

        public User Update(int id, User newUser)
        {
            User oldUser = _managmentContext.Users.Where(i => i.Id == id).FirstOrDefault();
            oldUser.FirstName = newUser.FirstName;
            oldUser.LastName = newUser.LastName;
            oldUser.Gender = newUser.Gender;
            oldUser.City = newUser.City;
            oldUser.State = newUser.State;
            oldUser.DateOfBirth = newUser.DateOfBirth;
            oldUser.Phone = newUser.Phone;
            oldUser.Email = newUser.Email;
            _managmentContext.SaveChanges();
            return oldUser;
        }

        public string Remove(int id)
        {
            User user = Get(id);
            _managmentContext.Users.Remove(user);
            _managmentContext.SaveChanges();
            return "User deleted successfully";
        }

        public bool Login(string email, string password)
        {
            var existuser = _managmentContext.Users.Where(i => i.Email == email).FirstOrDefault();

            if (existuser.Email == email && existuser.Password == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AdminLogin(string email, string password)
        {
            var existuser = _managmentContext.Users.Where(i => i.Email == email).FirstOrDefault();

            if (existuser.Email == email && existuser.Password == password && existuser.IsAdmin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
