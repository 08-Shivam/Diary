using System;
using System.Web.UI.WebControls;
using System.IO;
using Diary.Models;
using Newtonsoft.Json;
using System.Linq;
namespace Diary.Pages
{
    public partial class Notepad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Clear(); // lblNote.Text
            clear(); // lblCreate.Text
            lblDateTime.Text = DateTime.Now.ToString("F");
            if (Session["userid"] != null)
            {
                lblDetail.Text = "Welcome to the Digital Diary: " + Session["email"].ToString();

                if (!IsPostBack)
                {
                    BindFiles();
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
            emptyalert.Visible = false;
        }

        private void BindFiles()
        {
            string userid = Session["userid"].ToString();
            DataModel model = (DataModel)Session["data"];
            var files = model.Directories.Where(x => x.UserID == userid).ToList();

            repeatFiles.DataSource = files;
            repeatFiles.DataBind();
        }


        protected void btSave_Click(object sender, EventArgs e)
        {
            string rootPath = @"D:\Notepad_Diary\";
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            if (!dirInfo.Exists) {
                dirInfo.Create();
            }
            //if (!System.IO.Directory.Exists(rootPath))
            //{
            //    System.IO.Directory.CreateDirectory(rootPath);
            //}

            //string fileName = lblCreate.Text + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";
            string fileName = lblCreate.Text;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Response.Write("<script>alert('File name cannot be empty.');</script>");
                return;
            }
            fileName += ".txt";
            string filepath = Path.Combine(rootPath, fileName);
            string content = txtNote.Text;

            if (string.IsNullOrWhiteSpace(content))
            {
                Response.Write("<script>alert('Cannot save empty file.');</script>");
                return;
            }

            try
            {
                File.WriteAllText(filepath, content);

                DataModel model = (DataModel)Session["data"];

                model.Directories.Add(new Models.Directory
                {
                    Id = model.Directories.Any() ? model.Directories.Max(x => x.Id) + 1 : 1,
                    Name = lblCreate.Text,
                    Path = filepath,
                    UserID = Session["userid"].ToString()
                });

                string str = JsonConvert.SerializeObject(model),
                    path = Server.MapPath("~/assets/data.json");

                File.WriteAllText(path, str);

                Session["data"] = model;
                BindFiles();

                Response.Write("<script>alert('File Saved Successfully as :" + fileName + "');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error : " + ex.Message + "')</script>");
            }
            Clear();
        }
        void Clear()
        {
            txtNote.Text = string.Empty;
        }

        protected void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                var id = Convert.ToInt32(((LinkButton)sender).CommandArgument);

                var model = (DataModel)Session["data"];

                var filepath = model.Directories.Where(x => x.Id == id).Select(s => s.Path).First();

                var str = File.ReadAllText(filepath);
                var fileName= Path.GetFileNameWithoutExtension(filepath);

                txtNote.Text = str;
                lblCreate.Text = fileName;
                emptyalert.Visible = false;
                Emptyelert(); //emptyalert.Text
            } 
            catch (Exception ex)
            {
                emptyalert.Text=(ex.Message);
                emptyalert.Visible = true;
                Clear(); // lblNote.Text
                clear(); // lblCreate.Text
            }
        }
        void Emptyelert()
        {
            emptyalert.Text = string.Empty;
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                var commandArgument = btn.CommandArgument;

                if (string.IsNullOrEmpty(commandArgument))
                {
                    Response.Write("<script>alert('CommandArgument is missing or empty.')</script>");
                    return;
                }

                if (!int.TryParse(commandArgument, out int id))
                {
                    Response.Write("<script>alert('Invalid file ID format.')</script>");
                    return;
                }

                var model = (DataModel)Session["data"];

                var fileToDelete = model.Directories.FirstOrDefault(x => x.Id == id);
                if (fileToDelete != null)
                {
                    if (File.Exists(fileToDelete.Path))
                    {
                        File.Delete(fileToDelete.Path);
                    }

                    model.Directories.Remove(fileToDelete);

                    string jsonData = JsonConvert.SerializeObject(model);
                    string jsonFilePath = Server.MapPath("~/assets/data.json");
                    File.WriteAllText(jsonFilePath, jsonData);

                    Session["data"] = model;

                    BindFiles();
                }
                else
                {
                    Response.Write("<script>alert('No files exist.')</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "')</script>");
            }
            clear();
            Clear();
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Remove("email");
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        void clear()
        {
            lblCreate.Text = string.Empty;
        }
    }
}