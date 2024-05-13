

namespace AssetManagementSystemUI
{
    [Route("Software/[action]")]
    public class SoftwareUIController : Controller
    {
        private readonly HelperAPI _api = new();

        private readonly AssetService assetService;

        public SoftwareUIController(AssetService _assetService)
        {
            this.assetService = _assetService;
        }
        public async Task<IActionResult> Display()
        {
            List<SoftwareViewModel> softwares = new List<SoftwareViewModel>();

            using (HttpClient client = _api.Initial())
            {
                HttpResponseMessage res = await client.GetAsync("api/AssetDetail/Display");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    softwares = JsonConvert.DeserializeObject<List<SoftwareViewModel>>(result);
                    softwares = softwares.Where(x => x.CategoryId == (int)Assets.Software).ToList();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(softwares);
            }
        }

        public ActionResult Add()
        {
            string token = HttpContext.Session.GetString("_admintoken");
            if (token == null)
            {
                TempData["errormsg"] = "You are Unauthorized to visit this page";
                return RedirectToAction("AdminLogin", "Admin");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Add(SoftwareViewModel software)
        {
            string _token = HttpContext.Session.GetString("_admintoken");
            using (HttpClient client = _api.Initial())
            {
                software.CategoryId = (int)Assets.Software;
                software.SerialId = assetService.GetCategories().Result.Where(i => i.Name == Assets.Software.ToString()).Select(i => i.SerialId).FirstOrDefault() + (new Random()).Next(100, 1000).ToString();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                if (ModelState.IsValid)
                {
                    var postTask = client.PostAsJsonAsync<SoftwareViewModel>("api/AssetDetail/Add", software);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Software Added Sucessfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        TempData["msg"] = "You are currently unauthorized";
                        return RedirectToAction("Display");
                    }
                }
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            SoftwareViewModel software = new SoftwareViewModel();
            string _token = HttpContext.Session.GetString("_admintoken");

            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                HttpResponseMessage res = await client.GetAsync($"api/AssetDetail/Search/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    software = JsonConvert.DeserializeObject<SoftwareViewModel>(result);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


                return View(software);
            }
        }

        [HttpPost]
        public IActionResult Edit(SoftwareViewModel software)
        {
            string _token = HttpContext.Session.GetString("_admintoken");

            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                if (ModelState.IsValid)
                {
                    var putTask = client.PutAsJsonAsync<SoftwareViewModel>($"api/AssetDetail/Update/{software.Id}", software);
                    putTask.Wait();

                    var result = putTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Software Updated Sucessfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        TempData["msg"] = "You are currently unauthorized";
                        return RedirectToAction("Display");
                    }
                }
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            string _token = HttpContext.Session.GetString("_admintoken");

            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                HttpResponseMessage res = await client.DeleteAsync($"api/AssetDetail/Delete/{id}");

                if (res.IsSuccessStatusCode)
                {
                    TempData["msg"] = "Software Deleted Sucessfully";
                    return RedirectToAction("Display");
                }
                else
                {
                    TempData["msg"] = "You are currently unauthorized";
                    return RedirectToAction("Display");
                }
            }
        }
    }
}
