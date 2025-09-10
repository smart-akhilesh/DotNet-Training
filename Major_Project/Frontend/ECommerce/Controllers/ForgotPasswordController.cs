using System.Net.Http;

using System.Text;

using System.Threading.Tasks;

using System.Web.Mvc;

using ECommerce.Models;

using Newtonsoft.Json;

namespace ECommerce.Controllers

{

    public class ForgotPasswordController : Controller

    {

        private readonly string requestOtpUrl = "https://localhost:44353/api/forgotpassword/requestotp";

        private readonly string resetPasswordUrl = "https://localhost:44353/api/forgotpassword/reset";

        // GET: Show the combined Forgot Password view

        [HttpGet]

        public ActionResult Index()

        {

            return View(new ForgotPasswordViewModel());

        }

        // POST: Handle both steps in one view

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Index(ForgotPasswordViewModel model)

        {

            if (!ModelState.IsValid)

                return View(model);

            string apiUrl = model.IsStep2 ? resetPasswordUrl : requestOtpUrl;

            using (var client = new HttpClient())

            {

                var json = JsonConvert.SerializeObject(model);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)

                {

                    if (!model.IsStep2)

                    {

                        // OTP sent → move to Step 2

                        model.IsStep2 = true;

                        model.Message = "OTP has been sent to your registered email.";

                        ModelState.Clear(); // clear previous validation errors

                        return View(model);

                    }

                    else

                    {

                        // Password reset successful → redirect to login

                        TempData["ResetSuccessMessage"] = "Password reset successful. Please log in.";

                        return RedirectToAction("Signin", "Account", new { role = "User" });

                    }

                }

                else

                {

                    ModelState.AddModelError("", result);

                    return View(model);

                }

            }

        }

    }

}

