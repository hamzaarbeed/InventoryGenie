using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class EmployeeFunctions:Employee
    {

        public static void Login(string UserName, string Password)
        {
            //finds employee with the same Username and password
            LoggedInEmployee = Context.Employees.Include(x=>x.Role).FirstOrDefault(x => x.UserName == UserName && x.Password == Password);
        }

        public static List<Role> GetAllRoles() {
            return Context.Roles.OrderBy(x=>x.RoleName).ToList();
        }

        public static void Logout()
        {
            LoggedInEmployee = null;
        }

        public static string ChangePassword(int Id, string newPassword, string confirmedNewPassword)
        {
            //if new password doesn't match confirmed new password
            if (newPassword != confirmedNewPassword)
            {
                //then save this message to be shown in ChangePassword view
                return "new password doesn't match confirmed new password";
            }

            LoggedInEmployee = Context.Employees.Include(x=>x.Role).FirstOrDefault(x=> x.Id == Id);//gets an employee that has that Id
            //if the new password the same as old password
            if (newPassword == LoggedInEmployee.Password)
            {
                //then save this message to be shown in ChangePassword view
                return "new password doesn't match confirmed new password";
            }


            //if all field are ok then change password
            LoggedInEmployee.Password = newPassword;
            //this user doesn't need to change password in the coming logins.
            LoggedInEmployee.IsTemporaryPassword = false;

            //save changes
            Context.SaveChanges();

            //save Login Data

            return null;

        }
    }
}
