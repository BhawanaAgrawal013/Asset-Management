namespace AssetManagementSystemUI
{
    [Route("Book/[action]")]
    public class BookUIController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        private readonly AssetService assetService;

        public BookUIController(AssetService _assetService)
        {
            this.assetService = _assetService;
        }

        //[Route("display")]
        public async Task<IActionResult> Index()
        {

			List<BookViewModel> books = new List<BookViewModel>();
            using (HttpClient client = _api.Initial())
            {
                HttpResponseMessage res = await client.GetAsync("api/AssetDetail/Display");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    books = JsonConvert.DeserializeObject<List<BookViewModel>>(result);
                    books = books.Where(i => i.CategoryId == (int)Assets.Book).ToList();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(books);
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(BookViewModel book)
        {
            string _token = HttpContext.Session.GetString("_admintoken");

            using (HttpClient client = _api.Initial())
            {
                book.CategoryId = (int)Assets.Book;
                book.SerialId = assetService.GetCategories().Result.Where(i => i.Name == Assets.Book.ToString()).Select(i => i.SerialId).FirstOrDefault() + (new Random()).Next(100, 1000).ToString();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                if (ModelState.IsValid)
                {
                    var postTask = client.PostAsJsonAsync<BookViewModel>("api/AssetDetail/Add", book);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Book Added Sucessfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("~/Views/Shared/UnAuthorized.cshtml");
                    }
                }
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            BookViewModel book = new BookViewModel();
            string _token = HttpContext.Session.GetString("_admintoken");
            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                HttpResponseMessage res = await client.GetAsync($"api/AssetDetail/Search/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    book = JsonConvert.DeserializeObject<BookViewModel>(result);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


                return View(book);
            }
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel book)
        {
            string _token = HttpContext.Session.GetString("_admintoken");
            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                if (ModelState.IsValid)
                {
                    var putTask = client.PutAsJsonAsync<BookViewModel>($"api/AssetDetail/Update/{book.Id}", book);
                    putTask.Wait();

                    var result = putTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Book Updated Sucessfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["msg"] = "You are currently unauthorized";
                        return RedirectToAction("Index");
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
                    TempData["msg"] = "Book Deleted Sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "You are currently unauthorized";
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
