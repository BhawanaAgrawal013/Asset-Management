using Microsoft.AspNetCore.Authorization;

namespace AssetManagementSystemUI
{
    [Route("Admin/[action]")]
    public class AdminController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        public const string SessionToken = "_admintoken";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AssetService assetService;

        public AdminController(AssetService _assetService,  IHttpContextAccessor httpContextAccessor)
        {
            this.assetService = _assetService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Services()
        {
			ViewBag.BookPoints = JsonConvert.SerializeObject(assetService.BookPieData());
            ViewBag.SoftwarePoints = JsonConvert.SerializeObject(assetService.SoftwarePieData());
            ViewBag.HardwarePoints = JsonConvert.SerializeObject(assetService.HardwarePieData());
			return View();
        }

        public IActionResult AdminLogin()
        {
            HttpContext.Session.Remove(SessionToken);
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(UserViewModel user)
        {
            if(string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return View();
            }
            else
            {
                var res = string.Empty;
                using (HttpClient client = _api.Initial())
                {
                    var message = new HttpRequestMessage(HttpMethod.Post, $"api/User/AdminLogin/{user.Email}/{user.Password}");
                    var postTask = client.SendAsync(message);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        res = result.Content.ReadAsStringAsync().Result;
                        HttpContext.Session.SetString(SessionToken, res);
                        return RedirectToAction("Services", "Admin");
                    }
                    else
                    {
                        TempData["errormsg"] = "Wrong Email or Password entered";
                        return View();
                    }
                }
            }
            
        }

        public ActionResult<AssetViewModel> Assign()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Assign(AssetViewModel assetView)
        {
            if (string.IsNullOrEmpty(assetView.AssetName))
            {
                TempData["msg"] = "Please fill all the inputs";
                return View();
            }
            try
            {
                TempData["msg"] = assetService.Assign(assetView);
                return RedirectToAction("Display", "AssetUI");
            }
            catch
            {
                TempData["msg"] = "Please fill all the inputs";
                return View();
            }
        }

        public ActionResult Unassign(int id)
        {
            try
            {
                assetService.Unassign(id);
                TempData["msg"] = "Unassigned successfully";
                return RedirectToAction("Display", "AssetUI");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AcceptRequest(int id)
        {
            try
            {
                TempData["msg"] = assetService.AcceptRequest(id);
                return RedirectToAction("DisplayRequest", "AssetUI");
            }
            catch
            {
                TempData["msg"] = "Error Occured";
                return View();
            }

        }

        public ActionResult DeclineRequest(int id) 
        {
            try
            {
                return View(assetService.GetAssetViews().Result.Where(i => i.Id == id).FirstOrDefault());
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeclineRequest(AssetViewModel assetView)
        {
            try
            {
                TempData["msg"] = assetService.DeclineRequest(assetView.Id, assetView.Reason);
                return RedirectToAction("DisplayRequest", "AssetUI");
            }
            catch
            {
                return View();
            }
        }
    }
}
