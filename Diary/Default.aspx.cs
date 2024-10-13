using Diary.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace Diary
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            PerformLoginJson();
        }
        private void PerformLoginJson()
        {
            try
            {
                string role=txtRole.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
                {
                    lblMessage.Text = "Email, Password and are mandatory.";
                    return;
                }
                string path = Server.MapPath("~/assets/data.json");

                DataModel model = new DataModel();

                if (File.Exists(path))
                {
                    string data = File.ReadAllText(path);
                    model = JsonConvert.DeserializeObject<DataModel>(data);
                } else
                {
                    //Response.Redirect("Register.aspx");
                    Response.Redirect("Register.aspx");
                }

                if (model.Register != null)
                {
                    var user = model.Register.FirstOrDefault(x => x.Email == email && x.Password == password && x.Role==role);
                    if (user != null)
                    {
                        Session["role"] = user.Role;
                        Session["Password"] = user.Password;
                        Session["email"] = user.Email;
                        Session["data"] = model;

                        HttpCookie emailCookie = new HttpCookie("email");
                        emailCookie.Value = email;
                        emailCookie.Expires = DateTime.Now.AddMinutes(1);
                        Response.Cookies.Add(emailCookie);
                        Response.Redirect("Notepad.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid Role or Email or Password";
                    }
                }
                else
                {
                    lblMessage.Text = "No user found. Please Register first.";
                }
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}
