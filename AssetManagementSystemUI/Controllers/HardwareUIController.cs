namespace AssetManagementSystemUI
{
    [Route("Hardware/[action]")]
    public class HardwareUIController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        private readonly AssetService assetService;

        public HardwareUIController(AssetService _assetService)
        {
            this.assetService = _assetService;
        }
        public async Task<IActionResult> Display()
        {
            List<HardwareViewModel> hardwares = new List<HardwareViewModel>();
            using (HttpClient _client = _api.Initial())
            {

                HttpResponseMessage res = await _client.GetAsync("api/AssetDetail/Display");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    hardwares = JsonConvert.DeserializeObject<List<HardwareViewModel>>(result);
                    hardwares = hardwares.Where(x => x.CategoryId == (int)Assets.Hardware).ToList();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(hardwares);
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
        public IActionResult Add(HardwareViewModel hardware)
        {
            string _token = HttpContext.Session.GetString("_admintoken");
            using (HttpClient client = _api.Initial())
            {
                hardware.CategoryId = (int)Assets.Hardware;
                hardware.SerialId = assetService.GetCategories().Result.Where(i => i.Name == Assets.Hardware.ToString()).Select(i => i.SerialId).FirstOrDefault() + (new Random()).Next(100, 1000).ToString();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                if (ModelState.IsValid)
                {
                    var postTask = client.PostAsJsonAsync<HardwareViewModel>("api/AssetDetail/Add", hardware);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Hardware Added Sucessfully";
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
            HardwareViewModel hardware = new HardwareViewModel();
            string _token = HttpContext.Session.GetString("_admintoken");
            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                HttpResponseMessage res = await client.GetAsync($"api/AssetDetail/Search/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    hardware = JsonConvert.DeserializeObject<HardwareViewModel>(result);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


                return View(hardware);
            }
        }

        [HttpPost]
        public IActionResult Edit(HardwareViewModel hardware)
        {
            string _token = HttpContext.Session.GetString("_admintoken");
            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                if (ModelState.IsValid)
                {
                    var putTask = client.PutAsJsonAsync<HardwareViewModel>($"api/AssetDetail/Update/{hardware.Id}", hardware);
                    putTask.Wait();

                    var result = putTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Hardware Updated Sucessfully";
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
                    TempData["msg"] = "Hardware Deleted Sucessfully";
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
