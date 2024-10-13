using System;
using System.Collections.Generic;
using Diary.Models;
using Newtonsoft.Json;
using System.IO;

namespace Diary
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DirectoryInfo d = new DirectoryInfo(@"D:/Notepad_Diary/assets"); // Create directory in machine

                if (!d.Exists)
                {
                    d.Create();  //create folder if not exist
                    var x = new DataModel();
                    var str = JsonConvert.SerializeObject(x);

                    File.WriteAllText(Path.Combine(d.FullName, "data.json"), str);
                }
            }
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Server.MapPath("~/assets/data.json");
                DataModel dataModel = new DataModel();

                string data = File.ReadAllText(path);
                dataModel = JsonConvert.DeserializeObject<DataModel>(data);



                if (dataModel.Register == null)
                {
                    dataModel.Register = new List<RegisterUser>();
                }

                RegisterUser newUser = new RegisterUser
                {
                    Role=txtRole.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                };

                dataModel.Register.Add(newUser);

                string output = JsonConvert.SerializeObject(dataModel, Formatting.Indented);
                File.WriteAllText(path, output);

                Response.Redirect("Default.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
        }
    }
}