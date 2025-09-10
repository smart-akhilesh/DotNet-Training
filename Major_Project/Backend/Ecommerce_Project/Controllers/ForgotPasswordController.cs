using System;

using System.Collections.Generic;

using System.Linq;

using System.Net;

using System.Net.Http;

using System.Net.Mail;

using System.Web.Http;

using Ecommerce_Project.Models;

using Ecommerce_Project.DTO;

using System.Collections.Concurrent;

namespace Ecommerce_Project.Controllers

{

    [RoutePrefix("api/forgotpassword")]

    public class ForgotPasswordController : ApiController

    {

        majorprojectEntities db = new majorprojectEntities();

        // Temporary in-memory OTP store

        private static ConcurrentDictionary<string, (string Otp, DateTime Expiry, string Role)> otpStore = new ConcurrentDictionary<string, (string, DateTime, string)>();

        // POST api/forgotpassword/requestotp

        [HttpPost]

        [Route("requestotp")]

        public IHttpActionResult RequestOtp([FromBody] ForgotPasswordDTO model)

        {

            if (model == null || string.IsNullOrWhiteSpace(model.UsernameOrEmail))

                return BadRequest("Username or Email is required.");

            var key = model.UsernameOrEmail.ToLower();

            // Check in User table

            var user = db.Users.FirstOrDefault(u => u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail);

            if (user != null)

            {

                return GenerateAndSendOtp(key, user.Email, "User");

            }

            // Check in Retailer table

            var retailer = db.Retailers.FirstOrDefault(r => r.Username == model.UsernameOrEmail || r.Email == model.UsernameOrEmail);

            if (retailer != null)

            {

                return GenerateAndSendOtp(key, retailer.Email, "Retailer");

            }

            // Check in Admin table

            var admin = db.Admins.FirstOrDefault(a => a.Username == model.UsernameOrEmail || a.Email == model.UsernameOrEmail);

            if (admin != null)

            {

                return GenerateAndSendOtp(key, admin.Email, "Admin");

            }

            return NotFound();

        }

        private IHttpActionResult GenerateAndSendOtp(string key, string email, string role)

        {

            var otp = new Random().Next(1000, 9999).ToString();

            var expiry = DateTime.UtcNow.AddMinutes(5);

            otpStore[key] = (otp, expiry, role);

            try

            {

                SendOtpEmail(email, otp);

            }

            catch (Exception ex)

            {

                return InternalServerError(ex);

            }

            return Ok($"OTP has been sent to your registered {role} email.");

        }

        // POST api/forgotpassword/reset

        [HttpPost]

        [Route("reset")]

        public IHttpActionResult ResetPassword([FromBody] ForgotPasswordDTO model)

        {

            if (model == null)

                return BadRequest("Invalid request.");

            var key = model.UsernameOrEmail?.ToLower();

            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(model.OTP) ||

                string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.ConfirmPassword))

                return BadRequest("Username/Email, OTP, New Password, and Confirm Password are required.");

            if (!otpStore.TryGetValue(key, out var storedOtpInfo))

                return BadRequest("No OTP requested or OTP expired.");

            if (DateTime.UtcNow > storedOtpInfo.Expiry)

            {

                otpStore.TryRemove(key, out _);

                return BadRequest("OTP expired. Please request a new one.");

            }

            if (storedOtpInfo.Otp != model.OTP)

                return BadRequest("Invalid OTP.");

            if (model.NewPassword != model.ConfirmPassword)

                return BadRequest("Passwords do not match.");

            var hashedPassword = HashPassword(model.NewPassword);

            // Update password based on role

            if (storedOtpInfo.Role == "User")

            {

                var user = db.Users.FirstOrDefault(u => u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail);

                if (user != null)

                    user.Password = hashedPassword;

            }

            else if (storedOtpInfo.Role == "Retailer")

            {

                var retailer = db.Retailers.FirstOrDefault(r => r.Username == model.UsernameOrEmail || r.Email == model.UsernameOrEmail);

                if (retailer != null)

                    retailer.Password = hashedPassword;

            }

            else if (storedOtpInfo.Role == "Admin")

            {

                var admin = db.Admins.FirstOrDefault(a => a.Username == model.UsernameOrEmail || a.Email == model.UsernameOrEmail);

                if (admin != null)

                    admin.Password = hashedPassword;

            }

            db.SaveChanges();

            otpStore.TryRemove(key, out _);

            return Ok("Password has been reset successfully.");

        }

        private void SendOtpEmail(string toEmail, string otp)

        {

            var message = new MailMessage();

            message.To.Add(toEmail);

            message.Subject = "OTP for Password Reset";

            message.Body = $"Your OTP is: {otp}";

            message.IsBodyHtml = false;

            using (var smtp = new SmtpClient())

            {

                smtp.Send(message);

            }

        }

        private string HashPassword(string password)

        {

            using (var sha256 = System.Security.Cryptography.SHA256.Create())

            {

                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

                var hashBytes = sha256.ComputeHash(passwordBytes);

                return Convert.ToBase64String(hashBytes);

            }

        }

    }

}

