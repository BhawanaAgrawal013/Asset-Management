namespace AssetManagementWebApi
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILog logger;
        public UserController(IUserRepository userRepository, IConfiguration configuration, ILog logger)
        {
            this._userRepository = userRepository;
            _configuration = configuration;
            this.logger = logger;
        }


        [HttpGet]
        public ActionResult<List<User>> Display()
        {
            try
            {
                logger.Information("User list fetched successfully");
                return _userRepository.Display();
            }
            catch
            {
                logger.Error("User list could not be found");
                return NotFound();
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "User")]
        public ActionResult<User> Search(int id)
        {
            try
            {
                logger.Information($"User {id} fetched successfully");
                return Ok(_userRepository.Get(id));
            }
            catch
            {
                logger.Error($"User {id} could not be found");
                return NotFound("Id not found");
            }
        }

        [HttpPost]
        public ActionResult<string> Add(User user)
        {
            try
            {
                logger.Information("User added successfully");
                return _userRepository.Add(user);
            }
            catch
            {
                logger.Error($"User could not be added");
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "User")]
        public ActionResult Delete(int id)
        {
            try
            {
                logger.Information($"User {id} removed successfully");
                return Ok(_userRepository.Remove(id));
            }
            catch
            {
                logger.Error($"User {id} could not be deleted");
                return NotFound("Id not found");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "User")]
        public ActionResult<User> Update(int id, User user)
        {
            try
            {
                logger.Information($"User {id} updated successfully");
                return Ok(_userRepository.Update(id, user));
            }
            catch
            {
                logger.Error($"User {id} could not be updated");
                return NotFound("Id not found");
            }
        }

        [HttpPost("{Email}/{password}")]
        public ActionResult<string> Login([FromRoute]string Email, [FromRoute]string password)
        {
            try
            {
                if (_userRepository.Login(Email, password))
                {
                    var token = GenerateToken(Email, password);
                    logger.Information("User logged in successfully");
                    return Ok(token);
                }
                else
                {
                    logger.Error($"User could not be found");
                    return NotFound();
                }
                    
            }
            catch
            {
                logger.Error($"User could not be found");
                return BadRequest();
            }
        }

        [HttpPost("{Email}/{password}")]
        public ActionResult<bool> AdminLogin([FromRoute] string Email, [FromRoute] string password)
        {
            try
            {
                if (_userRepository.AdminLogin(Email, password))
                {
                    var token = GenerateToken(Email, password);
                    logger.Information("Admin logged in successfully");
                    return Ok(token);
                }
                else
                {
                    logger.Error($"Admin could not be found");
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        private string GenerateToken([FromRoute] string Email, [FromRoute] string password)
        {
            var user = _userRepository.Display().Where(i => i.Email == Email).FirstOrDefault();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                (!user.IsAdmin)? new Claim(ClaimTypes.Role,"User") : new Claim(ClaimTypes.Role,"Admin")
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(45), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
