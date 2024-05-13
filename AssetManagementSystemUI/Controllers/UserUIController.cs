using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AssetManagementSystemUI.Controllers
{
    public class UserUIController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        public const string SessionToken = "_token";

        public const string SessionEmail = "_userEmail";

        private readonly UserService _userService;

        private readonly AssetService _assetService;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserUIController(UserService userService, IHttpContextAccessor httpContextAccessor, AssetService assetService)
        {
            this._userService = userService;
            this._httpContextAccessor = httpContextAccessor;
            _assetService = assetService;
        }


        [Route("User/[action]")]
        public IActionResult Index()
        {
            ViewBag.DataPoints = JsonConvert.SerializeObject(_userService.PieData());
			return View();
        }

        [Route("User/[action]")]
        public IActionResult Profile()
        {
            try
            {
                string email = HttpContext.Session.GetString("_userEmail");
                UserViewModel user = new UserViewModel();
                user = _userService.GetUser().Result.Where(i => i.Email == email).FirstOrDefault();
                return View(user);
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Login()
        {
            HttpContext.Session.Remove(SessionToken);
            HttpContext.Session.Remove(SessionEmail);
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserViewModel user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return View();
            }
            using (HttpClient client = _api.Initial())
            {
                var message = new HttpRequestMessage(HttpMethod.Post, $"api/User/Login/{user.Email}/{user.Password}");
                var postTask = client.SendAsync(message);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    HttpContext.Session.SetString(SessionToken, res);
                    HttpContext.Session.SetString(SessionEmail, user.Email);
                    return RedirectToAction("Index", "UserUI");
                }
                else
                {
                    TempData["errormsg"] = "Wrong Email or Password entered";
                    return View();
                }
            }
        }

        [Route("Registeruser")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("Registeruser")]
        [HttpPost]
        public IActionResult Add(UserViewModel user)
        {
            using (HttpClient client = _api.Initial())
            {

                if (ModelState.IsValid)
                {
                    var postTask = client.PostAsJsonAsync<UserViewModel>("api/User/Add", user);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "User Registered Sucessfully. Please login using your email and password";
                        return RedirectToAction("Login");
                    }
                }
                return View();
            }
        }

        [Route("User/[action]")]
        public IActionResult Edit()
        {
            return View(_userService.EditProfile().Result);
        }

        [HttpPost]
        [Route("User/[action]")]
        public IActionResult Edit(UserViewModel user)
        {
            string _token = HttpContext.Session.GetString("_token");
            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                try
                {
                    var putTask = client.PutAsJsonAsync<UserViewModel>($"api/User/Update/{user.Id}", user);
                    putTask.Wait();

                    var result = putTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["usermsg"] = "User Details Updated Sucessfully";
                        return RedirectToAction("Profile", new {email = user.Email});
                    }
                    else
                    {
                        TempData["usermsg"] = "You are currently unauthorized";
                        return RedirectToAction("Profile");
                    }
                }
                catch
                {
                    return View();
                } 
            }
        }

        [Route("User/[action]")]
        public IActionResult RequestAsset()
        {
            return View();
        }

        [HttpPost]
        [Route("User/[action]")]
        public ActionResult RequestAsset(AssetViewModel assetView)
        {
            if(string.IsNullOrEmpty(assetView.AssetName))
            {
                TempData["msg"] = "Please fill all the inputs";
                return View();
            }
            try
            {
                TempData["msg"] = _userService.RequestAsset(assetView);
                return RedirectToAction("DisplayRequest");
            }
            catch
            {
                TempData["msg"] = "Please fill all the inputs";
                return View();
            }
        }

        [Route("User/[action]")]
        public IActionResult DisplayRequest()
        {
            try
            {
                return View(_userService.DisplayRequest().Result);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [Route("User/[action]")]
        public async Task<IActionResult> CancelRequest(int id)
        {
            using (HttpClient client = _api.Initial())
            {
                HttpResponseMessage res = await client.DeleteAsync($"api/Asset/Delete/{id}");

                if (res.IsSuccessStatusCode)
                {
                    TempData["msg"] = "Request Deleted Sucessfully";
                    return RedirectToAction("DisplayRequest");
                }
                else
                {
                    TempData["msg"] = "Not able to process the cancelling request at the moment";
                    return RedirectToAction("DisplayRequest");
                }
            }
        }

        [Route("User/[action]")]
        public IActionResult ReturnAsset(int id)
        {
            TempData["msg"] = _userService.ReturnAsset(id);
            return RedirectToAction("DisplayRequest");
        }
    }
}
