using System;
using System.Collections.Generic;
using System.Configuration;
using Diary.Models;
using Newtonsoft.Json;
using System.IO;

namespace Diary
{
    public partial class Register : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs2"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Server.MapPath("~/assets/data.json");
                DataModel dataModel = new DataModel();

                if (File.Exists(path))
                {
                    string data = File.ReadAllText(path);
                    dataModel = JsonConvert.DeserializeObject<DataModel>(data);
                }

                if (dataModel.Register == null)
                {
                    dataModel.Register = new List<RegisterUser>();
                }

                RegisterUser newUser = new RegisterUser
                {
                    Name = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                };

                dataModel.Register.Add(newUser);

                string output = JsonConvert.SerializeObject(dataModel, Formatting.Indented);
                File.WriteAllText(path, output);

                //lblMessage.Text = "You have successfully registered";
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
        }
    }
}
