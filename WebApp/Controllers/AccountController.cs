using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Context;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        MyContext myContext;
        public AccountController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var data = myContext.Users
                .Include(x => x.Employee) //Join
                .Include(x => x.Role) //Join
                .SingleOrDefault(x => x.Employee.Email.Equals(email) && x.Password.Equals(password));
            /*var data2 = myContext.Users
                .Include(x => x.Employee)
                .Include(x => x.Role)
                .Where(x => x.Employee.Email.Equals(email) && x.Password.Equals(password))
                .Select(x => new { x.Employee.FullName, x.Employee.Email, x.Role.Name });*/

            if (data != null)
            {
                ResponseLogin responseLogin = new ResponseLogin()
                {
                    FullName = data.Employee.FullName,
                    Email = data.Employee.Email,
                    Role = data.Role.Name
                };
                return RedirectToAction("Index", "Home", responseLogin);
            }
            return View();
        }

        //REGISTER
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string fullName, string email, DateTime birthDate, string password)
        {
            Employee employee = new Employee()
            {
                FullName = fullName,
                Email = email,
                BirthDate = birthDate
            };
            myContext.Employees.Add(employee);
            var result = myContext.SaveChanges();
            if (result > 0)
            {
                //Mengambil id employee dari email yang dimasukkan, karena id user bersifat composite key
                //dengan employee id dan tidak bersifat identity
                var id = myContext.Employees.SingleOrDefault(x => x.Email.Equals(email)).Id;
                User user = new User()
                {
                    Id = id,
                    Password = password,
                    RoleId = 1 //input 1 karena di bagian role, pada umumnya semakin naik idnya
                               //semakin tinggi posisinya. 1 = user/pengunjung, 2 = admin, 3 = super admin, dst.
                               //di bagian role harus diterapkan seperti itu, agar memudahkan jika ada role baru
                               //dari yang memiliki akses terbatas sampai bisa mengakses data apapun
                };
                myContext.Users.Add(user);
                var resultUser = myContext.SaveChanges();
                if (resultUser > 0)
                    return RedirectToAction("Login", "Account");
            }
            return View();
        }

        //CHANGE PASSWORD
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string email, string password, string newPass)
        {
            var data = myContext.Users
                .Include(x => x.Employee)
                .SingleOrDefault(x => x.Employee.Email.Equals(email) && x.Password.Equals(password));
            if (data != null)
            {
                data.Password = newPass;
                myContext.Entry(data).State = EntityState.Modified;
                var resultUser = myContext.SaveChanges();
                if (resultUser > 0)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }

        //FORGOT PASSWORD
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email, string newPass, string confPass)
        {
            var data = myContext.Users
                .Include(x => x.Employee)
                .SingleOrDefault(x => x.Employee.Email.Equals(email));
            if (data != null)
            {
                var id = myContext.Employees.SingleOrDefault(x => x.Email.Equals(email)).Id;
                if (newPass == confPass)
                {
                    data.Password = confPass;
                    myContext.Entry(data).State = EntityState.Modified;
                    var resultUser = myContext.SaveChanges();
                    if (resultUser > 0)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            return View();
        }
    }
}
