using System;
using System.Configuration;
using System.IO;
using Diary.Models;
using Newtonsoft.Json;
using System.Linq;
namespace Diary
{
    public partial class Login : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs2"].ConnectionString;
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
                string email=txtEmail.Text.Trim();
                string password=txtPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    lblMessage.Text = "Email & Password are mandatory.";
                    return;
                }

                string path = Server.MapPath("~/assets/data.json");
                DataModel model = new DataModel();

                if (File.Exists(path)) 
                {
                    string data=File.ReadAllText(path);
                    model=JsonConvert.DeserializeObject<DataModel>(data);
                }

                if (model.Register != null)
                {
                    var user = model.Register.FirstOrDefault(x => x.Email == email && x.Password == password);
                    if (user != null)
                    {
                        Session["userid"] = user.Name;
                        Session["Password"] = user.Password;
                        Session["email"] = user.Email;
                        Session["data"] = model;
                        Response.Redirect("Notepad.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid Email or Password";
                    }
                }
                else
                {
                    lblMessage.Text = "No user found. Please Register first.";
                }
            }

            catch (Exception ex) {
                Response.Write(ex.Message);
            }
        }
    }
}
